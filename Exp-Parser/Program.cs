namespace Exp_Parser;
class Program
{



    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Tokenizer tokenizer = new Tokenizer();
        var asd = tokenizer.Tokenize("(x-1)sin(2)/2(3^2(3-y)^2^2)");
        var ase = new Parser(asd).Parse();
        AstVisualizer visualizer = new AstVisualizer();
        Console.WriteLine(visualizer.Visualize(ase));
            
        Console.ReadKey();
        return;
    }
}