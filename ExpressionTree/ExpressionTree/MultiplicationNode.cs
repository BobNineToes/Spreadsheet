using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// Node multiplies the left and right nodes in the expression tree.
    /// </summary>
    internal class MultiplicationNode : BinaryNode
    {
        /// <summary>
        /// Evaluates the expression tree.
        /// </summary>
        /// <returns>The result of the left * right math.</returns>
        public override double Evaluate()
        {
            return this.left.Evaluate() * this.right.Evaluate();
        }
    }
}
