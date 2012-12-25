using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Silverlight.Testing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class SimplePanelTest : SilverlightTest
    {

        protected FlexCanvas[] FlexCanvases;
        protected FlexGrid[] FlexGrids;
        protected Canvas[] Canvases;
        protected ContentControl[] Controls;
        protected Rect[] Rects;
        protected int Count = 10;

        [TestInitialize]
        public virtual void SetUp()
        {
            FlexCanvases = new FlexCanvas[Count];
            FlexGrids = new FlexGrid[Count];
            Canvases = new Canvas[Count];
            Controls = new ContentControl[Count];
            for (var i = 0; i < Count; i++)
            {
                FlexCanvases[i] = new FlexCanvas {Name = "FlexCanvas_" + i.ToString()};
                FlexGrids[i] = new FlexGrid {Name = "FlexGrid_" + i.ToString()};
                Canvases[i] = new Canvas {Name = "Canvas_" + i.ToString()};
                this.Controls[i] = new ContentControl() { Name = "Control_" + i.ToString() };
            }
            Rects = new Rect[Count];
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            TestPanel.Children.Clear();
            foreach (var item in Canvases) item.Children.Clear();
            foreach (var item in FlexCanvases) item.Children.Clear();
            foreach (var item in FlexGrids) item.Children.Clear();
            Canvases = null;
            FlexCanvases = null;
            FlexGrids = null;
        }

        protected void MakeMatreshka()
        {
            FlexCanvases[0].SetPanelSize(new Size(100*Count, 100*Count));
            TestPanel.Children.Add(FlexCanvases[0]);
            for (var i = 1; i < Count; i++)
            {
                FlexCanvases[i].SetPanelSize(new Size(100*Count - i*100, 100*Count - i*100));
                FlexCanvases[i - 1].Children.Add(FlexCanvases[i]);
                //FlexCanvases[0].Dragged.HasObserver(FlexCanvases[i].Dragged).ShouldBeTrue();
                SimplePanel.SetPos(FlexCanvases[i], new Pos3D(100, 100));
            }
            TestPanel.InvalidateArrange();
            TestPanel.UpdateLayout();
            for (var i = 0; i < Count; i++) Rects[i] = FlexCanvases[i].GetBoundsTop();
        }

        [TestMethod]
        public void RemoveFromChildrenTest()
        {
            MakeMatreshka();
            Assert.AreSame(FlexCanvases[0], FlexCanvases[1].Parent);
            FlexCanvases[0].Children.Remove(FlexCanvases[1]);
            Assert.AreEqual(null, FlexCanvases[1].Parent);
            FlexCanvases[0].Children.Add(FlexCanvases[1]);
            Assert.AreSame(FlexCanvases[0], FlexCanvases[1].Parent);
        }

        [TestMethod]
        public void RemoveChildrenTest()
        {
            var p = this.FlexCanvases[0];
            for (int i = 0; i < Count; i++)
            {
                p.AddChild(this.FlexGrids[i]);
                p.AddChild(this.Controls[i]);
                p.AddChild(this.Canvases[i]);
            }
            p.Children.Count.ShouldBeEqual(30);
            p.RemoveChildren(i=>i is Canvas);
            p.Children.Count.ShouldBeEqual(20);
            p.RemoveChildren(i=>i is ContentControl);
            p.Children.Count(i => i is FlexGrid).ShouldBeEqual(10);
        }

        [TestMethod]
        public void MoveToAnotherPanelTest()
        {
            MakeMatreshka();
            var pos = SimplePanel.GetPos(FlexCanvases[8]);
            FlexCanvases[7].ChildToPanel(FlexCanvases[8], FlexCanvases[0]);
            TestPanel.UpdateLayout();

            var currentPos = SimplePanel.GetPos(FlexCanvases[8]);
            currentPos.ShouldBeEqual(new Pos3D(pos.X + FlexCanvases[7].GetBoundsTop().X, pos.Y + FlexCanvases[7].GetBoundsTop().Y));

            FlexCanvases[0].ChildToPanel(FlexCanvases[8], FlexCanvases[7]);
            TestPanel.UpdateLayout();
            currentPos = SimplePanel.GetPos(FlexCanvases[8]);
            currentPos.ShouldBeEqual(pos);
        }

        [TestMethod]
        public void TestRelativeRect()
        {
            MakeMatreshka();
            FlexCanvases[8].GetBounds().ShouldBeEqual(new Rect(100,100,200,200));
            FlexCanvases[7].GetBounds().ShouldBeEqual(new Rect(100, 100, 300, 300));
            FlexCanvases[6].GetBounds().ShouldBeEqual(new Rect(100, 100, 400, 400));
            FlexCanvases[8].GetRelativePosition(FlexCanvases[6]).ShouldBeEqual(new Point(200,200));
            FlexCanvases[6].GetRelativePosition(FlexCanvases[8]).ShouldBeEqual(new Point(-200, -200));
            FlexCanvases[6].GetRelativeRect(FlexCanvases[8]).ShouldBeEqual(new Rect(-200,-200,400,400));
            FlexCanvases[8].GetRelativeRect(FlexCanvases[6]).ShouldBeEqual(new Rect(200, 200, 200, 200));
        }
       
        [TestMethod]
        public void RelativeRectWithTransformsTest()
        {
            MakeMatreshka();            
            FlexCanvases[7].GetBounds().ShouldBeEqual(new Rect(100, 100, 300, 300));
            var transform = new TranslateTransform();
            FlexCanvases[7].RenderTransform = transform;
            transform.X = 100;
            FlexCanvases[7].GetBounds().ShouldBeEqual(new Rect(200, 100, 300, 300));
            transform.Y = 100;
            FlexCanvases[7].GetBounds().ShouldBeEqual(new Rect(200, 200, 300, 300));            
        }

        [TestMethod]
        public void OutOfPanelTest()
        {
            MakeMatreshka();
            var outQ = 0;
            var partialOutQ = 0;
            FlexCanvases[7].OutMode = OutMode.Watch;            
            FlexCanvases[7].OutPartial.Subscribe(i => partialOutQ += 1);
            FlexCanvases[7].Out.Subscribe(i => outQ += 1);
            TestPanel.UpdateLayout();
            outQ.ShouldBeEqual(0);
            partialOutQ.ShouldBeEqual(0);
            FlexCanvases[7].GetIntersection(FlexCanvases[8]).ShouldBeEqual(Intersection.Full);

            var bounds8 = FlexCanvases[8].GetBounds();//
            bounds8.X.ShouldBeEqual(100);
            bounds8.Y.ShouldBeEqual(100);
            bounds8.Width.ShouldBeEqual(200);
            bounds8 = FlexCanvases[8].GetBoundsTop();
            bounds8.X.ShouldBeEqual(800);
            bounds8.Y.ShouldBeEqual(800);
         

            var bounds7 = FlexCanvases[7].GetBoundsTop();            
            bounds7.X.ShouldBeEqual(700);
            bounds7.Y.ShouldBeEqual(700);
            bounds7.Width.ShouldBeEqual(300);
            bounds7.Height.ShouldBeEqual(300);
            var partialOutPos = new Point(0, FlexCanvases[7].Height - 10);
            SimplePanel.SetPos(FlexCanvases[8], new Pos3D(partialOutPos));
            TestPanel.UpdateLayout();

            
            
            FlexCanvases[7].GetIntersection(FlexCanvases[8]).ShouldBeEqual(Intersection.Partial);
            outQ.ShouldBeEqual(0);
            partialOutQ.ShouldBeEqual(1);
            var outPos = new Pos3D(FlexCanvases[7].Width + FlexCanvases[8].Width, FlexCanvases[7].Height + FlexCanvases[8].Height);
            SimplePanel.SetPos(FlexCanvases[8],outPos);
            TestPanel.UpdateLayout();
            /*
            var size = new Size(FlexCanvases[7].ActualWidth, FlexCanvases[7].ActualHeight);
            if (size.HasNullInfinityOrNaN()) size = new Size(FlexCanvases[7].Width, FlexCanvases[7].Height);
            size = FlexCanvases[7].ExtractSize(size);
             
            FlexCanvases[7].ArrangeChild(FlexCanvases[8],size);
             */
            bounds7 = FlexCanvases[7].GetBoundsTop();
            bounds7.X.ShouldBeEqual(700);
            bounds7.Y.ShouldBeEqual(700);
            bounds7.Width.ShouldBeEqual(300);
            bounds7.Height.ShouldBeEqual(300);

            var pos = SimplePanel.GetPos(FlexCanvases[8]);
            pos.X.ShouldBeEqual(500);
            pos.Y.ShouldBeEqual(500);
            (pos.X+bounds7.X).ShouldBeEqual(1200);
            FlexCanvases[7].Children.Contains(FlexCanvases[8]).ShouldBeTrue();
            TestPanel.UpdateLayout(); 
            /*
            bounds8 = FlexCanvases[8].GetBounds();//
            bounds8.X.ShouldBeEqual(500);
            bounds8.Y.ShouldBeEqual(500);
            */
         
            FlexCanvases[7].GetIntersection(FlexCanvases[8]).ShouldBeEqual(Intersection.None);
            outQ.ShouldBeEqual(1);
            partialOutQ.ShouldBeEqual(1);
        }


        /// <summary>
        /// Проверяем точность плейсинга
        /// </summary>
        [TestMethod]
        public void PlacesTest()
        {
            MakeMatreshka();
            for (int i = 0; i < Count; i++)
            {
                var desiredRect = new Rect(new Point(i * 100, i * 100), new Size(100 * Count - i * 100, 100 * Count - i * 100));
                Rects[i].ShouldBeEqual(desiredRect);
            }
            var desiredPos = new Point(700, 700);
            var actualPos = new Point(FlexCanvases[7].GetBoundsTop().X, FlexCanvases[7].GetBoundsTop().Y);
            actualPos.ShouldBeEqual(desiredPos);
        }

       


    }
}
