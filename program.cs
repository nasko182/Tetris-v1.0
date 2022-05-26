using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace _01._Basic_Stack_Operations
{

    internal class Program
    {
        // Settings
        static int LinesRows = 1;
        static int TetrisRows = 20;
        static int TetrisCols = 10;
        static int InfoCols = 7;
        static int ConsoleRows = 1 + LinesRows + 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;
        static int[] ScorePerLines = new int[] { 0, 20, 50, 150, 600 };
        //State
        static int HightScore = 0;
        static int score = 0;
        static int Lines = 0;
        static int Level = (Lines / 10) + 1;
        static int FrameCounter = 1;
        static int FrameToMoveFigure = 16;
        static bool[,] TetrisField = new bool[TetrisRows + 3, TetrisCols + 1];
        static List<bool[,]> Figures = new List<bool[,]>()
        {
            //T
            new bool[,]
            {
            {false,true,false },
            {true,true,true}
            },
            //O
            new bool[,]
            {
            {true,true},
            {true,true}
            },
            //S
            new bool[,]
            {
            {false,true,true },
            {true,true,false }
            },
            //Z
            new bool[,]
            {
            {true,true,false },
            {false,true,true},
            },
            //L
            new bool[,]
            {
            {false,false,true},
            {true,true,true}
            },
            //J
            new bool[,]
            {
            {true,false,false},
            { true,true,true}
            },
            //I
            new bool[,]
            { {true, true, true, true } },

        };
        static Random random = new Random();
        static int NextFigureIndex = random.Next(0, Figures.Count);
        static bool[,] CurrentFigure = Figures[0];
        static bool[,] NextFigure = Figures[NextFigureIndex];
        static int currentFigureRow = 3;
        static int currentFigureCol = 4;
        static int lastRow = currentFigureRow + CurrentFigure.GetLength(0);
        static int lastCol = currentFigureCol + CurrentFigure.GetLength(1);


        static void Main(string[] args)
        {
            if (File.Exists("score.txt"))
            {
                var allScores = File.ReadAllLines("score.txt");
                foreach (var score in allScores)
                {
                    var match = Regex.Match(score, @"= (?<score>[0-9]+)");
                    if (match.Success)
                    {
                        HightScore = Math.Max(HightScore, int.Parse(match.Groups[1].Value));
                    }
                }
            }
            {
                Console.Title = "Tetris v1.0";
                Console.WindowHeight = ConsoleRows;
                Console.WindowWidth = ConsoleCols;
                Console.BufferHeight = ConsoleRows;
                Console.BufferWidth = ConsoleCols;
                Console.CursorVisible = false;
                NextFigure = Figures[random.Next(0, Figures.Count)];
                CurrentFigure = NextFigure;
                
            }
            while (true)
            {
                lastRow = currentFigureRow + CurrentFigure.GetLength(0) - 1;
                lastCol = currentFigureCol + CurrentFigure.GetLength(1) - 1;
                FrameCounter++;
                //Read User Input
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        return;
                    }
                    if (key.Key == ConsoleKey.Spacebar ||
                        key.Key == ConsoleKey.UpArrow ||
                        key.Key == ConsoleKey.W)
                    {
                        RotateCurrentFigure();
                    }
                    if (key.Key == ConsoleKey.LeftArrow ||
                        key.Key == ConsoleKey.A)
                    {
                        
                        if (IsInside(currentFigureRow, currentFigureCol - 1,
                            lastRow,
                            lastCol - 1))
                        {
                            currentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow ||
                        key.Key == ConsoleKey.D)
                    {
                        
                        if (IsInside(currentFigureRow, currentFigureCol + 1,
                            lastRow,
                            lastCol + 1))
                        {
                            currentFigureCol++;
                        }
                    }
                    if (key.Key == ConsoleKey.DownArrow ||
                        key.Key == ConsoleKey.S)
                    {

                        FrameCounter = 1;
                        score++;
                        currentFigureRow++;
                        DrawInfo(lastRow, lastCol);


                    }

                }


                //Update game state
                if (FrameCounter % (FrameToMoveFigure - Level) == 0)
                {


                    FrameCounter = 1;

                    currentFigureRow++;
                    DrawInfo(lastRow, lastCol);


                }
                if (Collisium(CurrentFigure))
                {
                    DrawInfo(lastRow, lastCol);
                    AddCurrentFigureToTetrisField();
                    for (int i = 0; i < CurrentFigure.GetLength(0); i++)
                    {
                        for (int j = 0; j < CurrentFigure.GetLength(1); j++)
                        {
                            if (CurrentFigure[i, j])
                            {
                                score++;
                            }
                        }
                    }
                    int currentLines = CheckForFullLines();
                    score += ScorePerLines[currentLines] * Level;
                    Lines += currentLines;
                    Level = (Lines / 10) + 1;
                    CurrentFigure = NextFigure;
                    NextFigure = Figures[NextFigureIndex = random.Next(0, Figures.Count)];
                    currentFigureRow = 3;
                    currentFigureCol = 4;
                    if (Collisium(CurrentFigure))
                    {
                        File.AppendAllLines("score.txt", new List<string>
                        {
                        $"[{DateTime.Now.ToString()}] {Environment.UserName}= {score}"
                        });
                        // Console.Clear();
                        // TODO: NewHightScore
                        string scoreAsStirng = score.ToString();
                        scoreAsStirng += new string(' ', 6 - scoreAsStirng.Length);
                        Write("╔══════════╗", 4, 8);
                        Write("║ GAME     ║", 4, 9);
                        Write("║  OVER!   ║", 4, 10);
                        Write("║   SCORE: ║", 4, 11);
                        Write($"║  {scoreAsStirng}  ║", 4, 12);
                        Write("╚══════════╝", 4, 13);
                        Thread.Sleep(100000000);
                        return;
                    }
                }

                //Redeaw UI
                DrawBorder();
                DrawInfo(lastRow, lastCol);
                DrawTetrisField();
                DrawCurrentFigure(CurrentFigure);
                DrawNextFigure(NextFigure);

                Thread.Sleep(40);
            }

        }

        private static void RotateCurrentFigure()
        {
            var newFigure = new bool[CurrentFigure.GetLength(1),CurrentFigure.GetLength(0)];
            for (int i = 0; i < CurrentFigure.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentFigure.GetLength(1); j++)
                {
                    newFigure[j, CurrentFigure.GetLength(0) - i - 1] = CurrentFigure[i, j];
                }
            }
            if (!Collisium(newFigure))
            {
            CurrentFigure = newFigure;

            }
        }

        private static int CheckForFullLines()
        {
            int lines = 0;

            for (int i = TetrisField.GetLength(0) - 1; i >= 4; i--)
            {
                bool RowIsFull = true;
                for (int j = 1; j < TetrisField.GetLength(1); j++)
                {
                    if (!TetrisField[i, j])
                    {
                        RowIsFull = false;
                        break;
                    }
                }
                if (RowIsFull)
                {
                    for (int k = i; k >= 4; k--)
                    {
                        for (int c = 0; c < TetrisField.GetLength(1); c++)
                        {
                            TetrisField[k, c] = TetrisField[k - 1, c];
                        }
                    }
                    lines++;
                }
            }
            if (lines>1)
            {
                return 2;
            }
            return lines;
        }

        static bool Collisium(bool[,] figure)
        {
            if (currentFigureCol>TetrisField.GetLength(1)-figure.GetLength(1))
            {
                return true;
            }
            if (currentFigureRow - 1 + figure.GetLength(0) == TetrisField.GetLength(0))
            {
                return true;
            }
            for (int i = 0; i < figure.GetLength(0); i++)
            {
                for (int j = 0; j < figure.GetLength(1); j++)
                {
                    if (figure[i, j]
                        && TetrisField[currentFigureRow + i, currentFigureCol + j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static void AddCurrentFigureToTetrisField()
        {
            for (int i = 0; i < CurrentFigure.GetLength(0); i++)
            {
                for (int j = 0; j < CurrentFigure.GetLength(1); j++)
                {
                    if (CurrentFigure[i, j])
                    {
                        TetrisField[i + currentFigureRow - 1, j + currentFigureCol] = true;
                    }
                }
            }
        }

        private static void DrawCurrentFigure(bool[,] figure)
        {

            for (int i = 0; i < figure.GetLength(1); i++)
            {
                for (int j = 0; j < figure.GetLength(0); j++)
                {
                    if (figure[j, i])
                    {
                        Write("▓", i + currentFigureCol, j + currentFigureRow);
                    }
                }
            }
        }

        private static void DrawNextFigure(bool[,] figure)
        {

            for (int i = 0; i < figure.GetLength(1); i++)
            {
                for (int j = 0; j < figure.GetLength(0); j++)
                {
                    if (figure[j, i])
                    {
                        Write("▓", i + 14, j + 8);
                    }
                }
            }
        }

        private static bool IsInside(int FirstRow, int FirstCol, int LastRow, int LastCol)
        {
            return
                FirstRow >= 3 && FirstCol >= 1 &&
                FirstRow < TetrisField.GetLength(0) &&
                FirstCol < TetrisField.GetLength(1) &&
                LastRow >= 4 && LastCol >= 1 &&
                LastRow < TetrisField.GetLength(0) &&
                LastCol < TetrisField.GetLength(1);
        }


        static void DrawTetrisField()
        {
            for (int i = 0; i < TetrisField.GetLength(0); i++)
            {
                for (int j = 0; j < TetrisField.GetLength(1); j++)
                {
                    if (TetrisField[i, j])
                    {
                        Write("▓", j, i);
                    }
                }
            }
        }

        private static void DrawInfo(int row, int col)
        {
            DrawLevel(Level);
            Write(Lines.ToString(), 7, 1);
            Write(score.ToString(), 12, 3);
        }

        static void DrawBorder()
        {
            Console.SetCursorPosition(0, 0);
            //Top row
            string line = "┌";
            line += new string('─', TetrisCols);
            line += '┬';
            line += new string('─', InfoCols);
            line += '┐';
            Console.Write(line);
            //Line row 
            string LineRow = "│";
            LineRow += "LINES:";
            LineRow += new string(' ', 4);
            LineRow += "├";
            LineRow += new string('─', InfoCols);
            LineRow += "┤";
            Console.Write(LineRow);
            //ColseLine
            string closeLine = "├";
            closeLine += new string('─', TetrisCols);
            closeLine += "┤";
            closeLine += "Score: ";
            closeLine += "│";
            Console.Write(closeLine);
            //score
            line = "│";
            line += new string(' ', TetrisCols);
            line += '│';
            line += new string(' ', InfoCols);
            line += '│';
            Console.Write(line);
            //closeScore
            string closeScore = "│";
            closeScore += new string(' ', TetrisCols);
            closeScore += "├";
            closeScore += new string('─', InfoCols);
            closeScore += "┤";
            Console.Write(closeScore);
            //nextBorder
            line = "│";
            line += new string(' ', TetrisCols);
            line += '│';
            line += "╔";
            line += new string('═', InfoCols - 2);
            line += '╗';
            line += '│';
            Console.Write(line);
            //nextLine
            line = "│";
            line += new string(' ', TetrisCols);
            line += '│';
            line += "║";
            line += "NEXT:";
            line += '║';
            line += '│';
            Console.Write(line);
            for (int i = 0; i < 5; i++)
            {
                line = "│";
                line += new string(' ', TetrisCols);
                line += '│';
                line += "║";
                line += new string(' ', TetrisCols - 5);
                line += '║';
                line += '│';
                Console.WriteLine(line);
            }
            //nextBorder
            line = "│";
            line += new string(' ', TetrisCols);
            line += '│';
            line += "╚";
            line += new string('═', TetrisCols - 5);
            line += '╝';
            line += '│';
            Console.Write(line);
            //closeNext
            for (int i = 0; i < 4; i++)
            {
                closeScore = "│";
                closeScore += new string(' ', TetrisCols);
                closeScore += "├";
                closeScore += new string('─', InfoCols);
                closeScore += "┤";
                Console.Write(closeScore);
            }

            //levelLine
            line = "│";
            line += new string(' ', TetrisCols);
            line += '│';
            line += "LEVEL: ";
            line += '│';
            //emptyLine
            for (int i = 0; i < 3; i++)
            {
                Console.Write(line);
                line = "│";
                line += new string(' ', TetrisCols);
                line += '│';
                line += new string(' ', InfoCols);
                line += '│';
                Console.Write(line);
            }
            //endLine
            line = "└";
            line += new string('─', TetrisCols);
            line += '┴';
            line += new string('─', InfoCols);
            line += '┘';
            Console.Write(line);


        }


        static void DrawLevel(int level)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            string text = level.ToString();
            if (level < 9)
            {



                if (text[0] == '0')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '1')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ╗");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '2')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("╔═╝");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '3')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("══╗");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("══╣");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("══╝");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '4')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '5')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("╚═╗");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '6')
                {
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("╠═╗");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 22);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 23);
                }
                if (text[0] == '7')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[0] == '8')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╠═╣");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[0] == '9')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
            }
            else
            {
                if (text[1] == '0')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '1')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("  ╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '2')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╔═╝");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '3')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("══╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("══╣");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("══╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '4')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("   ");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '5')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╚═╗");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '6')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╠═╗");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '7')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '8')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╠═╣");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }
                if (text[1] == '9')
                {
                    Console.SetCursorPosition(16, 17 + 1);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19 + 1);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(16, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22 + 1);
                }

                Console.SetCursorPosition(12, 17);
                if (text[0] == '1')
                {
                    Console.WriteLine("  ╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '2')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("╔═╝");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '3')
                {
                    Console.WriteLine("══╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("══╣");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("══╝");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '4')
                {
                    Console.WriteLine("   ");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '5')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("╚═╗");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '6')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("╠═╗");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '7')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '8')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("╠═╣");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22 + 1);
                }
                if (text[0] == '9')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18 + 1);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 19 + 1);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(12, 20 + 1);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21 + 1);
                    Console.WriteLine("╚═╝");

                    Console.SetCursorPosition(12, 22 + 1);
                }



            }


            Console.ResetColor();

        }



        static void Write(string text, int row, int col, ConsoleColor color = ConsoleColor.Red)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(row, col);
            Console.Write(text);
            Console.ResetColor();
        }
        
    }
}


