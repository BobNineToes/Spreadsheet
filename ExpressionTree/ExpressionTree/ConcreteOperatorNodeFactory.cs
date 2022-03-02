using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    internal class ConcreteOperatorNodeFactory : OperationNodeFactory
    {
        public override OperatorNode FactoryMethod(string operation)
        {
            switch (operation)
            {
                case "+":
                    return new AdditionNode();
                case "-":
                    return new SubtractionNode();
                case "*":
                    return new MultiplicationNode();
                case "/":
                    return new DivisionNode();
                default:
                    return null;
            }
        }
    }
}
