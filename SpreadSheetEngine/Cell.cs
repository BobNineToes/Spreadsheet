using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Used by the Spreadsheet class to create cells.
    /// </summary>
    public class Cell : CellBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">integer of input row.</param>
        /// <param name="column">integer of input column.</param>
        public Cell(int row, int column)
            : base(row, column)
        {
        }
    }
}
