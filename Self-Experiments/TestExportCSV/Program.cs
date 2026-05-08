using System.Text;


StringBuilder builder = new StringBuilder();
builder.AppendLine("Id, Name, Email");
builder.AppendLine("1,Alice,alice@example.com"); 
builder.AppendLine("2,Bob,bob@example.com"); 


File.WriteAllText("users.csv", builder.ToString());