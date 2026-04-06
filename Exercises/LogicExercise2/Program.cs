// See https://aka.ms/new-console-template for more information
Console.Write("Input a number: ");
int n = Convert.ToInt32(Console.ReadLine());

for (int x = 1; x <= n; x++)
{
    if(x%3==0 && x%5==0 && x%7==0)Console.Write("foobarjazz");
    else if(x%3==0 && x%5==0)Console.Write("foobar");
    else if(x%3==0 && x%7==0)Console.Write("foojazz");
    else if(x%5==0 && x%7==0)Console.Write("barjazz");
    else if(x%3==0)Console.Write("foo");
    else if(x%5==0)Console.Write("bar");
    else if(x%7==0)Console.Write("jazz");
    else Console.Write(x);
    
    if(x!=n)Console.Write(", ");
    else Console.WriteLine("\n"); //Biar ada 1 baris kosong aja, penanda output selesai
}