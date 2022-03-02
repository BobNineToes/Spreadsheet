using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// Handles the actual NodeFactory work, returning the appropriate node based on the expression passed in.
    /// </summary>
    internal class ConcreteNodeFactory : NodeFactory
    {
        /// <summary>
        /// Creates and assigns new nodes based on the expression.
        /// </summary>
        /// <param name="expression">Input from the cell that is to be turned into a node.</param>
        /// <returns>Appropriate node based on the expression.</returns>
        public override Node FactoryMethod(string expression)
        {
            OperationNodeFactory operatorNodeFactory = new ConcreteOperatorNodeFactory();
            OperatorNode @operator = operatorNodeFactory.FactoryMethod(expression);

            if (@operator != null)
            {
                return @operator;
            }
            else
            {
                bool isInteger = int.TryParse(expression, out int intResult);
                bool isDouble = double.TryParse(expression, out double doubleResult);

                if (isInteger)
                {
                    return new ConstantNode(intResult);
                }
                else if (isDouble)
                {
                    return new ConstantNode(doubleResult);
                }
                else
                {
                    return new ReferenceNode(expression);
                }
            }
        }
    }
}
