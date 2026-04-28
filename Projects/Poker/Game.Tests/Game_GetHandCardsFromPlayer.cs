using NUnit.Framework;
// using Game;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Game.UnitTests;

[TextFixture]`
public class Game_GetHandCardsFromPlayer
{
    private Game _game;
    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }
    [Test]
    public void GetHand_GetCardsFromPlayerHands()
    {
        IPlayer player = _game.GetCurrentPlayer();
        Assert.That(_game.GetHand(player) == _game.playerHands[player], Is.True, "This is exactly the card from the player..")
        
    }
    
    [TestCase()]
}

