using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Events;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Tests.EventsTests
{
    [Ignore]
    [TestClass]
    public class ZoomEventTest:GridTestBase<WidgetGrid>
    {
        public double Q;

        public override void SetUp()
        {
            this.Q = 0;
            base.SetUp();
        }

        protected void ZoomAtHandler(ZoomAtEvent args)
        {
            var ani = this.Grids.ZoomAt(this.Cell, args.Duration).Go();
            ani.Add(i => this.Q = i);
        }

        protected void ZoomBackHandler(TimeSpan duration)
        {
           // var bounds = this.Grids.GetSlot();            
            var ani = this.Grids.ZoomAt(this.Grids.Space.Canvas, duration).Go();
            ani.Add(i => this.Q = i);
        }

        [TestMethod]
        public void AnimatedZoomEventAtTest()
        {
            var zoomEvent = "ZoomAt";
            //this.Grids.SmartEventManager.AddEventHandler<TimeSpan>(zoomBack, ZoomBackHandler);
            this.Grids.EventManager.AddEventHandler<ZoomAtEvent>(zoomEvent, ZoomAtHandler);
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            this.UpdateLayout();

            this.Cell.RaiseEvent(new ZoomAtEvent(this.Cell, new TimeSpan(0, 0, 0, 10)));
            //var q = 0.0;
            //var ani = this.Grids.ZoomAt(this.Cell, new TimeSpan(0, 0, 0, 10)).Go();
            //ani.Add(i => q = i);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            this.TestPanel.UpdateLayout();
            Q.ShouldBeEqual(0.0);


            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            this.TestPanel.UpdateLayout();
            Q.ShouldBeEqual(0.1);
            /*
            this.Grids.CanvasShift.X.ShouldBeEqual(1000);
            this.Grids.CanvasShift.Y.ShouldBeEqual(1000);
            */
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 10));
            this.TestPanel.UpdateLayout();
            Q.ShouldBeEqual(1);

            this.Cell.GetBounds().ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            /*
            this.Cell.RaiseEvent(zoomBack,new TimeSpan(0,0,0,10));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 11));
            this.TestPanel.UpdateLayout();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 12));            
            this.TestPanel.UpdateLayout();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 22));
            

            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            //this.Grids.Space.
             */
        }

    }
}
