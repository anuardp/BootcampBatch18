public class Deck : IDeck
{
    List<ICard> Cards{get;}

    public Deck(List<ICard> cards)
    {
        Cards = cards;
    }    
}