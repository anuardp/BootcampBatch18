// // See https://aka.ms/new-console-template for more information


// using System;
// namespace InheritanceDemo
// {
//     class A
//     {
//         public A(int number)
//         {
//             Console.WriteLine($"Class A Constructor is Called : {number}");
//         }
//         public void Method1()
//         {
//             Console.WriteLine("Method 1");
//         }
//         public void Method2()
//         {
//             Console.WriteLine("Method 2");
//         }
//     }

//     class B : A
//     {
//         public B(int num) : base(num)
//         {
//             Console.WriteLine($"Class B Constructor is Called : {num}");
//         }
//         public void Method3()
//         {
//             Console.WriteLine("Method 3");
//         }
//         static void Main()
//         {
//             B obj1 = new B(10);
//             B obj2 = new B(20);
//             B obj3 = new B(30);
//             Console.ReadKey();
//         }
//     }
// }


// using System;
// namespace ReadOnlyStructsDemo
// {
//     //Creating a Structure using the struct
//     //struct in C# are value type
//     public struct Rectangle
//     {
//         public double Height { get; set; }
//         public double Width { get; set; }

//         //Property Expression Bodied Member
//         public double Area => (Height * Width);

//         //Constructor Initializing the Height and Width Properties
//         public Rectangle(double height, double width)
//         {
//             Height = height;
//             Width = width;
//         }

//         //Overriding the Object Class ToString() Method to return the Height, Width, and Area
//         //But ToString Method is not chaning the Values of Height, Width, and Area Properties
//         public override string ToString()
//         {
//             return $"(Total area for height: {Height}, width: {Width}) is {Area}";
//         }
//     }

//     class Program
//     {
//         static void Main(string[] args)
//         {
//             //Creating an Instance of the Rectangle Class
//             Rectangle rectangle = new Rectangle(10, 20);

//             //Get the Height of Rectangle
//             Console.WriteLine("Height: " + rectangle.Height);

//             //Get the Width of Rectangle
//             Console.WriteLine("width: " + rectangle.Width);

//             //Get the Area of Rectangle
//             Console.WriteLine("Rectangle Area: " + rectangle.Area);

//             //Get the Height, Width, and Area of Rectangle
//             //In this case, it will internally invoke the override ToString Method
//             Console.WriteLine("Rectangle: " + rectangle);
//             Console.ReadKey();
//         }
//     }
// }
using System;

namespace CobaCoba2
{
    public struct Coords
    {
        public Coords(double x, double y = 0)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        public override string ToString() => $"({X}, {Y})";
        public double DistanceFromOrigin() => Math.Sqrt(X*X + Y*Y);

        public void Program()
        {
            
        }
    }  
    class Program {
        static void Main(string[] args) {
            // Your code here
            Coords c1 = new Coords(2,4);   // Calls explicit constructor: p1.x will be 1, p1.y will be 1
                // Coords c2 = default;       // Calls implicit default constructor: p2.x will be 0, p2.y will be 0 
            Coords c2 = new Coords(3);
            Console.WriteLine(c1);
            Console.WriteLine(c2);
            Console.WriteLine($"Jarak dari titik 0: {c1.DistanceFromOrigin()}");
            Console.WriteLine(c1.X);
            Console.WriteLine(c1.Y);
        }
    }   
}


