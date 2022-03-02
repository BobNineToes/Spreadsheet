/*
 * PROGRAM: Spreadsheet_Alexander_Crowther
 * AUTHOR: Alexander Crowther, Cpt_S 321
 * PURPOSE: Manages the GUI interface that connects to the SpreadSheetEngine namespace,
 *      ensuring the user inputs are properly passed to the SpreadSheetEngine
 * CLASSES IN NAMESPACE: Program.cs, Spreadsheet_CptS321.cs
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CptS321
{
    /// <summary>
    /// Allows the whole project to run.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
