using System.Linq.Expressions;

namespace Exp_Parser.Model.Nodes;

internal abstract class Node(int precedence)
    {
        internal int Precedence { get; set; } = precedence;

        internal virtual bool IsClosed => true;

        internal abstract Expression BuildExpression(Expression? callerExpression = null);

        internal void RaisePrec() => Precedence = 0;

        internal virtual bool TryAddNode(Node node) => false;
    }
