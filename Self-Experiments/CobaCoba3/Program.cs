// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");

namespace CobaCoba3;
    // public delegate int Hitung(int h);
    // class Program
    // {
        
    //     static void Main(string[] args)
    //     {
    //         Console.Write("Input Number 1: ");
    //         int num1 = Convert.ToInt32(Console.ReadLine());
    //         Console.Write("Input Number 2: ");
    //         int num2 = Convert.ToInt32(Console.ReadLine());

    //         Hitung h = Square;
    //         int ans1 = h(num1);
    //         int ans2 = h(num2);

    //         Console.WriteLine($"Jawaban 1: {ans1}");
    //         Console.WriteLine($"Jawaban 2: {ans2}");
    //     }
    //     static int Square(int h) => h*h;
    // }

// class Program
// {
//     static void Main(string[] args)
//     {
//             Console.Write("Input Jumlah Orang: ");
//             int num = Convert.ToInt32(Console.ReadLine());

//             var orang = new List<Dictionary<string, string>>();

//         while (num > 0)
//         {
//             string? name = Convert.ToString(Console.ReadLine());
//             string? job = Convert.ToString(Console.ReadLine());
//             string? age = Convert.ToString(Console.ReadLine());

//             orang.Add(new Dictionary<string, string>
//             {
//                {"Name", name}, 
//                {"Age", age}, 
//                {"Job", job}, 
//             });

//             num--;
//         }
//         foreach(var org in orang)
//         {
//             foreach(var data in org)
//             {
//                 Console.WriteLine($"{data.Key}: {data.Value}");
//             }
//         }
//     }
// }

class Program
{
    public delegate void PriceChangedHandler(decimal oldPrice, decimal newPrice);
    
    public class Broadcaster
    {
        public event PriceChangedHandler PriceChanged;
    }
    static void Main(string[] args)
    {

    }
}