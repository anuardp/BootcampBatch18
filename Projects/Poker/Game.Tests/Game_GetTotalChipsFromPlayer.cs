using NUnit.Framework;
// using Game;
using System.Linq;
using System.Security.Cryptograph   y.X509Certificates;

namespace Game.UnitTests;

[TextFixture]
public class Game_GetTotalChipsFromPlayer
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
        
        Assert.That(cekBagiKartuSesuai, Is.True, "Each players already receive 2 cards as the rule stated." );
    }
    [TestCase(deck)]
}

