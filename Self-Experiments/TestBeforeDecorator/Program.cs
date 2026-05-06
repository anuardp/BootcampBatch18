public interface IBeverage
{
    string GetDescription();
    double Cost();
}

public class Espresso : IBeverage
{
    public string GetDescription() => "Espresso";
    public double Cost() => 1.99;
}
public class HouseBlend : IBeverage
{
    public string GetDescription() => "House Blend";
    public double Cost() => 0.89;
}


public class EspressoWithMilk : IBeverage
{
    public string GetDescription() => "Espresso, Milk";
    public double Cost() => 1.99 + 0.30;
}
public class EspressoWithMocha : IBeverage
{
    public string GetDescription() => "Espresso, Mocha";
    public double Cost() => 1.99 + 0.40;
}
// Espresso + Milk + Mocha
public class EspressoWithMilkMocha : IBeverage
{
    public string GetDescription() => "Espresso, Milk, Mocha";
    public double Cost() => 1.99 + 0.30 + 0.40;
}

// HouseBlend + Milk
public class HouseBlendWithMilk : IBeverage
{
    public string GetDescription() => "House Blend, Milk";
    public double Cost() => 0.89 + 0.30;
}

// HouseBlend + Mocha
public class HouseBlendWithMocha : IBeverage
{
    public string GetDescription() => "House Blend, Mocha";
    public double Cost() => 0.89 + 0.40;
}

// HouseBlend + Milk + Mocha
public class HouseBlendWithMilkMocha : IBeverage
{
    public string GetDescription() => "House Blend, Milk, Mocha";
    public double Cost() => 0.89 + 0.30 + 0.40;
}

class Program
{
    static void Main()
    {
        var order1 = new EspressoWithMilkMocha();
        Console.WriteLine($"{order1.GetDescription()} : ${order1.Cost():F2}");

        var order2 = new HouseBlendWithMilk();
        Console.WriteLine($"{order2.GetDescription()} : ${order2.Cost():F2}");
    }
}