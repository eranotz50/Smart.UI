using System;
using System.Windows;
using System.Windows.Markup;
using DesignData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class WallXamlTest : BasicWallTestBase
    {
        [TestInitialize]
        public override void SetUp()
        {
            Animator.TestInit();
            Panel = XamlReader.Load(@"
        <Wall xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Items='{Binding Items}'  Name='WallPanel' CellsInLine='4' Orientation='Horizontal' Background='Black' LinesLength='200' OtherLinesLength='200' CanvasUpdateMode='KeepCanvas' CanvasWidth='2000' CanvasHeight='1000' Width='1000' Height='1000'/>          
") as Wall;
            Panel.MovementTime = default(TimeSpan);
            Panel.ShouldNotBeNull();
            Panel.RowDefinitions.PanelUpdateMode = PanelUpdateMode.None;
            Panel.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.None;
            TestPanel.Children.Add(Panel);
        }


        [TestMethod]
        public void WallSimpleDataTest()
        {
            Panel.DataContext = new WallSimpleData();
            TestPanel.UpdateLayout();

            Panel.Items.Count.ShouldBeEqual(10);

            Rect b = Panel.GetBounds();
            b.Width.ShouldBeEqual(1000);
            b.Height.ShouldBeEqual(1000);
            Panel.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 2000, 1000));

            Panel.OtherLinesLength.Value.ShouldBeEqual(200);
            Panel.LinesLength.Value.ShouldBeEqual(200);

            CheckItemPositions();
        }


        [TestMethod]
        public void CellsInLineChangeTest()
        {
            var data = new WallSampleData();
            Panel.DataContext = data;
            Panel.Items.Count.ShouldBeEqual(10);

            UpdateLayout();

            Panel.Items.Count.ShouldBeEqual(10);
            Panel.ItemLines.Count.ShouldBeEqual(3);
            CheckItemPositions();

            UpdateLayout();

            Panel.CellsInLine = 2;
            TestPanel.UpdateLayout();

            Panel.ItemLines.Count.ShouldBeEqual(5);
            Panel.ItemLines.OtherLineTriplets.Count.ShouldBeEqual(2);
            CheckItemPositions();


            Panel.CellsInLine = 5;
            UpdateLayout();

            /*
            this.Panel.ItemLines.Count.ShouldBeEqual(2);
            this.Panel.ItemLines.OtherLineTriplets.Count.ShouldBeEqual(5);
            this.CheckItemPositions();

            this.UpdateLayout();

            this.Panel.CellsInLine = 4;
            this.Panel.ItemLines.Count.ShouldBeEqual(3);
            this.Panel.ItemLines.OtherLineTriplets.Count.ShouldBeEqual(4);
            this.CheckItemPositions();

            this.UpdateLayout();


            for (var i = 0; i < 10; i++) data.Items.Add(data.Make(i));

            this.UpdateLayout();

            this.Panel.Items.Count.ShouldBeEqual(20);
            this.Panel.ItemLines.Count.ShouldBeEqual(5);
            this.Panel.ItemLines.OtherLineTriplets.Count.ShouldBeEqual(4);
            this.CheckItemPositions();
             */
        }


        [TestMethod]
        public void WallSampleDataTest()
        {
            var data = new WallSampleData();
            Panel.DataContext = data;
            TestPanel.UpdateLayout();

            Panel.Items.Count.ShouldBeEqual(10);

            Rect b = Panel.GetBounds();
            b.Width.ShouldBeEqual(1000);
            b.Height.ShouldBeEqual(1000);
            Panel.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 2000, 1000));


            Panel.OtherLinesLength.Value.ShouldBeEqual(200);
            Panel.LinesLength.Value.ShouldBeEqual(200);

            CheckItemPositions();

            var subWall = data.Items[0] as Wall;
            Assert.IsNotNull(subWall);
            var place = new Rect(10, 10, 200, 200);
            subWall.GetBounds().ShouldBeEqual(place);
            subWall.Space.Panel.ShouldBeEqual(new Rect(0, 0, 200, 200));
            subWall.Items.Count.ShouldBeEqual(10);
            subWall.ItemLines.Count.ShouldBeEqual(5);
        }
    }
}