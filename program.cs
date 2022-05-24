using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01._Basic_Stack_Operations
{

    internal class Program

    {
        static void Main(string[] args)
        {
            int[] rowsAndCols = Console.ReadLine()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToArray();
            int row = rowsAndCols[0];
            int col = rowsAndCols[1];
            char[,] field = new char[row, col];
            int startRow = 0;
            int startCol = 0;
            for (int i = 0; i < field.GetLength(0); i++)
            {
                string input = Console.ReadLine();
                for (int j = 0; j < field.GetLength(1); j++)
                {
                    field[i, j] = input[j];
                    if (field[i, j] == 'P')
                    {
                        startRow = i;
                        startCol = j;
                    }
                }
            }
            string commands = Console.ReadLine();
            field[startRow, startCol] = '.';

            for (int c = 0; c < commands.Length; c++)
            {
                bool isInside = true;
                char command = commands[c];
                if (command == 'R')
                {
                    if (IsInside(field, startRow, startCol + 1))
                    {
                        startCol++;

                    }
                    else
                    {
                        isInside = false;
                    }
                }
                else if (command == 'L')
                {
                    if (IsInside(field, startRow, startCol - 1))
                    {
                        startCol--;

                    }
                    else
                    {
                        isInside = false;
                    }
                }
                else if (command == 'U')
                {
                    if (IsInside(field, startRow - 1, startCol))
                    {
                        startRow--;
                    }
                    else
                    {
                        isInside = false;
                    }
                }
                else if (command == 'D')
                {
                    if (IsInside(field, startRow + 1, startCol))
                    {
                        startRow++;
                    }
                    else
                    {
                        isInside = false;
                    }
                }
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        char ch = field[i, j];
                        if (ch == 'B')
                        {
                            //Up
                            if (IsInside(field, i - 1, j))
                            {
                                if (field[i - 1, j] != 'b' )
                                {
                                    field[i - 1, j] = 'b';
                                }
                            }
                            //Down
                            if (IsInside(field, i + 1, j))
                            {
                                if (field[i + 1, j] != 'b' )
                                {
                                    field[i + 1, j] = 'b';
                                }
                            }
                            //Left
                            if (IsInside(field, i, j - 1))
                            {
                                if (field[i, j - 1] != 'b' )
                                {
                                    field[i, j - 1] = 'b';
                                }
                            }
                            //Right
                            if (IsInside(field, i, j + 1))
                            {
                                if (field[i, j + 1] != 'b' )
                                {
                                    field[i, j + 1] = 'b';
                                }
                            }

                        }
                    }
                }
                for (int i = 0; i < field.GetLength(0); i++)
                {
                    for (int j = 0; j < field.GetLength(1); j++)
                    {
                        if (field[i, j] == 'b')
                        {
                            field[i, j] = 'B';
                        }
                    }
                }
                if (field[startRow, startCol] == 'B')
                {
                    for (int i = 0; i < field.GetLength(0); i++)
                    {
                        for (int j = 0; j < field.GetLength(1); j++)
                        {
                            Console.Write(field[i, j]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine($"dead: {startRow} {startCol}");
                    return;
                }
                if (!isInside)
                {
                    for (int i = 0; i < field.GetLength(0); i++)
                    {
                        for (int j = 0; j < field.GetLength(1); j++)
                        {
                            Console.Write(field[i, j]);
                        }
                        Console.WriteLine();
                    }
                    Console.WriteLine($"won: {startRow} {startCol}");
                    return;
                }


            }


        }

        private static bool IsInside(char[,] field, int startRow, int startCol)
        {
            return startRow >= 0 && startRow < field.GetLength(0) && startCol >= 0 && startCol < field.GetLength(1);
        }

    }
}

