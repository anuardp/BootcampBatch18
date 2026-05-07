using System.Diagnostics;

int Fibonacci(int n)
{
    if (n == 0) return 0;
    if (n == 1) return 1;
    return Fibonacci(n - 1) + Fibonacci(n - 2);
}

// int number = 45;
// Console.WriteLine(Fibonacci(number));




void TestFibPerformance()
{
    Stopwatch sw = Stopwatch.StartNew();
    int result = Fibonacci(35);
    sw.Stop();
    Console.WriteLine($"Fibonacci(35) = {result}, time = {sw.ElapsedMilliseconds} ms");
    if (sw.ElapsedMilliseconds > 1000)
        Console.WriteLine("Too Slow...");
}
TestFibPerformance();
