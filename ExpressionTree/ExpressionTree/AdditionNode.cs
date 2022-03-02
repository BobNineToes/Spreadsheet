using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    internal class AdditionNode : BinaryNode
    {
        public override double Evaluate()
        {
            return this.left.Evaluate() + this.right.Evaluate();
        }
    }
}
