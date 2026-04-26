using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        GameHomeUIHeader();
        Console.Write("\nSelect Menu: ");
        string? option = Console.ReadLine();
        
        while (option != "1" && option != "2")
        {
            Console.Clear();
            GameHomeUIHeader();
            Console.Write("\nThere's only 2 options.. No need to complicate yourself!!\n\nSelect Menu: ");
            option = Console.ReadLine();
        }
        if (option == "1")
        {
            List<IPlayer> players = SelectPlayer();
            // Constructor: Game(players, board, deck, pot, smallBlind, bigBlind)
            List<ICard> deck = new List<ICard>();
            List<ICard> board = new List<ICard>();
            List<IPot> pots = new List<IPot>();
            Game game = new Game(players, board, deck, pots, 10, 20);

            // Events hanya untuk log, tidak untuk render ulang (agar tidak konflik)
            game.OnPlayerActed += (player, action, amount) =>
            {
                Console.WriteLine($"{player.Name} {action}" + (amount > 0 ? $" {amount}" : ""));
            };
            game.OnPhaseChanged += phase => Console.WriteLine($"\n*** {phase} ***");
            game.OnHandEnded += winner => Console.WriteLine($"\n🏆 {winner.Name} wins the hand!\n");

            bool quit = false;
            while (!quit && game.GetPlayers().Count > 1)
            {
                MainGameUI(game);
                Console.Write("\nA round has finished. Press Enter to continue, or type 'quit' to exit: ");
                string? gameFinished = Console.ReadLine();
                if (gameFinished?.ToLower() == "quit") quit = true;
            }
            if (game.GetPlayers().Count <= 1)
                Console.WriteLine("\nGame over! Only one player remains.");
            else
                Console.WriteLine("\nThanks for playing!");
        }
        else
        {
            Console.WriteLine("\nExit the game. Thank you for playing.");
        }
    }

    static void GameHomeUIHeader()
    {
        Console.WriteLine("--------------------Poker--------------------");
        Console.WriteLine("\nMenu:\n[1] Play\n[2] Exit");
    }

    static List<IPlayer> SelectPlayer()
    {
        Console.Clear();
        Console.WriteLine("--------------------Poker--------------------");
        Console.Write("\nNumber of human players: ");
        int totalHumanPlayer = int.Parse(Console.ReadLine() ?? "1");
        var players = new List<IPlayer>();
        for (int i = 0; i < totalHumanPlayer; i++)
        {
            Console.Write($"Enter Player {i + 1} name: ");
            string? playerName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(playerName)) playerName = $"Human{i + 1}";
            players.Add(new Player((i + 1).ToString(), playerName, "human"));
        }

        Console.Write("Number of bot players: ");
        int totalBotPlayer = int.Parse(Console.ReadLine() ?? "0");
        for (int i = 0; i < totalBotPlayer; i++)
        {
            players.Add(new Player((totalHumanPlayer + i + 1).ToString(), $"Bot{i + 1}", "bot"));
        }
        return players;
    }

    static void MainGameUI(Game game)
    {
        Console.Clear();
        Console.WriteLine("--------------------Poker--------------------");
        game.StartNewRound();

        while (!game.IsGameEndedEarly() && game.Phase != GamePhase.Showdown)
        {
            IPlayer currentPlayer = game.GetCurrentPlayer();
            RenderGameState(game, currentPlayer);   // tampilkan state sebelum aksi

            if (currentPlayer is Player p && p.Type == "human")
            {
                HumanTurn(game, currentPlayer);
            }
            else
            {
                var (action, amount) = game.DecideBotAction(currentPlayer);
                Console.WriteLine($"{currentPlayer.Name} decides: {action}" + (amount > 0 ? $" {amount}" : ""));
                game.HandleAction(currentPlayer, action, amount);
            }

            // Setelah aksi, render ulang agar chip dan bet terupdate (opsional, karena loop akan render lagi)
            RenderGameState(game, game.GetCurrentPlayer());

            if (game.IsBettingRoundOver())
            {
                if (game.Phase == GamePhase.PreFlop)
                    game.DealFlop();
                else if (game.Phase == GamePhase.Flop)
                    game.DealTurn();
                else if (game.Phase == GamePhase.Turn)
                    game.DealRiver();
                game.MoveToNextPhase();

                if (game.Phase != GamePhase.Showdown)
                {
                    Console.WriteLine($"\n*** {game.Phase} ***");
                    RenderGameState(game, null); // tampilkan board baru
                }
            }
        }
        if (game.Phase == GamePhase.Showdown)
        {
            RenderShowdown(game);
        }
        else if (game.IsGameEndedEarly())
        {
            var winner = game.GetActivePlayers().FirstOrDefault();
            if (winner != null)
                Console.WriteLine($"\nOnly {winner.Name} remains. They win the pot!");
        }
        
        game.AwardPot();
        var dead = game.GetPlayers().Where(p => game.GetTotalChips(p) == 0).ToList();
        foreach (var d in dead)
            game.RemovePlayer(d);
    }

    static void HumanTurn(Game game, IPlayer human)
    {
        int toCall = game.CurrentBetAmount - game.GetPlayerBet(human);
        int chips = game.GetTotalChips(human);
        Console.WriteLine($"\nYour turn: {human.Name} (chips: {chips})");
        Console.WriteLine($"Current bet to call: {toCall}");
        Console.WriteLine("Actions: (f)old, (c)call, (r)aise, (a)ll-in");
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
                while (true)
                {
                    Console.Write("Raise to total amount (must be > current bet): ");
                    if (!int.TryParse(Console.ReadLine(), out amount))
                    {
                        Console.WriteLine("Invalid number. Please enter a valid amount.");
                        continue;
                    }
                    if (amount <= game.CurrentBetAmount)
                    {
                        Console.WriteLine($"Raise must be higher than current bet ({game.CurrentBetAmount}). Try again.");
                        continue;
                    }
                    // Valid, keluar loop
                    break;
                }
                break;
            case "a":
                action = PlayerAction.AllIn;
                amount = chips;
                break;  
            default:
                Console.WriteLine("Invalid input. Folding.");
                action = PlayerAction.Fold;
                break;
        }

        game.HandleAction(human, action, amount);
    }

    static void RenderGameState(Game game, IPlayer? currentPlayer)
    {
        Console.WriteLine("\n--- Game State ---");
        Console.Write("Cards on Board: ");
        var board = game.GetBoard();
        if (board.Count == 0)
            Console.Write("[Xx] [Xx] [Xx] [Xx] [Xx]");
        else if (board.Count == 3)
            Console.Write($"[{board[0].Suit}{board[0].Rank}] [{board[1].Suit}{board[1].Rank}] [{board[2].Suit}{board[2].Rank}] [Xx] [Xx]");
        else if (board.Count == 4)
            Console.Write($"[{board[0].Suit}{board[0].Rank}] [{board[1].Suit}{board[1].Rank}] [{board[2].Suit}{board[2].Rank}] [{board[3].Suit}{board[3].Rank}] [Xx]");
        else if (board.Count == 5)
            Console.Write($"[{board[0].Suit}{board[0].Rank}] [{board[1].Suit}{board[1].Rank}] [{board[2].Suit}{board[2].Rank}] [{board[3].Suit}{board[3].Rank}] [{board[4].Suit}{board[4].Rank}]");
        Console.WriteLine();

        // Tampilkan hand hanya untuk pemain yang sedang giliran (jika human)
        if (currentPlayer != null && currentPlayer is Player p && p.Type == "human")
        {
            var hand = game.GetHand(currentPlayer);
            if (hand.Count == 2)
                Console.WriteLine($"Your hand: [{hand[0].Suit}{hand[0].Rank}] [{hand[1].Suit}{hand[1].Rank}]");
        }

        // Tampilkan semua pemain beserta chip dan bet terupdate
        foreach (var player in game.GetPlayers())
        {
            string status = "";
            if (game.IsFolded(player)) status = "[FOLD]";
            else if (game.IsAllIn(player)) status = "[ALL-IN]";
            Console.WriteLine($"{player.Name,-10} Chips: {game.GetTotalChips(player),-5} Bet: {game.GetPlayerBet(player),-5} {status}");
        }
        Console.WriteLine($"Pot: {game.GetTotalPot()}");
        Console.WriteLine("------------------");
    }

    static void RenderShowdown(Game game)
    {
        Console.WriteLine("\n--- SHOWDOWN ---");
        var activePlayers = game.GetActivePlayers(); // yang tidak fold
        foreach (var player in activePlayers)
        {
            var hand = game.GetHand(player);
            var strength = game.EvaluateHand(player);
            Console.WriteLine($"{player.Name}'s hand: [{hand[0].Suit}{hand[0].Rank}] [{hand[1].Suit}{hand[1].Rank}] => {strength.Rank}");
            // optional: tampilkan tie breakers jika perlu
            // Console.WriteLine($"   Tie breakers: {string.Join(", ", strength.TieBreakers)}");
        }
        var winners = game.GetWinnersOnRound();
        Console.Write("\nWinner(s): ");
        foreach (var w in winners)
            Console.Write($"{w.Name} ");
        Console.WriteLine();
    }
}