using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Tests.ScrollAndZoomTests
{
    [TestClass]
    public class ZoomTest:GridTestBase<WidgetGrid>
    {
      
        [TestMethod]
        public void TestZoomIn()
        {
            Grids.Name = "ZoomInTest";
            var bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.SetCanvasSize(new Size(this.Grids.ActualWidth*2,this.Grids.ActualHeight*2));
            TestPanel.UpdateLayout();
            bounds = this.Cell.GetBounds();            
            bounds.X.ShouldBeEqual(800);
            bounds.Y.ShouldBeEqual(800);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);            
        }


        [TestMethod]
        public void ZoomInOutTest()
        {
            this.Grids.Space.Zoom(2);
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
        public void AnimatedZoomTest()
        {
            this.Grids.Zoom(2, new TimeSpan(0, 0, 0, 1)).Go();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));                                   
            this.TestPanel.UpdateLayout();            
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            this.TestPanel.UpdateLayout();                        
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));            
            this.TestPanel.UpdateLayout();
            
            var bounds = this.Cell.GetBounds();                      
            bounds.X.ShouldBeEqual(300);
            bounds.Y.ShouldBeEqual(300);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
        }

        [TestMethod]
        public void ZoomAtTest()
        {
            this.Grids.ZoomAt(this.Cell);
            this.TestPanel.UpdateLayout();
            var bounds = this.Cell.GetBounds();
            bounds.X.ShouldBeEqual(0);
            bounds.Y.ShouldBeEqual(0);
            bounds.Width.ShouldBeEqual(1000);
            bounds.Height.ShouldBeEqual(1000);
            this.TestPanel.UpdateLayout();
        }

       
        [TestMethod]
        public void AnimatedZoomAtTest()
        {
            var q = 0.0;
            var ani = this.Grids.ZoomAt(this.Cell, new TimeSpan(0,0,0,10)).Go();
            ani.Add(i=>q = i);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            this.TestPanel.UpdateLayout();
            q.ShouldBeEqual(0.0);

           
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            this.TestPanel.UpdateLayout();
            q.ShouldBeEqual(0.1);
            /*
            this.Grids.CanvasShift.X.ShouldBeEqual(1000);
            this.Grids.CanvasShift.Y.ShouldBeEqual(1000);
            */
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 10));
            this.TestPanel.UpdateLayout();
            q.ShouldBeEqual(1);

            this.Cell.GetBounds().ShouldBeEqual(new Rect(0, 0, 1000, 1000));
        }

        [TestMethod]
        public void AnimatedZoomAtOtherPositionTest()
        {
            var q = 0.0;
            this.Grids.SetCanvasSize(new Size(10000,10000));
            this.Grids.SetPanelShift(new Point(1000,1000));
            this.TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(3000, 3000, 1000, 1000));
            var ani = this.Grids.ZoomAt(this.Cell, new TimeSpan(0, 0, 0, 10)).Go();
            ani.Add(i => q = i);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            this.TestPanel.UpdateLayout();
            q.ShouldBeEqual(0.0);


            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            this.TestPanel.UpdateLayout();
            q.ShouldBeEqual(0.1);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 10));
            this.TestPanel.UpdateLayout();
            q.ShouldBeEqual(1);

            this.Cell.GetBounds().ShouldBeEqual(new Rect(0,0,1000,1000));
        }   


    }
}
