using System.Collections.Generic;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    /// <summary>
    /// Tests wether FlexGrid is updated timely
    /// </summary>
    [TestClass]
    public class UpdateTest: SilverlightTest//GridTestBase<FlexGrid>
    {
        public int Q;
        public FlexGrid Fl;
        public List<LineDefinition> Lines;

        [TestInitialize]
        public void SetUp()
        {
            Fl = new FlexGrid();
            TestPanel.Children.Add(Fl);
            this.Lines = new List<LineDefinition>();
            for (var i = 0; i < 10; i++)
            {
                this.Lines.Add(new LineDefinition());
            }
            Q = 0;
        }

        [TestCleanup]
        public void CleanUp()
        {
            Q = 0;
            TestPanel.Children.Remove(Fl);
            this.Lines = null;
            Fl = null;
        }

        [TestMethod]
        public void GridUpdateTest()
        {
            Fl.RowDefinitions.Update.ShouldNotBeNull();
            Fl.RowDefinitions.Update += () => this.Q++;
            Fl.RowDefinitions.BlockPanelUpdate.ShouldBeFalse();
            Fl.RowDefinitions.Add(Lines[0]);
            Q.ShouldBeEqual(1);
            Fl.RowDefinitions.Add(Lines[1]);
            Q.ShouldBeEqual(2);
            Fl.RowDefinitions.Remove(Lines[0]);
            Q.ShouldBeEqual(3);
            /*
            OtherLineTriplets[0].Length = new RelativeValue();;
            Q.ShouldBeEqual(3);
            OtherLineTriplets[1].Length = new RelativeValue();
            Q.ShouldBeEqual(4);            
             */
        }
    }
}
