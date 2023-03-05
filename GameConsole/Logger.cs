using GameConsole.Data;
using GameConsole.Data.Entities;

class Logger
{
    public UserDbContext UserDb;
    public Logger()
    {
        UserDb = new UserDbContext();
    }
    public static string LengthChecker(int length, string type)
    {
        string name = "";
        while (name.Trim().Length < length)
        {
            Console.Clear();
            Console.WriteLine($"{type} must be {length} characters or longer");
            Console.WriteLine($"Choose a {type}:\n");
            Console.Write(">");
            name = Console.ReadLine();
        }
        return name;
    }
    public void Register()
    {
        
        string username = LengthChecker(4, "username");
        string password = LengthChecker(8, "password");
        bool isUser = UserDb.UserRecords.Where(u => u.Username == username.ToLower()).Count() == 0;
        if (isUser)
        {
            UserDb.Add(new UserTable { Username = username.ToLower(), Password = password, DateCreated = DateTime.Now });
            UserDb.SaveChanges();
            return;
        }
    }
    public string Login()
    {
        bool isUser = false;
        string username = "";
        string password = "";
        Console.Clear();
        Console.WriteLine($"Username:\n");
        Console.Write(">");
        username = Console.ReadLine();
        Console.WriteLine($"Password:\n");
        Console.Write(">");
        password = Console.ReadLine();
        isUser = UserDb.UserRecords.Where(u => u.Username == username && u.Password == password).Count() == 1;
        if (!isUser)
        {
            Console.WriteLine("\nDidnt find a match");
            Thread.Sleep(1500);
        UserIdentification();
        }
        return username;
    }
    public string UserIdentification()
    {
        string name = "";
        while (name.Length == 0)
        {
            LoggerMenu();
            var key = Console.ReadKey();
            switch (key.Key)
            {
                case ConsoleKey.L:
                    name = Login();
                    return name;
                case ConsoleKey.R:
                    Register();
                    break;
                case ConsoleKey.G:
                    name = "adm";
                    break;
                case ConsoleKey.Z:
                    Console.Clear();
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
        return name;
    }
    public void LoggerMenu()
    {
        Console.Clear();
        Console.WriteLine("GAMECENTER Login Menu\n\n" +
                              "Press L for login\n" +
                              "Press R for register\n" +
                              "Press G for guest\n\n" +
                              "Press Z for exit");
    }
}
