using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Snake
{
   

    class Program
    {
        // скорость (не может быть меньше 21!!)
        public static int sleep = 66;

        public static int trying = 0;

        // цвета для тела змейки, границы, текста, еды
        public static ConsoleColor snakeCol = ConsoleColor.Red;
        public static ConsoleColor borderCol = ConsoleColor.Magenta;
        public static ConsoleColor textCol = ConsoleColor.White;
        public static ConsoleColor smallFoodCol = ConsoleColor.Yellow;
        public static ConsoleColor bigFoodCol = ConsoleColor.Green;

        public static int bigFoodWaiting = 7500; // количество миллисекунд, когда активна большая еда

        // локальный рекорд по очкам
        public static int localRecord = 0;

        static void Main(string[] args)
        {            

            Console.Title = "Zmeuga";            

            ConsoleKeyInfo keyinfo;
            SnakeBody snake;
            bool goodGaming = true; // змейка не ест саму себя, не заходит за пределы границы

        start:

            
            sleep = 66;
            ScreenParam();
            snake = new SnakeBody();
            snake.zero();
            goodGaming = true;
            snake.SetSmallFood();
            trying++;
            SetTry();

        pause:
            do
            {
            

                keyinfo = Console.ReadKey(true);
                if (keyinfo.Key == ConsoleKey.UpArrow)
                {
                    do
                    {
                        Thread.Sleep(Program.sleep);

                        goodGaming = snake.Move(keyinfo);
                    } while (Console.KeyAvailable == false && goodGaming);
                }
                if (keyinfo.Key == ConsoleKey.DownArrow)
                {
                    do
                    {
                        Thread.Sleep(sleep);

                        goodGaming = snake.Move(keyinfo);
                    } while (Console.KeyAvailable == false && goodGaming);
                }
                if (keyinfo.Key == ConsoleKey.RightArrow)
                {
                    do
                    {
                        Thread.Sleep(sleep - 15);

                        goodGaming = snake.Move(keyinfo);
                    } while (Console.KeyAvailable == false && goodGaming);
                }
                if (keyinfo.Key == ConsoleKey.LeftArrow)
                {
                    do
                    {
                        Thread.Sleep(sleep - 15);

                        goodGaming = snake.Move(keyinfo);
                    } while (Console.KeyAvailable == false && goodGaming);
                }


            } while (keyinfo.Key != ConsoleKey.Spacebar && goodGaming);

            if(keyinfo.Key == ConsoleKey.Spacebar)
            {
                goto pause;
            }

            Thread.Sleep(1000);

            Console.Clear();
            Console.SetCursorPosition(1, 1);
            Console.ForegroundColor = textCol;
            Console.Write("Вы проиграли. Финальный счет: " + snake.GetPoints());
            Thread.Sleep(1000);


            Console.ReadKey(true);
            Console.Clear();

            goto start;
        }

        public static void SetTry()
        {
            Console.ForegroundColor = textCol;
            Console.SetCursorPosition(132, 5);
            Console.Write("Попытка: " + trying);
            Console.ForegroundColor = snakeCol;

        }
        public static void ScreenParam()
        {
            char sym = '0';
            
            Console.CursorVisible = false;
            Console.SetWindowSize(150, 45);
            Console.SetBufferSize(150, 45);

            Console.ForegroundColor = borderCol;
            int
                w = Console.WindowWidth - 20,
                h = Console.WindowHeight;

            sym = '═';
            for (int i = 0; i < w; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write(sym);
            }

            
            for (int i = 0; i < w; i++)
            {
                Console.SetCursorPosition(i, h - 2);
                Console.Write(sym);
            }

            sym = '║';
            for (int i = 0; i < h - 1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(sym);
            }

            for (int i = 0; i < h - 1; i++)
            {
                Console.SetCursorPosition(w - 1, i);
                Console.Write(sym);
            }

            Console.SetCursorPosition(0, 0);
            Console.Write('╔');

            Console.SetCursorPosition(w-1, 0);
            Console.Write('╗');

            Console.SetCursorPosition(w-1, h-2);
            Console.Write('╝');

            Console.SetCursorPosition(0, h-2);
            Console.Write('╚');

            Console.ForegroundColor = textCol;
            Console.SetCursorPosition(132, 7);
            Console.Write("Пробел");
            Console.SetCursorPosition(132, 8);
            Console.Write("(для паузы)");

            SetRecord();            
        }

        public static void SetMyPoint(int count)
        {
            Console.ForegroundColor = textCol;
            Console.SetCursorPosition(132, 3);
            Console.Write(count);

            if(count>localRecord)
            {
                localRecord = count;
                SetRecord();
            }
            Console.ForegroundColor = snakeCol;
        }

        public static void SetRecord()
        {
            Console.ForegroundColor = textCol;
            Console.SetCursorPosition(132, 0);
            Console.Write("Pекорд:");
            Console.SetCursorPosition(132, 1);
            Console.Write(localRecord);
            Console.ForegroundColor = snakeCol;
        }

        public static void MoreSpeed()
        {
            if (sleep > 15) sleep -= 10;
        }

    }
}
