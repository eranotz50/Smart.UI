using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Graphs;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Layout;
using Smart.UI.Classes.Utils;

namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class MultipleWallsTest : WallTestBase
    {

        [TestInitialize]
        public override void SetUp()
        {
            this.Panel = new Wall() { Width = 1000, Height = 1000 , LinesLength =  new RelativeLength(100)};
            this.TestPanel.Children.Add(Panel);
            this.Items = new LinesCollection<FrameworkElement>();          
            ItemsInALine = 4;
            this.Count = 20;
            var c = 0;
            var r = 0;            
            //this.Items2D = new WallCollection2D<FrameworkElement>();
            for (var i = 0; i < Count; i++)
            {
                var item = MakeWallItem();
                this.Items.Add(item);
               // this.Items2D[r, c] = item;
                if (r == ItemsInALine - 1)
                {
                    r = 0;
                    c++;
                }
                else
                {
                    r++;
                }
            }
            this.Items.Count.ShouldBeEqual(20);
            //this.Items2D.Count.ShouldBeEqual(this.ItemsInALine);
            this.UpdateLayout();
            //this.Items2D.Last().Count.ShouldBeEqual()
        }

        protected Wall MakeWallItem(ContentSize size = ContentSize.Medium)
        {
            var item = new Wall
                {
                    Background = new SolidColorBrush(ColorsDesign.GetRandomColor()),
                    Items = new SmartCollection<FrameworkElement>(),
                    Orientation = Orientation.Vertical,
                    CellsInLine = 2,
                    RowDefinitions = {PanelUpdateMode = PanelUpdateMode.Canvas}
                };
            for (var i = 0; i < 10; i++)
            {
                item.Items.Add(MakeItem(ContentSize.Medium));
            }

            return item;
        }

      

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();            
        }

        [TestMethod]
        public void WallOfWallsTest()
        {
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Items = Items;
            this.UpdateLayout();



        }
    }
}
