using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Maintains the history of cell actions for proper undo/redo recall order.
    /// </summary>
    public class UndoRedo
    {
        public string text;
        private IUndoRedoInterface[] cellHistory;

        public UndoRedo()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedo"/> class.
        /// </summary>
        /// <param name="inputText">Action message (e.g. "Text changed.").</param>
        /// <param name="inputAction">Inputed action.</param>
        public UndoRedo(string inputText, IUndoRedoInterface[] inputAction)
        {
            this.text = inputText;
            this.cellHistory = inputAction;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UndoRedo"/> class.
        /// </summary>
        /// <param name="inputText">Action message.</param>
        /// <param name="inputAction">List of past actions for undo/redo.</param>
        public UndoRedo(string inputText, List<IUndoRedoInterface> inputAction)
        {
            this.text = inputText;
            this.cellHistory = inputAction.ToArray();
        }

        /// <summary>
        /// Undos the previous action.
        /// </summary>
        /// <param name="sheet">Spreadsheet to be modified.</param>
        /// <returns>Updated undo/redo list.</returns>
        public UndoRedo Undo(SpreadSheet sheet)
        {
            List<IUndoRedoInterface> list = new List<IUndoRedoInterface>();

            foreach (IUndoRedoInterface fix in this.cellHistory)
            {
                list.Add(fix.Execute(sheet));
            }

            return new UndoRedo(this.text, list.ToArray());
        }
    }
}
