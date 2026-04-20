public class Player : IPlayer
{
    string Id{get; set;}
    string Name{get; set;}

    public Player(string id, string name)
    {
        Id = id;
        Name = name;
    }

    
}