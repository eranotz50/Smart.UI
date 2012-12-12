using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.SmartGridTests
{
    [TestClass]
    public class MovingToCellTest:GridTestBase<WidgetGrid>
    {
        [TestMethod]
        public void RectangleMoveToTest()
        {
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            Cell.MoveTo(new Rect(800, 800, 200, 200), new TimeSpan(0, 0, 0, 4)).Go();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(500);
            Bounds.Y.ShouldBeEqual(500);
            Bounds.Width.ShouldBeEqual(125);
            Bounds.Height.ShouldBeEqual(125);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 3));
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(600);
            Bounds.Y.ShouldBeEqual(600);
            Bounds.Width.ShouldBeEqual(150);
            Bounds.Height.ShouldBeEqual(150);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 4));
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(700);
            Bounds.Y.ShouldBeEqual(700);
            Bounds.Width.ShouldBeEqual(175);
            Bounds.Height.ShouldBeEqual(175);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 5));
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(800);
            Bounds.Y.ShouldBeEqual(800);
            Bounds.Width.ShouldBeEqual(200);
            Bounds.Height.ShouldBeEqual(200);
        }

        [TestMethod]
        public void MoveToCell()
        {
            Cell.SetColumnSpan(2).SetRowSpan(2);
            Grids.MoveUp(Cell, 2, new TimeSpan(0, 0, 0, 1)).Go();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 3));
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(200);
            Bounds.Width.ShouldBeEqual(200);
            Bounds.Height.ShouldBeEqual(200);
            //Grids.MoveDown(Cell, 2, new TimeSpan(0, 0, 0, 1)).Go();
            Grids.MoveRight(Cell, 2, new TimeSpan(0, 0, 0, 1)).Go();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 3));
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(600);
            Bounds.Width.ShouldBeEqual(200);
            Bounds.Height.ShouldBeEqual(200);
        }

        protected int Next { get; set; }
        protected int Complete { get; set; }
        protected int D { get; set; }

        [TestMethod]
        public void CellMoveWorksOnceTest()
        {
            while (Grids.Children.Count > 0)
            {
                Grids.Children.Pop();
            }
            Grids.AddChild(Cell);
            TestPanel.UpdateLayout();
            Animator.EachFrame.Add(i => TestPanel.UpdateLayout());
            var move = Grids.MoveToCell(Cell, new CellsRegion(2, 2), new TimeSpan(0, 0, 0, 2));
            move.DoOnNext += i=> Next++;
            move.DoOnCompleted +=()=>
                                     {
                                         Next = 0;
                                         Complete = 1;
                                     };
            move.Go();
            move.DetachOnCompleted.ShouldBeTrue();
            move.Detacher += ()=>D++;
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));
            Complete.ShouldBeEqual(0);
            Next.ShouldBeEqual(2);
            D.ShouldBeEqual(0);            
            
            Animator.EachFrame.OnNext(new TimeSpan(0,0,0,1));
            Complete.ShouldBeEqual(0);
            Next.ShouldBeEqual(3);
            D.ShouldBeEqual(0);            
            
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));
            Complete.ShouldBeEqual(1);
            Next.ShouldBeEqual(0);
            D.ShouldBeEqual(1);
            
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 3));
            Complete.ShouldBeEqual(1);
            Next.ShouldBeEqual(0);
            D.ShouldBeEqual(1);
            
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 4));
            Complete.ShouldBeEqual(1);
            Next.ShouldBeEqual(0);
            D.ShouldBeEqual(1);                        
            
            
        }
    }
}
