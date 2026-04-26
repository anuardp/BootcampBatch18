using System;
using System.Collections.Generic;
using System.Linq;

class Program
{   
    static void Main()
    {
        GameHomeUI();
    }

    static void GameHomeUIHeader()
    {
        Console.WriteLine("--------------------Poker--------------------");
        Console.WriteLine("\nMenu:\n[1] Play\n[2] Exit");

        Console.Write("\nSelect Menu: ");
    }

    static void SelectPlayerUI()
    {
        Console.WriteLine("--------------------Poker--------------------");
        Console.Write("\nPlayers on table: ");
    }
    static void SelectBotUI()
    {
        Console.Write("\nBots on table: ");
    }
    static void GameHomeUI()
    {
        GameHomeUIHeader();
        string option = Console.ReadLine();
        
        while(option != "1" && option != "2")
        {
            Console.Clear();
            GameHomeUIHeader();
            Console.Write("\nThere's only 2 option.. No need to complicate yourself!!\n\nSelect Menu: ");
            option = Console.ReadLine();
        }
        if (option == "1")
        {   
            //Setup player and bot
            List<IPlayer> players = new List<IPlayer>();
            int indexNoName = 0;

            SelectPlayerUI();
            int totalHumanPlayer = Convert.ToInt32(Console.ReadLine());
            for(int i = 0; i < totalHumanPlayer; i++)
            {
                Console.Write($"\nEnter Player {i+1} name: ");
                string? playerName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(playerName))
                {
                    playerName = $"NoName{indexNoName+1}";
                    indexNoName++;
                }
                IPlayer player= new Player(Convert.ToString(i+1), playerName, "human");
                players.Add(player);
            }


            SelectBotUI();
            int totalBotPlayer = Convert.ToInt32(Console.ReadLine());
            for(int i = 0; i  < totalBotPlayer; i++)
            {
                IPlayer bot = new Player(Convert.ToString(totalHumanPlayer + i + 1), $"Bot{i+1}", "bot");
                players.Add(bot);
            }

            foreach(IPlayer p in players)
            {
                Console.WriteLine($"{p.Id} - {p.Name} - {p.Type}");
            }
        }
        else
        {
            Console.WriteLine("\nExit the game. Thank you for playing. Make sure you're not broke next time you decided to play again later!!");
        }
    }
}   