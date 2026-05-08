using System.Diagnostics;

// int Fibonacci(int n)
// {
//     if (n == 0) return 0;
//     if (n == 1) return 1;
//     return Fibonacci(n - 1) + Fibonacci(n - 2);
// }

// int number = 15;
// Console.WriteLine(Fibonacci(number)); // Output: 610



// void TestFibPerformance()
// {
//     Stopwatch sw = Stopwatch.StartNew();
//     int result = Fibonacci(35);
//     sw.Stop();
//     Console.WriteLine($"Fibonacci(35) = {result}, time = {sw.ElapsedMilliseconds} ms");
//     if (sw.ElapsedMilliseconds > 1000)
//         Console.WriteLine("Too Slow...");
// }
// TestFibPerformance();





int Fibonacci(int n)
{
    if (n <= 1) return n;
    int a = 0, b = 1;
    for (int i = 2; i <= n; i++)
    {
        int tmp = a + b;
        a = b;
        b = tmp;
    }
    return b;
}
Console.WriteLine(Fibonacci(45)); 

void VerifyFix()
{
    int[] testInputs = { 0, 1, 2, 10, 45 };
    int[] expected = { 0, 1, 1, 55, 1134903170 };
    
    for (int i = 0; i < testInputs.Length; i++)
    {
        int result = Fibonacci(testInputs[i]);
        if (result != expected[i])
        {
            Console.WriteLine($"GAGAL: Fibonacci({testInputs[i]}) = {result}, seharusnya {expected[i]}");
            return;
        }
        Console.WriteLine($"Input: {testInputs[i]} - Expected result: {expected[i]} - Reality: {Fibonacci(testInputs[i])}");
    }
    Console.WriteLine("✅ Semua nilai benar");


    Stopwatch sw = Stopwatch.StartNew();
    Fibonacci(45);
    sw.Stop();
    if (sw.ElapsedMilliseconds < 100)
        Console.WriteLine($"Performa baik: {sw.ElapsedMilliseconds} ms");
    else
        Console.WriteLine($"Performa lambat: {sw.ElapsedMilliseconds} ms");


    Console.WriteLine("Verifikasi selesai. Bug dinyatakan FIXED.");
}

VerifyFix();