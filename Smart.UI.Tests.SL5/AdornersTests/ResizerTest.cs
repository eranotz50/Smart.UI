using System.Windows;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets.PanelAdorners;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Tests.AdornersTests
{
   
    [TestClass]
    public class ResizerTest:GridTestBase<WidgetGrid>
    {
        public Resizer Resizer;
        public RectangleAndEllipseBoundary Boundary;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Resizer = new Resizer();
        }
         
        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            this.Resizer = null;
        }

        [TestMethod]
        public void ResizerChangeSizeTest()
        {
            Cell.SetPlace(Cell.GetSlot());
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            Cell.GrowHorizontal(10,AlignmentX.Right);
            TestPanel.UpdateLayout();            
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 110, 100));
            TestPanel.UpdateLayout();
            Cell.Grow(new Point(-10,0),AlignmentX.Right,AlignmentY.Top);
            TestPanel.UpdateLayout();            
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            Resizer.TargetName = Cell.Name;
            Grids.GetFlexCanvasAdorners().Add(Resizer);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            Resizer.Boundary.Host.ShouldBeEqual(Grids);
            Resizer.Boundary.Target.ShouldBeEqual(Cell);
            var b = Resizer.Boundary;
            var fly = GetUp(b.Left);
            fly.CurrentMouse = fly.LastMouse.Substract(new Point(10, 0));
            Cell.OnFly(fly);
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(390, 400, 110, 100));
        }

        [TestMethod]
        public void ResizerHostBoundaryTest()
        {
            this.Grids.GetFlexCanvasAdorners().Add(this.Resizer);
            this.Resizer.Boundaries.Count.ShouldBeEqual(1);
            this.Boundary = this.Resizer.Boundaries[0];
            Resizer.Target.Name.ShouldBeEqual(Grids.Name);
            Resizer.Boundary.Host.ShouldBeEqual(Grids);
            Resizer.Boundary.Target.ShouldBeEqual(Grids);
            this.Boundary.Target.ShouldBeEqual(Grids);
            
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();            
            //Grids.GetLocation().ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            this.Boundary.Left.GetBounds().ShouldBeEqual(new Rect(-5, 0, 5, 1000));
            this.Boundary.Right.GetBounds().ShouldBeEqual(new Rect(1000, 0, 5, 1000));
            this.Boundary.Top.GetBounds().ShouldBeEqual(new Rect(0, -5, 1000, 5));
            this.Boundary.Bottom.GetBounds().ShouldBeEqual(new Rect(0, 1000, 1000, 5));
            var wh = Boundary.CornerSize.Width/2 - 0.5;
            this.Boundary.TopLeft.GetBounds().ShouldBeEqual(new Rect(-wh, -wh, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
            this.Boundary.TopRight.GetBounds().ShouldBeEqual(new Rect(1000-wh, -wh, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
            this.Boundary.BottomLeft.GetBounds().ShouldBeEqual(new Rect(0-wh, 1000-wh, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
            this.Boundary.BottomRight.GetBounds().ShouldBeEqual(new Rect(1000-wh, 1000-wh, Boundary.CornerSize.Width, Boundary.CornerSize.Height));           
        }
        
        [TestMethod]
        public void ResizerTargetBoundaryTest()
        {
           Resizer.TargetName = this.Cell.Name;
           this.Grids.GetFlexCanvasAdorners().Add(this.Resizer);
           this.Resizer.Boundaries.Count.ShouldBeEqual(1);
           this.Boundary = this.Resizer.Boundaries[0];
           Resizer.Target.Name.ShouldBeEqual(Cell.Name);
           TestPanel.UpdateLayout();
           this.Boundary.Left.GetBounds().ShouldBeEqual(new Rect(395, 400, 5, 100));
           this.Boundary.Right.GetBounds().ShouldBeEqual(new Rect(500, 400, 5, 100));
           this.Boundary.Top.GetBounds().ShouldBeEqual(new Rect(400, 395, 100, 5));
           this.Boundary.Bottom.GetBounds().ShouldBeEqual(new Rect(400, 500, 100, 5));
           this.Boundary.TopLeft.GetBounds().ShouldBeEqual(new Rect(400 - this.Resizer.CornerSize.Width / 2 + 0.5, 400 - this.Resizer.CornerSize.Height / 2 + 0.5, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
           this.Boundary.TopRight.GetBounds().ShouldBeEqual(new Rect(500 - this.Resizer.CornerSize.Width / 2 + 0.5, 400 - this.Resizer.CornerSize.Height / 2 + 0.5, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
           this.Boundary.BottomLeft.GetBounds().ShouldBeEqual(new Rect(400 - this.Resizer.CornerSize.Width / 2 + 0.5, 500 - this.Resizer.CornerSize.Height / 2 + 0.5, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
           this.Boundary.BottomRight.GetBounds().ShouldBeEqual(new Rect(500 - this.Resizer.CornerSize.Width / 2 + 0.5, 500 - this.Resizer.CornerSize.Height / 2 + 0.5, Boundary.CornerSize.Width, Boundary.CornerSize.Height));
        }


        [TestMethod]
        public void ResizerAndCanvasSizeChange()
        {
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            Resizer.TargetName = this.Cell.Name;
            this.Grids.GetFlexCanvasAdorners().Add(this.Resizer);
            TestPanel.UpdateLayout();
            this.Boundary = Resizer.Boundary;
            this.Boundary.Left.GetBounds().ShouldBeEqual(new Rect(395, 400, 5, 100));
            this.Grids.SetCanvasSize(new Size(this.Grids.ActualWidth * 2, this.Grids.ActualHeight * 2));
            TestPanel.UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(new Rect(800, 800, 200, 200));
            this.Boundary.Left.GetBounds().ShouldBeEqual(new Rect(795, 800, 5, 200));
        }


        [TestMethod]
        public void AddRemoveBoundaryTest()
        {
            this.Grids.GetFlexCanvasAdorners().Add(this.Resizer);
            TestPanel.UpdateLayout();            
            Resizer.AddBoundary(this.Cell).AddChildren();
            TestPanel.UpdateLayout();                
            var b = this.Resizer.BoundaryByTarget(this.Cell);
            b.Left.GetBounds().ShouldBeEqual(new Rect(395, 400, 5, 100));
            b.Bottom.GetBounds().ShouldBeEqual(new Rect(400, 500, 100, 5));
            this.Grids.Children.Contains(b.Left).ShouldBeTrue();
            this.Grids.Children.Contains(b.BottomRight).ShouldBeTrue();
            this.Grids.Children.ShouldContain(b.Left).ShouldContain(b.BottomRight).ShouldContain(b.BottomRight);
            var b2 = this.Resizer.BoundaryByTarget(this.Cell).ShouldBeSame(b); 
            this.Resizer.DeactivateBoundaryByTarget(this.Cell);
            b2.Host.ShouldBeEqual(null);
            b2.Target.ShouldBeEqual(null);
            b2.Activated.ShouldBeEqual(false);
            TestPanel.UpdateLayout();
            this.Grids.Children.ShouldNotContain(b.Left).ShouldNotContain(b.BottomRight).ShouldNotContain(b.BottomRight);
            var b3 = this.Resizer.AddBoundary(this.Cell);
            b3.ShouldBeSame(b);
            b3.AddChildren().AfterLayoutUpdate(this.TestPanel).Left.GetBounds().ShouldBeEqual(new Rect(395, 400, 5, 100));
        }
    }
}
