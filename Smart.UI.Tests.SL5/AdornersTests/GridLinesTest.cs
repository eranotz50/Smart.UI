using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.TestExtensions;
using Smart.UI.Panels;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets.PanelAdorners;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Tests.AdornersTests
{
    /// <summary>
    /// Testing gridlines adorner
    /// </summary>
    [TestClass]
    public class GridLinesTest:GridTestBase<WidgetGrid>
    {
        public GridLines Lines;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            Lines = new GridLines();
            this.Grids.GetSmartGridAdorners().Add(this.Lines);
            this.TestPanel.UpdateLayout();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            Lines = null;
        }

        [TestMethod]
        public void ColumnMovementTest()
        {
            this.TestPanel.UpdateLayout();
            var line = this.Lines.Cols[4];
            line.Parent.ShouldBeSame(this.Grids);
            line.GetColumn().ShouldBeEqual(4);
            this.TestPanel.UpdateLayout();
            this.Lines.Cols[4].ShouldBeSame(line);
            var bounds = line.GetBounds();
            bounds.X.ShouldBeEqual(400+this.Grids.ColumnDefinitions[4].AbsoluteValue- bounds.Width);
            //Grids.DragManager.ShouldBeInstanceOf(typeof(WidgetDragManager));
            DragManager.GetDragSubject(line).ShouldBeInstanceOf(typeof (ColumnDragSubject));
            //GridDragManager.GetColumnMover(line).ShouldNotBeNull();
            var fly = this.Grids.DragManager.StartFlight(this.Grids.DragManager.BuildDragObject(line), f => SimplePanel.GetPlace(f as FrameworkElement).TopLeft());
            fly.Target.ShouldBeEqual(line);
            DragManager.GetDragSubject(line).ShouldBeInstanceOf(typeof(ColumnDragSubject));
            DragManager.GetDragSubject(fly.Target).ShouldBeInstanceOf(typeof(ColumnDragSubject));
            bounds = fly.Target.GetBounds();
            bounds.X.ShouldBeEqual(400 + this.Grids.ColumnDefinitions[4].AbsoluteValue - bounds.Width);           
            fly.CurrentMouse = new Point(50, 50);
            fly.Pos();
            TestPanel.UpdateLayout();
            bounds = line.GetBounds();
            bounds.X.ShouldBeEqual(400 + this.Grids.ColumnDefinitions[4].AbsoluteValue - bounds.Width);
            bounds.X.ShouldBeEqual(400 + 150 - bounds.Width);
            
        }
    }
}
