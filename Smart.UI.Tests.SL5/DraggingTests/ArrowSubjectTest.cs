using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.DraggingTests
{
    [TestClass]
    public class ArrowSubjectTest:TestBases.PanelTestBase<FlexCanvas>
    {
        public ArrowSubject ArrowSubject;
        public Rectangle Red;
        public Rectangle Green;
        public String XamlStr;

        [TestInitialize]
        public override void SetUp()
        {           
  
        }

        public void Init()
        {
            this.Panel = XamlReader.Load(XamlStr) as FlexCanvas;
            this.Red = Panel.ChildByName<Rectangle>("red");
            this.Green = Panel.ChildByName<Rectangle>("green");
            this.TestPanel.Children.Add(this.Panel);
            this.TestPanel.UpdateLayout();
            this.Red.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.Green.GetBounds().ShouldBeEqual(new Rect(200, 200, 100, 100));            
        }

        [TestCleanup]
        public override void CleanUp()
        {            
            base.CleanUp();
            this.Red = null;
            this.Green = null;            
        }

        [TestMethod]
        public void TestArrowDrag()
        {
            this.XamlStr = 
                @"<FlexCanvas FlexCanvas.Left='10' FlexCanvas.Top='100' FlexCanvas.Bottom='100' FlexCanvas.Right='10' Background='Navy' DragManager.ChildrenDockMode='DockEverywhere' DragManager.ChildrenDragCanvas='NearestParent'>           
            <Rectangle Name='red' Fill='Red' Width='100' Height='100' SimplePanel.Pos='0;0'>
            </Rectangle>
            <Rectangle Name='green' Fill='Green' Width='100' Height='100' SimplePanel.Pos='200;200'/>
        </FlexCanvas>".WithNamespace();         
            this.Init();
            this.ArrowSubject = new ArrowSubject();
            this.Red.SetDragSubject(this.ArrowSubject);
            var fly = this.GetUp(this.Red);
            fly.WithCurrent(new Point(100,100)).Pos();
            this.TestPanel.UpdateLayout();
            var sub = fly.Target.GetDragSubject<ArrowSubject>();
            sub.ShouldBeEqual(this.ArrowSubject);
            sub.Arrow.ShouldNotBeNull();
            sub.Arrow.StartPoint.ShouldBeEqual(new Point(0, 0));
            sub.Arrow.EndPoint.ShouldBeEqual(new Point(100, 100));
            sub.Arrow.Parent.ShouldBeEqual(this.Panel);
            fly.WithCurrent(new Point(200, 200)).Pos();
            sub.Arrow.StartPoint.ShouldBeEqual(new Point(0, 0));
            sub.Arrow.EndPoint.ShouldBeEqual(new Point(200, 200));            
        }

       
        [TestMethod]
        public void XamlSubjectDragTest()
        {
            this.XamlStr =
                @"<FlexCanvas FlexCanvas.Left='10' FlexCanvas.Top='100' FlexCanvas.Bottom='100' FlexCanvas.Right='10' Background='Navy' DragManager.ChildrenDockMode='DockEverywhere' DragManager.ChildrenDragCanvas='NearestParent'>           
            <Rectangle Name='red' Fill='Red' Width='100' Height='100' SimplePanel.Pos='0;0'>
                <DragManager.DragSubject> <ArrowSubject/></DragManager.DragSubject>
            </Rectangle>
            <Rectangle Name='green' Fill='Green' Width='100' Height='100' SimplePanel.Pos='200;200'/>
        </FlexCanvas>".WithNamespace();
            this.Init();          
            var fly = this.GetUp(this.Red);
            fly.WithCurrent(new Point(100, 100)).Pos();
            this.TestPanel.UpdateLayout();
            this.ArrowSubject = fly.Target.GetDragSubject<ArrowSubject>();
            this.ArrowSubject.Arrow.ShouldNotBeNull();
            this.ArrowSubject.Arrow.StartPoint.ShouldBeEqual(new Point(0, 0));
            this.ArrowSubject.Arrow.EndPoint.ShouldBeEqual(new Point(100, 100));
            this.ArrowSubject.Arrow.Parent.ShouldBeEqual(this.Panel);
            fly.WithCurrent(new Point(200, 200)).Pos();
            this.ArrowSubject.Arrow.StartPoint.ShouldBeEqual(new Point(0, 0));
            this.ArrowSubject.Arrow.EndPoint.ShouldBeEqual(new Point(200, 200));
        }

        [TestMethod]
        public void ComplexArrowDragTest()
        {
        }
    }
}
