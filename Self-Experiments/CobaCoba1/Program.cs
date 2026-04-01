// Tes Array??

// int[] prime = [2,3,5,7,11,13,17,19,23,29];

// foreach (var prima in prime)
// {   
//     Console.WriteLine(prima);
    
// }

// int[] primeTwo = prime[1..^1];
// Console.Write(primeTwo[^1]);


/*Guess the random number*/
// bool GuessNumber(int number, int ans)
// {
//     return number == ans;
// }
// Random random = new Random();


// int answer = random.Next(1,101);
// // Console.WriteLine(answer);

// Console.Write("Guess number: ");
// int guess = Convert.ToInt32(Console.ReadLine());
// while(!GuessNumber(guess, answer)){
//     // Console.WriteLine("Incorrect! Guess Again..");
//     if(guess<answer)Console.WriteLine("Number too low.");
//     else if(guess>answer)Console.WriteLine("Number too high.");
//     Console.WriteLine("Guess number: ");
//     guess = Convert.ToInt32(Console.ReadLine());
// }
// Console.WriteLine($"Yeah, {guess} is the correct number");


int[,] num = new int[5,5];

for(int i = 0; i < num.GetLength(0); i++)
{
    for(int j = 0; j < num.GetLength(1); j++)
    {
        num[i,j]=i*j;
        Console.Write(num[i,j]);
        if(j<num.GetLength(1))Console.Write(" ");
    }
    Console.WriteLine();
}

















