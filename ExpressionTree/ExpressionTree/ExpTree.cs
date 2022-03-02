using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// This class manages the assigning of nodes and building the expressiontree for evaluating the arithmatic.
    /// </summary>
    public class ExpTree
    {
        private readonly string mismatch = "Expression has mismatched parantheses.";
        private Dictionary<string, HashSet<ReferenceNode>> variable;
        private Node rootNode = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpTree"/> class.
        /// </summary>
        /// <param name="expression">The expression the user put in the spreadsheet cell.</param>
        public ExpTree(string expression)
        {
            expression = expression.Replace(" ", string.Empty);
            this.variable = new Dictionary<string, HashSet<ReferenceNode>>();

            try
            {
                this.rootNode = this.Compile(this.PostFix(expression));
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets a list of all the variable keys in the dictionary.
        /// </summary>
        public List<string> ExpressionVariables
        {
            get { return new List<string>(this.variable.Keys); }
        }

        /// <summary>
        /// Evaluates the expression tree starting at the rootNode.
        /// </summary>
        /// <returns>The solution from the tree, or throws if no value is unknown.</returns>
        public double Evaluate()
        {
            try
            {
                return this.rootNode.Evaluate();
            }
            catch (NullReferenceException)
            {
                throw;
            }
        }

        /// <summary>
        /// Sets the dictionary references to catalog all the cell names and values.
        /// </summary>
        /// <param name="variableName">The name of the cell.</param>
        /// <param name="variableValue">The value in the named cell.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            if (this.variable.ContainsKey(variableName))
            {
                foreach (ReferenceNode node in this.variable[variableName])
                {
                    node.CellValue = variableValue;
                }
            }
            else
            {
                throw new KeyNotFoundException(string.Format("{0} is not present in the expression.", variableName));
            }
        }

        private string PostFix(string expression)
        {
            HashSet<char> operators = new HashSet<char>(new char[] { '+', '-', '*', '/' });
            Dictionary<char, int> operatorPrecedence = new Dictionary<char, int>
            {
                ['('] = 0,
                ['+'] = 1,
                ['-'] = 1,
                ['*'] = 2,
                ['/'] = 2,
                [')'] = 100
            };

            var list = this.Tokenize(expression, true);
            Queue<string> output = new Queue<string>(list.Capacity);
            Stack<char> operationStack = new Stack<char>();

            foreach (string token in list)
            {
                if (int.TryParse(token, out int intResult) || double.TryParse(token, out double doubleResult) ||
                    Regex.Match(token, @"[A-Za-z]+[0-9]+").Success)
                {
                    output.Enqueue(token);
                }
                else
                {
                    if (operators.Contains(token[0]))
                    {
                        while (operationStack.Count != 0 &&
                            operatorPrecedence[operationStack.Peek()] > operatorPrecedence[token[0]])
                        {
                            output.Enqueue(operationStack.Pop().ToString());
                        }

                        operationStack.Push(token[0]);
                    }
                    else if (token.StartsWith("("))
                    {
                        operationStack.Push(token[0]);
                    }
                    else if (token.StartsWith(")"))
                    {
                        try
                        {
                            while (operationStack.Peek() != '(')
                            {
                                output.Enqueue(operationStack.Pop().ToString());
                            }
                        }
                        catch (InvalidOperationException)
                        {
                            throw new Exception(this.mismatch);
                        }

                        operationStack.Pop();
                    }
                    else
                    {
                        throw new ArgumentException(string.Format("{0} is not valid.", token));
                    }
                }
            }

            while (operationStack.Count > 0)
            {
                if (operationStack.Peek() != '(' || operationStack.Peek() != ')')
                {
                    output.Enqueue(operationStack.Pop().ToString());
                }
                else
                {
                    throw new Exception(this.mismatch);
                }
            }

            return string.Join(" ", output.ToArray());
        }

        private List<string> Tokenize(string expression, bool extraSymbol)
        {
            string @pattern = @"[\d]+\.?[\d]*|[A-Za-z]+[0-9]+|[-/\+\*\(\)]";

            if (extraSymbol)
            {
                pattern += "|.+";
            }

            Regex regex = new Regex(@pattern);
            MatchCollection matches = Regex.Matches(expression, @pattern);
            return matches.Cast<Match>().Select(match => match.Value).ToList();
        }

        private Node Compile(string expression)
        {
            var tokenList = this.Tokenize(expression, false);
            Stack<Node> nodeStack = new Stack<Node>();
            NodeFactory nodeFactory = new ConcreteNodeFactory();

            foreach (string token in tokenList)
            {
                Node node = nodeFactory.FactoryMethod(token);

                switch (node)
                {
                    case OperatorNode opNode:
                        switch (opNode)
                        {
                            case BinaryNode binaryNode:
                                BinaryNode binaryOperator = binaryNode as BinaryNode;
                                Node right = nodeStack.Pop();
                                Node left = nodeStack.Pop();
                                binaryOperator.right = right;
                                binaryOperator.left = left;
                                nodeStack.Push(binaryOperator);
                                break;
                            default:
                                break;
                        }

                        break;
                    case ReferenceNode refNode:
                        ReferenceNode referenceNode = node as ReferenceNode;

                        if (!this.variable.ContainsKey(referenceNode.CellName))
                        {
                            this.variable.Add(referenceNode.CellName, new HashSet<ReferenceNode>());
                        }

                        this.variable[referenceNode.CellName].Add(referenceNode);
                        nodeStack.Push(node);
                        break;
                    case ConstantNode consNode:
                        nodeStack.Push(node);
                        break;
                    default:
                        break;
                }
            }

            return nodeStack.Pop();
        }
    }
}
