using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Manages the cell(s) background color.
    /// </summary>
    public class BackgroundColor : IUndoRedoInterface
    {
        private uint cellColor;
        private CellBase cell;

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundColor"/> class.
        /// </summary>
        /// <param name="colorChoice">Color chosen by user.</param>
        /// <param name="cellChoice">Cell(s) chosen by user.</param>
        public BackgroundColor(uint colorChoice, CellBase cellChoice)
        {
            this.cell = cellChoice;
            this.cellColor = colorChoice;
        }

        /// <summary>
        /// Executes the background color change.
        /// </summary>
        /// <param name="sheet">The spreadsheet that being modified.</param>
        /// <returns>The new color that is assigned to the chosen cells.</returns>
        public IUndoRedoInterface Execute(SpreadSheet sheet)
        {
            uint color = this.cell.BackgroundColor;
            this.cell.BackgroundColor = this.cellColor;
            return new BackgroundColor(color, this.cell);
        }
    }
}
