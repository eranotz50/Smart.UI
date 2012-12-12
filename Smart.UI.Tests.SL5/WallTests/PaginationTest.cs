/*
using System.Linq;
using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Extensions;
using Smart.UI.Panels.InfoPanels.ContentWall;

namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class PaginationTest : WallTestBase
    {
        [TestMethod]
        public void CountCellsTest()
        {
            var p = (this.Panel.Paginator as Paginator);
            this.Panel.Items = this.Items;
            p.CellsFromContentSize(ContentSize.Small).ShouldBeEqual(1);
            p.CellsFromContentSize(ContentSize.Medium).ShouldBeEqual(2);
            p.CellsFromContentSize(ContentSize.Large).ShouldBeEqual(3);
            this.Panel.CountCells().ShouldBeEqual(40);           
            var c = 0;
            var r = 0;
            ItemsInALine = 4;          
        }


        [TestMethod]
        public void AntiOverflowTest()
        {
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Large;
            this.Panel.ItemLines.AvoidStackOverflow.ShouldBeTrue();
            this.Panel.CellsInLine = 1;
            this.Panel.Items =Items;

            this.UpdateLayout();
            this.Panel.ItemLines.Count.ShouldBeEqual(Items.Count);
        }


        [TestMethod]
        public void SmallPaginateItemsTest()
        {
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;
            this.Panel.Items = Items;
            this.Panel.CountCells().ShouldBeEqual(20);
            this.Panel.CellsInLine = 4;
            this.Items.Count.ShouldBeEqual(20);
            this.Panel.ItemLines.FlatCount().ShouldBeEqual(20);
            this.Panel.CountCells().ShouldBeEqual(20);
            this.Panel.ItemLines.Count.ShouldBeEqual(5);

            this.Panel.ItemLines[0].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[1].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[2].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[3].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[4].Count.ShouldBeEqual(4);
            this.TestPanel.UpdateLayout();
            this.Panel.RowDefinitions.Count.ShouldBeEqual(12);
            this.Panel.ColumnDefinitions.Count.ShouldBeEqual(15);
        }

        [TestMethod]
        public void Follow2DTest()
        {
            var items = new Collection2D<SmartCollection<FrameworkElement>, FrameworkElement>();
            items.Clear();
            items.Clear();
            items.Count.ShouldBeEqual(0);
            items.FillFollowFlat(this.Items);
            items.Count.ShouldBeEqual(1);
            items[0].Count.ShouldBeEqual(20);
            items.NextPageSelector = items.PaginationGenerator(4);
            items.ReArrange();
            items.Count.ShouldBeEqual(5);
            items.Clear();
            items.UnFollowFlat(this.Items);
            items.NextPageSelector = items.PaginationGenerator(5);
            items.FillFollowFlat(this.Items);
            items.FlatCount().ShouldBeEqual(20);
            items.Count.ShouldBeEqual(4);
            var f = this.Items[6];
            items[1, 1].ShouldBeEqual(f);
            this.Items.RemoveAt(6);
            items[1, 1].ShouldNotBeEqual(f);
            items.FlatCount().ShouldBeEqual(19);
            this.Items.Insert(1, f);
            items[0][1].ShouldBeEqual(f);
            items.FlatCount().ShouldBeEqual(20);
        }

    }
}
*/