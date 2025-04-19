using Exp_Parser.Engine;
using Exp_Parser.Model.Tokens;

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
        var asd = tokenizer.Tokenize("-1+3*4");//(5x/250)^2sin(35x)*log(10,9)*-1");

        var parser = Parser.BuildTree(asd);


        
            
        Console.ReadKey();
        return;
    }
}