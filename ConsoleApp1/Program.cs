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
    public void Draw()
    {
        foreach (Point p in pList)
            p.Draw();
    }
    internal bool IsHit(Figure figure)
    {
        foreach (var p in pList)
        {
            if (figure.IsHit(p))
                return true;
        }
        return false;
    }
    private bool IsHit(Point point)
    {
        foreach (var p in pList)
        {
            if (p.IsHit(point))
                return true;
        }
        return false;
    }
}
class Walls
{
    List<Figure> wallList;

    public Walls(int mapWidth, int mapHeight)
    {
        wallList = new List<Figure>();

        HorizontalLine upLine = new HorizontalLine(0, 78, 0, '░');
        HorizontalLine downLine = new HorizontalLine(0, 78, 34, '░');
        VerticaleLine leftLine = new VerticaleLine(0, 0, 34, '░');
        VerticaleLine rightLine = new VerticaleLine(78, 0, 34, '░');

        wallList.Add(upLine);
        wallList.Add(downLine);
        wallList.Add(leftLine);
        wallList.Add(rightLine);
    }

    internal bool IsHit(Figure figure)
    {
        foreach (var wall in wallList)
        {
            if (wall.IsHit(figure))
            {
                return true;
            }
        }
        return false;
    }

    public void Draw()
    {
        foreach (var wall in wallList)
        {
            wall.Draw();
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
        if (key == ConsoleKey.LeftArrow && direction != Direction.RIGHT) direction = Direction.LEFT;
        else if (key == ConsoleKey.RightArrow && direction != Direction.LEFT) direction = Direction.RIGHT;
        else if (key == ConsoleKey.UpArrow && direction != Direction.DOWN) direction = Direction.UP;
        else if (key == ConsoleKey.DownArrow && direction != Direction.UP) direction = Direction.DOWN;
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
class Barrier
{
    Random rnd = new Random();
    List<Figure> figures;
    public Barrier(int mapWidht, int mapHeight)
    {
        figures = new List<Figure>();
        int k = rnd.Next(5, 10);
        for (int i = 0; i < k; i++)
        {
            int x = rnd.Next(10, mapWidht - 10);
            int y1 = rnd.Next(10, mapHeight - 10);
            int y2 = rnd.Next(10, mapHeight - 10);
            VerticaleLine randomVLine = new VerticaleLine(x, y1, y2, '░');
            figures.Add(randomVLine);
        }
    }
    internal bool IsHit(Figure figure)
    {
        foreach (var f in figures)
        {
            if (f.IsHit(figure))
            {
                return true;
            }
        }
        return false;
    }
    public void Draw()
    {
        foreach (var f in figures)
        {
            f.Draw();
        }
    }

}
class Program
{
    static void Main()
    {
        Console.SetBufferSize(80, 35);
        Console.SetWindowSize(80, 35);
        Thread.Sleep(3000);
        string start = "Игра начнется через: ";
        Console.WriteLine(start);
        Thread.Sleep(1000);
        Console.Write("...3");
        Thread.Sleep(1000);
        Console.Write("...2");
        Thread.Sleep(1000);
        Console.Write("...1");
        Thread.Sleep(1000);
        Console.Clear();
        Walls walls = new Walls(80, 35);
        walls.Draw();
        Barrier barrier = new Barrier(80, 35);
        barrier.Draw();
        Point p = new Point(30, 20, '*');
        Snake snake = new Snake(p, 2, Direction.LEFT);
        snake.Draw();
        FoodCreator foodCreator = new FoodCreator(80, 35, '$');
        Point food = foodCreator.CreateFood();
        food.Draw();
        while (true)
        {
            if (walls.IsHit(snake) || barrier.IsHit(snake))
            {
                Console.Clear();
                Console.WriteLine("Игра окончена.\n\nНажмите Enter, чтобы выйти...");
                Console.ReadLine();
                break;
            }
            if (snake.Eat(food))
            {
                snake.Draw();
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
