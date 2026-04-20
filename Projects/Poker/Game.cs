using System.Security.Cryptography;

public class Game
{
    public List<IPlayer> Players {get;set;}
    public List<IDeck> Deck{get; set;}
    public List<IBoard> Board{get; set;}

    public Dictionary<IPlayer, List<ICard>> PlayerHands{get; set;}
    public Dictionary<IPlayer, List<IChip>> PlayerChips{get; set;}
    public Dictionary<IPlayer, List<IChip>> PlayerBets{get; set;}
    public Dictionary<IPlayer, bool> PlayerFolded{get; set;}        
    public Dictionary<IPlayer, bool> PlayerAllIn{get; set;}

    public GamePhase Phase {get; set;}


    public Game(List<IPlayer> players, List<IDeck> deck, List<IBoard> board)
    {
        Players = players;
        Deck = deck;
        Board = board;
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