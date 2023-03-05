
using GameConsole;
using GameConsole.Data;

class GameCenter
{
    private void PrintLeaderboard()
    {
        Console.WriteLine("GAMECENTER Leaderboard\n" +
                          "\nPress T for 2048 leaderboard\n" +
                          "Press A for TicTacToe leaderboard\n" +
                          "Press S for Snake leaderboard\n\n" +
                          "Press ESC to exit");
    }
    private void LeaderBoard()
    {
        TicTacToeDbContext db1 = new TicTacToeDbContext();
        TZFEDbContext db2 = new TZFEDbContext();
        SnakeDbContext db3 = new SnakeDbContext();
        int number = 0;
        bool leaderboard = false;
        while (true)
        {

            if (!leaderboard)
            {
                PrintLeaderboard();
            }
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.A:
                    Console.Clear();
                    PrintLeaderboard();
                    Console.WriteLine("\nTicTacToe leaderboard\n");
                    number = 0;
                    
                    foreach (var item in db1.TicTacToeRecords.OrderByDescending(x => x.Result))
                    {
                        Console.WriteLine($"{++number}. {item.Name} - {item.Result}");
                    }
                    leaderboard = true;
                    break;
                case ConsoleKey.T:
                    Console.Clear();
                    PrintLeaderboard();
                    Console.WriteLine("\nTwoZeroFourEight leaderboard\n");
                    number = 0;
                    foreach (var item in db2.TZFERecords.OrderByDescending(x => x.Score))
                    {
                        Console.WriteLine($"{++number}. {item.Name} - {item.Score}");
                    }
                    leaderboard = true;
                    break;
                case ConsoleKey.S:
                    Console.Clear();
                    PrintLeaderboard();
                    number = 0;
                    Console.WriteLine("\nSnake leaderboard\n");
                    foreach (var item in db3.SnakeRecords.OrderByDescending(x => x.Score))
                    {
                        Console.WriteLine($"{++number}. {item.Name} - {item.Score}");
                    }
                    leaderboard = true;
                    break;
                case ConsoleKey.L:
                    Console.Clear();
                    LeaderBoard();
                    break;
                case ConsoleKey.Escape:
                    Console.Clear();
                    return;
                default:
                    Console.WriteLine("\nInvalid input");
                    Thread.Sleep(2000);
                    Console.Clear();
                    break;

            }
        }
    }
    private void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("GAMECENTER\n" +
                          "\nPress T for 2048\n" +
                          "Press A for TicTacToe\n" +
                          "Press S for Snake\n\n" +
                          "Press L for game leaderboards\n" +
                          "Press U to switch user\n" +
                          "Press ESC to exit");
    }

    public void Start()
    {
        Logger logger= new Logger();
        string name = logger.UserIdentification();
        while (true)
        {
            PrintMenu();
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.T:
                    Console.Clear();
                    Game game1 = new TwoZeroFourEight(name);
                    game1.Start();
                    break;
                case ConsoleKey.S:
                    Console.Clear();
                    Game game2 = new Snake(name);
                    game2.Start();
                    break;
                case ConsoleKey.A:
                    Console.Clear();
                    Game game3 = new TicTacToe(name);
                    game3.Start();
                    break;
                case ConsoleKey.U:
                    Console.Clear();
                    name = logger.UserIdentification();
                    break;
                case ConsoleKey.L:
                    Console.Clear();
                    LeaderBoard();
                    break;
                case ConsoleKey.Escape: 
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("\nInvalid input");
                    Thread.Sleep(2000);
                    break;
            }
        }
    }
}
