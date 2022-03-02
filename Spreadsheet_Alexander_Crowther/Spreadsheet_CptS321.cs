/*
 * PROGRAM: SpreadSheetEngine, HW8
 * AUTHOR: Alexander Crowther, Cpt_S 321
 * PURPOSE: Create a functioning spreadsheet that calls on the spreadsheet engine for
 *      managing the spreadsheet cells. The user is able to input words and numbers, also
 *      change the cell background colors.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace CptS321
{
    /// <summary>
    /// The code to manage the spreadsheet form.
    /// </summary>
    public partial class Form1 : Form
    {
        public EditUndoRedo undoRedo = new EditUndoRedo();
        private SpreadSheet sheet = new SpreadSheet(50, 26);
        private int rowSelection = 0;
        private int columnSelection = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.Text = "Crowther's Spreadsheet - CptS321";
        }

        /// <summary>
        /// Calls the buildform, update menu, and delegate event handlers methods.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.BuildForm1();
            this.DelegateEventHandlers();
            this.UpdateMenu();
        }

        /// <summary>
        /// Builds the spreadsheet when called upon.
        /// </summary>
        private void BuildForm1()
        {
            this.Size = new Size(1000, 600);
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Size = new Size(970, 500);
            string columnLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            // Setup the dataGridView1 layout and position.
            foreach (char columnHeader in columnLetter)
            {
                this.dataGridView1.Columns.Add(columnHeader.ToString(), columnHeader.ToString());
            }

            this.dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridView1.Rows.Add(50);

            for (int index = 0; index < 50; index++)
            {
                var row = this.dataGridView1.Rows[index];
                row.HeaderCell.Value = (index + 1).ToString();
            }

            // Setup the Edit Box in relation to the dataGridView.
            this.textBox1.Size = new Size(this.dataGridView1.Width - this.dataGridView1.RowHeadersWidth, 30);
            this.textBox1.Location = new Point(this.dataGridView1.RowHeadersWidth, this.menuStrip1.Height + 5);
        }

        /// <summary>
        /// Updates the undo/redo dropdown menu. Have not figured out how to show the next
        ///   undo/redo in the stack.
        /// </summary>
        private void UpdateMenu()
        {
            ToolStripItemCollection tempCollection = this.menuStrip1.Items;
            ToolStripMenuItem menuItem = tempCollection[0] as ToolStripMenuItem;

            foreach (ToolStripItem item in menuItem.DropDownItems)
            {
                if (item.Text.Contains("Undo"))
                {
                    item.Enabled = this.undoRedo.IsNotEmptyUndo;
                }

                if (item.Text.Contains("Redo"))
                {
                    item.Enabled = this.undoRedo.IsNotEmptyRedo;
                }
            }
        }

        /// <summary>
        /// Groups all the event handlers in one place.
        /// </summary>
        private void DelegateEventHandlers()
        {
            this.dataGridView1.CellBeginEdit += new DataGridViewCellCancelEventHandler(this.DataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(this.DataGridView1_CellEndEdit);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.DataGridView1_CellEnter);
            this.textBox1.KeyDown += new KeyEventHandler(this.TextBox1_KeyDown);
            this.textBox1.Leave += new EventHandler(this.TextBox1_Leave);
            this.textBox1.EnabledChanged += new EventHandler(this.TextBox1_EnabledChange);
            this.sheet.PropertyChanged += new PropertyChangedEventHandler(this.OnCellPropertyChanged);
        }

        /// <summary>
        /// Closes the program when done.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Allows the user to modify the selected cell.
        /// </summary>
        /// <param name="sender">object of selected cell.</param>
        /// <param name="e">e handles the event argument.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridView view = sender as DataGridView;
            this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.sheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
        }

        /// <summary>
        /// Ends the cell manipulation when the user is finished with the cell.
        /// </summary>
        /// <param name="sender">object of selected cell.</param>
        /// <param name="e">e handles the event argumnent.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView view = sender as DataGridView;
                CellBase tempCell = this.sheet.GetCell(e.RowIndex, e.ColumnIndex);
                IUndoRedoInterface[] action = new IUndoRedoInterface[1];
                action[0] = new UndoText(tempCell.Text, tempCell);

                if (view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    tempCell.Text = view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                }
                else
                {
                    tempCell.Text = string.Empty;
                }

                this.undoRedo.UndoMore(new UndoRedo("Text changed.", action));
                view.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = tempCell.CellValue;
            }
            catch (IndexOutOfRangeException exception)
            {
                MessageBox.Show(exception.Message, "Index out of range.", MessageBoxButtons.OK);
            }

            this.UpdateMenu();
        }

        /// <summary>
        /// Connects the textbox to the selected cell to show the text value.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            this.textBox1.Text = this.sheet.GetCell(e.RowIndex, e.ColumnIndex).Text;
            this.rowSelection = e.RowIndex;
            this.columnSelection = e.ColumnIndex;
        }

        /// <summary>
        /// Handles the property changed.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CellBase activeCell = sender as CellBase;

            if (e.PropertyName == "Value changed.")
            {
                this.dataGridView1.Rows[activeCell.RowIndex].Cells[activeCell.ColumnIndex].Value = activeCell.CellValue;

                if (this.sheet.Error)
                {
                    MessageBox.Show(this.sheet.ErrorMessage, "Invalid input.", MessageBoxButtons.OK);
                    this.sheet.Error = false;
                    this.sheet.ErrorMessage = string.Empty;
                }
            }

            if (e.PropertyName == "Background color change.")
            {
                this.dataGridView1.Rows[activeCell.RowIndex].Cells[activeCell.ColumnIndex].Style.BackColor = Color.FromArgb((int)activeCell.BackgroundColor);
            }
        }

        /// <summary>
        /// Tracks when the user pushes a key.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.TextBox1_Leave(sender, new EventArgs());
            }
        }

        /// <summary>
        /// Tracks when the cell edit is complete.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void TextBox1_Leave(object sender, EventArgs e)
        {
            this.dataGridView1.Rows[this.rowSelection].Cells[this.columnSelection].Value = this.textBox1.Text;
            this.DataGridView1_CellEndEdit(this.dataGridView1, new DataGridViewCellEventArgs(this.columnSelection, this.rowSelection));
            this.textBox1.Enabled = false;
        }

        /// <summary>
        /// Sometimes, it is incredibly wasteful to put summaries on everything, including things that are self explanatory.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">e.</param>
        private void TextBox1_EnabledChange(object sender, EventArgs e)
        {
            if (!this.textBox1.Enabled)
            {
                this.textBox1.Enabled = true;
            }
        }

        /// <summary>
        /// Run demo no longer works correctly, had to change how the cell copying works (e.g. "=A1").
        /// Did not bother as it is not necessary for this assignment.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void RunDemoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            int index;

            // Populates the B column cells with the B + row number.
            for (index = 0; index < 50; index++)
            {
                this.sheet.Sheet[index, 1].Text = "This is cell B" + (index + 1).ToString();
            }

            // Copies over column B to column A.
            for (index = 0; index < 50; index++)
            {
                this.sheet.Sheet[index, 0].Text = "=B" + (index + 1).ToString();
            }

            // Populates random cells (excluding columns A & B) with a message.
            for (index = 0; index < 50; index++)
            {
                int row = random.Next(0, 49);
                int column = random.Next(2, 25);

                this.sheet.Sheet[row, column].Text = "Randomness happens!";
            }
        }

        /// <summary>
        /// Clears the spreadsheet for further use.
        /// </summary>
        /// <param name="sender">User object input.</param>
        /// <param name="e">Event argument.</param>
        private void ClearSpreadsheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ClearSpreadsheet();
        }

        /// <summary>
        /// Initiates the undo command and updates the menu.
        /// </summary>
        /// <param name="sender">User object input.</param>
        /// <param name="e">Event argument.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.undoRedo.Undo(this.sheet);
            this.UpdateMenu();
        }

        /// <summary>
        /// Initiates the redo command and updates the menu.
        /// </summary>
        /// <param name="sender">User object input.</param>
        /// <param name="e">Event argument.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.undoRedo.Redo(this.sheet);
            this.UpdateMenu();
        }

        /// <summary>
        /// Initiates the cell background color change for the selected cells.
        /// </summary>
        /// <param name="sender">User object input.</param>
        /// <param name="e">Event argument.</param>
        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int color = 0;
            ColorDialog colorDialog = new ColorDialog();
            List<IUndoRedoInterface> undo = new List<IUndoRedoInterface>();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                color = colorDialog.Color.ToArgb();

                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    CellBase newCell = this.sheet.GetCell(cell.RowIndex, cell.ColumnIndex);
                    undo.Add(new BackgroundColor(newCell.BackgroundColor, newCell));
                    newCell.BackgroundColor = (uint)color;
                }

                this.undoRedo.UndoMore(new UndoRedo("Background color change.", undo));
                this.UpdateMenu();
                this.Invalidate();
            }
        }

        /// <summary>
        /// Allows the user to load an XML file.
        /// </summary>
        /// <param name="sender">User object input.</param>
        /// <param name="e">Event argument.</param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "XML files (*.xml)|*.xml";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                this.ClearSpreadsheet();

                using (Stream loadFile = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    this.sheet.Load(loadFile);
                }

                this.UpdateMenu();
            }
        }

        /// <summary>
        /// Clears the cells in the spreadsheet.
        /// </summary>
        private void ClearSpreadsheet()
        {
            for (int rowIndex = 0; rowIndex < this.sheet.RowCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < this.sheet.ColumnCount; ++columnIndex)
                {
                    if (!this.IsDefaultCell(rowIndex, columnIndex))
                    {
                        this.sheet.Sheet[rowIndex, columnIndex].Text = string.Empty;
                        this.sheet.Sheet[rowIndex, columnIndex].BackgroundColor = 4294967295;
                    }
                }
            }
        }

        /// <summary>
        /// Checks to see if the cell is has not been modified.
        /// </summary>
        /// <param name="row">INteger row.</param>
        /// <param name="column">Integer column.</param>
        /// <returns>True if default, false otherwise.</returns>
        private bool IsDefaultCell(int row, int column)
        {
            return this.sheet.Sheet[row, column].Text == string.Empty
                || this.sheet.Sheet[row, column].CellValue == string.Empty
                || this.sheet.Sheet[row, column].BackgroundColor == 4294967295;
        }

        /// <summary>
        /// Saves the spreadsheet cells to an XML file.
        /// </summary>
        /// <param name="sender">User object input.</param>
        /// <param name="e">Event argument.</param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = saveFileDialog.OpenFile()) != null)
                {
                    this.sheet.Save(myStream);
                    myStream.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
