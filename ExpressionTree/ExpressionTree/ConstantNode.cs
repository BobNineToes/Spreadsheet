using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// This node contains the integer or double value for the expressiontree.
    /// </summary>
    internal class ConstantNode : Node
    {
        private int? intValue;
        private double? doubleValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">Integer value.</param>
        public ConstantNode(int value)
        {
            this.intValue = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// </summary>
        /// <param name="value">Double value.</param>
        public ConstantNode(double value)
        {
            this.doubleValue = value;
        }

        /// <summary>
        /// Evaluates the integer or double value stored in the node.
        /// </summary>
        /// <returns>Integer or double value stored in the node.</returns>
        public override double Evaluate()
        {
            if (this.intValue.HasValue)
            {
                return this.intValue.Value;
            }

            return this.doubleValue.Value;
        }
    }
}
