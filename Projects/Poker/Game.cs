using System.Security.Cryptography;

public class Game
{
    private List<IPlayer> _players {get;set;}
    private List<ICard> _deck{get; set;}
    private List<ICard> _board{get; set;}

    private Dictionary<IPlayer, List<ICard>> _playerHands{get; set;}
    private Dictionary<IPlayer, List<IChip>> _playerChips{get; set;}
    private Dictionary<IPlayer, int> _playerBets{get; set;}
    private Dictionary<IPlayer, bool> _playerChecked{get; set;}        
    private Dictionary<IPlayer, bool> _playerCalled{get; set;}        
    private Dictionary<IPlayer, bool> _playerFolded{get; set;}        
    private Dictionary<IPlayer, bool> _playerAllIn{get; set;}

    private GamePhase _phase {get; set;}
    private int _dealerIndex{get; set;}
    private int _smallBlindIndex;
    private int _bigBlindIndex;
    private int _currentPlayerIndex;
    private int _currentBetAmount; 
    private List<IPot> _pots;  // for main pot and side pots

    public int SmallBlind{get; set;}
    public int BigBlind{get; set;}

    public event Action<IPlayer, PlayerAction, int> OnPlayerActed;
    public event Action<GamePhase> OnPhaseChanged;
    public event Action<IPlayer> OnHandEnded;



    public Game(List<IPlayer> players, List<ICard> deck, List<ICard> board, int smallBlind, int bigBlind)
    {
        _players = players;
        _deck = new List<ICard>();
        _board = new List<ICard>();
        SmallBlind = smallBlind;
        BigBlind = bigBlind;

        _playerHands = new Dictionary<IPlayer, List<ICard>>();
        _playerChips = new Dictionary<IPlayer, List<IChip>>();
        _playerBets = new Dictionary<IPlayer, int>();
        _playerChecked = new Dictionary<IPlayer, bool>();
        _playerCalled = new Dictionary<IPlayer, bool>();
        _playerFolded = new Dictionary<IPlayer, bool>();
        _playerAllIn = new Dictionary<IPlayer, bool>();

        foreach(var p in players)
        {
            _playerHands[p] = new List<ICard>();
            _playerChips[p] = AmountToChips(1000);   
            _playerBets[p] = 0;
            _playerChecked[p] = false;
            _playerCalled[p] = false;
            _playerFolded[p] = false;
            _playerAllIn[p] = false;

            Random rng = new Random();
            _dealerIndex = rng.Next(0, players.Count);
            _smallBlindIndex = (_dealerIndex + 1) % players.Count();
            _bigBlindIndex = (_dealerIndex + 2) % players.Count();
            _currentBetAmount = 0;
            _pots = new List<IPot>();
            _phase = GamePhase.PreFlop;
        }
    }

    private List<IChip> AmountToChips(int amount)
    {
        var chips = new List<IChip>();
        int[] chipsValue = {5000, 1000, 500, 100, 25};
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
            case 25: return "White";
            default: return "Unknown";
        }
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

        foreach(Suit suit  in Enum.GetValues(typeof(Suit)))
        {
            foreach(Rank rank in Enum.GetValues(typeof(Rank)))
            {
                _deck.Add(new Card(rank, suit));
            }
        }
        ShuffleDeck(_deck);
    }

    public void AddCardToBoard(List<ICard> board, GamePhase phase) //5 buah kartu ke atas board dari deck
    {
        if(_phase == GamePhase.Flop)
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

    public void DealHoleCards() // Bagi 2 kartu ke masing-masing player
    {
        int i = 0;
        while (i < 2)
        {
            foreach(var p in _playerHands.Keys)
            {
                _playerHands[p].Add(DrawCard());
            }   
            i--;
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
    }
    public void Call(IPlayer player) 
    {
        _playerCalled[player] = true;
        
    }
    public void Raise(IPlayer player, int bet) // Player naikin jumlah bet 
    {
        
    }
    public void Check(IPlayer player) //Skip turn untuk sementara (tidak bet, tapi tidak fold). Hanya berlaku jika bet saat ini nilai currentBet masih 0
    {
        if(_currentBetAmount == 0)
        {
            _playerChecked[player] = true;
        }
        else throw new InvalidOperationException("Cannot check anymore, Bet or Fold...");
    }
    public void AllIn(IPlayer player) //Player bet semua chip yang dipunya...
    {
        _playerAllIn[player] = true;

    }
    // public void DecideBotAction(IPlayer bot)
    
    public void AddToPot(IPlayer player, int betAmount) //Tambah chip ke dalam Pot
    {
        int prevTotal = _playerBets[player];
        int currentTotal = _playerBets[player];
    }

    public void SwitchTurn(List<IPlayer> player) //Ganti giliran pemain
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % player.Count; 
    }

    public IPlayer EvaluateWinner() //Cek siapa yang menang. Kalau >1 yang menang, hadiah displit. 
    {
        
    }

    public HandRank EvaluateHand(IPlayer player) //cek tingkat kekuatan kartu player
    {
        //tambah 5 kartu dari board ke player.

        foreach(var boardCard in _board)
        {
            _playerHands[player].Add(boardCard);
        }

        
    }

    public void AwardPot(IPlayer winner)
    {
        
    }

    public void StartNewRound() 
    {
        foreach(var p in _players)
        {
            _playerBets[p] = 0;
            _playerCalled[p] = false;
            _playerFolded[p] = false;
            _playerAllIn[p] = false;

            Random rng = new Random();
            _dealerIndex = rng.Next(0, _players.Count);
            _smallBlindIndex = (_dealerIndex + 1) % _players.Count();
            _bigBlindIndex = (_dealerIndex + 2) % _players.Count();
            _currentBetAmount = 0;
            _pots = new List<IPot>();
            _phase = GamePhase.PreFlop;
        }        
    }


    //Handle Action

    public List<ICard> GetHand(IPlayer player)
    {
        return _playerHands[player];
    }

    public void GetTotalChips(IPlayer player)
    {
        int totalChips = _playerChips.Count;
        int totalChipScore = 0;
        foreach(var chip in _playerChips[player])
        {
            totalChipScore += chip.Value;
        }
        
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

    public bool IsBettingRoundOver() //cek apakah jumlah player yang masih belum fold tinggal satu saja atau belum
    {
        int betEndCheck = 0;
        foreach(var p in _players)if(_playerCalled[p])betEndCheck++;
        
        return betEndCheck == _players.Count - 1;
    }

}   