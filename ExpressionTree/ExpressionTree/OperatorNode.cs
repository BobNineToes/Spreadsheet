using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    internal abstract class OperatorNode : Node
    {
        public abstract override double Evaluate();
    }
}
