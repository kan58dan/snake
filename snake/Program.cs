using System;
using System.Threading;

namespace snake
{
    class Program
    {

        static void Main(string[] args)
        {
            ConsoleKeyInfo keyInfo;

            Console.WriteLine("맵 크기 지정하기\n가로 세로");
            String mInput = Console.ReadLine();
            String[] mS = mInput.Split();
            int x = int.Parse(mS[0]); int y = int.Parse(mS[1]);
            Console.WriteLine("가로 x:{0},세로 y:{1}\n", x, y);
            int[,] map = new int[y, x];
            MapView(map, x, y);

            Console.WriteLine("이대로 진행하나요? (Y/N)");
            do
            {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Y)
                {
                    Console.WriteLine("\n게임을 시작합니다");
                    Thread.Sleep(2000);
                    Console.Clear();
                    SnakeGame(map, x, y);

                    break;
                } else if (keyInfo.Key == ConsoleKey.N)
                {
                    Console.WriteLine("\n맵을 재설정합니다");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine("맵 크기 지정하기\n가로 세로");
                    mInput = Console.ReadLine();
                    mS = mInput.Split();
                    x = int.Parse(mS[0]); y = int.Parse(mS[1]);
                    Console.WriteLine("가로 x:{0},세로 y:{1}\n", x, y);
                    map = new int[y, x];
                    MapView(map, x, y);
                    Console.WriteLine("이대로 진행하나요? (Y/N)");
                }
            } while (keyInfo.Key != ConsoleKey.Escape);

            
        }


        static void SnakeGame(int[,] map,int x, int y)
        {
            ConsoleKeyInfo keyInfo;

            Random rand = new Random();
            int snake = 1; int snakeTail = 2;  //뱀,뱀의 꼬리
            int xRnd = rand.Next(x); int yRnd = rand.Next(y);
            int feed = 3; int feedC = 0;    //먹이, 먹이 존재여부
            int score = 0; //뱀의 꼬리 갯수이자 점수
            int count = 0; //이동 횟수
            int die = 0;   //죽음 여부
            int oldX = 0; int oldY = 0;
            int[,] tailPosition = new int[x, y];
            //뱀이 맵 테두리 안쪽으로 생성되게 만듬////
            if (yRnd == 0) yRnd++;                //
            else if(yRnd == y) yRnd--;            //
            if (xRnd == 0) xRnd++;                //
            else if (xRnd == x) xRnd--;           //
            ////////////////////////////////////////
            int xSnake = xRnd; int ySnake = yRnd;
            map[ySnake, xSnake] = snake;

            MapView(map, x, y);
            while (score<25)
            {
                int oldTailY = 0; int oldTailX = 0;

                if (die == 1 || die == 2)
                {
                    Console.WriteLine("게임오버");
                    Thread.Sleep(500);
                    Console.Write("사유 : ");
                    if (die == 1) Console.WriteLine("벽에 부딛혀 죽음");
                    else Console.WriteLine("꼬리에 부딛혀 죽음");
                    Thread.Sleep(2000);
                    break;
                }//죽으면 게임종료
                if(feedC == 0)
                {
                    while (true)
                    {
                        yRnd = rand.Next(y); xRnd = rand.Next(x);
                        if(map[yRnd, xRnd] != snake || map[yRnd, xRnd] != snakeTail) break; //뱀과 뱀꼬리에 좌표찍히면 재배열
                    }
                        feedC = 1;
                    map[yRnd, xRnd] = feed;
                }//먹이 재생성
                if (score > 0)
                {
                    map[oldY, oldX] = snakeTail;
                    oldTailX = oldX;
                    oldTailY = oldY;
                }

                MapView(map,x,y);
                
                Console.WriteLine("\n현재 점수 :{0}점\t{1}번 이동", score,count);
                Console.WriteLine("먹이 위치{0},{1} 현재 뱀 위치{2},{3}",yRnd,xRnd,ySnake,xSnake);
                keyInfo = Console.ReadKey();
                Console.Clear();

               
                switch (keyInfo.Key)    //이동 상호작용
                {
                    case ConsoleKey.W:
                        oldX = xSnake; oldY = ySnake;
                        map[ySnake, xSnake] = 0;
                        ySnake -= 1;    
                        if (ySnake < 0 || map[ySnake, xSnake] == snakeTail) //뱀이 배열을 넘어가거나 꼬리 밟으면 죽음
                        {
                            if (map[ySnake, xSnake] == snakeTail) die = 2;
                            else die = 1;
                            break;
                        }
                        
                        if (map[ySnake, xSnake] == feed)
                        {
                            feedC = 0;
                            score++;
                        }
                        map[ySnake, xSnake] = snake;

                        break;

                    case ConsoleKey.S:
                        oldX = xSnake; oldY = ySnake;
                        map[ySnake, xSnake] = 0;
                        ySnake += 1;
                        if (ySnake >= y || map[ySnake, xSnake] == snakeTail)
                        {
                            if (map[ySnake, xSnake] == snakeTail) die = 2;
                            else die = 1;
                            break;
                        }

                        if (map[ySnake, xSnake] == feed)
                        {
                            feedC = 0;
                            score++;
                        }
                        map[ySnake, xSnake] = snake;
                        break;

                    case ConsoleKey.A:
                        oldX = xSnake; oldY = ySnake;
                        map[ySnake, xSnake] = 0;
                        xSnake -= 1;
                        if (xSnake < 0 || map[ySnake, xSnake] == snakeTail)
                        {
                            if (map[ySnake, xSnake] == snakeTail) die = 2;
                            else die = 1;
                            break;
                        }

                        if (map[ySnake, xSnake] == feed)
                        {
                            feedC = 0;
                            score++;
                        }
                        map[ySnake, xSnake] = snake;
                        break;

                    case ConsoleKey.D:
                        oldX = xSnake; oldY = ySnake;
                        map[ySnake, xSnake] = 0;
                        xSnake += 1;
                        if (xSnake >= x || map[ySnake, xSnake] == snakeTail)
                        {
                            if (map[ySnake, xSnake] == snakeTail) die = 2;
                            else die = 1;
                            break;
                        }

                        if (map[ySnake, xSnake] == feed)
                        {
                            feedC = 0;
                            score++;
                        }
                        map[ySnake, xSnake] = snake;
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        Console.WriteLine("\nㅇ게임을 종료합니다.");
                        return;
                }

                if(score > 0)map[oldTailY, oldTailX] = 0;
                count++;
            }
            Console.Clear();
            if(score > 24)Console.WriteLine("축하합니다 당신은{0}번 움직여서 성공했습니다!!",count);
        }



        static void MapView(int[,] map, int x, int y)
        {
            
            for (int i = 0; i < y; i++)
            {
                Console.SetCursorPosition(5, i+5);
                for (int j = 0; j < x; j++)
                {
                    Console.Write(map[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
