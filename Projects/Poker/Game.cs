using System.Security.Cryptography;

public class Game
{
    private List<IPlayer> _players {get;set;}
    private List<IDeck> _deck{get; set;}
    private List<IBoard> _board{get; set;}

    private Dictionary<IPlayer, List<ICard>> PlayerHands{get; set;}
    private Dictionary<IPlayer, List<IChip>> PlayerChips{get; set;}
    private Dictionary<IPlayer, List<IChip>> PlayerBets{get; set;}
    private Dictionary<IPlayer, bool> PlayerFolded{get; set;}        
    private Dictionary<IPlayer, bool> PlayerAllIn{get; set;}

    public GamePhase Phase {get; set;}
    private int _dealerIndex;
    private int _smallBlindIndex;
    private int _bigBlindIndex;
    private int _currentPlayerIndex;
    private int _currentBetAmount; 
    private List<IPot> _pots;  // for main pot and side pots

    public int SmallBlind{get; set;}
    public int BigBlind{get; set;}

    public event Action<IPlayer, PlayerAction, int> OnPlayerActed;



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
        _playerFolded = new Dictionary<IPlayer, bool>();
        _playerAllIn = new Dictionary<IPlayer, bool>();

        foreach(var p in players)
        {
            _playerHands[p] = new List<ICard>();
            _playerChips[p] = AmountToChips(1000);   
            _playerBets[p] = 0;
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

    public void ShuffleDeck()
    {
        
    }
    
    public void DrawCard()
    {
        
    }

    public void ResetDeck()
    {
        
    }

    public void AddCardToBoard(ICard card)
    {
        
    }

    public void ResetBoard() //Reset kartu di board -> Setelah satu sesi permainan berakhir 
    {
        
    }

    public void DealHoleCards()
    {
        
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
        
    }
    public void Fold(IPlayer player) //kasih tag fold ke player (player tidak akan bisa bet lagi untuk keseluruhan ronde permainan dan gak bisa menangin chip)
    {
        
    }
    public void Call(IPlayer player) // Player samain jumlah bet yang dikasih ke pot dengan player sebelumnya
    {
        
    }
    public void Raise(IPlayer player) // Player naikin jumlah bet 
    {
        
    }
    public void Check(IPlayer player) //
    {
        
    }
    public void AllIn(IPlayer player)
    {
        
    }
    // public void DecideBotAction(IPlayer bot)
    
    public void AddToPot() //Tambah chip ke dalam Pot
    {
        
    }
    public IPlayer EvaluateWinner()
    {
        
    }

    public HandRank EvaluateHand(IPlayer player)
    {
        
    }

    public void AwardPot(IPlayer winner)
    {
        
    }

    public void StartNewRound()
    {
        
    }

    //Handle Action

    public List<ICard> GetHand(IPlayer player)
    {
        
    }

    public void GetTotalChips(IPlayer player)
    {
        
    }
    public bool IsFolded(IPlayer player)
    {
        
    }

    public bool IsAllIn(IPlayer player)
    {
        
    }
    public bool IsGameEndedEarly()
    {
        
    }

    public bool IsBettingRoundOver()
    {
        
    }

}   