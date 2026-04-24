using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        Console.Title = "Poker Game";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Poker ===\n");

        // Setup, anggap 1 player human dan 4 bot
        Console.Write("Enter your name: ");
        string playerName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(playerName)) playerName = "You";

        var players = new List<IPlayer>
        {
            new Player("1", playerName, "human"),
            new Player("2", "Bot1", "bot"),
            new Player("3", "Bot2", "bot"),
            new Player("4", "Bot3", "bot"),
            new Player("5", "Bot4", "bot")
        };

       
        var deck = new List<ICard>();  
        var board = new List<ICard>();
        var pot = new List<IPot>();
        var game = new Game(players,  10, 50);
        
        game.OnPlayerActed += (player, action, amount) =>
        {
            Console.WriteLine($"{player.Name} {action}" + (amount > 0 ? $" {amount}" : ""));
            RenderGameState(game);
        };
        game.OnPhaseChanged += phase => Console.WriteLine($"\n*** {phase} ***");
        game.OnHandEnded += winner => Console.WriteLine($"\n🏆 {winner.Name} wins the hand!\n");

        bool quit = false;
        while (!quit && game.GetActivePlayers().Count > 1)
        {
            Console.WriteLine("\n--- NEW HAND ---");
            game.StartNewRound();

            // Main hand loop: continue until hand ends (showdown or early win)
            while (!game.IsGameEndedEarly() && game.Phase != GamePhase.Showdown)
            {
                // Process current player's turn
                IPlayer current = game.GetCurrentPlayer();
                if (current is Player p && p.Type == "human")
                {
                    HumanTurn(game, current);
                }
                else
                {
                    // Bot decision (simple random)
                    var (action, amount) = BotDecision(game, current);
                    Console.WriteLine($"{current.Name} decides: {action}" + (amount > 0 ? $" {amount}" : ""));
                    game.HandleAction(current, action, amount);
                }

                // After action, check if betting round is over, then move to next phase
                if (game.IsBettingRoundOver())
                {
                    if (game.Phase == GamePhase.PreFlop) game.DealFlop();
                    else if (game.Phase == GamePhase.Flop) game.DealTurn();
                    else if (game.Phase == GamePhase.Turn) game.DealRiver();
                    game.MoveToNextPhase();
                }
            }

            // Hand ended early or showdown
            if (game.IsGameEndedEarly())
            {
                var winner = game.GetActivePlayers().First();
                Console.WriteLine($"\nOnly {winner.Name} remains – they win!");
                game.AwardPot();  
            }
            else
            {
                game.AwardPot();
            }

            // Remove players with zero chips
            var dead = game.GetPlayers().Where(p => game.GetTotalChips(p) == 0).ToList();
            foreach (var d in dead) game.RemovePlayer(d);  

            Console.WriteLine("\nPress any key for next hand, or Q to quit...");
            if (Console.ReadKey(true).Key == ConsoleKey.Q) quit = true;
        }

        Console.WriteLine("\nGame over! Thanks for playing.");
    }

    static void HumanTurn(Game game, IPlayer player)
    {
        int toCall = game.CurrentBetAmount - game.GetPlayerBet(player);
        int chips = game.GetTotalChips(player);
        Console.WriteLine($"\nYour turn: {player.Name} (chips: {chips})");
        Console.WriteLine($"Current bet to call: {toCall}");
        Console.WriteLine("Actions: (f)old, (c)all, (r)aise, (a)ll-in");
        Console.Write("Choose: ");
        string input = Console.ReadLine()?.ToLower();

        PlayerAction action;
        int amount = 0;

        switch (input)
        {
            case "f":
                action = PlayerAction.Fold;
                break;
            case "c":
                if (toCall == 0)
                    action = PlayerAction.Check;
                else
                {
                    action = PlayerAction.Call;
                    amount = toCall;
                }
                break;
            case "r":
                action = PlayerAction.Raise;
                Console.Write("Raise to total amount: ");
                if (!int.TryParse(Console.ReadLine(), out amount) || amount <= toCall)
                {
                    Console.WriteLine("Invalid raise. Please call instead.");
                    action = toCall > 0 ? PlayerAction.Call : PlayerAction.Check;
                    amount = toCall;
                }
                break;
            case "a":
                action = PlayerAction.AllIn;
                amount = chips;
                break;
            default:
                Console.WriteLine("Invalid, folding.");
                action = PlayerAction.Fold;
                break;
        }
        game.HandleAction(player, action, amount);
    }

    static (PlayerAction action, int amount) BotDecision(Game game, IPlayer bot)
    {
        int chips = game.GetTotalChips(bot);
        int toCall = game.CurrentBetAmount - game.GetPlayerBet(bot);
        Random rand = new Random();

        if (toCall == 0)
        {
            return (PlayerAction.Check, 0);
        }
        else
        {
            if (chips < toCall)
                return (PlayerAction.AllIn, chips);
            else
            {
                int r = rand.Next(100);
                if (r < 40) return (PlayerAction.Fold, 0);
                else if (r < 80) return (PlayerAction.Call, toCall);
                else return (PlayerAction.Raise, Math.Min(chips, toCall + game.BigBlind));
            }
        }
    }

    static void RenderGameState(Game game)
    {
        Console.WriteLine("\n--- Game State ---");
        Console.Write("Cards on Board: ");
        var board = game.GetBoardCards();
        if (board.Count == 0) Console.Write("(none)");
        else Console.WriteLine(string.Join(", ", board.Select(c => c.ToString())));
        foreach (var p in game.GetPlayers())
        {
            string status = "";
            if (game.IsFolded(p)) status = "[FOLD]";
            else if (game.IsPlayerAllIn(p)) status = "[ALL-IN]";
            Console.WriteLine($"{p.Name,-10} Chips: {game.GetTotalChips(p),-5} Bet: {game.GetPlayerBet(p),-5} {status}");
        }
        Console.WriteLine($"Pot: {game.GetTotalPot()}");
        Console.WriteLine("------------------");
    }
}   