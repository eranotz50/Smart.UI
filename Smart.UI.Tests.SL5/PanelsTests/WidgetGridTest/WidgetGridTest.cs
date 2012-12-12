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
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class WidgetGridTest:GridTestBase<WidgetGrid>
    {

        public override void SetUp()
        {
            base.SetUp();
            this.Grids.MovementTime = default(TimeSpan);
        }
    
        [TestMethod]
        public void GetCanvasTest()
        {
            
            //проверяем базовые параметры
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);               
         
            var c = new CanvasExtractor().ExtractFrom(Cell.GetBounds(),new Size(Grids.ActualWidth,Grids.ActualHeight));
            var p = this.Grids.Populate<CanvasPlaceholder>(Cell, new Size(Grids.ActualWidth, Grids.ActualHeight));
            p.Left.ShouldBeEqual(c.Left.AbsoluteValue);
            p.Bottom.ShouldBeEqual(c.Bottom.AbsoluteValue);

            c.Left.AbsoluteValue.ShouldBeEqual(400.0);
            c.Right.AbsoluteValue.ShouldBeEqual(500.0);
            c.Top.AbsoluteValue.ShouldBeEqual(400.0);            
            c.Bottom.AbsoluteValue.ShouldBeEqual(500.0);
            Cell.SetPlace(new Rect(10, 10, 50, 50));
            
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(10);
            Bounds.Y.ShouldBeEqual(10);
            Bounds.Width.ShouldBeEqual(50);
            Bounds.Height.ShouldBeEqual(50);

            Cell.SetPlace(Rect.Empty);
            TestPanel.UpdateLayout();

            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
           
            Cell.SetPlace(new Rect(30, 30, 70, 70));
            this.UpdateLayout();
           // Grids.ReceiveChild(Cell, new Rect(30, 30, 70, 70), new Rect(30, 30, 70, 70));
            Grids.ParkToCanvas(Cell); 
            TestPanel.UpdateLayout();
            
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(30);
            Bounds.Y.ShouldBeEqual(30);
            Bounds.Width.ShouldBeEqual(70);
            Bounds.Height.ShouldBeEqual(70);
            FlexCanvas.GetLeft(Cell).AbsoluteValue.ShouldBeEqual(30);
            FlexCanvas.GetRight(Cell).AbsoluteValue.ShouldBeEqual(900);
            FlexCanvas.GetTop(Cell).AbsoluteValue.ShouldBeEqual(30);
            FlexCanvas.GetBottom(Cell).AbsoluteValue.ShouldBeEqual(900);              
           
        }

     

        [TestMethod]
        public void DragWidgetsTest()
        {
            Cell.SetRowSpan(2);
            Cell.SetColumnSpan(2);
            TestPanel.UpdateLayout();
            
            var bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);

            Grids.DragEnabled = true;
            Cell.SetDragCanvas("NearestParent");
            var fly = this.Grids.DragManager.StartFlight(this.Grids.DragManager.BuildDragObject(Cell), i => new Point(0, 0));
            TestPanel.UpdateLayout();
            
            fly.Target.ShouldBeEqual(Cell);
            Grids.GetCellsRegion(Cell).ShouldBeEqual(new CellsRegion(4, 4, 2, 2));
            FlexGrid.GetRelativeToGrid(Cell).ShouldBeFalse();

            Grids.GetCellsRect(Grids.GetCellsRegion(Cell)).ShouldBeEqual(new Rect(400, 400, 200, 200));

            Cell.GetPos().ShouldBeEqual(default(Pos3D));
            Cell.GetPlace().ShouldBeEqual(Rect.Empty);
            
            Cell.GetSlot().ShouldBeEqual(new Rect(400, 400, 200, 200));            
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 200, 200));
            

            fly.CurrentMouse = new Point(-70, -70);
            fly.Pos();
            
            TestPanel.UpdateLayout();

            FlexGrid.GetRelativeToGrid(Cell).ShouldBeTrue();
            Grids.GetCellsRegion(Cell).ShouldBeEqual(new CellsRegion(4, 4, 2, 2));
            
            bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(330);
            bounds.Y.ShouldBeEqual(330);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
            /*
            this.Grids.Dock.OnNext(new Tuple<DragPanel, FrameworkElement>(Grids, fly.Target));
            TestPanel.UpdateLayout();
            fly.Target.GetRow().ShouldBeEqual(3);
            fly.Target.GetColumn().ShouldBeEqual(2);
             */
        }

        [TestMethod]
        public void RowsColsNumTest()
        {
            Grids.Name = "RowsColsNumTest";
            var bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.RowsNum = 20;
            TestPanel.UpdateLayout();
            
            bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(200);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(50);
            this.Grids.ColumnDefinitions.Count.ShouldBeEqual(10);
            this.Grids.RowDefinitions.Count.ShouldBeEqual(20);
            this.Grids.ColsNum.ShouldBeEqual(10);
            this.Grids.RowsNum.ShouldBeEqual(20);
            this.Grids.ColsNum = 20;
            TestPanel.UpdateLayout();

            this.Grids.ColumnDefinitions.Count.ShouldBeEqual(20);
            this.Grids.RowDefinitions.Count.ShouldBeEqual(20);            
            
            bounds = Cell.GetBounds();            
            bounds.X.ShouldBeEqual(200);
            bounds.Y.ShouldBeEqual(200);
            bounds.Width.ShouldBeEqual(50);
            bounds.Height.ShouldBeEqual(50);
            this.Grids.ColsNum = 10;
            this.Grids.RowsNum = 10;
            TestPanel.UpdateLayout();
            
            bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(100);
            bounds.Height.ShouldBeEqual(100);
            
            this.Grids.ColsNum = 5;
            this.Grids.RowsNum = 10;
            TestPanel.UpdateLayout();
            bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(800);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(100);
            this.Grids.RowsNum = 5;
            
            TestPanel.UpdateLayout();
            bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(800);
            bounds.Y.ShouldBeEqual(800);
            bounds.Width.ShouldBeEqual(200);
            bounds.Height.ShouldBeEqual(200);
        }

        [TestMethod]
        public void WidgetDragAndDropTest()
        {
            Animator.EachFrame.Add(i => this.TestPanel.UpdateLayout());
            while (Grids.Children.Count > 0) Grids.Children.Pop();
            Cell.SetColumnSpan(5).SetRowSpan(5).SetColumn(5).SetRow(5);
            var cell2 = this.Cells[2][2];
            cell2.SetColumnSpan(5).SetRowSpan(5).SetColumn(0).SetRow(0);
            cell2.RowDefinitions = new LineDefinitions() { new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition() };
            cell2.ColumnDefinitions = new LineDefinitions() { new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition() };

            Grids.AddChild(Cell);
            Grids.AddChild(cell2);
            var child = Cells[0][0];
            Cell.RowDefinitions = new LineDefinitions() { new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition() };
            Cell.ColumnDefinitions = new LineDefinitions() { new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition(), new LineDefinition() };
            Cell.AddChild(child.SetColumn(1).SetRow(1));
            
            this.TestPanel.UpdateLayout();

            Cell.GetBounds().ShouldBeEqual(new Rect(500, 500, 500, 500));
            child.GetBounds().ShouldBeEqual(new Rect(100, 100, 100, 100));
            child.Parent.ShouldBeEqual(this.Cell);
         
            var fly = Cell.DragManager.StartFlight(Cell.DragManager.BuildDragObject(child), i => (i as FrameworkElement).GetBounds().TopLeft());            
            UpdateLayout();

            child.Parent.ShouldBeEqual(this.Grids);
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 500, 500, 500));           
            child.GetBounds().ShouldBeEqual(new Rect(600, 600, 100, 100));

            fly.CurrentMouse = new Point(-200, -200);
            Cell.OnFly(fly);
            TestPanel.UpdateLayout();

            child.Parent.ShouldBeEqual(this.Grids);
            child.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            var dock = Grids.FindDock(fly.Target);
            dock.Name.ShouldBeEqual(cell2.Name);
            (child.Parent as DragPanel).DockChild(fly);
            
            TestPanel.UpdateLayout();

            //Grids.ParkingDuration = new TimeSpan(0, 0, 0, 1);
            Grids.MovementTime = new TimeSpan(0, 0, 0, 1);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));
            TestPanel.UpdateLayout();
            (fly.Target.Parent as FrameworkElement).Name.ShouldBeEqual(cell2.Name);
            SimplePanel.GetPlace(fly.Target).ShouldBeEqual(Rect.Empty);
            fly.Target.GetRow().ShouldBeEqual(4);
            fly.Target.GetColumn().ShouldBeEqual(4);
        }

        [TestMethod]
        public void InvisibleChangeTest1()
        {
            var f = this.Cells[5, 5];
            f.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            var r = this.Grids.GetCellsRegion(f);
            r.ShouldBeEqual(new CellsRegion(5, 5, 1, 1));
            var newR = new CellsRegion(1, 1, 2, 2);
            this.Grids.GetCellsRegion(f).ShouldBeEqual(r);
            newR.SetPosition(f);
            this.Grids.PlaceInCells(f,this.Grids.GetCellsRect(r),newR);
            this.UpdateLayout();

            this.Grids.GetCellsRegion(f).ShouldBeEqual(newR);
            f.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            FlexGrid.GetRelativeToGrid(f).ShouldBeTrue();

            this.Grids.PlaceInCells(f,this.Grids.GetCellsRect(newR));
            this.UpdateLayout();

            f.GetBounds().ShouldBeEqual(new Rect(100, 100, 200, 200));
            FlexGrid.GetRelativeToGrid(f).ShouldBeFalse();
        }

        [TestMethod]
        public void InvisibleChangeTest2()
        {
            this.UpdateLayout();
            var f = this.Cells[5, 5];
            f.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            var r = this.Grids.GetCellsRegion(f);
            r.ShouldBeEqual(new CellsRegion(5, 5, 1, 1));
            var newR = new CellsRegion(1, 1, 2, 2);
            this.Grids.GetCellsRegion(f).ShouldBeEqual(r);
            
            this.Grids.InvisibleChange(f,newR);
            this.UpdateLayout();

            this.Grids.GetCellsRegion(f).ShouldBeEqual(newR);
            f.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            FlexGrid.GetRelativeToGrid(f).ShouldBeTrue();

            this.Grids.PlaceInCells(f, this.Grids.GetCellsRect(newR));
            this.UpdateLayout();

            f.GetBounds().ShouldBeEqual(new Rect(100, 100, 200, 200));
            FlexGrid.GetRelativeToGrid(f).ShouldBeFalse();
        }


        [TestMethod]
        public void InvisibleChangeAnimationTest()
        {            
            var f = this.Cells[5, 5];
            f.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            var r = this.Grids.GetCellsRegion(f);
            r.ShouldBeEqual(new CellsRegion(5, 5, 1, 1));
            var newR = new CellsRegion(1, 1, 2, 2);
            this.Grids.MoveInCellsTo(f, newR, new TimeSpan(0, 0, 0, 1)).Go();
            this.UpdateLayout();

            Animator.PlusOneSecond();
            this.UpdateLayout();

            Animator.PlusOneSecond();
            this.UpdateLayout();

            Animator.PlusOneSecond();
            this.UpdateLayout();      

            f.GetBounds().ShouldBeEqual(new Rect(100, 100, 200, 200));
            FlexGrid.GetRelativeToGrid(f).ShouldBeFalse();
        }




    }
}
