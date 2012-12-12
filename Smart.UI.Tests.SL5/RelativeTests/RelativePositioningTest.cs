using System;
using System.Windows;
using System.Windows.Shapes;
using Smart.UI.Panels;
using Smart.UI.Relatives;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Smart.UI.Tests.RelativeTests
{
  
    [TestClass]
    public class RelativePositioningTest:GridTestBase<WidgetGrid>
    {
        public FrameworkElement LeftAdorner;
        public FrameworkElement RightAdorner;
        public FrameworkElement TopAdorner;
        public FrameworkElement BottomAdorner;
        public FrameworkElement FreeAdorner;


        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.LeftAdorner = new Rectangle() {Name = "LeftAdorner",Width = 100, Height = 100};
            this.RightAdorner = new Rectangle() { Name = "RightAdorner", Width = 100, Height = 100 };
            this.TopAdorner = new Rectangle() { Name = "TopAdorner", Width = 100, Height = 100 };
            this.BottomAdorner = new Rectangle() { Name = "BottomAdorner", Width = 100, Height = 100 };
            this.FreeAdorner = new Rectangle() { Name = "FreeAdorner", Width = 100, Height = 100 };
            this.Grids.AddChild(this.LeftAdorner);
            this.Grids.AddChild(this.RightAdorner);
            this.Grids.AddChild(this.TopAdorner);
            this.Grids.AddChild(this.BottomAdorner);
            this.Grids.AddChild(this.FreeAdorner);
            this.TestPanel.UpdateLayout();
            RelativeConverter.OddActions = false;
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            this.LeftAdorner = null;
            this.RightAdorner = null;
            this.TopAdorner = null;
            this.BottomAdorner = null;
            this.FreeAdorner = null;
        }
        [TestMethod]
        public void UpdateTest()
        {
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.LeftAdorner.SetSlot(new Rect(300, 300, 100, 100));
            TestPanel.UpdateLayout();
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
         //   this.LeftAdorner.UpdateArrange();
            this.Grids.InvalidateArrange();
            TestPanel.UpdateLayout();
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(300, 300, 100, 100));
        }


        [TestMethod]
        public void RelativePositionTest()
        {            
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400,400,100,100));
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.Cell.SetOnArrange(i => this.LeftAdorner.PosLeft(i.Param1.X-i.Param1.Width-10).PosTop(i.Param1.Y));
            this.Cell.UpdateOnArrange(i => this.RightAdorner.PosLeft(10 + i.Param1.X + i.Param1.Width).PosTop(i.Param1.Y));
            this.Cell.UpdateOnArrange(new SimpleSubject<Args<FrameworkElement,Rect,Size>>(i => this.TopAdorner.PosLeft(i.Param1.X).PosTop(i.Param1.Y-i.Param1.Height-10)));
            this.Cell.UpdateOnArrange(i => this.BottomAdorner.PosTop(10 + i.Param1.Y + i.Param1.Height).PosLeft(i.Param1.X));
            this.Cell.UpdateOnArrange(i => this.FreeAdorner.PosLeft(i.Param1.X - i.Param1.Width - 10).PosTop(i.Param1.Y - i.Param1.Height - 10));
            Cell.InvalidateMeasure();
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();            
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(290, 400, 100, 100));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(510, 400, 100, 100));
            this.TopAdorner.GetBounds().ShouldBeEqual(new Rect(400, 290, 100, 100));
            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(400, 510, 100, 100));
            this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(290, 290, 100, 100));            
        }

       

        [TestMethod]
        public void LayoutCycleTest()
        {

            this.BottomAdorner.PutTheBottomOf(Cell, 10);
            TestPanel.UpdateLayout();

            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(0, 510, 100, 100));

            Cell.SetRowSpan(2).SetColumnSpan(2).SetRow(5).SetColumn(5);
            TestPanel.UpdateLayout();
            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(0, 710, 100, 100));
        }
      
       
        [TestMethod]
        public void RelativeSizingTest()
        {
            this.FreeAdorner.SetPositionFrom(Cell);
            this.FreeAdorner.SetHeightFrom(Cell, 10);
            this.FreeAdorner.SetWidthFrom(Cell, -10);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            //this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(500, 500, 90, 110));
            this.FreeAdorner.ActualWidth.ShouldBeEqual(90.0);
            this.FreeAdorner.ActualHeight.ShouldBeEqual(110.0);
            Cell.SetRowSpan(2).SetColumnSpan(2);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            this.FreeAdorner.ActualWidth.ShouldBeEqual(190.0);
            this.FreeAdorner.ActualHeight.ShouldBeEqual(210.0);
            
         //   this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(600, 600, 190, 210));
            
        }

        [TestMethod]
        public void SetRelativePositionsTest()
        {
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            
            this.LeftAdorner.PutLeftFrom(Cell, 10);
            TestPanel.UpdateLayout();
            
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(290, 0, 100, 100));
            
            this.RightAdorner.PutRightFrom(Cell, 10);
            TestPanel.UpdateLayout();
            
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(510, 0, 100, 100));
            
            this.TopAdorner.PutOnTopOf(Cell, 10);
            TestPanel.UpdateLayout();
            
            this.TopAdorner.GetBounds().ShouldBeEqual(new Rect(0, 290, 100, 100));
            
            this.BottomAdorner.PutTheBottomOf(Cell, 10);
            TestPanel.UpdateLayout();
            
            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(0, 510, 100, 100));           

            //TestPanel.UpdateLayout();

            // this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(290, 290, 100, 100));

        }

        [TestMethod]
        public void SetPositionFromTest()
        {
            this.FreeAdorner.SetPositionFrom(Cell, new Point(10, 10));
            TestPanel.UpdateLayout();           

            this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(510, 510, 100, 100));
            Cell.SetOnArrange(new SimpleSubject<Args<FrameworkElement, Rect, Size>>());            
            this.FreeAdorner.SetPositionFrom(Cell, new Point(-10, -10));
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            
            this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(290, 290, 100, 100)); 
        }

        [TestMethod]
        public void MiddleRightBottomCenterTest()
        {
            this.FreeAdorner.SetLeft(100).SetTop(100).SetHeight(100).SetWidth(100);
            var relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosRightMiddle,null,null);
            this.RightAdorner.SetRelativePosition(relPos);            
            this.RightAdorner.SetRelativeTo(this.FreeAdorner.Name);

            relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosCenterBottom,null,null);
            this.BottomAdorner.SetRelativePosition(relPos);
            this.BottomAdorner.SetRelativeTo(this.FreeAdorner.Name);

            this.UpdateLayout();

            this.FreeAdorner.GetSlot().ShouldBeEqual(new Rect(100, 100, 100, 100));

            this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(100, 100, 100, 100));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(200, 100, 100, 100));

            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(100, 200, 100, 100));
            this.FreeAdorner.SetHeight(200).SetWidth(200);            
            this.UpdateLayout();

            this.FreeAdorner.GetSlot().ShouldBeEqual(new Rect(100, 100, 200, 200));


            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(300, 150, 100, 100));
            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(150, 300, 100, 100));            
        }


        [TestMethod]
        public void MiddleLeftTopCenterTest()
        {
            this.FreeAdorner.SetLeft(100).SetTop(100).SetHeight(100).SetWidth(100);
            var relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosLeftMiddle, null, null);
            this.LeftAdorner.SetRelativePosition(relPos);
            this.LeftAdorner.SetRelativeTo(this.FreeAdorner.Name);

            relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosCenterTop, null, null);
            this.TopAdorner.SetRelativePosition(relPos);
            this.TopAdorner.SetRelativeTo(this.FreeAdorner.Name);

            this.UpdateLayout();

            this.FreeAdorner.GetSlot().ShouldBeEqual(new Rect(100, 100, 100, 100));

            this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(100, 100, 100, 100));
            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(0, 100, 100, 100));

            this.TopAdorner.GetBounds().ShouldBeEqual(new Rect(100, 0, 100, 100));
            this.FreeAdorner.SetLeft(200).SetTop(200);
            this.UpdateLayout();

            this.FreeAdorner.GetSlot().ShouldBeEqual(new Rect(200, 200, 100, 100));


            this.LeftAdorner.GetBounds().ShouldBeEqual(new Rect(100, 200, 100, 100));
            this.TopAdorner.GetBounds().ShouldBeEqual(new Rect(200, 100, 100, 100));
        }
     

        [TestMethod]
        public void RelativeWithParamsChain()
        {
            this.FreeAdorner.SetLeft(100).SetTop(100).SetHeight(100).SetWidth(100);
            var relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosRightMiddle, null, null);
            this.RightAdorner.SetRelativePosition(relPos);
            this.RightAdorner.SetRelativeTo(this.FreeAdorner.Name);

            relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosCenterBottom, null, null);
            this.BottomAdorner.SetRelativePosition(relPos);
            this.BottomAdorner.SetRelativeTo(this.RightAdorner.Name);


            relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(RelativeFunctions.PosRightMiddle, null, null);            
            this.TopAdorner.SetRelativePosition(relPos); ///position and name are different
            this.TopAdorner.SetRelativeTo(this.BottomAdorner.Name);

            this.UpdateLayout();

            this.FreeAdorner.GetBounds().ShouldBeEqual(new Rect(100, 100, 100, 100));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(200, 100, 100, 100));
            this.BottomAdorner.GetBounds().ShouldBeEqual(new Rect(200, 200, 100, 100));
            this.TopAdorner.GetBounds().ShouldBeEqual(new Rect(300, 200, 100, 100));            
        }

        [TestMethod]
        public void RelativeMixedTest(){

            this.FreeAdorner.SetLeft(100).SetTop(100).SetHeight(100).SetWidth(100);
            Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> rfunc = RelativeConverter.Instance.ConvertFrom("right;bottom");
            var relPos = new ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>(rfunc,null,null);
            this.RightAdorner.SetRelativePosition(relPos);            
            this.RightAdorner.SetRelativeTo(this.FreeAdorner.Name);
            this.UpdateLayout();
            this.RightAdorner.GetSlot().ShouldBeEqual(new Rect(200,200,100,100));
            this.RightAdorner.GetBounds().ShouldBeEqual(new Rect(200,200,100,100));
            
            rfunc = RelativeConverter.Instance.ConvertFrom("sameleft;bottom");
            relPos.Action = rfunc;
            this.Grids.InvalidateMeasure();
            this.UpdateLayout();
         
            this.RightAdorner.GetSlot().ShouldBeEqual(new Rect(100, 200, 100, 100));

            rfunc = RelativeConverter.Instance.ConvertFrom("right;sametop");
            relPos.Action = rfunc;
            this.Grids.InvalidateMeasure();            
            this.UpdateLayout();

            this.RightAdorner.GetSlot().ShouldBeEqual(new Rect(200, 100, 100, 100));

        }
/*
        [TestMethod]
        public void TestRightWithParams()
        {
            throw new NotImplementedException();
        }
        */
    }
}
