using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    /// <summary>
    /// Тестируем функции связанные со снепингом к гриду
    /// </summary>
    [TestClass]
    public class SnapingTest:GridTestBase<SmartGrid>
    {
        /// <summary>
        /// определяем ближайшую линию и расстояние от неё до координаты
        /// </summary>
        [TestMethod]
        public void GetLineDistanceTest()
        {
            this.Bounds = Cell.GetBounds();                        
            var dist = this.Grids.ColumnDefinitions.GetLineDistance(10000000);
            dist.Found.ShouldBeFalse();
            dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(4);
            dist.Dist.ShouldBeEqual(0.0);
            dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X + this.Bounds.Width);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(5);
            dist.Dist.ShouldBeEqual(0.0);
            dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X + this.Bounds.Width-10);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(5);
            dist.Dist.ShouldBeEqual(10.0);
            dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X +  10);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(4);
            dist.Dist.ShouldBeEqual(-10.0);           
        }

        /// <summary>
        /// тоже самое, но уже по отрезку
        /// </summary>
        [TestMethod]
        public void GetLineDistanceWidthTest()
        {
            this.Bounds = Cell.GetBounds();                        
            var dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X+5,10);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(4);
            dist.Dist.ShouldBeEqual(-5.0);
            
            Cell.SetRelativeToGrid(true).SetLeft(10);
            TestPanel.UpdateLayout();
            Bounds = Cell.GetBounds();

            dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(4);
            dist.Dist.ShouldBeEqual(-10.0);
            

            dist = this.Grids.ColumnDefinitions.GetLineDistance(this.Bounds.X,this.Bounds.Width-1);
            dist.Found.ShouldBeTrue();
            dist.Num.ShouldBeEqual(5);
            dist.Dist.ShouldBeEqual(1.0);
        }

        [TestMethod]
        public void GetElementDistanceTest()
        {            
            this.Bounds = Cell.GetBounds();                       
            var dist = this.Grids.GetColumnRowDistance(Cell);
            dist.Item1.Num.ShouldBeEqual(4);
            dist.Item1.Dist.ShouldBeEqual(0.0);
            dist.Item2.Num.ShouldBeEqual(4);
            dist.Item2.Dist.ShouldBeEqual(0.0);


            Cell.SetRelativeToGrid(true).SetLeft(10).SetTop(5).SetRight(20).SetBottom(10);
            TestPanel.UpdateLayout();
            dist = this.Grids.GetColumnRowDistance(Cell);
            
            dist.Item1.Num.ShouldBeEqual(4);
            dist.Item1.Dist.ShouldBeEqual(-10.0);
            dist.Item2.Num.ShouldBeEqual(4);
            dist.Item2.Dist.ShouldBeEqual(-5.0);
        }

    }
}
