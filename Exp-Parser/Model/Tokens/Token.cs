using Exp_Parser.Model.Nodes;

namespace Exp_Parser.Model.Tokens;


    internal abstract class Token
    {
        internal abstract Node CreateNode();

       
        internal virtual bool OpeningBracket => false;
        internal virtual bool ClosingBracket => false;
        internal virtual bool IsSeparator => false;
    }
