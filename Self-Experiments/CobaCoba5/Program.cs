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
int w = Console.LargestWindowHeight;
int h = Console.LargestWindowHeight;

string kartu ="10   \n♠    \n    ♠\n   10";
string kartu2 ="2    \n♦    \n    ♦\n    2";
Panel card = new Panel(new Markup($"[black on white]{kartu}[/]")).Padding(0,0,0,0).Border(BoxBorder.None);
Panel card2 = new Panel(new Markup($"[red on white]{kartu2}[/]")).Padding(0,0,0,0).Border(BoxBorder.None);
// AnsiConsole.Write(card);
// Console.WriteLine();
// AnsiConsole.Write(card2);


// AnsiConsole.MarkupLine("[#00FF00]HIJAUUUUUUUUUU[/]");
// AnsiConsole.MarkupLine("[on white]Highlighted[/]");


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

Grid grid = new Grid();


Columns cards = new Columns(
    card,
    card2
)
{
    Padding = new Padding(2,0,2,0),
    Expand = false
};
grid.AddColumn(new GridColumn {Width = 20});
grid.AddColumn(new GridColumn {Width = 20});
grid.AddColumn(new GridColumn {Width = 20});

AnsiConsole.Write(cards);