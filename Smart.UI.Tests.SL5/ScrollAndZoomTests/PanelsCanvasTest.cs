using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets.PanelAdorners;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Tests.ScrollAndZoomTests
{
    [TestClass]
    public class PanelsCanvasTest : GridTestBase<SmartGrid>
    {
        [TestMethod]
        public void OutModeTest()
        {
            Cells[9, 9].GetBounds().ShouldBeEqual(new Rect(900, 900, 100, 100));
            this.TestPanel.UpdateLayout();
            this.Grids.OutMode = OutMode.Clip;
            this.TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            Cells[9, 9].GetBounds().ShouldBeEqual(new Rect(900, 900, 100, 100));
            this.Grids.SetCanvasSize(new Size(2000, 2000));
            TestPanel.UpdateLayout();

            Cell.GetSlot().ShouldBeEqual(new Rect(800, 800, 200, 200));
            Cell.GetBounds().ShouldBeEqual(new Rect(800, 800, 200, 200));
            Cell.Visibility.ShouldBeEqual(Visibility.Visible);
            Cells[9, 9].Visibility.ShouldBeEqual(Visibility.Collapsed);
            Cells[9, 9].GetBounds().ShouldBeEqual(new Rect(900, 900, 100, 100));
            this.Grids.SetCanvasSize(new Size(10000, 10000));
            var lines = new GridLines();            
            var adorners = Adorners.GetSmartGridAdorners(this.Grids);
            adorners.Add(lines);
            TestPanel.UpdateLayout();

            Cell.Visibility.ShouldBeEqual(Visibility.Collapsed);
            Cells[9, 9].Visibility.ShouldBeEqual(Visibility.Collapsed);
            Cell.GetBounds().ShouldBeEqual(new Rect(800, 800, 200, 200));
            Cells[9, 9].GetBounds().ShouldBeEqual(new Rect(900, 900, 100, 100));

            lines.Cols.Count.ShouldBeEqual(10);
            lines.Rows.Count.ShouldBeEqual(10);

            lines.Cols[0].GetBounds().ShouldBeEqual(new Rect(1000 - lines.LineThickness, 0, lines.LineThickness, 10000));
            lines.Cols[0].Visibility.ShouldBeEqual(Visibility.Visible);
            lines.Cols[1].Visibility.ShouldBeEqual(Visibility.Collapsed);
            this.Grids.SetCanvasSize(new Size(1000, 1000));
            TestPanel.UpdateLayout();

            var linesBounds = lines.Cols.Select(c => c.GetBounds()).ToCollection();
            linesBounds[0].ShouldBeEqual(new Rect(100 - lines.LineThickness, 0, lines.LineThickness, 1000));
            linesBounds[1].ShouldBeEqual(new Rect(200 - lines.LineThickness, 0, lines.LineThickness, 1000));
            lines.Cols[0].Visibility.ShouldBeEqual(Visibility.Visible);
            lines.Cols[1].Visibility.ShouldBeEqual(Visibility.Visible);


            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            Cells[9, 9].GetBounds().ShouldBeEqual(new Rect(900, 900, 100, 100));
            Cell.Visibility.ShouldBeEqual(Visibility.Visible);
            Cells[9, 9].Visibility.ShouldBeEqual(Visibility.Visible);

        }

        [TestMethod]
        public void VisibleCanvasTest()
        {
            Grids.Name = "VisibleCanvasTest";
            Grids.OutMode = OutMode.Clip;
            var bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.SetCanvasSize(new Size(1000, 1000));
            this.Grids.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            this.Grids.Width = 500;
            this.Grids.Height = 500;
            TestPanel.UpdateLayout();      
      
            this.Grids.SetCanvasSize(new Size(1000, 1000));            
            TestPanel.UpdateLayout();

            this.Grids.RenderSize.ShouldBeEqual(new Size(500, 500));
            this.Grids.Space.Panel.Size().ShouldBeEqual(new Size(500, 500));
            bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.Space.Shift(new Point(100,100));
            this.Grids.Space.Panel.TopLeft().ShouldBeEqual(new Point(100, 100));
            TestPanel.UpdateLayout();

            bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(300);
            bounds.Y.ShouldBeEqual(300);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
        }   

        








    }
}
