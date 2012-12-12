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

namespace Smart.UI.Tests.WallTests.AnimationTests
{
    [TestClass]
    public class CellsAnimationTest:GridTestBase<WidgetGrid>
    {

       
        [TestMethod]
        public void RelativeToCellChangeTest()
        {
            this.UpdateLayout();
            var region = this.Grids.GetCellsRect(1, 1, 2, 2).ShouldBeEqual(new Rect(100, 100, 200, 200));
            this.Grids.GetCellsRect(1, 1, 1, 1).ShouldBeEqual(new Rect(100, 100, 100, 100));
            this.Cell.SetRow(1).SetColumn(1).SetRowSpan(2).SetColumnSpan(2);
            this.UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(region).ShouldBeEqual(this.Grids.GetCellsRect(1, 1, 2, 2));
            this.Cell.SetRelativeToGrid(true);
            this.Cell.SetLeft(10).SetRight(10).SetBottom(10).SetTop(10);
            this.UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(new Rect(110, 110, 180, 180));
            this.Grids.Extractor.Grow(this.Cell, new Point(-20, -20));
            this.UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(new Rect(120, 120, 160, 160));            
        }

       [TestMethod]
       public void MovementTest()
        {
            this.Cell.SetCellsRegion(new CellsRegion(1, 1, 2, 2));
           this.UpdateLayout();

           var rect = this.Grids.GetCellsRect(1, 1, 2, 2).ShouldBeEqual(new Rect(100, 100, 200, 200));          
           this.UpdateLayout();

           this.Cell.GetBounds().ShouldBeEqual(this.Grids.GetCellsRect(1, 1, 2, 2)).ShouldBeEqual(rect);
           this.Grids.PlaceInCells(Cell,new Rect(150,150,100,100));
           this.UpdateLayout();

           FlexGrid.GetRelativeToGrid(this.Cell).ShouldBeTrue();           
           Cell.GetBounds().ShouldBeEqual(new Rect(150, 150, 100, 100));
           Cell.GetLeft().ShouldBeEqual(50);
           Cell.GetRight().ShouldBeEqual(50);
           Cell.GetBottom().ShouldBeEqual(50);
           Cell.GetTop().ShouldBeEqual(50);

           this.Grids.PlaceInCells(Cell, new Rect(100, 100, 200, 200));
           this.UpdateLayout();

           FlexGrid.GetRelativeToGrid(this.Cell).ShouldBeFalse();
           this.Cell.GetBounds().ShouldBeEqual(this.Grids.GetCellsRect(1, 1, 2, 2)).ShouldBeEqual(rect).ShouldBeEqual(
               new Rect(100, 100, 200, 200));
           
           this.Grids.PlaceInCells(Cell,new Rect(0,0,400,400));
           
           this.UpdateLayout();

           this.Cell.GetBounds().ShouldBeEqual(new Rect(0, 0, 400, 400));
           this.Grids.GetCellsRegion(Cell).ShouldBeEqual(new CellsRegion(1, 1, 2, 2));

           Cell.GetLeft().ShouldBeEqual(-100);
           Cell.GetRight().ShouldBeEqual(-100);
           Cell.GetBottom().ShouldBeEqual(-100);
           Cell.GetTop().ShouldBeEqual(-100);
           FlexGrid.GetRelativeToGrid(this.Cell).ShouldBeTrue();

           this.Grids.PlaceInCells(Cell, new Rect(100, 100, 200, 200));
           this.UpdateLayout();

           FlexGrid.GetRelativeToGrid(this.Cell).ShouldBeFalse();
           this.Cell.GetBounds().ShouldBeEqual(this.Grids.GetCellsRect(1, 1, 2, 2)).ShouldBeEqual(rect).ShouldBeEqual(
               new Rect(100, 100, 200, 200));
       }

        [TestMethod]
        public void GrowthAnimationTest()
        {
            this.Cell.SetCellsRegion(new CellsRegion(1, 1, 2, 2));
            this.UpdateLayout();

            var rect = new Rect(100, 100, 200, 200);
            

            this.Cell.GetBounds().ShouldBeEqual(this.Grids.GetCellsRect(1, 1, 2, 2)).ShouldBeEqual( rect);
            this.Cell.ResizeInCellsRelatively(new Point(0.5, 0.5),new TimeSpan(0,0,0,2)).Go();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));          
            UpdateLayout();

            this.Cell.GetBounds().ShouldBeEqual(this.Grids.GetCellsRect(1, 1, 2, 2));
            Animator.PlusOneSecond();

            UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(rect.ResizeRectRelatively(new Point(0.75,0.75)));

            Animator.PlusOneSecond();

            UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(rect.ResizeRectRelatively(new Point(0.5, 0.5)));
            Animator.PlusOneSecond();

            UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(rect.ResizeRectRelatively(new Point(0.5, 0.5)));

            this.Cell.MoveInCells(new Rect(100,100,200,200), new TimeSpan(0, 0, 0, 2)).Go();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();

            UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(rect.ResizeRectRelatively(new Point(0.75, 0.75)));
            FlexGrid.GetRelativeToGrid(Cell).ShouldBeTrue();
            Animator.PlusOneSecond();

            UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(rect);

            FlexGrid.GetRelativeToGrid(Cell).ShouldBeFalse();



        }

    }
}
