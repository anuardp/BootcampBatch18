public class Pot : IPot
{
    public int Amount{get; set;}
    public List<IPlayer> EligiblePlayers{get; set;}    

    public Pot(List<IPlayer> EligiblePlayers)
    {
        EligiblePlayers = List<IPlayer>(EligiblePlayers);
        Amount = 0;
    }
}
