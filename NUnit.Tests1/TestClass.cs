// NUnit 3 tests
// See documentation : https://github.com/nunit/docs/wiki/NUnit-Documentation
using NUnit.Framework;
using SpreadSheetEngine;

namespace NUnit.Tests1
{
    /// <summary>
    /// Maintains the test codes.
    /// </summary>
    [TestFixture]
    public class TestClass
    {
        /// <summary>
        /// Supposed to test input into a cell's value.
        /// </summary>
        [Test]
        public void TestCellTextMessage()
        {
            Cell testCell = new Cell(0, 0);
            testCell.Text = "Test";
            testCell.SetValue(testCell.Text);

            Assert.That(testCell.Text, Is.EqualTo("Test"));
        }

        /// <summary>
        /// Test to see if Value is set.
        /// </summary>
        [Test]
        public void TestCellValueMessage()
        {
            Cell testCell = new Cell(0, 0);
            testCell.Text = "Test";
            testCell.SetValue(testCell.Text);

            Assert.That(testCell.Text, Is.EqualTo("Test"));
        }

        /// <summary>
        /// Tests when cell A2 = A1's value, change A1's value, and still A2 = A1's value.
        /// </summary>
        [Test]
        public void TestUpdatingCell()
        {
            SpreadSheet testSheet = new SpreadSheet(5, 5);
            testSheet.GetCell(0, 0).Text = "42";
            testSheet.GetCell(1, 0).Text = "=A1";
            Assert.That(testSheet.GetCell(0, 0).CellValue, Is.EqualTo(testSheet.GetCell(1, 0).CellValue));
            testSheet.GetCell(0, 0).Text = "55";
            Assert.That(testSheet.GetCell(0, 0).CellValue, Is.EqualTo(testSheet.GetCell(1, 0).CellValue));
        }
    }
}
