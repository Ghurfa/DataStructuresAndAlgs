using System;
using System.Diagnostics;
using System.Threading;

namespace SinglyLinkedSnake
{
    class Program
    {
        public class SnakeBodyNode
        {
            public SnakeBodyNode Next;
            public int XPosition;
            public int YPosition;
            public int Direction;
            public int NextDirection;

            public SnakeBodyNode(int xPosition, int yPosition, int direction, SnakeBodyNode nextNode = null)
            {
                XPosition = xPosition;
                YPosition = yPosition;
                Direction = direction;
                NextDirection = direction;
                Next = nextNode;
            }
        }
        public class Snake
        {
            public SnakeBodyNode HeadNode;
            public bool IsDead;
            public Snake()
            {
                ResetSnake();
            }
            public void ResetSnake()
            {
                Clear();
                for (int i = 0; i < 3; i++)
                {
                    AddToEnd();
                }
                IsDead = false;
            }
            public void AddToEnd()
            {
                SnakeBodyNode lastNode = HeadNode;
                if (lastNode != null)
                {
                    while (lastNode.Next != null)
                    {
                        lastNode = lastNode.Next;
                    }
                    int xSpeed = 0;
                    int ySpeed = 0;
                    switch (lastNode.Direction)
                    {
                        case 0:
                            xSpeed = 0;
                            ySpeed = -1;
                            break;
                        case 1:
                            xSpeed = 1;
                            ySpeed = 0;
                            break;
                        case 2:
                            xSpeed = 0;
                            ySpeed = 1;
                            break;
                        case 3:
                            xSpeed = -1;
                            ySpeed = 0;
                            break;
                    }
                    lastNode.Next = new SnakeBodyNode(lastNode.XPosition - xSpeed, lastNode.YPosition - ySpeed, lastNode.Direction);
                }
                else
                {
                    Random random = new Random();
                    int xPosition = random.Next(8, 13);
                    int yPosition = random.Next(8, 13);
                    int direction = random.Next(4);
                    HeadNode = new SnakeBodyNode(xPosition, yPosition, direction);
                }
                Length++;
            }
            public bool IsEmpty() => Length <= 0;
            public void Clear()
            {
                HeadNode = null;
                Length = 0;
            }
            public int Length;
        }
        public class Apple
        {
            public int XPosition;
            public int YPosition;

            public Apple()
            {
                Random random = new Random();
                XPosition = random.Next(0, 20);
                YPosition = random.Next(0, 20);
            }
        }
        static void Main(string[] args)
        {
            void checkForArrowKey(ref Snake snakeToMove)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo input = Console.ReadKey(true);
                    int newDirection = 0;
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            newDirection = 0;
                            break;
                        case ConsoleKey.RightArrow:
                            newDirection = 1;
                            break;
                        case ConsoleKey.DownArrow:
                            newDirection = 2;
                            break;
                        case ConsoleKey.LeftArrow:
                            newDirection = 3;
                            break;
                    }
                    if (Math.Abs(newDirection - snakeToMove.HeadNode.Direction) != 2)
                    {
                        snakeToMove.HeadNode.Direction = newDirection;
                        snakeToMove.HeadNode.NextDirection = snakeToMove.HeadNode.Direction;
                    }
                }
            }
            int wrap(int value, int minValue, int maxValue)
            {
                if (value < minValue)
                {
                    return maxValue;
                }
                else if (value > maxValue)
                {
                    return minValue;
                }
                else
                {
                    return value;
                }
            }
            void moveStep(ref Snake snakeToMove)
            {
                int xSpeed = 0;
                int ySpeed = 0;
                SnakeBodyNode currentNode = snakeToMove.HeadNode;
                for (int i = 0; i < snakeToMove.Length; i++)
                {
                    switch (currentNode.Direction)
                    {
                        case 0:
                            xSpeed = 0;
                            ySpeed = -1;
                            break;
                        case 1:
                            xSpeed = 1;
                            ySpeed = 0;
                            break;
                        case 2:
                            xSpeed = 0;
                            ySpeed = 1;
                            break;
                        case 3:
                            xSpeed = -1;
                            ySpeed = 0;
                            break;
                    }
                    currentNode.XPosition += xSpeed;
                    currentNode.YPosition += ySpeed;
                    if (currentNode.Next != null)
                    {
                        currentNode.Next.NextDirection = currentNode.Direction;
                    }
                    currentNode.Direction = currentNode.NextDirection;
                    currentNode.XPosition = wrap(currentNode.XPosition, 0, 19);
                    currentNode.YPosition = wrap(currentNode.YPosition, 0, 19);
                    currentNode = currentNode.Next;
                }
            }
            void checkIfEatenApple(ref Snake snakeToCheck, ref Apple appleToEat)
            {
                if (snakeToCheck.HeadNode.XPosition == appleToEat.XPosition && snakeToCheck.HeadNode.YPosition == appleToEat.YPosition)
                {
                    appleToEat = new Apple();
                    snakeToCheck.AddToEnd();
                }
            }
            void drawText(string text, int xPosition, int yPosition, bool writeLine = false, ConsoleColor color = ConsoleColor.White)
            {
                Console.SetCursorPosition(xPosition, yPosition);
                Console.ForegroundColor = color;
                Console.Write(text);
                if (writeLine)
                {
                    Console.WriteLine();
                }
            }
            void drawField()
            {
                drawText(new string('█', 22), 0, 2, true);
                for (int i = 0; i < 20; i++)
                {
                    drawText("█" + new string(' ', 20) + "█", 0, i + 3, true);
                }
                drawText(new string('█', 22), 0, 23);
            }
            void drawSnake(Snake snakeToDraw)
            {
                SnakeBodyNode currentNode = snakeToDraw.HeadNode;
                for (int i = 0; i < snakeToDraw.Length; i++)
                {
                    drawText("S", currentNode.XPosition + 1, currentNode.YPosition + 3);
                    currentNode = currentNode.Next;
                }
            }
            void drawApple(Apple appleToDraw)
            {
                drawText("A", appleToDraw.XPosition + 1, appleToDraw.YPosition + 3, false, ConsoleColor.Red);
            }
            void checkIfIntersecting(ref Snake snakeToCheck)
            {
                int headXPosition = snakeToCheck.HeadNode.XPosition;
                int headYPosition = snakeToCheck.HeadNode.YPosition;
                SnakeBodyNode currentNode = snakeToCheck.HeadNode.Next;
                for (int i = 0; i < snakeToCheck.Length - 1; i++)
                {
                    if (currentNode.XPosition == headXPosition && currentNode.YPosition == headYPosition)
                    {
                        snakeToCheck.IsDead = true;
                    }
                    currentNode = currentNode.Next;
                }
            }
            while (true)
            {
                Snake snake = new Snake();
                Apple apple = new Apple();
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                while (!snake.IsDead)
                {
                    if (stopwatch.ElapsedMilliseconds >= 200)
                    {
                        Console.ForegroundColor = ConsoleColor.White;

                        checkForArrowKey(ref snake);
                        moveStep(ref snake);
                        checkIfEatenApple(ref snake, ref apple);
                        drawField();
                        drawSnake(snake);
                        drawApple(apple);
                        checkIfIntersecting(ref snake);

                        drawText($"Score: {snake.Length}", 0, 0);
                        stopwatch.Restart();
                    }
                }
                Console.ReadKey();
            }
        }
    }
}
