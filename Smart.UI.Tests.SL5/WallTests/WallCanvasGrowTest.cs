using System.Windows.Markup;
using DesignData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;

namespace Smart.UI.Tests.WallTests
{
    [TestClass]
    public class WallCanvasGrowTest : BasicWallTestBase
    {
        [TestInitialize]
        public override void SetUp()
        {
            this.Panel = XamlReader.Load(@"
        <Wall xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Items='{Binding Items}'  Name='WallPanel' CellsInLine='4' Orientation='Horizontal' Background='Black' LinesLength='200' OtherLinesLength='200' CanvasUpdateMode='KeepCanvas' CanvasWidth='2000' CanvasHeight='1000' Width='1000' Height='1000'/>          
") as Wall;
            this.Panel.ShouldNotBeNull();
            this.Panel.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;
            this.Panel.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;
            this.TestPanel.Children.Add(this.Panel);
        }

        [TestMethod]
        public void CanvasGrowTest()
        {
            this.Panel.DataContext = new WallData();
            this.TestPanel.UpdateLayout();
            this.Panel.Space.Canvas.Width.ShouldBeEqual(this.Panel.ColumnDefinitions.Length);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(this.Panel.RowDefinitions.Length);
        }
        /*
        [TestMethod]
        public void CanvasGrowTest()
        {
            this.Panel.DataContext = new WallSimpleData();
            this.TestPanel.UpdateLayout();

            this.Panel.Items.Count.ShouldBeEqual(10);

            var b = this.Panel.GetBounds();
            b.Width.ShouldBeEqual(1000);
            b.Height.ShouldBeEqual(1000);
            Panel.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 2000, 1000));

            Panel.OtherLinesLength.Value.ShouldBeEqual(200);
            Panel.LinesLength.Value.ShouldBeEqual(200);

            this.CheckItemPositions();
            
        }
         */
    }
}
