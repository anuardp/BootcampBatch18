public class Chip : IChip
{
    int Value{get;}
    string Color{get;}

    public Chip(int value, string color)
    {
        Value = value;
        Color = color;
    }
}