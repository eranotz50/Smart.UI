using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class LinesMovementsTest:GridTestBase<SmartGrid>
    {
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Grids.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Panel;
            this.Grids.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Panel;        
          
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
        }

        [TestMethod]
        public void TestOneLineMovement()
        {
            this.Grids.ColumnDefinitions.MoveLine(4,50);
            this.Grids.RowDefinitions.MoveLine(4, 500);
            
            this.TestPanel.UpdateLayout();
            Bounds = Grids.GetBounds();
            Bounds.Width.ShouldBeEqual(1000.0);
            Bounds.Height.ShouldBeEqual(1000.0);
          
            Grids.ColumnDefinitions[3].Value.ShouldBeEqual(150.0);
            Grids.ColumnDefinitions[4].Value.ShouldBeEqual(100.0);            
            Grids.ColumnDefinitions[5].Value.ShouldBeEqual(50.0);
            Grids.RowDefinitions[3].Value.ShouldBeEqual(200.0);
            Grids.RowDefinitions[5].Value.ShouldBeEqual(0.0);
            
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(450);
            Bounds.Y.ShouldBeEqual(500);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
            
            this.Grids.ColumnDefinitions.MoveLine(4, -50);
            this.Grids.RowDefinitions.MoveLine(4, -100);
            this.TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
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
        public void TestThreeLinesMovement()
        {
            this.Cell.SetColumn(3).SetColumnSpan(3).SetRow(3).SetRowSpan(3).SetDragCanvas("NearestParent").SetDragMode(DragMode.Custom);
            this.TestPanel.UpdateLayout();
            
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(300.0);
            Bounds.Y.ShouldBeEqual(300.0);
            Bounds.Width.ShouldBeEqual(300.0);
            Bounds.Height.ShouldBeEqual(300.0);

           
            this.Grids.ColumnDefinitions.MoveLines(3, 5, 50);
            this.Grids.RowDefinitions.MoveLines(3, 5, 70);
            this.TestPanel.UpdateLayout();

            this.CheckThreeLinesShift();                        

        }

      

        [TestMethod]
        public void TestSplitting()
        {
            this.Grids.ColumnDefinitions.SplitLines(2, 4, -50.0);
            this.TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();

            Bounds.X.ShouldBeEqual(350);
            Bounds.Width.ShouldBeEqual(150);
            this.Grids.ColumnDefinitions.SplitLines(2, 4, 50.0);
            this.TestPanel.UpdateLayout();
            
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);

        }


    }
}
