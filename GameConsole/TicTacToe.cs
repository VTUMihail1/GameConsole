using Microsoft.EntityFrameworkCore.Metadata.Internal;
using GameConsole.Data;
using GameConsole.Data.Entities;

namespace GameConsole
{
    class TicTacToe : Game
    {
        private char[,] board;
        private int player;
        private int moves;
        private int playerXWins;
        private int playerOWins;
        private string _name;
        private string result;
        private TicTacToeDbContext db;
        private bool gameIsOver;
        public TicTacToe(string name)
        {
            board = new char[3, 3];
            player = 0;
            moves = 0;
            playerXWins = 0;
            playerOWins = 0;
            _name = name;
            db = new TicTacToeDbContext();
            result = "Draw";
            gameIsOver = false;
        }
        private void PrintMenu()
        {
            Console.WriteLine("\n\nPress 1-9 to choose a position\n\n" +
                                  "Press R to restart\n" +
                                  "Press ESC to exit\n");
        }
        private void InitializeBoard()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    board[i, j] = ' ';
                }
            }
        }
        private void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine($"TIC TAC TOE      Player X Wins: {playerXWins}");
            Console.WriteLine($"                 Player O Wins: {playerOWins}");
            Console.WriteLine("|" + new string('-', 3 * 2 + 1) + "|");
            for (int i = 0; i < 3; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < 3; j++)
                {
                    Console.Write(board[i, j] + " ");
                }
                Console.WriteLine("|");
            }
            Console.Write("|" + new string('-', 3 * 2 + 1) + "|");
        }
        private void WhoWon(char player)
        {
            if (WinCheck(player))
            {
                if (player == 'X')
                {
                    result = "Win";
                    playerXWins++;
                }
                else
                {
                    result = "Lose";
                    playerOWins++;
                }
                PrintBoard();
                Console.WriteLine($"\nPlayer {player} wins");
                DbUpdate();
                Thread.Sleep(3000);
                Restart();
            }
        }
        private void Player(char player)
        {
            Random random = new Random();
            while (true)
            {
                PrintMenu();
                if (player == 'X')
                {
                    do
                    {
                        int input = InputChekcer();
                        if(input == -1)
                        {
                            return;
                        }
                        this.player = input - 1;
                    }
                    while (board[this.player / 3, this.player % 3] != ' ');
                }
                else
                {
                    while (board[this.player / 3, this.player % 3] != ' ')
                    {
                        this.player = random.Next(9);
                    }

                }
                board[this.player / 3, this.player % 3] = player;
                moves++;
                WhoWon(player);
                break;
            }
        }
        private void DrawChecker()
        {
            if (moves == 9)
            {
                PrintBoard();
                Console.WriteLine("\nIt's a draw");
                result = "Draw";
                DbUpdate();
                Thread.Sleep(5000);
                return;
            }
        }
        private bool WinCheck(int player)
        {
            if (board[0, 0] == player && board[0, 1] == player && board[0, 2] == player ||
                board[1, 0] == player && board[1, 1] == player && board[1, 2] == player ||
                board[2, 0] == player && board[2, 1] == player && board[2, 2] == player ||
                board[0, 0] == player && board[1, 0] == player && board[2, 0] == player ||
                board[0, 1] == player && board[1, 1] == player && board[2, 1] == player ||
                board[0, 2] == player && board[1, 2] == player && board[2, 2] == player ||
                board[0, 0] == player && board[1, 1] == player && board[2, 2] == player ||
                board[0, 2] == player && board[1, 1] == player && board[2, 0] == player)
            {
                return true;
            }
            return false;
        }
        private void Restart()
        {
            moves = 0;
            Array.Clear(board, 0, 3 * 3);
            Start();
        }
        private int InputChekcer()
        {
            while (true)
            {

                Console.Clear();
                PrintBoard();
                PrintMenu();
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                    case ConsoleKey.D5:
                    case ConsoleKey.D6:
                    case ConsoleKey.D7:
                    case ConsoleKey.D8:
                    case ConsoleKey.D9:
                        return key.Key - ConsoleKey.D0;
                    case ConsoleKey.R:
                        Restart();
                        break;
                    case ConsoleKey.Escape:
                        Console.Clear();
                        gameIsOver = true;
                        return -1;
                    default:
                        Console.WriteLine("\nInvalid input");
                        Thread.Sleep(1500);
                        break;
                }
            }
            
        }
        private void DbUpdate()
        {
            bestScore = Math.Max(bestScore, score);
            if (_name != "adm")
            {
                db.Add(new TicTacToeTable { Name = _name, Result=result, DateStarted = DateTime.Now });
                db.SaveChanges();
            }
        }
        public override void Start()
        {
            Console.Title = "TicTacToe";
            InitializeBoard();

            while (true)
            {
                PrintBoard();
                Player('X');
                if (gameIsOver)
                {
                    break;
                }
                DrawChecker();
                Player('O');
            }
        }
    }
}
