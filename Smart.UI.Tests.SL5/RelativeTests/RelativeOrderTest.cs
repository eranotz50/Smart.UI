
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Smart.UI.Graphs.TestControls;
using Smart.UI.Panels;
using Smart.UI.Relatives;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;

namespace Smart.UI.Tests.RelativeTests
{
    /*
    [TestClass]
    public class RelativeOrderTest : GridTestBase<TestSmartGrid>
    {
        [TestMethod]
        public void BeforeSwapAndAfterTest()
        {
            var c1 = this.Cells[1, 1];
            var c2 = this.Cells[2, 2];

            c1.Parent.ShouldNotBeNull();
            c2.Parent.ShouldNotBeNull();

            this.Grids.Children.ShouldHaveElementBefore(c1, c2);

            var i1 = this.Grids.Children.IndexOf(c1);
            var i2 = this.Grids.Children.IndexOf(c2);


            this.Grids.Children.ShouldHaveElementBefore(c1, c2);
            Grids.Children.Swap(c1, c2);
            this.Grids.Children.IndexOf(c1).ShouldBeEqual(i2);
            this.Grids.Children.IndexOf(c2).ShouldBeEqual(i1);
            this.Grids.Children.ShouldHaveElementBefore(c2, c1);
            Grids.Children.Swap(i1, i2);
            this.Grids.Children.IndexOf(c1).ShouldBeEqual(i1);
            this.Grids.Children.IndexOf(c2).ShouldBeEqual(i2);


            this.Grids.Children.ShouldHaveElementBefore(c1, c2);

            i1 = this.Grids.Children.IndexOf(c1);
            i2 = this.Grids.Children.IndexOf(c2);

            this.Grids.Children.PutBefore(c2, c1);
            this.Grids.Children.IndexOf(c2).ShouldBeEqual(i1);
            this.Grids.Children.IndexOf(c1).ShouldBeEqual(i1 + 1);
            this.Grids.Children[i2].ShouldNotBeSame(c2);
            this.Grids.Children.PutAfter(c2, c1);

            i1 = this.Grids.Children.IndexOf(c1);
            i2 = this.Grids.Children.IndexOf(c2);


            this.Grids.Children.PutBefore(i2, i1);
            this.Grids.Children.IndexOf(c2).ShouldBeEqual(i1);
            this.Grids.Children.IndexOf(c1).ShouldBeEqual(i1 + 1);
            this.Grids.Children[i2].ShouldNotBeSame(c2);

            c1.Parent.ShouldNotBeNull();
            c2.Parent.ShouldNotBeNull();
        }

        [TestMethod]
        public void OnArrangeTest()
        {
            var a = 0;
            Cell.RunOnFirstArrange(i => a++);
            a.ShouldBeEqual(0);
            Grids.InvalidateMeasure();
            TestPanel.UpdateLayout();
            a.ShouldBeEqual(1);

        }

        [TestMethod]
        public void AfterMoveTest()
        {
            var c1 = this.Cells[1, 1];
            var c2 = this.Cells[2, 2];
            var c3 = this.Cells[3, 3];
            var c4 = this.Cells[4, 4];

            this.Grids.Children.ShouldHaveElementBefore(c1, c2).ShouldHaveElementBefore(c2, c3).ShouldHaveElementBefore(
                c3, c4);

            var i1 = this.Grids.Children.IndexOf(c1);
            var i2 = this.Grids.Children.IndexOf(c2);
            var i3 = this.Grids.Children.IndexOf(c3);
            var i4 = this.Grids.Children.IndexOf(c4);

            TestPanel.UpdateLayout();

            c1.Parent.ShouldNotBeNull();
            c2.Parent.ShouldNotBeNull();
            c3.Parent.ShouldNotBeNull();
            c4.Parent.ShouldNotBeNull();


            c1.Parent.ShouldNotBeNull();
            c1.SetRelativeElement(c2);
            this.Grids.Children.ShouldHaveElementBefore(c2, c1);

            c2.Parent.ShouldNotBeNull();
            c2.SetRelativeElement(c3);
            this.Grids.Children.ShouldHaveElementBefore(c3, c2);
            this.Grids.Children.ShouldHaveElementBefore(c2, c1);

            c3.Parent.ShouldNotBeNull();
            c3.SetRelativeElement(c4);
            this.Grids.Children.ShouldHaveElementBefore(c4, c3);
            this.Grids.Children.ShouldHaveElementBefore(c3, c2);
            this.Grids.Children.ShouldHaveElementBefore(c2, c1);
            this.Grids.Children.ShouldHaveElementBefore(c2, c1);

            TestPanel.UpdateLayout();
            this.Grids.Children.ShouldHaveElementBefore(c2, c1);
            this.Grids.Children.ShouldHaveElementBefore(c3, c2);
            this.Grids.Children.ShouldHaveElementBefore(c4, c3);

        }

        [TestMethod]
        public void InvalidateMeasureTest()
        {
            this.Grids.MeasureCount.ShouldBeEqual(1);            
            this.Grids.InvalidateMeasure();
            //this.Grids.AfterArrange.DoOnNext+=i=>mcount++;
            this.Grids.UpdateLayout();
            this.Grids.MeasureCount.ShouldBeEqual(2);
            this.Grids.UpdateLayout();
            this.Grids.MeasureCount.ShouldBeEqual(2);
            this.Grids.UpdateLayout();
            this.Grids.MeasureCount.ShouldBeEqual(2);
            this.Grids.InvalidateMeasure();
            var b = true;
            this.Cell.SetOnArrange(i =>
                {
                    if (b) this.Cell.GetNearestDragParent().InvalidateMeasure();
                    b = false;
                });
            var c = 0;
            this.Cells[9, 9].SetOnArrange(i => { c++; });
            this.Grids.UpdateLayout();
            c.ShouldBeEqual(2);  
            this.Grids.MeasureCount.ShouldBeEqual(4);
            this.Grids.UpdateLayout();
            c.ShouldBeEqual(2);            
            this.Grids.MeasureCount.ShouldBeEqual(4);
            
            
            
        }
    }
    */
}
