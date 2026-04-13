using System;

// public class StockPriceChangedEventArgs : EventArgs
// {
//     public string Symbol { get; }
//     public double OldPrice { get; }
//     public double NewPrice { get; }

//     public StockPriceChangedEventArgs(string symbol, double oldPrice, double newPrice)
//     {
//         Symbol = symbol;
//         OldPrice = oldPrice;
//         NewPrice = newPrice;
//     }
// }

// public class Stock
// {
//     public string Symbol { get; }
//     private double _price;

//     public event EventHandler<StockPriceChangedEventArgs>? PriceChanged;

//     public Stock(string symbol, double initialPrice)
//     {
//         Symbol = symbol;
//         _price = initialPrice;
//     }

//     public double Price
//     {
//         get => _price;
//         set
//         {
//             if (_price != value)
//             {
//                 var old = _price;
//                 _price = value;
//                 OnPriceChanged(old, value);
//             }
//         }
//     }

//     protected virtual void OnPriceChanged(double oldPrice, double newPrice)
//     {
//         PriceChanged?.Invoke(this, new StockPriceChangedEventArgs(Symbol, oldPrice, newPrice));
//     }
// }

// public class Program
// {
//     public static void Main()
//     {
//         Stock msft = new Stock("MSFT", 300.00);

//         Stock ihsg = new Stock("IHSG", 400.00);

//         // Subscribe
//         msft.PriceChanged += OnStockPriceChanged;
//         msft.PriceChanged += (s, e) => Console.WriteLine($"Lambda: {e.Symbol} changed to {e.NewPrice}");

//         ihsg.PriceChanged += OnStockPriceChanged;

//         msft.Price = 310.50;   // Triggers event
//         msft.Price = 305.75;   // Triggers again

//         ihsg.Price = 443.25;

//         // Unsubscribe
//         msft.PriceChanged -= OnStockPriceChanged;
//     }

//     private static void OnStockPriceChanged(object? sender, StockPriceChangedEventArgs e)
//     {
//         Console.WriteLine($"{e.Symbol}: {e.OldPrice:F2} → {e.NewPrice:F2}");
//     }

//     static void AlertHugeStockPriceChanged(object? sender, StockPriceChangedEventArgs e)
//     {
//         if(Math.Abs(e.OldPrice - e.NewPrice))Console.WriteLine("Alert!! Stock Price Changed >10%!!");
//     }
// }


// public class TemperatureEventArgs : EventArgs
// {
//     public double Celsius { get; }
//     public TemperatureEventArgs(double celsius) => Celsius = celsius;
// }

// public class Thermometer
// {
//     public event EventHandler<TemperatureEventArgs>? TemperatureChanged;

//     protected virtual void OnTemperatureChanged(double celsius)
//     {
//         TemperatureChanged?.Invoke(this, new TemperatureEventArgs(celsius));
//     }

//     private double _currentTemp;
//     public double CurrentTemp
//     {
//         get => _currentTemp;
//         set
//         {
//             if (_currentTemp != value)
//             {
//                 _currentTemp = value;
//                 OnTemperatureChanged(value);
//             }
//         }
//     }
// }
// class Program
// {
//     static void Main(string[] args)
//     {
//         var thermo = new Thermometer();
//         thermo.TemperatureChanged += (s, e) => Console.WriteLine($"New temp: {e.Celsius}°C");
//         thermo.CurrentTemp = 23.5;   // triggers event
//         thermo.CurrentTemp = 24.5;   // triggers event
//         thermo.CurrentTemp = 26;   // triggers event
//     }
// }







// using System;

// public class StockPriceChangedEventArgs : EventArgs
// {
//     public string Symbol { get; }
//     public double OldPrice { get; }
//     public double NewPrice { get; }

//     public StockPriceChangedEventArgs(string symbol, double oldPrice, double newPrice)
//     {
//         Symbol = symbol;
//         OldPrice = oldPrice;
//         NewPrice = newPrice;
//     }
// }

// public class Stock
// {
//     public string Symbol { get; }
//     private double _price;

//     public event EventHandler<StockPriceChangedEventArgs>? PriceChanged;

//     public Stock(string symbol, double initialPrice)
//     {
//         Symbol = symbol;
//         _price = initialPrice;
//     }

//     public double Price
//     {
//         get => _price;
//         set
//         {
//             if (_price != value)
//             {
//                 var old = _price;
//                 _price = value;
//                 OnPriceChanged(old, value);
//             }
//         }
//     }

//     protected virtual void OnPriceChanged(double oldPrice, double newPrice)
//     {
//         PriceChanged?.Invoke(this, new StockPriceChangedEventArgs(Symbol, oldPrice, newPrice));
//     }
// }

// public class Program
// {
//     public static void Main()
//     {
//         Stock msft = new Stock("MSFT", 300.00);

//         // Subscribe
//         msft.PriceChanged += OnStockPriceChanged;
//         msft.PriceChanged += (s, e) => Console.WriteLine($"Lambda: {e.Symbol} changed to {e.NewPrice}");
//         msft.PriceChanged += PercentageChanged;

//         msft.Price = 310.50;   // Triggers event
//         msft.Price = 305.75;   // Triggers again

//         // Unsubscribe
//         msft.PriceChanged -= OnStockPriceChanged;
//     }

//     private static void OnStockPriceChanged(object? sender, StockPriceChangedEventArgs e)
//     {
//         Console.WriteLine($"{e.Symbol}: {e.OldPrice:F2} → {e.NewPrice:F2}");
//     }

//     private static void PercentageChanged(object? sender, StockPriceChangedEventArgs e)
//     {
//         Console.WriteLine($"Percentage changed: {Math.Abs((e.OldPrice - e.NewPrice)/e.OldPrice)*100:F2}%");
//     }
// }
object o = 4.5;
int? x = o as int?; // 'o' is not an int, so 'x' becomes null
Console.WriteLine(x.HasValue); // False

Console.WriteLine($"{false | null}");

