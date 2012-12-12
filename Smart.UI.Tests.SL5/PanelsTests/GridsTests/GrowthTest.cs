using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class GrowthTest:PanelTestBase<SmartGrid>
    {

        [TestMethod]
        public void WidthAndCanvasChangeTest()
        {
            this.UpdateLayout();
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.Width += 500;
            this.Panel.Height += 500;
            this.UpdateLayout();

            this.Panel.Width.ShouldBeEqual(1500);
            this.Panel.Height.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1500);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1500);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1500);
            this.Panel.Width -= 500;
            this.Panel.Height -= 500;
            this.UpdateLayout();

            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1000);

        }

        [TestMethod]
        public void GrowCanvasTest()
        {
            this.UpdateLayout();            
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.GrowHorizontal(500,AlignmentX.Right);
            this.Panel.GrowVertical(500,AlignmentY.Bottom);
            this.UpdateLayout();
            this.Panel.Width.ShouldBeEqual(1500);
            this.Panel.Height.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1500);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1500);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1500);
            this.Panel.GrowHorizontal(-500, AlignmentX.Right);
            this.Panel.GrowVertical(-500, AlignmentY.Bottom);           
            this.UpdateLayout();

         
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1000);
        }

        [TestMethod]
        public void GrowCanvasByLineDefinitions()
        {
            this.UpdateLayout();            
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.DeltaForce += 500;
            this.Panel.ColumnDefinitions.DeltaForce += 500;    
            this.UpdateLayout();
            
            //this.Panel.Width.ShouldBeEqual(1500);
            //this.Panel.Height.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1500);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1500);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1500);
            this.Panel.RowDefinitions.DeltaForce-=500;
            this.Panel.ColumnDefinitions.DeltaForce -= 500;           
            this.UpdateLayout();

            
            //this.Panel.Width.ShouldBeEqual(1000);
            //this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.Length.ShouldBeEqual(1000);
        }
    }
}
