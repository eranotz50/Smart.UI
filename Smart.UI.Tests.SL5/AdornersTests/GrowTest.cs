using System.Windows;
using System.Windows.Media;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.AdornersTests
{
    [TestClass]
    public class GrowTest:SilverlightTest
    {
        public FlexCanvas Canv;
        public FlexCanvas Element;

        [TestInitialize]
        public void SetUp()
        {
            Canv = new FlexCanvas();
            Element = new FlexCanvas();
            this.TestPanel.Children.Add(Canv);
            Canv.Width = 1000;
            Canv.Height = 1000;            
            Element.SetTop(200).SetLeft(200).SetRight(200).SetBottom(200);
            Canv.AddChild(Element);
            TestPanel.UpdateLayout();
        }

        [TestCleanup]
        public void CleanUp()
        {
            TestPanel.Children.Remove(Canv);
            Canv.RemoveChild(Element);
            Canv = null;
            Element = null;
        }

        /// <summary>
        /// Проверяем правильно ли изначально расположили
        /// </summary>
        [TestMethod]
        public void PosTest()
        {
            var rect = Element.GetBounds();
            rect.X.ShouldBeEqual(200.0);
            rect.Y.ShouldBeEqual(200.0);
            rect.Width.ShouldBeEqual(600.0);
            rect.Height.ShouldBeEqual(600.0);         
        }

        [TestMethod]
        public void ShiftTest()
        {
            Canv.Shift(Element, new Point(100, 100));
            TestPanel.UpdateLayout();
            var rect = Element.GetBounds();
            rect.X.ShouldBeEqual(300.0);
            rect.Y.ShouldBeEqual(300.0);
            rect.Width.ShouldBeEqual(600.0);            
        }

        [TestMethod]
        public void GrowLeftRightCenterTest()
        {         
            
            Element.GrowHorizontal(100.0,AlignmentX.Left);
            TestPanel.UpdateLayout();
            var rect = Element.GetBounds();
            rect.X.ShouldBeEqual(100.0);
            rect.Width.ShouldBeEqual(700.0);
            FlexCanvas.GetRight(Element).AbsoluteValue.ShouldBeEqual(200);            
            Element.GrowHorizontal(-100,AlignmentX.Right);
            FlexCanvas.GetRight(Element).AbsoluteValue.ShouldBeEqual(300);
            var p = Canv.Populate<CanvasPlaceholder>(Element, new Size(Canv.ActualWidth, Canv.ActualHeight));
            p.Right.ShouldBeEqual(300);
            p.Left.ShouldBeEqual(100.0);
            var b = p.GetBoundary(Element.DesiredSize, new Size(Canv.ActualWidth, Canv.ActualHeight));
            b.Width.ShouldBeEqual(600.0);            
            TestPanel.UpdateLayout();
            rect = Element.GetBounds();            
            rect.X.ShouldBeEqual(100.0);
            FlexCanvas.GetRight(Element).AbsoluteValue.ShouldBeEqual(300);
            rect.Width.ShouldBeEqual(600.0);
            Element.GrowHorizontal(100,AlignmentX.Center);
            TestPanel.UpdateLayout();            
            rect = Element.GetBounds();                        
            rect.Width.ShouldBeEqual(700.0);
            rect.X.ShouldBeEqual(50.0);
            rect.Y.ShouldBeEqual(200.0);           
        }

        [TestMethod]
        public void GrowTopMiddleBottomTest()
        {
            Element.GrowVertical(100,AlignmentY.Top);
            TestPanel.UpdateLayout();
            var rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(100.0);
            rect.Width.ShouldBeEqual(600.0);
            rect.Height.ShouldBeEqual(700.0);
            Element.GrowVertical(100,AlignmentY.Bottom);
            TestPanel.UpdateLayout();
            rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(100.0);
            rect.X.ShouldBeEqual(200.0);
            rect.Width.ShouldBeEqual(600.0);
            rect.Height.ShouldBeEqual(800.0);
            Element.GrowVertical(100, AlignmentY.Bottom);          
            TestPanel.UpdateLayout();
            rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(100.0);
            rect.Height.ShouldBeEqual(900.0);
            Element.GrowVertical(-200,AlignmentY.Center);
            TestPanel.UpdateLayout();
            rect = Element.GetBounds();
            rect.Height.ShouldBeEqual(700.0);
            rect.Y.ShouldBeEqual(200.0);
            rect.Width.ShouldBeEqual(600.0);
            rect.X.ShouldBeEqual(200.0);            
        }

        [TestMethod] 
        public void GrowTopRightTests()
        {
            var rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(200.0);
            Element.Grow(new Point(100,100),AlignmentX.Right,AlignmentY.Top);
            TestPanel.UpdateLayout();
            rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(100.0);
            rect.Height.ShouldBeEqual(700.0);
            rect.Width.ShouldBeEqual(700.0);
            rect.X.ShouldBeEqual(200.0);            
        }

        [TestMethod]
        public void GrowTopLeftTests()
        {
            Element.Grow(new Point(100, 100), AlignmentX.Left, AlignmentY.Top);            
            TestPanel.UpdateLayout();
            var rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(100.0);
            rect.Height.ShouldBeEqual(700.0);
            rect.Width.ShouldBeEqual(700.0);
            rect.X.ShouldBeEqual(100.0);

        }

        [TestMethod]
        public void GrowBottomLeftTests()
        {
            Element.Grow(new Point(100, 100), AlignmentX.Left, AlignmentY.Bottom);            
            TestPanel.UpdateLayout();
            var rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(200.0);
            rect.Height.ShouldBeEqual(700.0);
            rect.Width.ShouldBeEqual(700.0);
            rect.X.ShouldBeEqual(100.0);
        }

        [TestMethod]
        public void GrowBottomRightTests()
        {
            Element.Grow(new Point(100, 100), AlignmentX.Right, AlignmentY.Bottom);            
            TestPanel.UpdateLayout();
            var rect = Element.GetBounds();
            rect.Y.ShouldBeEqual(200.0);
            rect.Height.ShouldBeEqual(700.0);
            rect.Width.ShouldBeEqual(700.0);
            rect.X.ShouldBeEqual(200.0);

        }

       
    }
}
