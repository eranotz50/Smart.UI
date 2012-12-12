namespace Smart.UI.Tests.RelativeTests
{
    /*
    [TestClass]
    public class CustomUpdateLayoutTest: GridTestBase<TestSmartGrid>
    {
        public TestSmartGrid TestCell;
        public TestButton TestButton;
        public RelativeConverter Rel;

        [TestInitialize]
        public override void SetUp()
        {
 	        base.SetUp();
            this.Rel = new RelativeConverter();
            this.TestCell = Cells[0, 0];
            this.TestButton = new TestButton();
            //this.Grids.AddChild(TestButton);
        }

        [TestCleanup]
        public override void CleanUp()
        {
 	        base.CleanUp();
            Rel = null;
            this.TestCell = null;
        }

        [TestMethod]
        public void MeasureAndArrangeCountsTest()
        {
            Grids.LayoutCount.ShouldBeEqual(1);

            Cell.LayoutCount.ShouldBeEqual(1);
            Cell.MeasureCount.ShouldBeEqual(1);
            Cell.ArrangeCount.ShouldBeEqual(1);
            
            TestCell.LayoutCount.ShouldBeEqual(1);            
            TestCell.MeasureCount.ShouldBeEqual(1);
            TestCell.ArrangeCount.ShouldBeEqual(1);

            var list = new List<FrameworkElement>(this.Grids.Children);
            foreach (var element in list)
            {
                if (element == TestCell || element == Cell) continue;
                this.Grids.Children.Remove(element);
            }
            
            this.Grids.AddChild(TestButton);
            
            this.TestButton.MeasureCount.ShouldBeEqual(0);
            this.TestButton.ArrangeCount.ShouldBeEqual(0);

            TestPanel.UpdateLayout();

            this.TestButton.UpdateMeasure();

            Grids.LayoutCount.ShouldBeEqual(2);

            //Cell.LayoutCount.ShouldBeEqual(1);            
            Cell.LayoutCount.ShouldBeEqual(2);                        
            Cell.MeasureCount.ShouldBeEqual(1);
            Cell.ArrangeCount.ShouldBeEqual(1);

            TestCell.LayoutCount.ShouldBeEqual(2);                        
            TestCell.MeasureCount.ShouldBeEqual(1);
            TestCell.ArrangeCount.ShouldBeEqual(1);

            this.TestButton.MeasureCount.ShouldBeEqual(1);
            this.TestButton.ArrangeCount.ShouldBeEqual(1);
            
            //TestCell.UpdateMeasure();
            TestCell.UpdateArrange();
            TestPanel.UpdateLayout();

            Grids.LayoutCount.ShouldBeEqual(3);

            this.TestButton.MeasureCount.ShouldBeEqual(2);
            this.TestButton.ArrangeCount.ShouldBeEqual(2);

            Cell.LayoutCount.ShouldBeEqual(3);
            Cell.MeasureCount.ShouldBeEqual(1);
            Cell.ArrangeCount.ShouldBeEqual(1);

            TestCell.LayoutCount.ShouldBeEqual(3);            
            TestCell.MeasureCount.ShouldBeEqual(1);
            TestCell.ArrangeCount.ShouldBeEqual(2);
        }

        [Ignore]
        [TestMethod]
        public void LayoutAndRelativePositioningTest()
        {
            Cell.MeasureCount.ShouldBeEqual(1);
            Cell.ArrangeCount.ShouldBeEqual(1);
            TestCell.MeasureCount.ShouldBeEqual(1);
            TestCell.ArrangeCount.ShouldBeEqual(1);
            
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            TestCell.SetPlace(new Rect(0, 0, 100, 100));
            //emulate depprop setting before adding to stage
            this.Grids.RemoveChild(TestCell);
            TestPanel.UpdateLayout();
            TestCell.Parent.ShouldBeEqual(null);


            //set relative position property
            var str = "Right";
            var act = (Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object);
            act.ShouldBeEqual(Relative.PosRight);
            TestCell.SetValue(Relative.PositionProperty, act);
            RelativeConverter.ConvertFrom(TestCell.GetPosition()).ShouldBeEqual(act);
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeConverter.ConvertFrom(TestCell.GetPosition()));
            TestPanel.UpdateLayout();

            //set relative.To
            TestCell.SetValue(Relative.ToProperty, Cell.Name);
            TestCell.GetRelativeElement().ShouldBeEqual(null);


            //adding TestCell
            Grids.AddChild(TestCell);
            TestPanel.UpdateLayout();

            var relEl = TestCell.GetRelativeElement();
            relEl.ShouldBeEqual(Cell);
            RelativeConverter.ConvertFrom(TestCell.GetPosition()).ShouldBeEqual(act);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);

            //action with args
            var relPos = TestCell.GetRelativePosition();
            relPos.Action.ShouldBeEqual(RelativeConverter.ConvertFrom(TestCell.GetPosition()));
            relPos.Param1.ShouldBeEqual(TestCell);
            relPos.Action.ShouldBeEqual(act).ShouldNotBeNull();


            TestCell.Parent.ShouldBeEqual(Grids);
            relEl.GetOnArrange().ShouldNotBeNull().DoOnNext.ShouldNotBeNull();


            // Cell.UpdateMeasure();
            // Cell.UpdateArrange();
            //Cell.GetOnArrange().OnNext(new Args<FrameworkElement, Rect, Size>(Cell,Cell.GetSlot(),Grids.RenderSize ));

            TestPanel.UpdateLayout();

            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            TestPanel.UpdateLayout();

            TestCell.GetPlace().ShouldBeEqual(new Rect(500, 0, 100, 100));
            TestCell.GetSlot().ShouldBeEqual(new Rect(500, 0, 100, 100));
            TestCell.GetBounds().ShouldBeEqual(new Rect(500, 0, 100, 100));
            TestPanel.SetPlace(new Rect(0, 0, 100, 100));
            TestPanel.UpdateLayout();

            Cell.SetColumn(5);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 0, 100, 100));
            str = "bottom";
            TestCell.SetPosition((Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object));
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            TestPanel.UpdateLayout();
            TestCell.GetPlace().ShouldBeEqual(new Rect(600, 500, 100, 100));
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 500, 100, 100));
            TestCell.GetSlot().ShouldBeEqual(new Rect(600, 500, 100, 100));
        }
    }
     */
}
