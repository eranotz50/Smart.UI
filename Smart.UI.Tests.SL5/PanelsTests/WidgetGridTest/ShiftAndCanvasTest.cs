using System;
using System.Net;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;


namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class ShiftAndCanvasTest:GridTestBase<WidgetGrid>
    {
        [TestMethod]
        public void GrowCanvasTest()
        {
            var size = this.Grids.Space.Canvas.Size();
            this.Grids.Space.GrowCenteredBy(new Point(size.Width, size.Height));
            this.TestPanel.UpdateLayout();
            this.Grids.Space.Panel.TopLeft().ShouldBeEqual(new Point(500, 500));
            this.Grids.Space.Canvas.Size().ShouldBeEqual(new Size(2000, 2000));
            var bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(300);
            bounds.Y.ShouldBeEqual(300);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
            this.Grids.Space.Zoom(0.5);
            TestPanel.UpdateLayout();
            bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
        }

        [TestMethod]
        public void ShiftTest()
        {
            Grids.Name = "ShiftTest";
            var bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.SetPanelShift(new Point(400, 200));

            this.Grids.SetCanvasSize(new Size(this.Grids.ActualWidth*2, this.Grids.ActualHeight*2));
            TestPanel.UpdateLayout();
            bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(600);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
        }

        [TestMethod]
        public void RelativeShiftTest()
        {
            Grids.Name = "PercentShiftTest";
            var bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.SetCanvasSize(new Size(this.Grids.ActualWidth * 2, this.Grids.ActualHeight * 2));
            this.Grids.SetRelativeShift(new Point(0.2, 0.1));
            TestPanel.UpdateLayout();
            bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(600);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
        }
    }
}
