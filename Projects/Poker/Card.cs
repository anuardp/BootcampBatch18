public class Card : ICard
{
    Suit Suit{get;}
    Rank Rank{get;}s

    // Suit ICard.Suit => Suit;
    // Rank ICard.Rank => Rank;

    public Card(Rank rank, Suit suit)
    {
        Rank = rank;
        Suit = suit;
    }
}