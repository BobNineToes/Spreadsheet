using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    /// <summary>
    /// Executes the undo/redo interface.
    /// </summary>
    public interface IUndoRedoInterface
    {
        IUndoRedoInterface Execute(SpreadSheet sheet);
    }

    public class EditUndoRedo
    {
        private Stack<UndoRedo> undo = new Stack<UndoRedo>();
        private Stack<UndoRedo> redo = new Stack<UndoRedo>();

        /// <summary>
        /// Gets a value indicating whether the undo stack is empty.
        /// </summary>
        public bool IsNotEmptyUndo
        {
            get
            {
                if (this.undo.Count == 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the redo stack is empty.
        /// </summary>
        public bool IsNotEmptyRedo
        {
            get
            {
                if (this.redo.Count == 0)
                {
                    return false;
                }

                return true;
            }
        }

        /// <summary>
        /// Pushes an undo action into the redo stack.
        /// </summary>
        /// <param name="sheet">Spreadsheet that holds the cells.</param>
        public void Undo(SpreadSheet sheet)
        {
            UndoRedo undoRedo = this.undo.Pop();
            this.redo.Push(undoRedo.Undo(sheet));
        }

        /// <summary>
        /// Pulls the most recent undone action to redo.
        /// </summary>
        /// <param name="sheet">Spreadsheet that holds the cells.</param>
        public void Redo(SpreadSheet sheet)
        {
            UndoRedo undoRedo = this.redo.Pop();
            this.undo.Push(undoRedo.Undo(sheet));
        }

        /// <summary>
        /// Adds cell actions to the undo stack.
        /// </summary>
        /// <param name="undoRedo">Inputed action.</param>
        public void UndoMore(UndoRedo undoRedo)
        {
            this.undo.Push(undoRedo);
            this.redo.Clear();
        }
    }
}
