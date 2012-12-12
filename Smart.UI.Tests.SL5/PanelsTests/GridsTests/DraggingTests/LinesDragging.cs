using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class LinesDraggingTest : GridTestBase<SmartGrid>
    {
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
        }

     
        protected void CheckThreeLinesShift()
        {
            Grids.ColumnDefinitions[2].Value.ShouldBeEqual(150.0);
            Grids.ColumnDefinitions[3].Value.ShouldBeEqual(100.0);
            Grids.ColumnDefinitions[4].Value.ShouldBeEqual(100.0);
            Grids.ColumnDefinitions[5].Value.ShouldBeEqual(100.0);
            Grids.ColumnDefinitions[6].Value.ShouldBeEqual(50.0);

            Grids.RowDefinitions[2].Value.ShouldBeNear(170);
            Grids.RowDefinitions[3].Value.ShouldBeNear(100);
            Grids.RowDefinitions[4].Value.ShouldBeNear(100);
            Grids.RowDefinitions[5].Value.ShouldBeNear(100);
            Grids.RowDefinitions[6].Value.ShouldBeNear(30);


            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(350.0);
            Bounds.Y.ShouldBeEqual(370.0);
            Bounds.Width.ShouldBeEqual(300.0);
            Bounds.Height.ShouldBeEqual(300.0);

        }

        

        [TestMethod]
        public void TestDragMovements()
        {
            this.Cell.SetColumn(3).SetColumnSpan(3).SetRow(3).SetRowSpan(3).SetDragCanvas("NearestParent")
                //.SetDragMode(DragMode.Custom)
                .SetColumnMover()
                .SetRowMover();

            var fly = this.Grids.DragManager.StartFlight(this.Grids.DragManager.BuildDragObject(Cell), i => new Point(0, 0));
            fly.CurrentMouse = new Point(50, 70);
            fly.Pos();


            this.TestPanel.UpdateLayout();
            this.CheckThreeLinesShift();
        }


        [TestMethod]
        public void TestDragSplitting()
        {
            this.Cell.SetColumn(3).SetColumnSpan(3).SetRow(3).SetRowSpan(3).SetDragCanvas("NearestParent").
                SetDragMode(DragMode.Custom)
                .SetColumnSplitter()
                .SetRowSplitter();

            var fly = this.Grids.DragManager.StartFlight(this.Grids.DragManager.BuildDragObject(Cell), i => new Point(0, 0));
            fly.CurrentMouse = new Point(50, 70);
            fly.Pos();


            this.TestPanel.UpdateLayout();
            Grids.ColumnDefinitions[2].Value.ShouldBeEqual(100.0);
            Grids.ColumnDefinitions[3].Value.ShouldBeEqual(100.0);
            Grids.ColumnDefinitions[4].Value.ShouldBeEqual(100.0);
            Grids.ColumnDefinitions[5].Value.ShouldBeEqual(150.0);
            Grids.ColumnDefinitions[6].Value.ShouldBeEqual(50.0);

            Grids.RowDefinitions[2].Value.ShouldBeNear(100);
            Grids.RowDefinitions[3].Value.ShouldBeNear(100);
            Grids.RowDefinitions[4].Value.ShouldBeNear(100);
            Grids.RowDefinitions[5].Value.ShouldBeNear(170);
            Grids.RowDefinitions[6].Value.ShouldBeNear(30);


            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(300.0);
            Bounds.Y.ShouldBeEqual(300.0);
            Bounds.Width.ShouldBeEqual(350.0);
            Bounds.Height.ShouldBeEqual(370.0);

        }


    }
}
