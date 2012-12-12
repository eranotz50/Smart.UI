using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class SmartGridDraggingTest:GridTestBase<SmartGrid>
    {
        [TestMethod]
        public void ComeBackAndLetDropNoAnimationTest()
        {
            var gridDragManager = this.Grids.DragManager as GridDragManager;
            if (gridDragManager != null)
                gridDragManager.ComeBackTime = default(TimeSpan);
            DragManager.SetChildrenDockMode(this.Grids,DockMode.DockOnFreeSpace);
            var d = Cells[0][0];
            Cell.AddChild(d.SetPlace(100, 100, 100, 100));
            Cell.SetRowSpan(2);
            Cell.SetColumnSpan(2);
            TestPanel.UpdateLayout();
            var bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
            bounds = d.GetBoundsTop();
            bounds.X.ShouldBeEqual(500);
            bounds.Y.ShouldBeEqual(500);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            var fly = Cell.DragManager.StartFlight(Cell.DragManager.BuildDragObject(d), i => SimplePanel.GetPlace(d).TopLeft());
            fly.DockMode = DockMode.DockOnFreeSpace;
            TestPanel.UpdateLayout();
            Animator.EachFrame.Add(i => TestPanel.UpdateLayout());
            Cell.Children.Contains(d).ShouldBeFalse();
            Grids.Children.Contains(d).ShouldBeTrue();
            bounds = d.GetBounds();
            bounds.X.ShouldBeEqual(500);
            bounds.Y.ShouldBeEqual(500);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            fly.CurrentMouse = new Point(-400, -400);
            fly.Pos();
            TestPanel.UpdateLayout();
            bounds = d.GetBounds();
            bounds.X.ShouldBeEqual(100);
            bounds.Y.ShouldBeEqual(100);
            fly.DockMode = DockMode.NoDock;
            Grids.DockChild(fly);
            TestPanel.UpdateLayout();

            bounds = d.GetBoundsTop();
            bounds.X.ShouldBeEqual(500);
            bounds.Y.ShouldBeEqual(500);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            (fly.Target.Parent as SmartGrid).ShouldHaveSameNames(this.Cell);
        }


        [TestMethod]
        public void ComeBackAndLetDropTest()
        {
            var gridDragManager = this.Grids.DragManager as GridDragManager;
            if (gridDragManager != null)
                gridDragManager.ComeBackTime = new TimeSpan(0, 0, 0, 1);
            DragManager.SetChildrenDockMode(this.Grids, DockMode.DockOnFreeSpace);            
            var d = Cells[0][0];
            Cell.AddChild(d.SetPlace(100, 100, 100, 100));
            Cell.SetRowSpan(2);
            Cell.SetColumnSpan(2);
            TestPanel.UpdateLayout();
            var bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
            bounds = d.GetBoundsTop();
            bounds.X.ShouldBeEqual(500);
            bounds.Y.ShouldBeEqual(500);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            var fly = Cell.DragManager.StartFlight(Cell.DragManager.BuildDragObject(d), i => SimplePanel.GetPlace(d).TopLeft());
            fly.DockMode = DockMode.DockOnFreeSpace;
            TestPanel.UpdateLayout();
            Animator.EachFrame.Add(i => TestPanel.UpdateLayout());
            Cell.Children.Contains(d).ShouldBeFalse();
            Grids.Children.Contains(d).ShouldBeTrue();
            bounds = d.GetBounds();
            bounds.X.ShouldBeEqual(500);
            bounds.Y.ShouldBeEqual(500);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            fly.CurrentMouse = new Point(-400, -400);
            fly.Pos();
            TestPanel.UpdateLayout();
            bounds = d.GetBounds();
            bounds.X.ShouldBeEqual(100);
            bounds.Y.ShouldBeEqual(100);
            fly.DockMode = DockMode.NoDock;
            Grids.DockChild(fly);
            fly.DockMode = DockMode.DockEverywhere;
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0, 500));
            TestPanel.UpdateLayout();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));

            TestPanel.UpdateLayout();

            bounds = d.GetBoundsTop();
            bounds.X.ShouldBeEqual(500);
            bounds.Y.ShouldBeEqual(500);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            (fly.Target.Parent as SmartGrid).ShouldHaveSameNames(this.Cell);
        }
    }
}
