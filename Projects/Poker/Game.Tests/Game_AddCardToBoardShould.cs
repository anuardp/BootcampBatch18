using NUnit.Framework;
// using Game;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Game.UnitTests;

[TextFixture]
public class Game_AddCardToBoardShould
{
    private Game _game;
    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }
    [Test]
    public void AddCardToBoard_IsOneCardAddedOnBoard()
    {
        List<ICard> board = _game.GetBoard();
        GamePhase phase = _game.GetPhase();

        int previousCardsOnBoard = board.Count;
        int currentCardsOnBoard = _game.AddCardToBoard(board, phase).Count

        Assert.That(currentCardsOnBoard - previousCardsOnBoard == 1, Is.True, $"One card has been added to board on {phase} phase" );
    }
    List<ICard> deck = new List<ICard>
    [TestCase()]
}

