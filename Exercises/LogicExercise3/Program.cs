// See https://aka.ms/new-console-template for more information
Console.Write("Input a number: ");
int n = Convert.ToInt32(Console.ReadLine());

for (int x = 1; x <= n; x++)
{   
    string answer = ""; 

    if(x%3==0)answer+="foo";   
    if(x%4==0)answer+="baz";   
    if(x%5==0)answer+="bar";   
    if(x%7==0)answer+="jazz";   
    if(x%9==0)answer+="huzz";   

    if(answer=="")Console.Write(x); //Kalau nilai answer tetap string kosong, bilangan tersebut tidak habis dibagi oleh salah satu dari 3, 4, 5, 7, dan 9. Output nilai x.
    else Console.Write(answer); //Kalau nilai x habis dibagi oleh minimal salah satu dari 3, 4, 5, 7, dan 9, output adalah nilai dari answer.

    if(x!=n)Console.Write(", ");
    else Console.WriteLine("\n"); //Biar ada 1 baris kosong aja, penanda output selesai
}