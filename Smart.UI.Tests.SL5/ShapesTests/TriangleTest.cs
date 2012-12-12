using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.PathTests
{
    [TestClass]
    public class TriangleTest:PanelTestBase<FlexCanvas>
    {
        public Triangle Triangle;
       
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Triangle = new Triangle();
            this.Panel.AddChild(this.Triangle);
        }

        [TestMethod]
        public void TestTriangleOrienations()
        {
            this.Triangle.SetLeft(500).SetTop(500);
            this.Triangle.Width = 100;
            this.Triangle.Height = 100;
            Triangle.Orientation.ShouldBeEqual(TriangleOrientation.Top);            
            TestPanel.UpdateLayout();
            Triangle.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            Triangle.A.ShouldBeEqual(new Point(100.0, 100.0));
            Triangle.B.ShouldBeEqual(new Point(0.0, 100.0));
            Triangle.C.ShouldBeEqual(new Point(50.0, 0.0));
            Triangle.Orientation = TriangleOrientation.Bottom;
            TestPanel.UpdateLayout();
            Triangle.A.ShouldBeEqual(new Point(0.0, 0.0));
            Triangle.B.ShouldBeEqual(new Point(100.0, 0.0));
            Triangle.C.ShouldBeEqual(new Point(50.0, 100.0));            
            Triangle.Orientation = TriangleOrientation.Left;
            TestPanel.UpdateLayout();
            Triangle.A.ShouldBeEqual(new Point(100.0, 0.0));
            Triangle.B.ShouldBeEqual(new Point(100.0, 100.0));
            Triangle.C.ShouldBeEqual(new Point(0.0, 50.0));
            Triangle.Orientation = TriangleOrientation.Right;
            TestPanel.UpdateLayout();
            Triangle.A.ShouldBeEqual(new Point(0.0, 100.0));
            Triangle.B.ShouldBeEqual(new Point(0.0, 0.0));
            Triangle.C.ShouldBeEqual(new Point(100.0, 50.0));
            

        }

    }
}
