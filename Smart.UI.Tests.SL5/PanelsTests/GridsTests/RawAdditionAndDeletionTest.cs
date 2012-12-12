using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions.StructExtensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class RawAdditionAndDeletionTest:GridTestBase<WidgetGrid>
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

        [TestMethod]
        public void AddRawAndColumnTest()
        {
            this.Grids.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Panel;
            this.Grids.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Panel;            
            this.Grids.Width.ShouldBeEqual(1000.0);
            this.Grids.Height.ShouldBeEqual(1000.0);
            this.Grids.AddRow();
            this.Grids.RowDefinitions.DeltaStars.ShouldBeEqual(100);
            this.TestPanel.UpdateLayout();

            this.Grids.Width.ShouldBeEqual(1000.0);
            this.Grids.Height.ShouldBeEqual(1100.0);
            this.Grids.RowDefinitions.StarLength.ShouldBeEqual(100);
            this.Grids.AddColumn();
            this.Grids.ColumnDefinitions.DeltaStars.ShouldBeEqual(100);
            this.TestPanel.UpdateLayout(); 
           
            this.Grids.Width.ShouldBeEqual(1100.0);
            this.Grids.Height.ShouldBeEqual(1100.0);
            this.Grids.ColumnDefinitions.StarLength.ShouldBeEqual(100);

            this.Grids.RemoveLastRow();
            this.Grids.RowDefinitions.DeltaStars.ShouldBeEqual(-100);
            this.Grids.RemoveLastColumn();
            this.Grids.ColumnDefinitions.DeltaStars.ShouldBeEqual(-100);
            this.UpdateLayout();
            

            this.Grids.Width.ShouldBeEqual(1000.0);
            this.Grids.Height.ShouldBeEqual(1000.0);
            this.UpdateLayout();

            this.Grids.Space.Canvas.Width = 1000;
            this.Grids.Space.Canvas.Height = 1000;

            this.Grids.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;
            this.Grids.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;        
            this.Grids.Space.Canvas.Size().Width.ShouldBeEqual(1000.0);
            this.Grids.Space.Canvas.Size().Height.ShouldBeEqual(1000.0);
            
            this.Grids.AddColumn(new LineDefinition(100));
            this.UpdateLayout();

            this.Grids.Space.Canvas.Size().Width.ShouldBeEqual(1100.0);
            this.Grids.Space.Canvas.Size().Height.ShouldBeEqual(1000.0);            
            this.UpdateLayout();

            this.Grids.AddRow(new LineDefinition(100));         
            this.UpdateLayout();
            this.Grids.Space.Canvas.Size().Width.ShouldBeEqual(1100.0);
            this.Grids.Space.Canvas.Size().Height.ShouldBeEqual(1100.0);   
        }

        [TestMethod]
        public void RemoveLastRawOrColumnTest()
        {
            this.Grids.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Panel;
            this.Grids.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Panel;        
            this.Grids.Width.ShouldBeEqual(1000.0);
            this.Grids.Height.ShouldBeEqual(1000.0);
            this.Grids.RemoveLastColumn();
            this.TestPanel.UpdateLayout();
            foreach (var cell in Cells[9])
            {
                cell.Parent.ShouldBeNull();
            }
            this.Grids.Width.ShouldBeEqual(900.0);
            this.Grids.RemoveLastRow();
            this.TestPanel.UpdateLayout();
            this.Grids.Height.ShouldBeEqual(900.0);
            //var last = this.Grids.ChildrenInCells<FrameworkElement>(new CellsRegion(9, 9));
        }

    }
}
