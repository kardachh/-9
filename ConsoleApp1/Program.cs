using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;


class Point
{
    public int x;
    public int y;
    public char sym;
    public Point() { }
    public Point(Point p)
    {
        x = p.x;
        y = p.y;
        sym = p.sym;
    }
    public Point(int x, int y, char sym)
    {
        this.x = x;
        this.y = y;
        this.sym = sym;
    }
    public void Draw()
    {
        Console.SetCursorPosition(x, y);
        Console.Write(sym);
    }
    public void Move(int offset, Direction direction)
    {
        if (direction == Direction.RIGHT)
            x = x + offset;
        else if (direction == Direction.LEFT)
            x = x - offset;
        else if (direction == Direction.UP)
            y = y - offset;
        else if (direction == Direction.DOWN)
            y = y + offset;

    }
    public void Clear()
    {
        sym = ' ';
        Draw();
    }
    public bool IsHit(Point p)
    {
        return p.x == this.x && p.y == this.y;
    }
}
class HorizontalLine : Figure
{
    public HorizontalLine(int xLeft, int xRight, int y, char sym)
    {
        pList = new List<Point>();
        for (int x=xLeft; x <= xRight; x++)
        {
            Point p = new Point(x, y, sym);
            pList.Add(new Point(x, y, sym));
        }
    }
}
class VerticaleLine : Figure
{
    public VerticaleLine(int x, int yUp, int yDown, char sym)
    {
        pList = new List<Point>();
        for (int y = yUp; y <= yDown; y++)
        {
            Point p = new Point(x, y, sym);
            pList.Add(new Point(x, y, sym));
        }
    }
}
class Figure
{
    protected List<Point> pList;
    public void Drow()
    {
        foreach(Point p in pList) { p.Draw(); }
    }
}
class Square : Figure
{
    public Square(int xUpLeft, int yUpLeft, int xDownRight,int yDownRight, char Sym)
    {
        pList = new List<Point>();
        for (int x = xUpLeft; x <= xDownRight; x++)
        {
            pList.Add(new Point(x, yUpLeft, Sym));
            pList.Add(new Point(x, yDownRight, Sym));
        }
        for (int y = yUpLeft;y <= yDownRight; y++)
        {
            pList.Add(new Point(xUpLeft, y, Sym));
            pList.Add(new Point(xDownRight, y, Sym));
        }
    }
}
enum Direction
{
    LEFT,
    RIGHT,
    UP,
    DOWN,
}
class Snake : Figure
{
    public Direction direction = new Direction();
    public Snake(Point tail, int length,Direction direction)
    {
        pList = new List<Point>();
        for (int i = 0; i < length; i++)
        {
            Point p = new Point(tail);
            p.Move(i, direction);
            pList.Add(p);
        }
    }
    public void HandleKey(ConsoleKey key)
    {
        if (key == ConsoleKey.LeftArrow) direction = Direction.LEFT;
        else if (key == ConsoleKey.RightArrow) direction = Direction.RIGHT;
        else if (key == ConsoleKey.UpArrow) direction = Direction.UP;
        else if (key == ConsoleKey.DownArrow) direction = Direction.DOWN;
    }
    internal void Move()
    {
        Point tail = pList.First();
        pList.Remove(tail);
        Point head = GetNextPoint();
        if (head.x < 1 || head.x > 77 || head.y < 1 || head.y > 33)
        {
            
        }
        pList.Add(head);
        tail.Clear();
        head.Draw();
    }
    public Point GetNextPoint()
    {
        Point head = pList.Last();
        Point nextPoint = new Point(head);
        nextPoint.Move(1, direction);
        return nextPoint;
    }
    internal bool Eat(Point food)
    {
        Point head = GetNextPoint();
        if (head.IsHit(food))
        {
            food.sym = head.sym;
            pList.Add(food);
            return true;
        }
        else return false;
    }
    
}
class FoodCreator
{
    int mapWidht;
    int mapHeight;
    char sym;
    Random random = new Random();
    public FoodCreator(int mapWidht1, int mapHeight1, char sym)
    {
        this.mapWidht = mapWidht1;
        this.mapHeight = mapHeight1;
        this.sym = sym;
    }
    public Point CreateFood()
    {
        int x = random.Next(2, mapWidht - 2);
        int y = random.Next(2, mapHeight - 2);
        return (new Point(x, y, sym));
    }
}
class Program
{
    static void Main()
    {
        Console.SetBufferSize(80, 40);

        Square sq = new Square(0, 0, 78, 34, '#');
        sq.Drow();
        Point p = new Point(4, 5, '*');
        Snake snake = new Snake(p, 2, Direction.RIGHT);
        snake.Drow();
        FoodCreator foodCreator = new FoodCreator(80, 35, '$');
        Point food = foodCreator.CreateFood();
        food.Draw();
        while (true)
        {
            if (snake.Eat(food))
            {
                snake.Drow();
                food = foodCreator.CreateFood();
                food.Draw();
            }
            else { snake.Move(); }
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                snake.HandleKey(key.Key);
            }
            Thread.Sleep(100);
        }
    }
}
