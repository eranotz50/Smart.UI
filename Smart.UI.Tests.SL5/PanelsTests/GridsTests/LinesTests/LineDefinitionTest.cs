using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;


namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    [TestClass]
    public class LineDefinitionTest : SilverlightTest
    {
        public LineDefinition LineDef;

        [TestInitialize]
        public void SetUp()
        {
            this.LineDef = new LineDefinition();
        }

        [TestCleanup]
        public void CleanUp()
        {
            this.LineDef = null;
        }

        public void UpdateTest()
        {
        }

    }
}
