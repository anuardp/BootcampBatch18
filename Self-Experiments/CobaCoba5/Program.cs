using Spectre.Console;

// var panel = new Panel("Hello, World!");
// panel.Border(BoxBorder.Rounded);
// panel.Header("Welcome");
// AnsiConsole.Write(panel);

// Panel card = new Panel("[white4    \n♠    \n    \n   ♠\n   4][/]");
// card.Border(BoxBorder.Square);



// Console.BackgroundColor = ConsoleColor.Green;
// Panel panel = new Panel("[green][/]")
//     .Header("Status")
//     .Border(BoxBorder.Double)
//     .BorderColor(Color.Green)
//     .Padding(1, 1, 1, 1);

// AnsiConsole.Write(panel);


Panel panel = new Panel("");

string testKartu = "3    \n♣    \n    ♣\n    3";
string kartu ="10   \n♠    \n    ♠\n   10";
string kartu2 ="2    \n♦    \n    ♦\n    2";
string kartuTutup = "/////\n/////\n/////\n/////";
Panel cardTest= new Panel($"[black on white]{testKartu}[/]").Padding(0,0,0,0).Border(BoxBorder.Rounded);
Panel card = new Panel(new Markup($"[black on white]{kartu}[/]")).Padding(0,0,0,0).Border(BoxBorder.Rounded);
Panel card2 = new Panel(new Markup($"[red on white]{kartu2}[/]")).Padding(0,0,0,0).Border(BoxBorder.Rounded);
Panel closedCard = new Panel(new Markup($"[white on black]{kartuTutup}[/]")).Padding(0,0,0,0).Border(BoxBorder.Rounded);



AnsiConsole.Write(cardTest);
Console.WriteLine();
AnsiConsole.Write(closedCard);
Console.WriteLine();



//    Panel panel1 = new Panel();
//    TextBox textBox1 = new TextBox();
//    Label label1 = new Label();
   
//    // Initialize the Panel control.
//    panel1.Location = new Point(56,72);
//    panel1.Size = new Size(264, 152);
//    // Set the Borderstyle for the Panel to three-dimensional.
//    panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;

//    // Initialize the Label and TextBox controls.
//    label1.Location = new Point(16,16);
//    label1.Text = "label1";
//    label1.Size = new Size(104, 16);
//    textBox1.Location = new Point(16,32);
//    textBox1.Text = "";
//    textBox1.Size = new Size(152, 20);

//    // Add the Panel control to the form.
//    this.Controls.Add(panel1);
//    // Add the Label and TextBox controls to the Panel.
//    panel1.Controls.Add(label1);
//    panel1.Controls.Add(textBox1);

Columns columns = new Columns(
    new Panel("First column"),
    new Panel("Second column"),
    new Panel("Third column"));




Columns cards = new Columns(
    card,
    card2
)
{
    Padding = new Padding(0,0,0,0),
    Expand = false
};

Columns testCards = new Columns(
    cardTest,
    closedCard
)
{
    Padding = new Padding(0,0,0,0),
    Expand = false
};
// grid.AddColumn(new GridColumn {Width = 20});
// grid.AddColumn(new GridColumn {Width = 20});
// grid.AddColumn(new GridColumn {Width = 20});

AnsiConsole.Write(cards);

Console.WriteLine();

AnsiConsole.Write(
    new Align(cards, HorizontalAlignment.Right)
);


//Anggap aja ini Meja
// Panel card = new Panel(new Markup($"[black on white]{kartu}[/]")).Padding(0,0,0,0).Border(BoxBorder.None);

string table = "";
int w = 60;
int h = 18;

while (h > 0)
{
    int tmp = w;
    while(tmp>0)
    {
        table+=" ";
        tmp--;
    }
    h--;
    table+="\n";
}

// Console.WriteLine(table);

Panel pokerTable = new Panel(new Markup($"[on green]{table}[/]")).Padding(0,0,0,0).Border(BoxBorder.None);
AnsiConsole.Write(new Align(pokerTable, HorizontalAlignment.Center));
AnsiConsole.Write(
    new Align(cards, HorizontalAlignment.Center)
);

//Gabungin meja dengan kartu


Rows mixedTableAndCards = new Rows(testCards, pokerTable).Expand();

AnsiConsole.Write(
    new Align(new Panel(mixedTableAndCards), HorizontalAlignment.Center)
);




Grid grid = new Grid();

grid.AddColumn(new GridColumn { Width = 20, Alignment = Justify.Right });
grid.AddColumn(new GridColumn());

grid.AddRow(
    new Text("System Information", new Style(Color.Yellow, decoration: Decoration.Bold)),
    new Text(""));
  
grid.AddEmptyRow();
  
// Add data rows
grid.AddRow(new Markup("OS:"), new Markup("[blue]Linux[/]"));
grid.AddRow(new Markup("CPU:"), new Markup("[green]8 cores @ 3.2GHz[/]"));
grid.AddRow(
    new Markup("Memory:"),
    new BreakdownChart()
        .Width(40)
        .AddItem("Used", 12, Color.Red)
        .AddItem("Available", 4, Color.Green));
grid.AddRow(
    new Markup("Disk:"),
    new Panel("[yellow]65% used[/]")
        .BorderColor(Color.Yellow));
  
AnsiConsole.Write(grid);

