using GameConsole.Data;
using GameConsole.Data.Entities;

class Snake : Game
{
    private string _name;
    private int[,] box;
    private int _size;
    private int y;
    private int x;
    private int length;
    private bool noPower;
    private string direction;
    private Queue<int> queueY;
    private Queue<int> queueX;
    private SnakeDbContext db;

    public Snake(string name)
    {
        _size = 10;
        _name = name;
        box = new int[_size, _size];
        y = _size / 2;
        x = _size / 2;
        length = 3;
        noPower = true;
        score = 0;
        direction = "right";
        queueY = new Queue<int>();
        queueX = new Queue<int>();
        db = new SnakeDbContext();
    }
    private void Add()
    {
        Random random = new Random();
        int result = random.Next(_size * _size);
        while (length < _size * _size)
        {
            if (box[result / _size, result % _size] == 0)
            {
                box[result / _size, result % _size] = 2;
                break;
            }
            result = random.Next(_size * _size);
        }
    }
    private void YouLostChecker(int y, int x)
    {
        try
        {
            if (box[y, x] == 1)
            {
                DbUpdate();
                Console.WriteLine("\n\nYou lost");
                Thread.Sleep(5000);
                return;
            }
        }
        catch (IndexOutOfRangeException)
        {
            DbUpdate();
            Console.WriteLine("\n\nYou lost");
            Thread.Sleep(2000);
            Restart();
            return;
        }

    }
    private void QueueAdd(int y, int x)
    {
        queueY.Enqueue(y);
        queueX.Enqueue(x);
        box[y, x] = 1;
    }
    private void Clear()
    {
        if (queueY.Count > length)
        {
            box[queueY.Dequeue(), queueX.Dequeue()] = 0;
        }
    }
    private void YouWinChecker()
    {
        if (score == _size * _size - length)
        {
            Console.Clear();
            
            DbUpdate();
            Console.WriteLine("You win");
            Thread.Sleep(5000);
            return;
        }
        else if (noPower)
        {
            Add();
            noPower = false;
        }
        else
        {
            Clear();
        }
    }
    private void PowerUpChecker()
    {
        if (box[y, x] == 2)
        {
            score++;
            length++;
            noPower = true;
        }
    }
    private void PrintBoard()
    {
        Console.WriteLine("SNAKE GAME" + (new string(' ', _size * 2 - 14) + "BEST SCORE: " + bestScore));
        Console.WriteLine((new string(' ', _size * 2 - 4) + "SCORE: " + score));
        Console.WriteLine("|" + new string('-', _size * 2 + 1) + "|");
        for (int i = 0; i < _size; i++)
        {
            Console.Write("| ");
            for (int j = 0; j < _size; j++)
            {
                if (box[i,j] == 0)
                {
                    Console.Write("  ");
                }
                else if (box[i,j] == 1)
                {
                    Console.Write("X ");
                }
                else
                {
                    Console.Write("O ");
                }
            }
            Console.WriteLine("|");
        }
        Console.Write("|" + new string('-', _size * 2 + 1) + "|");
    }
    private void DbUpdate()
    {
        bestScore = Math.Max(bestScore, score);
        if (_name != "adm")
        {
            db.Add(new SnakeTable { Name = _name, Score = score, DateStarted = DateTime.Now });
            db.SaveChanges();
        }
    }
    private void AutoMove()
    {
        switch (direction)
        {
            case "up":
                --y;
                break;
            case "right":
                ++x;
                break;
            case "down":
                ++y;
                break;
            case "left":
                --x;
                break;
        }
    }
    private void PrintMenu()
    {
        Console.WriteLine("\n\nPress UP ARROW to swipe up\n" +
                                  "Press RIGHT ARROW to swipe right\n" +
                                  "Press DOWN ARROW to swipe down\n" +
                                  "Press LEFT ARROW to swipe left\n\n" +
                                  "Press R to restart the game\n" +
                                  "Press ESC to exit\n");
    }
    private void Decline(string forbidden, string current, ref int point)
    {
        if (direction != forbidden)
        {
            --point;
            direction = current;
        }
        else
        {
            ++point;
        }
    }
    private void Increase(string forbidden, string current, ref int point)
    {
        if (direction != forbidden)
        {
            ++point;
            direction = current;
        }
        else
        {
            --point;
        }
    }
    private void Restart()
    {
        box = new int[_size, _size];
        y = 5;
        x = 5;
        length = 3;
        noPower = true;
        score = 0;
        direction = "right";
        queueY = new Queue<int>();
        queueX = new Queue<int>();
    }
    private ConsoleKeyInfo Timeout(int timeOutMS)
    {
        DateTime timeoutvalue = DateTime.Now.AddMilliseconds(timeOutMS);
        while (DateTime.Now < timeoutvalue)
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo cki = Console.ReadKey();
                return cki;
            }
            Thread.Sleep(100);
        }
        return new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
    }
    public override void Start()
    {
        while (true)
        {
            Console.Title = "Snake";
            YouWinChecker();
            PrintBoard();
            PrintMenu();
            var key = Timeout(500);
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    Decline("down", "up", ref y);
                    break;
                case ConsoleKey.RightArrow:
                    Increase("left", "right", ref x);
                    break;
                case ConsoleKey.DownArrow:
                    Increase("up", "down", ref y);
                    break;
                case ConsoleKey.LeftArrow:
                    Decline("right", "left", ref x);
                    break;
                case ConsoleKey.R:
                    DbUpdate();
                    Restart();
                    break;
                case ConsoleKey.Escape:
                    DbUpdate();
                    Console.Clear();
                    return;
                default:
                    AutoMove();
                    break;
            }
            YouLostChecker(y, x);
            PowerUpChecker();
            QueueAdd(y, x);
            Console.Clear();
        }
    }
}