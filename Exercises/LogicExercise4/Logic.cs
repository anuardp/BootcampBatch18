public class Logic
{
    List<Aturan>Rules;

    public Logic()
    {
        Rules = new List<Aturan>();
    }

    public void AddRules(int num, string word)
    {
        var rule = new Aturan(num, word);
        Aturan ada = Rules.Find(r => r.Number == num);

        if(ada.Number != 0)Console.WriteLine($"Rule with number {num} has already exist. Can't add rule!!");
        else 
        {
            Rules.Add(rule);
            Rules.Sort((a,b)=>a.Number.CompareTo(b.Number)); // Rule yang sudah ada diurut setiap kali ada penambahan rule baru.

            Console.WriteLine($"Rule {num} with '{word}' word has been added...");
        }
    }

    public void RemoveRules(int num)
    {   
        Aturan ada = Rules.Find(r => r.Number == num);
        if(ada.Number != 0)
        {
            string word = ada.Word;
            Rules.Remove(ada);
            Console.WriteLine($"Rule {num} with '{word}' word has been deleted...");
        }
        else Console.WriteLine($"Rule with number {num} doesn't exist. Can't delete a non-existent rule!!");
    }

    public void PrintRules()
    {
        Console.WriteLine("All rules so far: ");
        foreach(var r in Rules)
        {
            Console.WriteLine($"{r.Number} - {r.Word}");
        }
    }

    public string ConcatString(string s, string s2) => s += s2;
    public bool CekBagi(int a, int b) => a % b == 0;
    
    public void ExecuteLogic(int x)
    {
        Console.WriteLine($"Print numbers from 1 to {x}:");
        for(int i = 1; i <= x; i++)
        {
            string answer = "";
            foreach(var r in Rules)
            {
                if(CekBagi(i, r.Number))answer = ConcatString(answer, r.Word);    
            }
            if(answer=="")Console.Write(i); 
            else Console.Write(answer); 

            if(i!=x)Console.Write(", ");
            else Console.WriteLine("\n");
        }
    }
}