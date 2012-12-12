using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Widgets;


namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class CellsCheckingTest:GridTestBase<WidgetGrid>
    {

        [TestMethod]
        public void CheckCellTest()
        {
            var f = Grids.ChildrenInCells<FlexGrid>(2, 3);
            f.Count.ShouldBeEqual(1);
            f[0].ShouldBeSame(Cells[2][3]);
            f = Grids.ChildrenInCells<FlexGrid>(4, 5);
            f.Count.ShouldBeEqual(1);
            f[0].ShouldBeSame(Cells[4][5]);
            Cells[3][5].SetColumnSpan(2);
            f = Grids.ChildrenInCells<FlexGrid>(4, 5);
            f.Count.ShouldBeEqual(2);
            Cells[3][4].SetColumnSpan(2).SetRowSpan(2);
            f = Grids.ChildrenInCells<FlexGrid>(4, 5);
            f.Count.ShouldBeEqual(3);
        }

        [TestMethod]
        public void CheckCellsTest()
        {
            var f = Grids.ChildrenInCells<FlexGrid>(2, 3, 2, 2);
            f.Count.ShouldBeEqual(4);
            f.Contains(Cells[2][3]).ShouldBeTrue();
            f.Contains(Cells[2][4]).ShouldBeTrue();
            f.Contains(Cells[3][3]).ShouldBeTrue();
            f.Contains(Cells[3][4]).ShouldBeTrue();
            var rect = Grids.GetCellsRect(2, 3, 2, 2);
            rect.X.ShouldBeEqual(200);
            rect.Y.ShouldBeEqual(300);
            rect.Width.ShouldBeEqual(200);
            rect.Height.ShouldBeEqual(200);
            f = Grids.ChildrenInPlace<FlexGrid>(rect);
            var cells = f.Select(i => i.Name).ToList();
            cells.Count.ShouldBeEqual(4);
            f.Count.ShouldBeEqual(4);
            f.Contains(Cells[2][3]).ShouldBeTrue();
            f.Contains(Cells[2][4]).ShouldBeTrue();
            f.Contains(Cells[3][3]).ShouldBeTrue();
            f.Contains(Cells[3][4]).ShouldBeTrue();



            f = Grids.ChildrenInCells<FlexGrid>(4, 5, 2, 2);
            f.Count.ShouldBeEqual(4);
            f.Contains(Cells[4][5]).ShouldBeTrue();
            f.Contains(Cells[5][5]).ShouldBeTrue();
            f.Contains(Cells[4][6]).ShouldBeTrue();
            f.Contains(Cells[5][6]).ShouldBeTrue();

            rect = Grids.GetCellsRect(4, 5, 2, 2);
            f = Grids.ChildrenInPlace<FlexGrid>(rect);
            f.Count.ShouldBeEqual(4);
            f.Contains(Cells[4][5]).ShouldBeTrue();
            f.Contains(Cells[5][5]).ShouldBeTrue();
            f.Contains(Cells[4][6]).ShouldBeTrue();
            f.Contains(Cells[5][6]).ShouldBeTrue();

            Cells[3][5].SetColumnSpan(2);
            TestPanel.UpdateLayout();

            f = Grids.ChildrenInCells<FlexGrid>(4, 5, 2, 2);
            f.Count.ShouldBeEqual(5);
            f.Contains(Cells[3][5]).ShouldBeTrue();

            rect = Grids.GetCellsRect(4, 5, 2, 2);
            f = Grids.ChildrenInPlace<FlexGrid>(rect);
            f.Count.ShouldBeEqual(5);
            f.Contains(Cells[3][5]).ShouldBeTrue();
        }

    }
}
