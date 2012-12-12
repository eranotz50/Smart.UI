using System.Windows;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.UI.Relatives;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Tests.PanelsTests;

namespace Smart.UI.Tests.RelativeTests
{
    
    [TestClass]
    public class RelativeDragTest:GridTestBase<SmartGrid>
    {
        public FrameworkElement LeftAdorner;
        public FrameworkElement RightAdorner;
        public FrameworkElement TopAdorner;
        public FrameworkElement BottomAdorner;
        public FrameworkElement TopLeftAdorner;
        public FrameworkElement TopRightAdorner;
        public FrameworkElement BottomLeftAdorner;
        public FrameworkElement BottomRightAdorner;
        


        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.LeftAdorner = new Rectangle() { Width = 10, Height = 10 };
            this.RightAdorner = new Rectangle() { Width = 10, Height = 10 };
            this.TopAdorner = new Rectangle() { Width = 10, Height = 10 };
            this.BottomAdorner = new Rectangle() { Width = 10, Height = 10 };
            
            this.TopLeftAdorner = new Rectangle() { Width = 10, Height = 10 };
            this.TopRightAdorner = new Rectangle() { Width = 10, Height = 10 };
            this.BottomLeftAdorner = new Rectangle() { Width = 10, Height = 10 };
            this.BottomRightAdorner = new Rectangle() { Width = 10, Height = 10 };


            this.Grids.AddChild(this.LeftAdorner.SetLeft().SetTop());
            this.Grids.AddChild(this.RightAdorner.SetLeft().SetTop());
            this.Grids.AddChild(this.TopAdorner.SetLeft().SetTop());
            this.Grids.AddChild(this.BottomAdorner.SetLeft().SetTop());

            this.Grids.AddChild(this.TopLeftAdorner.SetLeft().SetTop());
            this.Grids.AddChild(this.TopRightAdorner.SetLeft().SetTop());
            this.Grids.AddChild(this.BottomLeftAdorner.SetLeft().SetTop());
            this.Grids.AddChild(this.BottomRightAdorner.SetLeft().SetTop());


            this.LeftAdorner.SetRelativeTo(Cell.Name).SetStringPosition("left").SetDragGrowthMode(GrowthMode.Left);
            this.RightAdorner.SetRelativeTo(Cell.Name).SetStringPosition("right").SetDragGrowthMode(GrowthMode.Right);
            this.TopAdorner.SetRelativeTo(Cell.Name).SetStringPosition("top").SetDragGrowthMode(GrowthMode.Top);
            this.BottomAdorner.SetRelativeTo(Cell.Name).SetStringPosition("bottom").SetDragGrowthMode(GrowthMode.Bottom);
            this.TopLeftAdorner.SetRelativeTo(Cell.Name).SetStringPosition("topleft").SetDragGrowthMode(GrowthMode.TopLeft);
            this.TopRightAdorner.SetRelativeTo(Cell.Name).SetStringPosition("topright").SetDragGrowthMode(GrowthMode.TopRight);
            this.BottomLeftAdorner.SetRelativeTo(Cell.Name).SetStringPosition("bottomleft").SetDragGrowthMode(GrowthMode.BottomLeft);
            this.BottomRightAdorner.SetRelativeTo(Cell.Name).SetStringPosition("bottomright").SetDragGrowthMode(GrowthMode.BottomRight);

            this.Cell.SetLeft(400).SetTop(400).SetWidth(100).SetHeight(100).SetRelativeToGrid(false);
            TestPanel.UpdateLayout();            
            

            this.TestPanel.UpdateLayout();
            this.TestPanel.UpdateLayout();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            this.LeftAdorner = null;
            this.RightAdorner = null;
            this.TopAdorner = null;
            this.BottomAdorner = null;

            this.TopLeftAdorner = null;
            this.TopRightAdorner = null;
            this.BottomLeftAdorner = null;
            this.BottomRightAdorner = null;
        }


        [TestMethod]
        public void PosAdornersTest()
        {            
            TestPanel.UpdateLayout();
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(390, 0, 10, 10));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(500, 0, 10, 10));
            this.TopAdorner.GetBounds().ShouldBeEqual(new Rect(0, 390, 10, 10));            
            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(0, 500, 10, 10));
            this.TopLeftAdorner.GetBounds().ShouldBeEqual(new Rect(390, 390, 10, 10));
            this.TopRightAdorner.GetBounds().ShouldBeEqual(new Rect(500, 390, 10, 10));
            this.BottomLeftAdorner.GetBounds().ShouldBeEqual(new Rect(390, 500, 10, 10));
            this.BottomRightAdorner.GetBounds().ShouldBeEqual(new Rect(500, 500, 10, 10));

        }

        [TestMethod]
        public void RelativeDragDragRightTest()
        {
            Relative.GetDragGrowthMode(RightAdorner).ShouldBeEqual(GrowthMode.Right);
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            var f = this.GetUp(this.RightAdorner);
            f.CurrentMouse = f.LastMouse.Add(new Point(50, 0));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(500, 0, 10, 10));   
            Grids.OnFly(f);
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 150, 100));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(550, 0, 10, 10));           
        }


    }
}
