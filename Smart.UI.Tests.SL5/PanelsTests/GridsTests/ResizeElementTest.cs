using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{


    [TestClass]
    public class ResizeElementTest : GridTestBase<SmartGrid>
    {


        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            //this.Element.SetRow(4).SetRowSpan(1).SetColumn(1).SetColumnSpan(1);
            Animator.TestInit();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            Animator.EachFrame = null;
        }

        [TestMethod]
        public void InsertAndRemoveRowColumnTest()
        {
            var cell = this.Cells[5][5];
            Bounds = cell.GetBounds();
            Bounds.X.ShouldBeEqual(500);
            Bounds.Y.ShouldBeEqual(500);
            Bounds.Width.ShouldBeEqual(100);
            Bounds.Height.ShouldBeEqual(100);
            cell.GetRow().ShouldBeEqual(5);
            cell.GetColumn().ShouldBeEqual(5);

            this.Grids.InsertRow(4, 50.0);
            this.Grids.InsertColumn(4, 50.0);
            this.Grids.Width += 50.0;
            this.Grids.Height += 50.0;
            this.TestPanel.UpdateLayout();
            
            //тестирем конкретную ячейку справа и снизу
            Bounds = cell.GetBounds();
            Bounds.X.ShouldBeEqual(550);
            Bounds.Y.ShouldBeEqual(550);
            cell.GetRow().ShouldBeEqual(6);
            cell.GetColumn().ShouldBeEqual(6);

            //тестируем по колонкам
            Bounds = this.Cells[7][2].GetBounds();
            this.Cells[7][2].GetColumn().ShouldBeEqual(8);
            this.Cells[7][2].GetRow().ShouldBeEqual(2);                        
            Bounds.X.ShouldBeEqual(750);
            Bounds.Y.ShouldBeEqual(200);
            
            
            //тестируем по рядкам
            Bounds = this.Cells[2][7].GetBounds();
            this.Cells[2][7].GetColumn().ShouldBeEqual(2);
            this.Cells[2][7].GetRow().ShouldBeEqual(8);
            Bounds.X.ShouldBeEqual(200);
            Bounds.Y.ShouldBeEqual(750);
            

            //проверяем что часть ячеек незатронута
            Bounds = this.Cells[2][2].GetBounds();
            Bounds.Y.ShouldBeEqual(200);
            Bounds.X.ShouldBeEqual(200);

            this.Grids.RemoveRow(4);
            this.Grids.RemoveColumn(4);
            this.Grids.Width -= 50.0;
            this.Grids.Height -= 50.0;
            this.TestPanel.UpdateLayout();


            //тестирем конкретную ячейку справа и снизу
            Bounds = cell.GetBounds();
            cell.GetRow().ShouldBeEqual(5);
            cell.GetColumn().ShouldBeEqual(5);
            Bounds.X.ShouldBeEqual(500);
            Bounds.Y.ShouldBeEqual(500);
            
            //тестируем по колонкам
            Bounds = this.Cells[7][2].GetBounds();
            this.Cells[7][2].GetColumn().ShouldBeEqual(7);
            this.Cells[7][2].GetRow().ShouldBeEqual(2);            
            Bounds.X.ShouldBeEqual(700);
            Bounds.Y.ShouldBeEqual(200);


            //тестируем по рядкам
            Bounds = this.Cells[2][7].GetBounds();
            this.Cells[2][7].GetColumn().ShouldBeEqual(2);            
            this.Cells[2][7].GetRow().ShouldBeEqual(7);
            Bounds.X.ShouldBeEqual(200);
            Bounds.Y.ShouldBeEqual(700);
            

            //проверяем что часть ячеек незатронута
            Bounds = this.Cells[2][2].GetBounds();
            Bounds.Y.ShouldBeEqual(200);
            Bounds.X.ShouldBeEqual(200);
            
        }
      
    }
}
