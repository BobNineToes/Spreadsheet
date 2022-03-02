/*
 * PROGRAM: SpreadSheetEngine
 * AUTHOR: Alexander Crowther, Cpt_S 321
 * PURPOSE: Create a spreadsheet that will assign user input to individual cells.
 *      The program also runs a demonstration, labeling, all the rows in the 'B'
 *      column, copying the 'B' column to the 'A' column, and generating a message
 *      in random cells across the rest of the spreadsheet.
 * CLASSES IN NAMESPACE: SpreadSheet.cs, CellBase.cs, Cell.cs
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using ExpressionTree;

namespace CptS321
{
    /// <summary>
    /// Controls the overall manipulation of the spreadsheet. Acts as the
    ///  factory to the cells.
    /// </summary>
    public class SpreadSheet
    {
        /// <summary>
        /// Creates a 2D array for the spreadsheet.
        /// </summary>
        public Cell[,] Sheet;
        private Dictionary<CellBase, HashSet<CellBase>> reference;
        private bool error = false;
        private string errorMessage = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheet"/> class.
        /// </summary>
        /// <param name="rowIndex">integer of rows.</param>
        /// <param name="columnIndex">integer of columns.</param>
        public SpreadSheet(int rowIndex, int columnIndex)
        {
            this.Sheet = new Cell[rowIndex, columnIndex];
            this.reference = new Dictionary<CellBase, HashSet<CellBase>>();

            for (int rIndex = 0; rIndex < rowIndex; rIndex++)
            {
                for (int cIndex = 0; cIndex < columnIndex; cIndex++)
                {
                    this.Sheet[rIndex, cIndex] = new Cell(rIndex, cIndex);
                    this.Sheet[rIndex, cIndex].PropertyChanged += new PropertyChangedEventHandler(this.OnCellPropertyChanged);
                }
            }
        }

        /// <summary>
        /// Allows for Form1 updates.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the row length of the Sheet array.
        /// </summary>
        public int RowCount => this.Sheet.GetLength(0);

        /// <summary>
        /// Gets the column length of the Sheet array.
        /// </summary>
        public int ColumnCount => this.Sheet.GetLength(1);

        public bool Error
        {
            get { return this.error; }
            set { this.error = value; }
        }

        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        /// <summary>
        /// Runs the propertychanged scripts. Calls for EvaluateCell to verify a change is needed.
        /// </summary>
        /// <param name="sender">object cell that was changed.</param>
        /// <param name="e">Property name that was changed.</param>
        public void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            CellBase tempCell = sender as CellBase;

            if (e.PropertyName == "Text changed.")
            {
                try
                {
                    this.RemoveReference(tempCell);
                    this.EvaluateCell(tempCell);
                }
                catch (Exception exception)
                {
                    this.error = true;
                    this.errorMessage = exception.Message;
                }

                this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value changed."));
            }

            if (e.PropertyName == "Value changed.")
            {
                try
                {
                    this.UpdateReferenceValue(tempCell);
                }
                catch (Exception exception)
                {
                    this.error = true;
                    this.errorMessage = exception.Message;
                }

                this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Value changed."));
            }

            if (e.PropertyName == "Background color change.")
            {
                this.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs("Background color change."));
            }
        }

        /// <summary>
        /// Retrieves the desired cell for manipulation.
        /// </summary>
        /// <param name="rowIndex">integer of desired row.</param>
        /// <param name="columnIndex">integer of desired column.</param>
        /// <returns>The desired cell for manipulation.</returns>
        public CellBase GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex >= this.Sheet.GetLowerBound(0) && rowIndex <= this.Sheet.GetUpperBound(0) &&
                columnIndex >= this.Sheet.GetLowerBound(1) && columnIndex <= this.Sheet.GetUpperBound(1))
            {
                return this.Sheet[rowIndex, columnIndex];
            }

            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Save the modified spreadsheet cells to an XML document.
        /// </summary>
        /// <param name="saveFile">Cells to be save d to XML file.</param>
        public void Save(Stream saveFile)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;
            XmlWriter xmlWriter = XmlWriter.Create(saveFile, xmlSettings);
            xmlWriter.WriteStartElement("spreadsheet");

            foreach (Cell cell in this.Sheet)
            {
                if (!this.IsDefaultCell(cell) || !this.IsDefaultBGColor(cell))
                {
                    this.WriteXml(xmlWriter, cell);
                }
            }

            xmlWriter.WriteEndElement();
            xmlWriter.Close();
        }

        /// <summary>
        /// Loads the chosen xml file. Could not figure out how to look for unspecified attributes, had to
        ///   settle withe the else statement, relying on the tests actually not needing the unusedattr thing.
        /// </summary>
        /// <param name="loadFile">XML file.</param>
        public void Load(Stream loadFile)
        {
            XDocument xDoc = XDocument.Load(loadFile);

            foreach (XElement element in xDoc.Root.Elements("cell"))
            {
                if (element.Attribute("CellName") != null)
                {
                    int[] cellName = this.MakeReference(element.Attribute("CellName").Value);
                    CellBase cell = this.GetCell(cellName[0], cellName[1]);
                    cell.Text = element.Element("CellText").Value;
                    cell.BackgroundColor = uint.Parse(element.Element("CellBGColor").Value, NumberStyles.HexNumber);
                }
                else
                {
                    int[] cellName = this.MakeReference(element.Attribute("name").Value);
                    CellBase cell = this.GetCell(cellName[0], cellName[1]);
                    cell.Text = element.Element("text").Value;
                    cell.BackgroundColor = uint.Parse(element.Element("bgcolor").Value, NumberStyles.HexNumber);
                }
            }
        }

        /// <summary>
        /// Writes the cell's information to the XML.
        /// </summary>
        /// <param name="xmlWriter">XML the cell is saved to.</param>
        /// <param name="cell">Each cell to be saved.</param>
        public void WriteXml(XmlWriter xmlWriter, Cell cell)
        {
            xmlWriter.WriteStartElement("cell");
            xmlWriter.WriteAttributeString("CellName", cell.CellName);
            xmlWriter.WriteElementString("CellBGColor", cell.BackgroundColor.ToString("x8"));
            xmlWriter.WriteElementString("CellText", cell.Text);
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Verifies whether a cell is empty.
        /// </summary>
        /// <param name="cell">The cell in question.</param>
        /// <returns>True if cell is empty, false if not.</returns>
        private bool IsDefaultCell(Cell cell)
        {
            return cell.CellValue == string.Empty || cell.Text == string.Empty;
        }

        /// <summary>
        /// Verifies whether a cell color is the default color.
        /// </summary>
        /// <param name="cell">The cell in question.</param>
        /// <returns>True if cell is empty, false if not.</returns>
        private bool IsDefaultBGColor(Cell cell)
        {
            return cell.BackgroundColor == 4294967295;
        }

        /// <summary>
        /// Sets the reference between 2 cells.
        /// </summary>
        /// <param name="cellReference">Cell to be referenced.</param>
        private int[] MakeReference(string cellReference)
        {
            int[] references = new int[this.Sheet.Rank];
            int index;
            int repeatLetters = 0;
            char columnReference = char.ToUpper(cellReference[0]);

            for (index = 0; index < cellReference.Length; ++index)
            {
                if (columnReference != cellReference[index])
                {
                    break;
                }

                ++repeatLetters;
            }

            try
            {
                references[0] = int.Parse(cellReference.Substring(index));
            }
            catch
            {
                throw new ArgumentException("ERROR: Invalid column header.");
            }

            references[0] -= 1;
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            references[1] = (alphabet.Length * --repeatLetters) + alphabet.IndexOf(columnReference);

            return references;
        }

        /// <summary>
        /// Removes the reference between 2 cells.
        /// </summary>
        /// <param name="cell">Cell to be dereferenced.</param>
        private void RemoveReference(CellBase cell)
        {
            if (this.reference.ContainsKey(cell))
            {
                this.reference[cell].Clear();
            }

            this.reference.Remove(cell);
        }

        /// <summary>
        /// Checks the cell input. Handles copying one cell to match another cell.
        /// </summary>
        /// <param name="tempCell">The abstract cell to be evaluated.</param>
        private void EvaluateCell(CellBase tempCell)
        {
            if (tempCell.Text.Length == 0)
            {
                tempCell.CellValue = string.Empty;
            }
            else if (tempCell.Text.StartsWith("="))
            {
                if (tempCell.Text.Length > 1)
                {
                    try
                    {
                        this.EvaluateCellFormula(tempCell);
                    }
                    catch
                    {
                        tempCell.CellValue = "#REF!";
                        throw;
                    }
                }
                else
                {
                    throw new ArgumentException(this.errorMessage = @"'=' is not valid.");
                }
            }
            else
            {
                tempCell.CellValue = tempCell.Text;
            }
        }

        private void EvaluateCellFormula(CellBase tempCell)
        {
            try
            {
                ExpTree tree = new ExpTree(tempCell.Text.Substring(1));

                foreach (string cellName in tree.ExpressionVariables)
                {
                    int[] indices = this.MakeReference(cellName);
                    CellBase cell = this.GetCell(indices[0], indices[1]);

                    if (!this.reference.ContainsKey(tempCell))
                    {
                        this.reference.Add(tempCell, new HashSet<CellBase>());
                    }

                    this.reference[tempCell].Add(cell);
                    bool isDouble = double.TryParse(cell.CellValue, out double result);

                    if (isDouble)
                    {
                        tree.SetVariable(cellName, result);
                    }
                    else
                    {
                        tree.SetVariable(cellName, 0.0);
                    }
                }

                tempCell.CellValue = tree.Evaluate().ToString();
            }
            catch
            {
                throw;
            }
        }

        private void UpdateReferenceValue(CellBase tempCell)
        {
            if (this.NoReferences(tempCell, tempCell))
            {
                foreach (CellBase key in this.reference.Keys)
                {
                    if (this.reference[key].Contains(tempCell))
                    {
                        this.EvaluateCell(key);
                    }
                }
            }
            else
            {
                throw new Exception("ERROR: Circular reference.");
            }
        }

        private bool NoReferences(CellBase root, CellBase tempCell)
        {
            if (!this.reference.ContainsKey(tempCell))
            {
                return true;
            }

            bool result = true;

            foreach (CellBase cell in this.reference[tempCell])
            {
                if (ReferenceEquals(cell, root))
                {
                    return false;
                }

                result = result && this.NoReferences(root, cell);
            }

            return result;
        }
    }
}
