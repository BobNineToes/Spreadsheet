using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// Assigns the left and right nodes in relation to the operator in the expression tree.
    /// </summary>
    internal abstract class BinaryNode : OperatorNode
    {
        public Node left;
        public Node right;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryNode"/> class.
        /// </summary>
        public BinaryNode()
        {
            this.left = null;
            this.right = null;
        }

        /// <summary>
        /// Carries the Evaluate member to the child nodes.
        /// </summary>
        /// <returns>Carries the evaluated child node's return to the expression tree.</returns>
        public abstract override double Evaluate();
    }
}
