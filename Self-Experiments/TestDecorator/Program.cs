public interface IBeverage
{
    string GetDescription();
    double Cost();
}

// ConcreteComponent
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

// Decorator (abstract)
public abstract class CondimentDecorator : IBeverage
{
    protected IBeverage _beverage;

    public CondimentDecorator(IBeverage beverage)
    {
        _beverage = beverage;
    }

    public abstract string GetDescription();
    public abstract double Cost();
}

public class Milk : CondimentDecorator
{
    public Milk(IBeverage beverage) : base(beverage) { }

    public override string GetDescription() =>
        _beverage.GetDescription() + ", Milk";

    public override double Cost() =>
        _beverage.Cost() + 0.30;
}

public class Mocha : CondimentDecorator
{
    public Mocha(IBeverage beverage) : base(beverage) { }

    public override string GetDescription() =>
        _beverage.GetDescription() + ", Mocha";

    public override double Cost() =>
        _beverage.Cost() + 0.40;
}

class Program
{
    static void Main()
    {
        // Espresso + Mocha + Milk
        IBeverage beverage = new Espresso();
        beverage = new Mocha(beverage);
        beverage = new Milk(beverage);

        Console.WriteLine($"{beverage.GetDescription()} : ${beverage.Cost():F2}");
        // Output: Espresso, Mocha, Milk : $2.69

        // HouseBlend + Milk
        IBeverage houseBlend = new HouseBlend();
        houseBlend = new Milk(houseBlend);

        Console.WriteLine($"{houseBlend.GetDescription()} : ${houseBlend.Cost():F2}");
        // Output: House Blend, Milk : $1.19

        // Espresso + double Mocha (ditumpuk dua kali)
        IBeverage doubleMocha = new Espresso();
        doubleMocha = new Mocha(doubleMocha);
        doubleMocha = new Mocha(doubleMocha);

        Console.WriteLine($"{doubleMocha.GetDescription()} : ${doubleMocha.Cost():F2}");
        // Output: Espresso, Mocha, Mocha : $2.79
    }
}