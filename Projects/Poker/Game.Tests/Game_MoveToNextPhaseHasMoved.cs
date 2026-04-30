using NUnit.Framework;
// using Game;
using System.Linq;

namespace Game.UnitTests;

[TextFixture]
public class Game_MovetoNextPhaseHasMoved
{
    private Game _game;
    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }
    [Test]
    public void MoveToNextPhase_IsAlreadyMoveToNextPhase()
    {
        
        GamePhase previousPhase = _game.GetPhase();
        GamePhase currentPhase = _game.MoveToNextPhase(previousPhase);

        Assert.That(previousPhase != currentPhase, Is.True, "The game has moved to next phase!" );
    }
    [TestCase(GamePhase.PreFlop)]
    [TestCase(GamePhase.Flop)]
    [TestCase(GamePhase.Turn)]
    [TestCase(GamePhase.River)]

    public void MoveToNextPhase_IsCurrentGamePhaseShowdown()
    {
        GamePhase previousPhase = _game.GetPhase();
        GamePhase currentPhase = _game.MoveToNextPhase(previousPhase);
        Assert.That(previousPhase != currentPhase, Is.False, "The game already reached Showdown phase, it's already over..." );
    }
    [TestCase(GamePhase.Showdown)]
}