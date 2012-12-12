using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    /// <summary>
    /// Test for linedefinitions (Row and Column Definitions in FlexGrid and siblings)
    /// </summary>
    public class LineDefinitionsTest:SilverlightTest 
    {
        protected LineDefinitions Rows;
        protected LineDefinitions Cols;
        protected RelativeValueConverter R;

        [TestInitialize]
        public virtual void SetUp()
        {
            R = new RelativeValueConverter();          
            Rows = new LineDefinitions();
            Cols = new LineDefinitions()
                       {
                           new LineDefinition().FromRelativeValue((RelativeValue)R.ConvertFrom(null, null, "0.2*")),
                           new LineDefinition().FromRelativeValue ((RelativeValue)R.ConvertFrom(null, null, "0.2*")),
                           new LineDefinition().FromRelativeValue ((RelativeValue)R.ConvertFrom(null, null, "0.3*")),
                           new LineDefinition().FromRelativeValue ((RelativeValue)R.ConvertFrom(null, null, "0.3*"))
                       };
            Cols.Length = 1000;
        }

        [TestCleanup]
        public virtual void TestCleanup()
        {
            Rows = null;
            Cols = null;
            R = null;
        }

        [TestMethod]
        public void LineDefinitionsLengthTest()
        {
            Cols.Count.ShouldBeEqual(4);
            Assert.AreEqual(0.2, Cols[0].Stars);
            Assert.AreEqual(200, Cols[0].Value);
            Assert.AreEqual(0.3, Cols[2].Stars);
            Assert.AreEqual(300, Cols[2].Value);
            Assert.AreEqual(Cols.StarLength, 1000);
            Assert.AreEqual(400, Cols.GetCoord(2));
            Assert.AreEqual(600, Cols.GetRight(2));
            Assert.AreEqual(600, Cols.GetLength(2, 2));
        }

        [TestMethod]
        public void DeltasTest()
        {
            this.Cols.DeltaValues.ShouldBeEqual(0.0);
            Cols.Length.ShouldBeEqual(500);
            Cols.StarLength.ShouldBeEqual(500);

            var star = new LineDefinition(1, 0.0);
            Cols.Add(star);
            Cols.DeltaStars.ShouldBeEqual(500);
            
            var auto = new LineDefinition(200) {IsAuto = true};
            Cols.Add(auto);            
            Cols.DeltaAuto.ShouldBeEqual(200);

            var abs = new LineDefinition(300);
            Cols.Add(abs);
            Cols.DeltaAbs.ShouldBeEqual(300);
            Cols.DeltaValues.ShouldBeEqual(500);
            Cols.DeltaAll.ShouldBeEqual(1000);
            Cols.Length = 1000;
            Cols.DeltaAll.ShouldBeEqual(0.0);
            auto.AbsoluteValue = 100;
            Cols.DeltaAuto.ShouldBeEqual(-100);
            abs.AbsoluteValue = 100;
            Cols.DeltaAbs.ShouldBeEqual(-200);
            Cols.DeltaValues.ShouldBeEqual(-300);
            star.IsAuto = true;
            Cols.DeltaValues.ShouldBeEqual(-300);
            

        }

        [TestMethod]
        public void UpdateTest()
        {
            var colsInt = 0;
            Cols.Update += () => colsInt += 1;
            Cols[1].Length ="0.2*";
            Assert.AreEqual(1, colsInt);
            Cols.Add(new LineDefinition("0.2*"));
            Assert.AreEqual(2, colsInt);
            Cols.RemoveAt(Cols.Count - 1);
            Assert.AreEqual(3, colsInt);
            Cols[3].AbsoluteValue += 100;
            Assert.AreEqual(4, colsInt);
            Cols[3].MinLength += 0.1;
            Assert.AreEqual(5, colsInt);
            Cols[3].MaxLength += 0.1;
            Assert.AreEqual(6, colsInt);
        }

        [TestMethod]
        public void ConstrainsTest()
        {
            for (var i = 0; i < 5; i++)
            {
                var ls = new LineDefinition()
                            {
                                Stars = 1,
                                MinLength = 100
                            };
                this.Rows.Add(ls);
                var labs = new LineDefinition() {IsAuto = true, MinLength = 100};
                this.Rows.Add(labs);
            }
            this.Rows.Length = 1000;
            this.Rows.Autos.Count.ShouldBeEqual(5);
            this.Rows.Stars.Count.ShouldBeEqual(5);
            this.Rows.StarLength.ShouldBeEqual(100);

            this.Rows.Length = 2000;
            this.Rows.AllValuesLength.ShouldBeEqual(500);
            this.Rows.AllStars.ShouldBeEqual(1500);
            /*
            this.Rows.Length = 800;
            this.Rows.AllValuesLength.ShouldBeEqual(500);
            this.Rows.AllStars.ShouldBeEqual(1500);
            */
        }
    }
}
