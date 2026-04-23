public class Pot : IPot
{
    public int Amount{get; set;}
    public List<IPlayer> EligiblePlayers{get; set;}    

    public Pot(List<IPlayer> eligiblePlayers, int Amount)
    {
        EligiblePlayers = eligiblePlayers ?? new List<IPlayer>();
        Amount = 0;
    }
}
