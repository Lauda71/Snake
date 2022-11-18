using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snake
{
    class SnakeBody
    {
        static int[]
            // координаты головы змейки
            body_cells = { 65, 23 },
            // x   y

            // координаты маленькой еды
            smallFood = { 0, 0 },

            // координаты удаления
            delete_track = { 65, 23 },
        
            // координаты большой еды
            bigFood = { 0, 0 };
            
        public void zero()
        {
            Array.Resize(ref body_cells, 2);
            body_cells[0] = 65;
            body_cells[1] = 23;
        }

        int
            bigFootCount = 5,   // для отсчета появения большой еды
            deleteCount = 0;    // для удаления последнего элемента цепочки змейки

        // для задания новых координат для маленькой и большой еды
        Random ran = new Random();

        char
                snakeSym = '@',     // элемент цепочки змейки
                smallFoodSym = '■', // элемент маленькой еды
                bigFoodSym = '█';   // элемент большой еды

        public int GetPoints()
        {
            return body_cells.Length / 2;
        }

        public bool Move(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                if (body_cells.Length > 2)
                {
                    ChanginCoordinates();
                }
                body_cells[1] = body_cells[1] - 1;

                return NextStep(keyInfo);
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                if (body_cells.Length > 2)
                {
                    ChanginCoordinates();
                }
                body_cells[1] = body_cells[1] + 1;

                return NextStep(keyInfo);
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                if (body_cells.Length > 2)
                {
                    ChanginCoordinates();
                }
                body_cells[0] = body_cells[0] + 1;

                return NextStep(keyInfo);
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                if (body_cells.Length > 2)
                {
                    ChanginCoordinates();
                }
                body_cells[0] = body_cells[0] - 1;

                return NextStep(keyInfo);
            }
            else return true;
        }

        public void ChanginCoordinates()
        {
            for (int i = body_cells.Length - 1; i >= 3; i -= 2)
            {
                body_cells[i] = body_cells[i - 2];
                body_cells[i - 1] = body_cells[i - 3];
            }
        }

        public bool NextStep(ConsoleKeyInfo keyInfo)
        {
            if (Check()) // если не касается границ и своего тела
            {
                if (CheckSmallFood())
                {
                    Array.Resize(ref body_cells, body_cells.Length + 2);    // создание двух новый элементов массива (для х- и у-координат) под новое звено тела змейки
                    Program.SetMyPoint(body_cells.Length / 2);

                    if ((body_cells.Length / 2) % 10 == 0) Program.MoreSpeed();

                    if (keyInfo.Key == ConsoleKey.UpArrow)
                    {
                        // x
                        body_cells[body_cells.Length - 2] = body_cells[body_cells.Length - 4] + 1;

                        // y
                        body_cells[body_cells.Length - 1] = body_cells[body_cells.Length - 3];
                    }
                    else if (keyInfo.Key == ConsoleKey.DownArrow)
                    {
                        // x
                        body_cells[body_cells.Length - 2] = body_cells[body_cells.Length - 4] - 1;

                        // y
                        body_cells[body_cells.Length - 1] = body_cells[body_cells.Length - 3];
                    }
                    else if (keyInfo.Key == ConsoleKey.RightArrow)
                    {
                        // x
                        body_cells[body_cells.Length - 2] = body_cells[body_cells.Length - 4];

                        // y
                        body_cells[body_cells.Length - 1] = body_cells[body_cells.Length - 3] - 1;
                    }
                    else if (keyInfo.Key == ConsoleKey.LeftArrow)
                    {
                        // x
                        body_cells[body_cells.Length - 2] = body_cells[body_cells.Length - 4];

                        // y
                        body_cells[body_cells.Length - 1] = body_cells[body_cells.Length - 3] + 1;
                    }

                    deleteCount++;
                    SetSmallFood(); // появление маленькой еды в новом месте
                    bigFootCount--;
                    //Program.SetMyPoint(bigFootCount);

                    if (bigFootCount == 0)
                    {
                        bigFootCount = ran.Next(5, 10); // задание счетчика на появление большой еды

                        SetBigFood();    // окончание счетчика приводит к появлению большой еды
                    }

                }
                else if (CheckBigFood())
                {
                    Array.Resize(ref body_cells, body_cells.Length + 12);
                    Program.SetMyPoint(body_cells.Length / 2);

                    ChanginCoordinates();
                    deleteCount += 5;
                }

                // прорисовка нового головного элемента цепочки змейки на новых координатах
                Console.SetCursorPosition(body_cells[0], body_cells[1]);

                Console.Write(snakeSym);

                // удаление последнего элемента змейки
                if (deleteCount == 0)
                {
                    Console.SetCursorPosition(delete_track[0], delete_track[1]);
                    Console.Write(" ");
                    delete_track[0] = body_cells[body_cells.Length - 2];
                    delete_track[1] = body_cells[body_cells.Length - 1];
                }
                else
                {
                    deleteCount--;
                }

                return true;
            }
            else return false;  // задета граница или элемент тела змейки
        }


        public bool Check() // провекра на нарушение границы
        {
            //if(BigFood.ThreadState == ThreadState.)

            bool ret = true;
            // проверка пройдена, граница не задета
            if (body_cells[0] != 0 && body_cells[0] != 129 && body_cells[1] != 0 && body_cells[1] != 43)
            {
                for (int i = 2; i < body_cells.Length - 1; i += 2)
                {
                    if (body_cells[0] != body_cells[i] || body_cells[1] != body_cells[i + 1]) { }
                    else ret = false;
                }

                return ret;
            }



            // проверка не пройдена, граница задета
            else return false;
        }

        public bool CheckSmallFood() // проверка на поедание маленькой еды
        {
            // маленькая еда съедена
            if (body_cells[0] == smallFood[0] && body_cells[1] == smallFood[1]) return true;

            // маленькая еда не съедена
            else return false;
        }

        public bool CheckBigFood() // проверка на поедание маленькой еды
        {
            // большая еда съедена
            if (body_cells[0] == bigFood[0] && body_cells[1] == bigFood[1]) return true;

            // большая еда не съедена
            else return false;
        }

        public void SetSmallFood()
        {
            bool setCurrect = true;
            do
            {
                setCurrect = true;
                // задание новых координат для маленькой еды
                smallFood[0] = ran.Next(1, 129);
                smallFood[1] = ran.Next(1, 43);



                // проверка на совпадение с координатами тела змейки
                for (int i = 0; i < body_cells.Length; i += 2)
                {
                    if (smallFood[0] != body_cells[i] && smallFood[1] != body_cells[i + 1]) { }
                    else
                        setCurrect = false;
                }


                // проверка на совпадение с координатами большой еды
                if (smallFood[0] != bigFood[0] && smallFood[1] != bigFood[1])
                {
                }
                else
                {
                    setCurrect = false;
                }




            } while (setCurrect == false);


            Console.ForegroundColor = Program.smallFoodCol;
            Console.SetCursorPosition(smallFood[0], smallFood[1]);
            Console.Write(smallFoodSym);
            Console.ForegroundColor = Program.snakeCol;
        }

        [Obsolete]
        public void SetBigFood()
        {
            bool setCurrect = true;
            do
            {
                setCurrect = true;
                // задание новых координат для маленькой еды
                bigFood[0] = ran.Next(1, 129);
                bigFood[1] = ran.Next(1, 43);

                // проверка на совпадение с координатами тела змейки
                for (int i = 0; i < body_cells.Length; i += 2)
                {
                    if (bigFood[0] != body_cells[i] && bigFood[1] != body_cells[i + 1])
                    { }
                    else
                        setCurrect = false;

                }

                // проверка на совпадение с координатами большой еды
                if (smallFood[0] != bigFood[0] && smallFood[1] != bigFood[1])
                { }
                else
                    setCurrect = false;

            } while (setCurrect == false);



            Thread BigFood = new Thread(new ThreadStart(BigFoodBar));
            BigFood.IsBackground = true;
            BigFood.Start();



            Console.ForegroundColor = Program.bigFoodCol;
            Console.SetCursorPosition(bigFood[0], bigFood[1]);
            Console.Write(bigFoodSym);
            Console.ForegroundColor = Program.snakeCol;
        }
                
        public static void BigFoodBar()
        {
            int snakePoints = body_cells.Length;

            Thread.Sleep(Program.bigFoodWaiting);

            if(snakePoints == body_cells.Length || (snakePoints + 2) == body_cells.Length )
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(bigFood[0], bigFood[1]);
                Console.Write(" ");
                bigFood[0] = 0; bigFood[1] = 0;
                Console.ForegroundColor = Program.snakeCol;
            }
            
        }

    }
}
