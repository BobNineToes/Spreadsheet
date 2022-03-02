using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class UndoText : IUndoRedoInterface
    {
        private CellBase cell;
        private string text;

        public UndoText(string inputText, CellBase tempCell)
        {
            this.text = inputText;
            this.cell = tempCell;
        }

        public IUndoRedoInterface Execute(SpreadSheet sheet)
        {
            CellBase tempCell = sheet.GetCell(this.cell.RowIndex, this.cell.ColumnIndex);
            string cellText = this.cell.Text;
            this.cell.Text = this.text;
            return new UndoText(cellText, tempCell);
        }
    }
}
