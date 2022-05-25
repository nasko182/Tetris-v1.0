using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01._Basic_Stack_Operations
{

    internal class Program
    {
        //static int NextFigureRows = 6;
        //static int NextFigureCols = 7;
        //static int LevelRows = 7;
        //static int ScoreRows = 2;

        static int LinesRows = 1;
        static int TetrisRows = 20;
        static int TetrisCols = 10;
        static int InfoCols = 7;
        static int ConsoleRows = 1 + LinesRows + 1 + TetrisRows + 1;
        static int ConsoleCols = 1 + TetrisCols + 1 + InfoCols + 1;

        //State
        static int score = 0;
        static int Lines = 0;
        static int Level = Lines / 10;
        static int FrameCounter = 1;
        static int FrameToMoveFigure = 16;
        static bool[,] TetrisField = new bool[TetrisRows, TetrisCols];
        static int currentFigureRow = 0;
        static int currentFigureCol = 0;
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



        static void Main(string[] args)
        {
            {
                Console.Title = "Tetris v1.0";
                Console.WindowHeight = ConsoleRows;
                Console.WindowWidth = ConsoleCols;
                Console.BufferHeight = ConsoleRows + 1;
                Console.BufferWidth = ConsoleCols;
            Console.CursorVisible = false;
            }
            while (true)
            {
                Random random = new Random();
                int CurrentFigureIndex = random.Next(Figures.Count + 1);
                bool[,] CurrentFigure = Figures[3];
                if (FrameCounter % FrameToMoveFigure - Level == 0)
                {
                    if (IsInside(currentFigureRow + 1, currentFigureCol,
                            currentFigureRow + CurrentFigure.GetLength(0) + 1,
                            currentFigureCol + CurrentFigure.GetLength(1)))
                    {
                        currentFigureRow++;
                        FrameCounter = 1;
                        //TODO: 
                    }
                }
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
                        //TODO: Rotate CurrentFigure
                    }
                    if (key.Key == ConsoleKey.LeftArrow ||
                        key.Key == ConsoleKey.A)
                    {
                        //TODO: Move CurrentFigure left
                        if (IsInside(currentFigureRow, currentFigureCol - 1,
                            currentFigureRow + CurrentFigure.GetLength(0) ,
                            currentFigureCol + CurrentFigure.GetLength(1)-1))
                        {
                            currentFigureCol--;
                        }
                    }
                    if (key.Key == ConsoleKey.RightArrow ||
                        key.Key == ConsoleKey.D)
                    {
                        //TODO: Move CurrentFigure Right
                        if (IsInside(currentFigureRow, currentFigureCol + 1,
                            currentFigureRow + CurrentFigure.GetLength(0) ,
                            currentFigureCol + CurrentFigure.GetLength(1)+1))
                        {
                            currentFigureCol++;
                        }
                    }
                    if (key.Key == ConsoleKey.DownArrow ||
                        key.Key == ConsoleKey.S)
                    {
                        //TODO: Move CurrentFigure down
                        if (IsInside(currentFigureRow + 1, currentFigureCol,
                            currentFigureRow + CurrentFigure.GetLength(0) + 1,
                            currentFigureCol + CurrentFigure.GetLength(1)))
                        {
                            FrameCounter = 1;
                            score++;
                            currentFigureRow++;
                        }
                    }

                }


                //Update game state
                FrameCounter++;
                //if (//TODO: Collisium())
                {
                    //TODO: AddCurrentFigureToTetrisField();
                    //TODO: ChekForFullLines();
                    //If (lines removed)
                    //{
                    //Score++; 
                    //}
                }
                
                //Redeaw UI
                DrawBorder();
                DrawInfo();
                //TODO: DrawTetrisField()
                DrawCurrentFigure(CurrentFigure);


                Thread.Sleep(40);
            }

        }

        private static void DrawCurrentFigure(bool[,] figure)
        {
            for (int i = 0; i < figure.GetLength(0); i++)
            {
                for (int j = 0; j < figure.GetLength(1); j++)
                {
                    if (figure[i,j])
                    {
                        Write("▓", i+1+currentFigureCol,j+3+currentFigureRow);
                    }
                }
            }
        }

        private static bool IsInside(int FirstRow, int FirstCol,int LastRow, int LastCol)
        {
            return FirstRow >= 0 && FirstCol >= 0 &&
                FirstRow < TetrisField.GetLength(0) &&
                FirstCol < TetrisField.GetLength(1)&& 
                LastRow >= 0 && LastCol >= 0 &&
                LastRow < TetrisField.GetLength(0)-1 &&
                LastCol < TetrisField.GetLength(1)+2;
        }

        
        static void DrawTetrisField(bool[,] tetrisField)
        {

        }

        private static void DrawInfo()
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
            for (int i = 0; i < 3; i++)
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
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '1')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("  ╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '2')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╔═╝");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '3')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("══╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("══╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("══╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '4')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '5')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╚═╗");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '6')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╠═╗");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '7')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[0] == '8')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╠═╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
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
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '1')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("  ╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '2')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╔═╝");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '3')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("══╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("══╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("══╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '4')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("   ");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '5')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╚═╗");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '6')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╠═╗");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '7')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '8')
                {
                    Console.SetCursorPosition(16, 17);
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(16, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 19);
                    Console.WriteLine("╠═╣");
                    Console.SetCursorPosition(16, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(16, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(16, 22);
                }
                if (text[1] == '9')
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

                Console.SetCursorPosition(12, 17);
                if (text[0] == '1')
                {
                    Console.WriteLine("  ╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '2')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("╔═╝");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '3')
                {
                    Console.WriteLine("══╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("══╣");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("══╝");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '4')
                {
                    Console.WriteLine("   ");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '5')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("╚═╗");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '6')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("║  ");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("╠═╗");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '7')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '8')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("╠═╣");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("╚═╝");
                    Console.SetCursorPosition(12, 22);
                }
                if (text[0] == '9')
                {
                    Console.WriteLine("╔═╗");
                    Console.SetCursorPosition(12, 18);
                    Console.WriteLine("║ ║");
                    Console.SetCursorPosition(12, 19);
                    Console.WriteLine("╚═╣");
                    Console.SetCursorPosition(12, 20);
                    Console.WriteLine("  ║");
                    Console.SetCursorPosition(12, 21);
                    Console.WriteLine("╚═╝");

                    Console.SetCursorPosition(12, 22);
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

