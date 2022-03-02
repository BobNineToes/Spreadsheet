using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// The reference node is used by the expression tree to store the node's value for referencing by the spreadsheet.
    /// </summary>
    internal class ReferenceNode : Node
    {
        private string cellName;
        private double? cellValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceNode"/> class.
        /// </summary>
        /// <param name="name">The name to be assigned to the node.</param>
        public ReferenceNode(string name)
        {
            this.cellName = name;
        }

        /// <summary>
        /// Gets the name of the cell.
        /// </summary>
        public string CellName
        {
            get { return this.cellName; }
        }

        /// <summary>
        /// Sets the number value of the cell.
        /// </summary>
        public double CellValue
        {
            set { this.cellValue = value; }
        }

        /// <summary>
        /// Evaluates the cell value.
        /// </summary>
        /// <returns>The value stored in the cell, or throws an exception if the value is not in the cell.</returns>
        public override double Evaluate()
        {
            if (this.cellValue.HasValue)
            {
                return this.cellValue.Value;
            }
            else
            {
                throw new NullReferenceException(string.Format("Variable {0}'s value is unknown", this.cellName));
            }
        }
    }
}
