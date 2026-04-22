using System.Security.Cryptography;

public class Game
{
    private List<IPlayer> _players {get;set;}
    private List<IDeck> _deck{get; set;}
    private List<IBoard> _board{get; set;}

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



    public Game(List<IPlayer> players, List<IDeck> deck, List<IBoard> board, int smallBlind, int bigBlind)
    {
        _players = players;
        _deck = deck;
        _board = board;
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
            }
        }
        return chips;
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

    public void ShuffleDeck()
    {
        
    }
    
    public void DrawCard() //bagi kartu ke masing-masing player
    {
        
    }

    public void ResetDeck()
    {
        _deck.RemoveAll();
    }

    public void AddCardToBoard(IBoard board, ICard card) //5 buah kartu ke atas board dari deck
    {
        
    }

    public void ResetBoard() //Reset kartu di board -> Setelah satu sesi permainan berakhir 
    {
        _board.RemoveAll();
        
    }

    public void DealHoleCards() // Bagi 2 kartu ke masing-masing player
    {
        int i = 0;
        while (i < 2)
        {
            foreach(var p in _players)
            {
                _playerHands[p].Add(_deck[0]);       
            }   
        }
    }

    public void DealFlop() //Buka 3 kartu pertama diatas board (3 kartu pertama diperlihatkan)
    {
        
    }
    public void DealTurn() //Buka kartu ke-4 
    {
        
    }
    public void DealRiver() //Buka kartu ke-5
    {
        
    }

    public void MoveToNextPhase() // Pindah ke fase ronde berikutnya (misal dari Flop ke Turn, Turn ke River)
    {
        if(_phase == GamePhase.PreFlop) _phase = GamePhase.Flop;
        else if(_phase == GamePhase.Flop) _phase = GamePhase.Turn;
        else if(_phase == GamePhase.Turn) _phase = GamePhase.River;
        else if(_phase == GamePhase.River) _phase = GamePhase.Showdown;
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
    public void Check(IPlayer player) //Skip turn untuk sementara (tidak bet, tapi tidak fold). Hanya berlaku jika bet saat ini nilainya masih 0
    {
        
    }
    public void AllIn(IPlayer player) //Player bet semua chip yang dipunya...
    {
        _playerAllIn[player] = true;


        
    }
    // public void DecideBotAction(IPlayer bot)
    
    public void AddToPot() //Tambah chip ke dalam Pot
    {
        
    }

    public void SwitchTurn(IPlayer player) //Ganti giliran pemain
    {
        
    }

    public IPlayer EvaluateWinner()
    {
        
    }

    public HandRank EvaluateHand(IPlayer player) //cek tingkat kekuatan kartu player
    {
        //tambah 5 kartu dari board ke player.
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
            _dealerIndex = rng.Next(0, p.Count);
            _smallBlindIndex = (_dealerIndex + 1) % p.Count();
            _bigBlindIndex = (_dealerIndex + 2) % p.Count();
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
        foreach(var chip in _playerChips.)
        {
            totalChipScore += _playerChips.Values;
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

    public bool IsBettingRoundOver() //cek apakah semua player yg tidak fold memutuskan untuk check atau all-in.
    {
        int betEndCheck = 0;
        foreach(var p in _players)if(_playerCalled[p] || _playerAllIn[p])betEndCheck++;
        
        if(betEndCheck == _players.Count)return true;
        else return false;
    }

}   