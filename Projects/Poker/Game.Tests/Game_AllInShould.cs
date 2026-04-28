using NUnit.Framework;
using Game;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Game.UnitTests;

[TextFixture]
public class Game_AllInShould
{
    private Game _game;
    [SetUp]
    public void Setup()
    {
        _game = new Game();
    }
    [Test]
    public void AllIn_DoesPlayerBetAllTheChips()
    {
        IPlayer player = _game.currentPlayer();
        
        int previousTotalChip = _game.GetTotalChips(player);
        _game.AllIn(player);
        int currentTotalChip = _game.GetTotalChips(player);
        

        Assert.That((currentTotalChip == 0 && previousTotalChip > 0), Is.True, $"The player indeed has bet all of their chips..");
    }
    
    [TestCase()]
}

