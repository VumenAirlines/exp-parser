using System.Reflection;
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
        MathExpressionTestCases.RunAllTests(test =>
        {
                Console.WriteLine("---------"+test+"----------");
                return Parser.BuildExpressionFor<double>(new Tokenizer().Tokenize(test),"x").Compile() switch
                {
                    Func<double, double> func => func,
                    _ => throw new Exception()
                };
        });
        
        //var asd = tokenizer.Tokenize("");//(5x/250)^2sin(35x)*log(10,9)*-1");


        //var a = Parser.BuildExpressionFor<double>(new Tokenizer().Tokenize("((x + 1) * 2) - 1"), "x").Compile();
       

        
            
        Console.ReadKey();
        return;
    }
}