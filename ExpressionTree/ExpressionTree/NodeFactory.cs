using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    internal abstract class NodeFactory
    {
        public abstract Node FactoryMethod(string expression);
    }
}
