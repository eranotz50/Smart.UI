using System;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class ResizeCellTest:GridTestBase<SmartGrid>
    {

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            Animator.TestInit();          
            
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            Animator.EachFrame = null;            
        }

        [TestMethod]
        public void TestLayout()
        {
            /*
            Bounds = Grids.GetBounds();
            Bounds.X.ShouldBeEqual(0.0);
            Bounds.Y.ShouldBeEqual(0.0);
             */
            Bounds = Grids.GetBounds();
            Bounds.Width.ShouldBeEqual(1000.0);
            Bounds.Height.ShouldBeEqual(1000.0);
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
        }

        
        [TestMethod]
        public void ResizeCell()
        {   
            //проверяем базовые параметры
            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
            
            
            var row = Cell.GetRow();
            var col = Cell.GetColumn();
            var nextRow = Grids.RowDefinitions[row + 1];
            var nextCol = Grids.ColumnDefinitions[col + 1];
            
            //проводим увеличение
            Grids.GrowColumn(col, 20, LineGrowthMode.WithRightNeighbour);
            Grids.GrowRow(row, 10, LineGrowthMode.WithRightNeighbour);            
            TestPanel.UpdateLayout();

            
            //смотрим не поменялась ли панелька по размеру
            Bounds = Grids.GetBounds();
            Bounds.Width.ShouldBeEqual(1000);
            Bounds.Height.ShouldBeEqual(1000);

            Grids.ColumnDefinitions[col].Stars.ShouldBeEqual(1.2);
            Grids.ColumnDefinitions[col].Value.ShouldBeEqual(120);            
            Grids.RowDefinitions[col].Stars.ShouldBeEqual(1.1);
            Grids.RowDefinitions[col].Value.ShouldBeNear(110);
            


            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(120);
            Bounds.Height.ShouldBeEqual(110);
            
            nextRow.Value.ShouldBeEqual(90);
            nextCol.Value.ShouldBeEqual(80);

            
            //проводим уменьшение
            Grids.GrowColumn(Cell.GetColumn(), -20, LineGrowthMode.WithRightNeighbour);
            Grids.GrowRow(Cell.GetRow(), -10, LineGrowthMode.WithRightNeighbour);
            
            TestPanel.UpdateLayout();


            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);

            nextRow.Value.ShouldBeEqual(100);
            nextCol.Value.ShouldBeEqual(100);


            Bounds = Grids.GetBounds();
            Bounds.Width.ShouldBeEqual(1000);
            Bounds.Height.ShouldBeEqual(1000);
        }

        /// <summary>
        /// Проверяем тоже самое, но с увеличением размера самого грида
        /// </summary>
        [TestMethod]
        public void ResizeCellWidthChange()
        {
            //проверяем базовые параметры
            Bounds = Grids.GetBounds();
            Bounds.Width.ShouldBeEqual(1000);
            Bounds.Height.ShouldBeEqual(1000);


            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);


            var row = Cell.GetRow();
            var col = Cell.GetColumn();
            var nextRow = Grids.RowDefinitions[row + 1];
            var nextCol = Grids.ColumnDefinitions[col + 1];

            //проводим увеличение
            Grids.GrowColumn(col, 200, LineGrowthMode.WithPanel);
            Grids.GrowRow(row, 100, LineGrowthMode.WithPanel);           
            TestPanel.UpdateLayout();

            Grids.Space.Canvas.Width.ShouldBeEqual(1200);
            Grids.Space.Canvas.Height.ShouldBeEqual(1100);
            Cell.GetColumnDefinition().Value.ShouldBeEqual(300);


         

            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(300);
            Bounds.Height.ShouldBeEqual(200);

            nextRow.Value.ShouldBeEqual(100);
            nextCol.Value.ShouldBeEqual(100);


            //проводим уменьшение
            Grids.GrowRow(Cell.GetRow(), -100, LineGrowthMode.WithPanel);
            Grids.GrowColumn(Cell.GetColumn(), -200, LineGrowthMode.WithPanel);
            TestPanel.UpdateLayout();


            Bounds = Cell.GetBounds();
            Bounds.X.ShouldBeEqual(400);
            Bounds.Y.ShouldBeEqual(400);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);

            nextRow.Value.ShouldBeEqual(100);
            nextCol.Value.ShouldBeEqual(100);



            Grids.Space.Canvas.Width.ShouldBeEqual(1000);
            Grids.Space.Canvas.Height.ShouldBeEqual(1000);
           
        }
        /// <summary>
        /// Тестируем анимационный ресайз колонок и рядков
        /// </summary>
        [TestMethod]
        public void TestResizeTo()
        {
            var col = Cell.GetColumnDefinition();
            var row = Cell.GetRowDefinition();
            Cell.ResizeMyColumnTo(300.0, LineGrowthMode.WithPanel, new TimeSpan(0, 0, 0, 4)).Go();
            Cell.ResizeMyRowTo(500.0, LineGrowthMode.WithPanel, new TimeSpan(0, 0, 0, 4)).Go();
            col.Value.ShouldBeEqual(100);
            row.Value.ShouldBeEqual(100);            

            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            col.Value.ShouldBeEqual(100);            
            row.Value.ShouldBeEqual(100);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));
            col.Value.ShouldBeEqual(150);            
            row.Value.ShouldBeEqual(200);            
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 3));
            col.Value.ShouldBeEqual(200);            
            row.Value.ShouldBeEqual(300);            
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 4));
            col.Value.ShouldBeEqual(250);            
            row.Value.ShouldBeEqual(400);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 5));
            col.Value.ShouldBeEqual(300);            
            row.Value.ShouldBeEqual(500);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 6));
            col.Value.ShouldBeEqual(300);
            row.Value.ShouldBeEqual(500);            

            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.Width.ShouldBeEqual(300);
            Bounds.Height.ShouldBeEqual(500);

            
            Grids.Space.Canvas.Width.ShouldBeEqual(1200);
            Grids.Space.Canvas.Height.ShouldBeEqual(1400);           
        }

        /// <summary>
        /// Тестируем анимационный ресайз колонок и рядков вместе
        /// </summary>
        [TestMethod]
        public void TestResizeMyCellTo()
        {
            var col = Cell.GetColumnDefinition();
            var row = Cell.GetRowDefinition();
            Cell.ResizeMyCellTo(new Size(300, 500), LineGrowthMode.WithPanel, new TimeSpan(0, 0, 0, 4)).Go();
            Cell.ResizeMyColumnTo(300.0, LineGrowthMode.WithPanel, new TimeSpan(0, 0, 0, 4)).Go();
            Cell.ResizeMyRowTo(500.0, LineGrowthMode.WithPanel, new TimeSpan(0, 0, 0, 4)).Go();
            col.Value.ShouldBeEqual(100);
            row.Value.ShouldBeEqual(100);

            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 1));
            col.Value.ShouldBeEqual(100);
            row.Value.ShouldBeEqual(100);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 2));
            col.Value.ShouldBeEqual(150);
            row.Value.ShouldBeEqual(200);
             Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 3));
            col.Value.ShouldBeEqual(200);
            row.Value.ShouldBeEqual(300);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 4));
            col.Value.ShouldBeEqual(250);
            row.Value.ShouldBeEqual(400);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 5));
            col.Value.ShouldBeEqual(300);
            row.Value.ShouldBeEqual(500);
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 6));
            col.Value.ShouldBeEqual(300);
            row.Value.ShouldBeEqual(500);
            

            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();
            Bounds.Width.ShouldBeEqual(300);
            Bounds.Height.ShouldBeEqual(500);

            Grids.Space.Canvas.Width.ShouldBeEqual(1200);
            Grids.Space.Canvas.Height.ShouldBeEqual(1400);   
        }
    }
}
