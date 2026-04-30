using NUnit.Framework;
// using Game;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Game.UnitTests;

[TextFixture]
public class Game_DealHoleCardsDrawed
{
    private Game _game;
    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }
    [Test]
    public void ResetDeck_DoesDeckAlreadyReset()
    {
        List<Rank> ranks = new List<Rank>();
        ranks.AddRange(Rank.GetValues<Rank>());
        List<Suit> suits = new List<Suit>();
        suits.AddRange(Suit.GetValues<Suit>());
        
        List<ICard> deck = new List<ICard>();
        foreach(Rank rank in ranks)
        {
            foreach(Suit suit in suits)
            {
                deck.Add(new Card(suit, rank));
            }
        }
        List<IPlayer> players = new List<IPlayer>();
        players.Add(new Player("1", "Adi", "human"));
        players.Add(new Player("2", "Budi", "human"));
        players.Add(new Player("3", "Chandra", "human"));
        players.Add(new Player("4", "Dodo", "human"));

        Dictionary<IPlayer, List<ICard>> playerHands = new Dictionary<IPlayer, List<ICard>>();
        
        List<ICard> allCardsForPlayers = deck[0..(players.Count*2)];
        _game.DealHoleCards(playerHands, deck);
        bool cekBagiKartuSesuai = true;
        int counter = 0;
        foreach(IPlayer p in playerHands.Keys)
        {
            if(playerHands[p][0]!=deck[counter] || playerHands[p][1] != deck[counter + players.Count])
            {
                cekBagiKartuSesuai = false;
            }
        }
        Assert.That(cekBagiKartuSesuai, Is.True, "Each players already receive 2 cards as the rule stated." );
    }
    List<ICard> deck = new List<ICard>
    {
        {new Card(Suit.Hearts, Rank.Nine)},
        {new Card(Suit.Hearts, Rank.Seven)},
        {new Card(Suit.Clubs, Rank.Four)},
        {new Card(Suit.Clubs, Rank.Six)},
        {new Card(Suit.Spades, Rank.Queen)},
        {new Card(Suit.Clubs, Rank.Three)},
        {new Card(Suit.Spades, Rank.Three)},
        {new Card(Suit.Spades, Rank.Four)},
        {new Card(Suit.Spades, Rank.Five)},
        {new Card(Suit.Spades, Rank.Six)},
        {new Card(Suit.Clubs, Rank.Five)},
        {new Card(Suit.Spades, Rank.Seven)},
        {new Card(Suit.Clubs, Rank.Queen)},
        {new Card(Suit.Clubs, Rank.King)},
        {new Card(Suit.Hearts, Rank.Three)},
        {new Card(Suit.Clubs, Rank.Ace)},
        {new Card(Suit.Spades, Rank.Two)},
        {new Card(Suit.Clubs, Rank.Nine)},
        {new Card(Suit.Clubs, Rank.Jack)},
        {new Card(Suit.Hearts, Rank.Two)},
        {new Card(Suit.Spades, Rank.Eight)},
        {new Card(Suit.Clubs, Rank.Two)},
        {new Card(Suit.Spades, Rank.Nine)},
        {new Card(Suit.Diamonds, Rank.Jack)},
        {new Card(Suit.Spades, Rank.Ten)},
        {new Card(Suit.Diamonds, Rank.Eight)},
        {new Card(Suit.Hearts, Rank.Four)},
        {new Card(Suit.Diamonds, Rank.King)},
        {new Card(Suit.Diamonds, Rank.Ace)},
        {new Card(Suit.Clubs, Rank.Seven)},
        {new Card(Suit.Diamonds, Rank.Seven)},
        {new Card(Suit.Clubs, Rank.Eight)},
        {new Card(Suit.Hearts, Rank.Ace)},
        {new Card(Suit.Spades, Rank.King)},
        {new Card(Suit.Diamonds, Rank.Nine)},
        {new Card(Suit.Hearts, Rank.Ten)},
        {new Card(Suit.Clubs, Rank.Ten)},
        {new Card(Suit.Diamonds, Rank.Ten)},
        {new Card(Suit.Spades, Rank.Jack)},
        {new Card(Suit.Spades, Rank.Ace)},
        {new Card(Suit.Diamonds, Rank.Two)},
        {new Card(Suit.Hearts, Rank.Eight)},
        {new Card(Suit.Diamonds, Rank.Three)},
        {new Card(Suit.Hearts, Rank.Six)},
        {new Card(Suit.Hearts, Rank.Jack)},
        {new Card(Suit.Diamonds, Rank.Queen)},
        {new Card(Suit.Hearts, Rank.Five)},
        {new Card(Suit.Hearts, Rank.Queen)},
        {new Card(Suit.Diamonds, Rank.Four)},
        {new Card(Suit.Diamonds, Rank.Five)},
        {new Card(Suit.Hearts, Rank.King)},
        {new Card(Suit.Diamonds, Rank.Six)}
    };
    [TestCase(deck)]
}

