using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.TestExtensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Panels;
using Smart.UI.Widgets;
using System.Linq;

namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class WidgetAnimationsTest : GridTestBase<WidgetGrid>
    {
        public WidgetGrid FirstPanel;
        public WidgetGrid SecondPanel;

        public override void SetUp()
        {
            base.SetUp();
            this.Grids.MovementTime = default(TimeSpan);
            this.FirstPanel = new WidgetGrid {ColsNum = 10, RowsNum = 10};
            this.SecondPanel = new WidgetGrid {ColsNum = 10, RowsNum = 10};
        }

        public override void CleanUp()
        {
            base.CleanUp();
            this.FirstPanel = null;
            this.SecondPanel = null;

        }

        [TestMethod]
        public void MoveInCellsMassiveTest()
        {
            this.Cell = this.Cells[5, 5]; ///changed from Cells[4,4]
            var objects = Cells.Flatten().ToList();
            Cells.Flatten().ForEach(this.Grids.RemoveChild);
            this.Grids.AddChild(this.FirstPanel.SetPlace(new Rect(100,100,1000,1000)));
            this.Grids.AddChild(this.SecondPanel.SetPlace(new Rect(600, 600, 1000, 1000)));
            this.UpdateLayout();
            this.FirstPanel.GetBounds().ShouldBeEqual(new Rect(100, 100, 1000, 1000));
            this.SecondPanel.GetBounds().ShouldBeEqual(new Rect(600, 600, 1000, 1000));
            foreach (var cell in objects) this.FirstPanel.AddChild(cell);
            this.UpdateLayout();
            this.FirstPanel.InvalidateMeasure();
            FlexGrid.GetRow(this.Cells[5,5]).ShouldBeEqual(5);
            FlexGrid.GetColumn(Cells[5, 5]);
            var bounds = this.Cells[5, 5].GetBounds();
            bounds.ShouldBeEqual(new Rect(500, 500, 100, 100));
            this.Cells[5, 5].GetRelativeRect(this.Grids).ShouldBeEqual(new Rect(600, 600, 100, 100));
            this.Cells[5, 5].GetRelativeRect(this.SecondPanel).ShouldBeEqual(new Rect(0, 0, 100, 100));
            var objs = objects.Select(s => new
                                                  {
                                                      Element = s,
                                                      Region = FirstPanel.GetCellsRegion(s)
                                                  }).ToList();
            FlexGrid.GetRelativeToGrid(this.Cells[5, 5]).ShouldBeFalse();
            
            objs.ForEach(o=>
            {
                var rect= o.Element.GetRelativeRect(this.SecondPanel);
                FirstPanel.RemoveChild(o.Element);                 
                SecondPanel.PlaceInCells(o.Element, bounds, o.Region);
                o.Region.SetPosition(o.Element);                            
                SecondPanel.AddChild(o.Element);
                SecondPanel.PlaceInCells(o.Element,rect,o.Region);
            });

            UpdateLayout();
             
            this.Cells[5, 5].GetRelativeRect(this.Grids).ShouldBeEqual(new Rect(600, 600, 100, 100));
            this.Cells[5, 5].GetRelativeRect(this.SecondPanel).ShouldBeEqual(new Rect(0, 0, 100, 100));
            FlexGrid.GetRow(this.Cells[5, 5]).ShouldBeEqual(5);
            FlexGrid.GetColumn(this.Cells[5, 5]).ShouldBeEqual(5);
            this.Cells[5, 5].GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.SecondPanel.GetCellsRect(new CellsRegion(5, 5, 1, 1)).ShouldBeEqual(new Rect(500, 500, 100, 100));
            FlexGrid.GetRelativeToGrid(this.Cells[5,5]).ShouldBeTrue();
            this.Cells[5, 5].GetLeft().ShouldBeEqual(-500);
            this.Cells[5, 5].GetTop().ShouldBeEqual(-500);

            UpdateLayout();

            var targets = objs.Select(s => new Tuple<FrameworkElement, Rect>(s.Element,SecondPanel.GetCellsRect(s.Region)));
            var time = new TimeSpan(0, 0, 0, 3);
            var ani = this.SecondPanel.MoveInCellsMassive(targets, time, Easings.CubicEaseInOut).Go();
            UpdateLayout();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            UpdateLayout();
            this.Cell.GetBounds().ShouldBeEqual(new Rect(500, 500, 100, 100));
            this.Cell.GetRelativeRect(this.Grids).ShouldBeEqual(new Rect(1100, 1100, 100, 100));
            this.Cells[0, 0].GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.Cells[0, 0].GetRelativeRect(this.Grids).ShouldBeEqual(new Rect(600, 600, 100, 100));




            //bounds = 
//            bounds.ShouldBeEqual(new Rect())

        }




    }
}