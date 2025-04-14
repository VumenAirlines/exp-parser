namespace Exp_Parser;
class Program
{
    static string[] expressions =
    [
        "-1(max(0,max(-2(e^(3-n))(x-1)^ro(a))))"
    ];



    static void Main(string[] args)
    {
     
        Tokenizer tokenizer = new Tokenizer();
        AstVisualizer visualizer = new AstVisualizer();
        List<string> res = expressions.Select(testCase => visualizer.Visualize(new Parser(tokenizer.Tokenize(testCase)).Parse())).ToList();
        Console.WriteLine(res[0]);
        
        
            
        Console.ReadKey();
        return;
    }
}