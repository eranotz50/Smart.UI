using System;
using System.Windows;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Tests.CuriosityTests
{
    [TestClass]
    public class PanelCuriousityTest:GridTestBase<WidgetGrid>
    {
        /// <summary>
        /// Checks what changes do effect Cell
        /// </summary>
        /// 
        [Ignore]
        [TestMethod]
        public void UpdateLayoutTest()
        {
            var element1 = new Rectangle() {Width = 100, Height = 100};
            Grids.AddChild(element1);
            var element2 = new Rectangle() { Width = 100, Height = 100 };
            Grids.AddChild(element2);
            var element3 = new Rectangle() { Width = 100, Height = 100 };
            Grids.AddChild(element3);
            TestPanel.UpdateLayout(); //NAPILNIK

            var q = 0;
            Cell.SetOnArrange(i => q++);
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(1);
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(1);
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            Cell.SetColumn(3);
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(2);
            Cell.GetBounds().ShouldBeEqual(new Rect(300, 400, 100, 100));
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(2);
            element1.InvalidateArrange();
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(2);
            element1.InvalidateMeasure();
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(2);
            element2.Parent.ShouldBeEqual(Cell.Parent);
            element2.SetPlace(50, 50, 100, 100);
            TestPanel.UpdateLayout();
            element2.GetBounds().ShouldBeEqual(new Rect(50, 50, 100, 100));
            q.ShouldBeEqual(2);
            element3.Parent.ShouldBeEqual(Cell.Parent);
            element3.SetTop(100);
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(2);
            Cell.SetColumn(4);
            TestPanel.UpdateLayout();
            q.ShouldBeEqual(3);
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
        }

        public TimeSpan Measure(Action test, String text)
        {

            var before = DateTime.Now.Ticks;
            var q = 100;
            for (int i = 0; i < q; i++) test();
            var after = DateTime.Now.Ticks;

            var elapsedTime = new TimeSpan(after - before);
           // MessageBox.Show(string.Format("Task took {0} milliseconds",elapsedTime.TotalMilliseconds));
            return elapsedTime;
        }

        [TestMethod]
        public void BoundsVersusSlot()
        {
            var boundsTime = Measure(() =>
                                         {
                                             foreach (var grid in Grids.Children)
                                             {
                                                 grid.GetRelativeRect((UIElement)grid.Parent);
                                             }
                                         }, "BoundsMeasureTime");
            var slotTime = Measure(() =>
                                         {
                                             foreach (var grid in Grids.Children)
                                             {
                                                 grid.GetSlot();
                                             }
                                         }, "SlotMeasureTime");
            Assert.IsTrue(slotTime.TotalMilliseconds<boundsTime.TotalMilliseconds*10);
        }

    }
}
