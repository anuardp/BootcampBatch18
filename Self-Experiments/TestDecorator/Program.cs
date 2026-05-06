public interface IBike
{
    string GetDetails();
    double GetPrice();
}

class AluminiumBike : IBike
{
    public double GetPrice() => 100.0;

    public string GetDetails() => "Aluminium Bike";
}

public class CarbonBike : IBike
{
    public double GetPrice() => 1000.0;

    public string GetDetails() => "Carbon";
}


public abstract class BikeAccessories : IBike
{
    private readonly IBike _bike;

    public BikeAccessories(IBike bike)
    {
        _bike = bike;
    }

    public virtual double GetPrice() => _bike.GetPrice();

    public virtual string GetDetails() => _bike.GetDetails();
}

class SecurityPackage : BikeAccessories
{
    public SecurityPackage(IBike bike) : base(bike)
    {
        // ...
    }

    public override string GetDetails() => $"{base.GetDetails()} + Security Package";
    public override double GetPrice() => base.GetPrice() + 1;
}

class SportPackage : BikeAccessories
{
    public SportPackage(IBike bike) : base(bike)
    {
       
    }

    public override string GetDetails() => $"{base.GetDetails()} + Sport Package";

    public override double GetPrice() => base.GetPrice() + 10;
}

public class BikeShop
{
    static void Main(string[] args)
    {
        AluminiumBike basicBike = new AluminiumBike();
        BikeAccessories upgraded = new SportPackage(basicBike);
        upgraded = new SecurityPackage(upgraded);

        CarbonBike sepedaku = new CarbonBike();

        Console.WriteLine($"Bike: '{upgraded.GetDetails()}' Cost: {upgraded.GetPrice()}");
        Console.WriteLine($"Bike: '{sepedaku.GetDetails()}' Cost: {sepedaku.GetPrice()}");
    }
}