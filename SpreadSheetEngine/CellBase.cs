using System;
using System.ComponentModel;

namespace CptS321
{
    /// <summary>
    /// Abstract class to 'template' each cell.
    /// </summary>
    public abstract class CellBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Text stores the actual cell input.
        /// </summary>
        protected string text = string.Empty;

        /// <summary>
        /// Value stores the cell 'output', what the user sees.
        /// </summary>
        protected string cellValue = string.Empty;
        private readonly string cellName = string.Empty; // The row/column cell name.
        private readonly int rowIndex;
        private readonly int columnIndex;
        private uint backgroundColor = 4294967295;

        /// <summary>
        /// Initializes a new instance of the <see cref="CellBase"/> class.
        /// </summary>
        /// <param name="row">integer row position.</param>
        /// <param name="column">intger column position.</param>
        public CellBase(int row, int column)
        {
            this.rowIndex = row;
            this.columnIndex = column;
            this.cellName += Convert.ToChar('A' + column);
            this.cellName += (row + 1).ToString();
        }

        /// <summary>
        /// Allows for Form1 updates.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets rowIndex.
        /// </summary>
        public int RowIndex { get { return this.rowIndex; } }

        /// <summary>
        /// Gets columnIndex.
        /// </summary>
        public int ColumnIndex { get { return this.columnIndex; } }

        /// <summary>
        /// Gets cellName.
        /// </summary>
        public string CellName { get { return this.cellName; } }

        /// <summary>
        /// Gets or sets the cell's background color.
        /// </summary>
        public uint BackgroundColor
        {
            get
            {
                return this.backgroundColor;
            }

            set
            {
                if (this.backgroundColor == value)
                {
                    return;
                }

                this.backgroundColor = value;
                this.OnPropertyChanged("Background color change.");
            }
        }

        /// <summary>
        /// Gets the cellValue.
        /// </summary>
        public string CellValue
        {
            get
            {
                return this.cellValue;
            }

            internal set
            {
                if (this.cellValue != value)
                {
                    this.cellValue = value;
                    this.OnPropertyChanged("Value changed.");
                }
            }
        }

        /// <summary>
        /// Gets or Sets text.
        /// For setting, checks to see if the new value matches text, if not, then
        ///  updates the text value and calls PropertyChanged.
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }

            set
            {
                if (this.text != value)
                {
                    this.text = value;
                    this.OnPropertyChanged("Text changed.");
                }
            }
        }

        protected void OnPropertyChanged(string message)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(message));
        }
    }
}
