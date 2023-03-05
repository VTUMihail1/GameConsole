using GameConsole.Data;
using GameConsole.Data.Entities;

class TwoZeroFourEight : Game
{
    private string _name;
    private int[,] array;
    private int _size;
    private TZFEDbContext db;

    public TwoZeroFourEight(string name)
    {
        _size = 4;
        array = new int[_size, _size];
        _name = name;
        db = new TZFEDbContext();
    }
    private void YouLostChecker()
    {
        bool gameIsOver = true;
        int[,] copy = array.Clone() as int[,];
        int tempScore = score;
        Left();
        Up();
        Right();
        Down();

        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                if (copy[i, j] != array[i, j])
                {
                    gameIsOver = false;
                    break;
                }
            }
        }
        if (gameIsOver)
        {
            DbUpdate();
            Console.WriteLine("\nYou lost");
            Thread.Sleep(6000);
            Restart();
        }
        array = copy.Clone() as int[,];
        score = tempScore;
    }
    private void Add()
    {
        HashSet<int> set = new HashSet<int>();
        Random random = new Random();
        int result = random.Next(_size * _size);
        while (set.Count != _size * _size)
        {
            if (array[result / _size, result % _size] == 0)
            {
                array[result / _size, result % _size] = 2;
                return;
            }
            set.Add(result);
            result = random.Next(_size * _size);
        }
        YouLostChecker();
    }
    private void Left()
    {
        int currentIndex;
        int currentNumber;
        for (int i = 0; i < _size; i++)
        {
            currentIndex = 0;
            currentNumber = 0;

            for (int j = 0; j < _size; j++)
            {
                if (array[i, j] != 0)
                {
                    if (currentNumber == array[i, j])
                    {
                        array[i, currentIndex++] = currentNumber * 2;
                        score += currentNumber * 2;
                        currentNumber = 0;
                    }
                    else if (currentNumber == 0)
                    {
                        currentNumber = array[i, j];
                    }
                    else
                    {
                        array[i, currentIndex++] = currentNumber;
                        currentNumber = array[i, j];
                    }
                    array[i, j] = 0;
                }
            }
            if (currentNumber != 0)
            {
                array[i, currentIndex] = currentNumber;
            }
        }
    }
    private void Up()
    {
        int currentIndex;
        int currentNumber;

        for (int i = 0; i < _size; i++)
        {
            currentIndex = 0;
            currentNumber = 0;

            for (int j = 0; j < _size; j++)
            {
                if (array[j, i] != 0)
                {
                    if (currentNumber == array[j, i])
                    {
                        array[currentIndex++, i] = currentNumber * 2;
                        score += currentNumber * 2;
                        currentNumber = 0;
                    }
                    else if (currentNumber == 0)
                    {
                        currentNumber = array[j, i];
                    }
                    else
                    {
                        array[currentIndex++, i] = currentNumber;
                        currentNumber = array[j, i];
                    }
                    array[j, i] = 0;
                }
            }
            if (currentNumber != 0)
            {
                array[currentIndex, i] = currentNumber;
            }
        }
    }
    private void Right()
    {
        int currentIndex;
        int currentNumber;
        for (int i = 0; i < _size; i++)
        {
            currentIndex = _size - 1;
            currentNumber = 0;

            for (int j = _size - 1; j >= 0; j--)
            {
                if (array[i, j] != 0)
                {
                    if (currentNumber == array[i, j])
                    {
                        array[i, currentIndex--] = currentNumber * 2;
                        score += currentNumber * 2;
                        currentNumber = 0;
                    }
                    else if (currentNumber == 0)
                    {
                        currentNumber = array[i, j];
                    }
                    else
                    {
                        array[i, currentIndex--] = currentNumber;
                        currentNumber = array[i, j];
                    }
                    array[i, j] = 0;
                }
            }
            if (currentNumber != 0)
            {
                array[i, currentIndex] = currentNumber;
            }
        }
    }
    private void Down()
    {
        int currentIndex;
        int currentNumber;

        for (int i = 0; i < _size; i++)
        {
            currentIndex = _size - 1;
            currentNumber = 0;

            for (int j = _size - 1; j >= 0; j--)
            {
                if (array[j, i] != 0)
                {
                    if (currentNumber == array[j, i])
                    {
                        array[currentIndex--, i] = currentNumber * 2;
                        score += currentNumber * 2;
                        currentNumber = 0;
                    }
                    else if (currentNumber == 0)
                    {
                        currentNumber = array[j, i];
                    }
                    else
                    {
                        array[currentIndex--, i] = currentNumber;
                        currentNumber = array[j, i];
                    }
                    array[j, i] = 0;
                }
            }
            if (currentNumber != 0)
            {
                array[currentIndex, i] = currentNumber;
            }
        }
    }
    private void Restart()
    {
        score = 0;
        Array.Clear(array, 0, _size * _size);
        Add();
    }
    private void DbUpdate()
    {
        bestScore = Math.Max(bestScore, score);
        if (_name != "adm")
        {
            db.Add(new TZFETable { Name = _name, Score = score, DateStarted = DateTime.Now });
            db.SaveChanges();
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
    private void PrintBoard()
    {
        Console.Clear();
        Console.WriteLine("2048 GAME" + (new string(' ', _size * 5 + (_size - 20)) + "BEST SCORE: " + bestScore));
        Console.WriteLine((new string(' ', _size * 5 + (_size - 11)) + "SCORE: " + score));
        for (int i = 0; i < _size; i++)
        {
            Console.WriteLine(new string('-', (_size + 1) * 5 + (_size - 4)));
            Console.Write("|  ");
            for (int j = 0; j < _size; j++)
            {
                Console.Write(array[i, j] + "  |  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine(new string('-', (_size + 1) * 5 + (_size - 4)));
    }
    public override void Start()
    {
        Console.Title = "2048";
        Add();
        Add();
        bool wrongButton = false;
        while (true)
        {
            PrintBoard();
            PrintMenu();
            var key = Console.ReadKey();

            switch (key.Key)
            {
                case ConsoleKey.RightArrow:
                    Right();
                    break;
                case ConsoleKey.LeftArrow:
                    Left();
                    break;
                case ConsoleKey.UpArrow:
                    Up();
                    break;
                case ConsoleKey.DownArrow:
                    Down();
                    break;
                case ConsoleKey.R:
                    DbUpdate();
                    Restart();
                    break;
                case ConsoleKey.Escape:
                    DbUpdate();
                    Console.Clear();
                    Console.WriteLine("TThank you for playing");
                    return;
                default:
                    wrongButton = true;
                    Console.WriteLine("\n\nInvalid option");
                    Thread.Sleep(2000);
                    break;

            }
            if (!wrongButton)
            {
                Add();
            }
        }
    }
}
