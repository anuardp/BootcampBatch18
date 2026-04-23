using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;

public class Game
{
    private List<IPlayer> _players {get;set;}
    private List<ICard> _deck{get; set;}
    private List<ICard> _board{get; set;}
    private List<IPot> _pots{get; set;} 



    private Dictionary<IPlayer, List<ICard>> _playerHands{get; set;}
    private Dictionary<IPlayer, List<IChip>> _playerChips{get; set;}
    private Dictionary<IPlayer, int> _playerBets{get; set;}  
    private Dictionary<IPlayer, bool> _playerCalled{get; set;}        
    private Dictionary<IPlayer, bool> _playerFolded{get; set;}        
    private Dictionary<IPlayer, bool> _playerAllIn{get; set;}

    private GamePhase _phase {get; set;}
    private int _dealerIndex{get; set;}
    private int _smallBlindIndex {get; set;}
    private int _bigBlindIndex {get; set;}
    private int _currentPlayerIndex {get; set;}
    private int _currentBetAmount{get; set;}
    

    public int SmallBlind{get; set;}
    public int BigBlind{get; set;}

    public event Action<IPlayer, PlayerAction, int> OnPlayerActed;
    public event Action<GamePhase> OnPhaseChanged;
    public event Action<IPlayer> OnHandEnded;



    public Game(List<IPlayer> players, List<ICard> deck, List<ICard> board, List<IPot> pot, int smallBlind, int bigBlind)
    {
        _players = players;
        _deck = new List<ICard>();
        _board = new List<ICard>();
        SmallBlind = smallBlind;
        BigBlind = bigBlind;
        _pots = new List<IPot>();
        _pots.Add(new Pot(_players.ToList(), 0));
        

        _playerHands = new Dictionary<IPlayer, List<ICard>>();
        _playerChips = new Dictionary<IPlayer, List<IChip>>();
        _playerBets = new Dictionary<IPlayer, int>();
        _playerFolded = new Dictionary<IPlayer, bool>();
        _playerAllIn = new Dictionary<IPlayer, bool>();

        Random rng = new Random();
        _dealerIndex = rng.Next(0, players.Count);
        _smallBlindIndex = (_dealerIndex + 1) % players.Count();
        _bigBlindIndex = (_dealerIndex + 2) % players.Count();

        foreach(var p in players)
        {
            _playerHands[p] = new List<ICard>();
            _playerChips[p] = AmountToChips(1000);   
            _playerBets[p] = 0;
            _playerFolded[p] = false;
            _playerAllIn[p] = false;

            
            _currentBetAmount = 0;
            _phase = GamePhase.PreFlop;
        }
    }

    private List<IChip> AmountToChips(int amount)
    {
        var chips = new List<IChip>();
        int[] chipsValue = {5000, 1000, 500, 100, 50};
        foreach(int chipValue in chipsValue)
        {
            while(amount>chipValue)
            {
                chips.Add(new Chip(chipValue, GetChipColorForValue(chipValue)));
                amount -= chipValue;    
            }
        }
        return chips;
    }

    private int ChipsToAmount(List<IChip> chips)
    {
        return chips.Sum(c => c.Value);
    }

    private List<IChip> RemoveChipsForBet(List<IChip> playerChips, int betAmount)
    {
        var sorted = playerChips.OrderByDescending(c => c.Value).ToList();
        var toRemove = new List<IChip>();
        int remaining = betAmount;

        foreach(var chip in sorted)
        {
            if(remaining <= 0)break;
            if(chip.Value <= remaining)
            {
                toRemove.Add(chip);
                remaining -= chip.Value;
            }
        }
        if(remaining > 0)
        {
            throw new InvalidOperationException("Player doesn't have enough chips to bet!!");
        }
        foreach(var chip in toRemove)playerChips.Remove(chip);
        return toRemove;
    }

    private string GetChipColorForValue(int value)
    {
        switch (value)
        {
            case 5000: return "Blue";
            case 1000: return "Black";
            case 500: return "Green";
            case 100: return "Red";
            case 50: return "White";
            default: return "Unknown";
        }
    }

    public void InitializeDeck()
    {
        _deck.Clear();  // assuming _deck is IDeck with a List<ICard> property
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                _deck.Add(new Card(suit, rank));
            }
        }
        ShuffleDeck(_deck);
    }

    public void ShuffleDeck(List<ICard> deck)
    {
        Random rng = new Random();
        int totalCards = deck.Count;

        for(int i = totalCards - 1; i > 0; i++)
        {
            int j = rng.Next(i+1);
            (deck[i], deck[j]) = (deck[j], deck[i]);
        }
    }
    
    public ICard DrawCard() //logic untuk draw kartu, selalu ambil dari yang paling atas setelah di-shuffle
    {
        if(_deck.Count == 0)throw new InvalidOperationException("No cards left in deck!!");
        
        ICard topCard = _deck[0];
        _deck.RemoveAt(0);
        return topCard;
    }

    public void ResetDeck()
    {
        _deck.Clear();
        InitializeDeck();
    }

    public void AddCardToBoard(List<ICard> board, GamePhase phase) //5 buah kartu ke atas board dari deck
    {
        if(phase == GamePhase.Flop)
        {
            for(int i=0; i < 3; i++)board.Add(DrawCard());
        }
        else if(phase == GamePhase.Turn || phase == GamePhase.River)
            board.Add(DrawCard());
    }

    public void ResetBoard() //Reset kartu di board -> Setelah satu sesi permainan berakhir 
    {
        _board.Clear();
    }

    private void PostBlinds()
    {
      
        IPlayer smallBlindPlayer = _players[_smallBlindIndex];
        IPlayer bigBlindPlayer = _players[_bigBlindIndex];

        // Small blind
        int smallBlindAmount = SmallBlind;
        
        int actualSmall = Math.Min(smallBlindAmount, GetTotalChips(smallBlindPlayer));
        if (actualSmall > 0)
        {
            if (actualSmall < smallBlindAmount)  // not enough for full blind
                _playerAllIn[smallBlindPlayer] = true;
            PlaceBet(smallBlindPlayer, actualSmall);
        }
        else
        {
        
            Fold(smallBlindPlayer);
        }

        // Big blind
        int bigBlindAmount = BigBlind;
        int actualBig = Math.Min(bigBlindAmount, GetTotalChips(bigBlindPlayer));
        if (actualBig > 0)
        {
            if (actualBig < bigBlindAmount)  // not enough for full blind
                _playerAllIn[bigBlindPlayer] = true;
            PlaceBet(bigBlindPlayer, actualBig);
            
            _currentBetAmount = actualBig;
        }
        else
        {
            Fold(bigBlindPlayer);
        }
    }

    public void DealHoleCards() // Bagi 2 kartu ke masing-masing player
    {
        int i = 0;
        while (i < 2)
        {
            foreach(var p in _playerHands.Keys)
            {
                _playerHands[p].Add(DrawCard());
            }   
            i++;
        }
    }

    //Buka 3 kartu pertama diatas board (3 kartu pertama diperlihatkan)
    public void DealFlop() => AddCardToBoard(_board, GamePhase.Flop);
    //Buka kartu ke-4 diatas board
    public void DealTurn() => AddCardToBoard(_board, GamePhase.Turn);
    //Buka kartu ke-5 diatas board
    public void DealRiver() => AddCardToBoard(_board, GamePhase.River);
    

    public void MoveToNextPhase() // Pindah ke fase ronde berikutnya (misal dari Flop ke Turn, Turn ke River)
    {
        if(_phase == GamePhase.PreFlop) _phase = GamePhase.Flop;
        else if(_phase == GamePhase.Flop) _phase = GamePhase.Turn;
        else if(_phase == GamePhase.Turn) _phase = GamePhase.River;
        else if(_phase == GamePhase.River) _phase = GamePhase.Showdown;

        OnPhaseChanged.Invoke(_phase);
    }
    public void Fold(IPlayer player) //kasih tag fold ke player (player tidak akan bisa bet lagi untuk keseluruhan ronde permainan dan gak bisa menangin chip)
    {
        _playerFolded[player] = true;
        UpdatePotEligibility();
    }
    public void Call(IPlayer player)  //Bet sesuai dengan jumlah bet dari lawan lainnya
    {
        _playerCalled[player] = true;

        int toCall = _currentBetAmount - _playerBets[player];
        if (toCall <= 0)
            throw new InvalidOperationException("No need to call, you can check.");
        
        int chips = GetTotalChips(player);
        if (chips < toCall)
        {
            //trigger All-In kalau jumlah chip tersisa kurang untuk Call
            AllIn(player);
            return;
        }
        
        PlaceBet(player, toCall);
    }
    public void Raise(IPlayer player, int raiseToTotal) // Player naikin jumlah bet 
    {
        // raiseToTotal -> total jumlah bet baru (termasuk jumlah bet saat ini)
        if (raiseToTotal <= _currentBetAmount)
            throw new InvalidOperationException("Raise must be higher than current bet.");
        
        int additional = raiseToTotal - _playerBets[player];
        if (additional <= 0)
            throw new InvalidOperationException("Raise amount must be positive.");
        
        int chips = GetTotalChips(player);
        if (chips < additional)
        {
            //Dianggap all-in kalau raisenya gak cukup
            AllIn(player);
            return;
        }
        
        PlaceBet(player, additional);
        _currentBetAmount = raiseToTotal;
    }
    public void Check(IPlayer player) //Skip turn untuk sementara (tidak bet, tapi tidak fold). Hanya berlaku jika bet masih bernilai 0
    {
        if (_currentBetAmount != 0) throw new InvalidOperationException("Cannot check – there is a bet to call.");

    }
    public void AllIn(IPlayer player) //Player bet semua chip yang dipunya...
    {
        int allInAmount = GetTotalChips(player);
        if (allInAmount == 0) return; 
        _playerAllIn[player] = true;
        PlaceBet(player, allInAmount); // handles pots, updates _playerBets, deducts chips

        if(allInAmount > _currentBetAmount)_currentBetAmount = allInAmount;
    }
    // public void DecideBotAction(IPlayer bot)
    
    public void PlaceBet(IPlayer player, int addChips) //Tambah bet ke dalam pot 
    {
        RemoveChipsForBet(_playerChips[player], addChips);

        int prevTotal = _playerBets[player];
        int currentTotal = prevTotal + addChips;
        _playerBets[player] = currentTotal;

        int remaining = addChips;

        for (int i = 0; i < _pots.Count && remaining > 0; i++)
        {
            IPot pot = _pots[i];
            int potCap = GetPotCap(i); 
        
            int alreadyInThisPot = Math.Min(prevTotal, potCap);
            int newInThisPot = Math.Min(currentTotal, potCap);
            int neededForThisPot = newInThisPot - alreadyInThisPot;
            
            if (neededForThisPot > 0)
            {
                int take = Math.Min(remaining, neededForThisPot);
                pot.Amount += take;
                remaining -= take;
            }
        }   
    }

    public int GetPotCap(int potIndex) //batas jumlah bet yang bisa ditaruh ke dalam suatu pot tertentu
    {   
        var activePlayers = _players.Where(p => !_playerFolded[p]).ToList();
        if (activePlayers.Count == 0) return 0;

        // Total distinct of bets, urut menaik
        var distinctBets = activePlayers.Select(p => _playerBets[p])
                                        .Distinct()
                                        .OrderBy(b => b)
                                        .ToList();

        //pot selalu terurut dari yang paling kecil
        if (potIndex < distinctBets.Count)
            return distinctBets[potIndex];
        else
            return distinctBets.Last(); // fallback (should not happen)
    }
   
    public void UpdatePotEligibility()
    {   
        for(int i=0;i<_pots.Count;i++)
        {
            int cap = GetPotCap(i);
            _pots[i].EligiblePlayers = _players.Where(p => !_playerFolded[p] && _playerBets[p] >= cap).ToList();
        }
    }

    public void SwitchTurn(List<IPlayer> player) //Ganti giliran pemain
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % player.Count; 
    }

    //Dapetin semua kombinasi 5 kartu dari 7 kartu (Total ada 21)
    private List<List<ICard>> GetAllFiveCardCombinations(List<ICard> cards)
    {
        var result = new List<List<ICard>>();
        int n = cards.Count; 
        for (int i = 0; i < n; i++)
            for (int j = i + 1; j < n; j++)
                for (int k = j + 1; k < n; k++)
                    for (int l = k + 1; l < n; l++)
                        for (int m = l + 1; m < n; m++)
                            result.Add(new List<ICard> { cards[i], cards[j], cards[k], cards[l], cards[m] });
        return result;
    }
        
    public HandStrength EvaluateHand(IPlayer player) //cek tingkat kekuatan kartu player
    {
        //tambah 2 kartu dari board dan 5 kartu dari board ke player.
        var allCards = _playerHands[player].Concat(_board).ToList();
        var bestStrength = new HandStrength(HandRank.HighCard, new List<Rank>());

        
        var combinations = GetAllFiveCardCombinations(allCards); 
        foreach (var five in combinations)
        {
            var strength = EvaluateFiveCardHand(five);
            if (CompareHandStrength(strength, bestStrength) > 0)
                bestStrength = strength;
        }
        return bestStrength;
    }
    private int CompareHandStrength(HandStrength a, HandStrength b)
    {
        if (a == null && b == null) return 0;
        if (a == null) return -1;
        if (b == null) return 1;

        if (a.Rank != b.Rank)
            return a.Rank.CompareTo(b.Rank);

        //Rank sama, cek dari tie-breakernya
        for (int i = 0; i < a.TieBreakers.Count && i < b.TieBreakers.Count; i++)
        {
            if (a.TieBreakers[i] != b.TieBreakers[i])
                return a.TieBreakers[i].CompareTo(b.TieBreakers[i]);
        }
        return 0;
    }

    public HandStrength EvaluateFiveCardHand(List<ICard> fiveCards)
    {
        // Urut kartu dari rank tertinggi (Ace dianggap paling tinggi)
        var sorted = fiveCards.OrderByDescending(c => c.Rank).ToList();
        var ranks = sorted.Select(c => (int)c.Rank).ToList();
        var suits = sorted.Select(c => c.Suit).ToList();

        // Cek flush (suits sama semua)
        bool isFlush = suits.Distinct().Count() == 1;

        // Cek straight
        bool isStraight = false;
        // Straight -> max - min = 4
        var distinctRanks = ranks.Distinct().ToList();
        if (distinctRanks.Count == 5 && distinctRanks.Max() - distinctRanks.Min() == 4)
            isStraight = true;
        // Kasus spesifik: Staright, tapi kartu Ace paling bawah (A,2,3,4,5)
        if (!isStraight && distinctRanks.Equals(new[] { 14, 2, 3, 4, 5 }))
        {
            isStraight = true;
            // Utk kasus spesifik, kartu tertinggi utk Tie-Breaker adalah 5
            ranks = new List<int> { 5, 4, 3, 2, 1 }; // urut ulang
            sorted = sorted.OrderByDescending(c => c.Rank == Rank.Ace ? 1 : (int)c.Rank).ToList();
        }

        // Frekuensi kemunculan masing-masing rank dari kartu ditangan
        var rankGroups = ranks.GroupBy(r => r).OrderByDescending(g => g.Count()).ThenByDescending(g => g.Key).ToList();
        int firstCount = rankGroups[0].Count();
        int secondCount = rankGroups.Count > 1 ? rankGroups[1].Count() : 0;

        // Cek HandRank dan tie-breakernya
        HandRank handRank;
        List<Rank> tieBreakers = new List<Rank>();

        // Four of a kind
        if (firstCount == 4)
        {
            handRank = HandRank.FourOfAKind;
            tieBreakers.Add((Rank)rankGroups[0].Key); // the four-of-a-kind rank
            tieBreakers.Add((Rank)rankGroups[1].Key); // the kicker
        }
        // Full house
        else if (firstCount == 3 && secondCount == 2)
        {
            handRank = HandRank.FullHouse;
            tieBreakers.Add((Rank)rankGroups[0].Key); // triplet rank
            tieBreakers.Add((Rank)rankGroups[1].Key); // pair rank
        }
        // Flush
        else if (isFlush)
        {
            handRank = HandRank.Flush;
            // Tie-breakers: all five ranks in descending order
            tieBreakers = sorted.Select(c => c.Rank).ToList();
        }
        // Straight
        else if (isStraight)
        {
            handRank = HandRank.Straight;
            // Highest card of the straight (for Ace-low, that's 5)
            int high = ranks.Max();
            tieBreakers.Add((Rank)high);
        }
        // Three of a kind
        else if (firstCount == 3)
        {
            handRank = HandRank.ThreeOfAKind;
            tieBreakers.Add((Rank)rankGroups[0].Key); // the triplet rank
            // Add the remaining two kickers (descending)
            foreach (var g in rankGroups.Skip(1))
                tieBreakers.Add((Rank)g.Key);
        }
        // Two pair
        else if (firstCount == 2 && secondCount == 2)
        {
            handRank = HandRank.TwoPair;
            // Higher pair first, then lower pair, then kicker
            tieBreakers.Add((Rank)rankGroups[0].Key);
            tieBreakers.Add((Rank)rankGroups[1].Key);
            tieBreakers.Add((Rank)rankGroups[2].Key);
        }
        // One pair
        else if (firstCount == 2)
        {
            handRank = HandRank.OnePair;
            tieBreakers.Add((Rank)rankGroups[0].Key); // pair rank
            // Add the three kickers in descending order
            foreach (var g in rankGroups.Skip(1))
                tieBreakers.Add((Rank)g.Key);
        }
        // High card
        else
        {
            handRank = HandRank.HighCard;
            tieBreakers = sorted.Select(c => c.Rank).ToList();
        }

        return new HandStrength(handRank, tieBreakers);
    }

    //Cek siapa yang menang. Kalau >1 yang menang, hadiah displit. 
    private List<IPlayer> GetWinners()
    {
        List<IPlayer> winners = new List<IPlayer>();
        HandStrength bestStrength = null;

        foreach (var player in _players.Where(p => !_playerFolded[p]))
        {
            HandStrength strength = EvaluateHand(player);
            if (bestStrength == null || CompareHandStrength(strength, bestStrength) > 0)
            {
                bestStrength = strength;
                winners.Clear();
                winners.Add(player);
            }
            else if (CompareHandStrength(strength, bestStrength) == 0)
            {
                winners.Add(player);
            }
        }
        return winners;
    }

    public void AwardPot()
    {
        foreach (var pot in _pots)
        {
            if (pot.Amount == 0) continue;
            
            // Determine winners among players eligible for this pot
            var eligibleWinners = GetWinners().Where(w => pot.EligiblePlayers.Contains(w)).ToList();
            if (eligibleWinners.Count == 0) continue;
            
            int share = pot.Amount / eligibleWinners.Count;
            int remainder = pot.Amount % eligibleWinners.Count;
            
            for (int i = 0; i < eligibleWinners.Count; i++)
            {
                int winnings = share + (i == 0 ? remainder : 0);
                // Tambahin chip ke player yg menang
                _playerChips[eligibleWinners[i]].AddRange(AmountToChips(winnings));
            }
            pot.Amount = 0;
        }
        _pots.Clear();
    }

    //Execute setelah 1 sesi game selesai
    public void StartNewRound() 
    {
        ResetDeck();
        ResetBoard();
        
        Random rng = new Random();
        _dealerIndex = rng.Next(0, _players.Count);
        _smallBlindIndex = (_dealerIndex + 1) % _players.Count();
        _bigBlindIndex = (_dealerIndex + 2) % _players.Count();
        
        _currentBetAmount = 0;
        _phase = GamePhase.PreFlop;  
        _pots.Clear();
        _pots.Add(new Pot(_players.ToList(), 0));
        
        foreach(var p in _players)
        {
            _playerHands[p].Clear();
            _playerBets[p] = 0;
            _playerFolded[p] = false;
            _playerAllIn[p] = false;
        }
              
    }

    public List<ICard> GetHand(IPlayer player)
    {
        return _playerHands[player];
    }

    public int GetTotalChips(IPlayer player)
    {
        int totalChipScore = 0;
        foreach(var chip in _playerChips[player])totalChipScore += chip.Value;
        return totalChipScore;
    }
    public bool IsFolded(IPlayer player)
    {
        return _playerFolded[player];
    }

    public bool IsAllIn(IPlayer player)
    {
        return _playerAllIn[player];
    }
    public bool IsGameEndedEarly() //Cek apa player yang tidak fold tinggal satu atau tidak.
    {
        int foldCheck = 0;
        foreach(var p in _players)if(_playerFolded[p])foldCheck++;
        
        if(foldCheck == _players.Count-1)return true;
        else return false;
    }

    public bool IsBettingRoundOver() //cek apa sesi taruhan sudah selesai (semua eligible player sudah call)
    {
        foreach (var player in _players)
        {
            if (_playerFolded[player])continue;
            if (_playerAllIn[player])continue;
            if (_playerBets[player] != _currentBetAmount)return false;
        }
    return true;
    }
}