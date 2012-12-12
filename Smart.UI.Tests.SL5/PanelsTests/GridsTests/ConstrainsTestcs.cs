using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class ConstrainsTestcs:GridTestBase<SmartGrid>
    {
        public override void SetUp()
        {
            base.SetUp();
        }

      
        [TestMethod]
        public void ConstrainsTest()
        {
            this.UpdateLayout();
            var col = this.Grids.ColumnDefinitions[4];
            var b = this.Grids.GetBounds();
            b.Width.ShouldBeEqual(1000);
            b.Height.ShouldBeEqual(1000);
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            col.StarLength.ShouldBeEqual(100);
            col.MinLength = 200;
            
            this.UpdateLayout();
            

        }

        [Ignore]
        [TestMethod]
        public void TestMechanis()
        {
            var col = this.Grids.ColumnDefinitions[4];
            var sum = this.Grids.ColumnDefinitions.Sum(i => i.Value);
            col.MinLength = 200;
            this.Grids.ColumnDefinitions.Length = 1000;
            this.Grids.ColumnDefinitions.Sum(i => i.Value).ShouldBeEqual(1000);

        }

        public override void CleanUp()
        {
            base.CleanUp();
        }
    }
}
