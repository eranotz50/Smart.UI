/*
using System;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.UI.Panels.InfoPanels.ContentWall;

namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class PositionTest : WallTest
    {
        [TestMethod]
        public void ElementsPositionTest()
        {
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;

            this.Panel.Width.ShouldBeEqual(1000.0);
            this.Panel.Height.ShouldBeEqual(1000.0);
            this.Panel.CellsInLine = 4;
            this.Panel.Width = 1100;
            this.Panel.Height = 1100;
            this.Panel.BetweenLines.AbsoluteValue.ShouldBeEqual(10);
            this.Panel.BetweenOtherLines.AbsoluteValue.ShouldBeEqual(10);
            this.Panel.LinesLength.AbsoluteValue.ShouldBeEqual(200);
            this.Panel.OtherLinesLength.AbsoluteValue.ShouldBeEqual(200);

            this.Panel.Items = Items;
            this.TestPanel.UpdateLayout();


            CheckItemPositions();

            var f = this.Panel.ItemLines[3, 3];
            const int margins = 10 * 2 * 3 + 10;
            const int contentBefore = 3 * 200;

            f.GetBounds().ShouldBeEqual(new Rect(margins + contentBefore, margins + contentBefore, 200, 200));
            this.Panel.ColumnDefinitions.Count.ShouldBeEqual(this.Panel.ItemLines.Count * 3);
            this.Panel.RowDefinitions.Count.ShouldBeEqual(this.Panel.ItemLines.OtherLineTriplets.Count * 3);
        }

       

        [TestMethod]
        public void PositioningTest()
        {
            this.Panel.LinesLength = new LineDefinition(1, 0.0);
            this.Panel.OtherLinesLength = new LineDefinition(1, 0.0);
            //NAPILNIK
            //this.Panel.BetweenLines = new RelativeLength(1,0.0);
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;

            this.Panel.Width.ShouldBeEqual(1000.0);
            this.Panel.Height.ShouldBeEqual(1000.0);
            this.Panel.CellsInLine = 4;
            this.Panel.Width = 1100;
            this.Panel.Height = 1100;
            this.Panel.BetweenLines.AbsoluteValue.ShouldBeEqual(10);
            this.Panel.BetweenOtherLines.AbsoluteValue.ShouldBeEqual(10);
            this.Panel.LinesLength.IsStar.ShouldBeTrue();

            this.Panel.OtherLinesLength.IsStar.ShouldBeTrue();
            this.Panel.LinesLength.Stars.ShouldBeEqual(1);
            this.Panel.OtherLinesLength.Stars.ShouldBeEqual(1);
            this.Panel.CellsInLine.ShouldBeEqual(4);
            this.Panel.Items = Items;
            this.TestPanel.UpdateLayout();

            this.Panel.RowDefinitions[0].AbsoluteValue.ShouldBeEqual(10);
            this.Panel.RowDefinitions[0].IsAbsolute.ShouldBeTrue();
            this.Panel.RowDefinitions[1].IsStar.ShouldBeTrue();
            //    this.Panel.RowDefinitions[1].MinLength.ShouldBeEqual(100);
            this.Panel.RowDefinitions[1].Stars.ShouldBeEqual(1);
            //this.Panel.RowDefinitions[1].StarLength.ShouldBeEqual(200);
            this.Panel.ItemLines[0][0].GetRow().ShouldBeEqual(1);
            this.Panel.ItemLines[0][0].GetColumn().ShouldBeEqual(1);
            this.Panel.ItemLines[0][1].GetRow().ShouldBeEqual(4);
            this.Panel.ItemLines[0][1].GetColumn().ShouldBeEqual(1);
            this.TestPanel.UpdateLayout();

            this.Panel.ColumnDefinitions.IndexOf(this.Panel.ItemLines[0].Position.Before).ShouldBeEqual(0);
            this.Panel.ColumnDefinitions.IndexOf(this.Panel.ItemLines[0].Position.Content).ShouldBeEqual(1);
            this.Panel.ColumnDefinitions.IndexOf(this.Panel.ItemLines[0].Position.After).ShouldBeEqual(2);

            this.Panel.ColumnDefinitions.IndexOf(this.Panel.ItemLines[1].Position.Before).ShouldBeEqual(3);
            this.Panel.ColumnDefinitions.IndexOf(this.Panel.ItemLines[1].Position.Content).ShouldBeEqual(4);
            this.Panel.ColumnDefinitions.IndexOf(this.Panel.ItemLines[1].Position.After).ShouldBeEqual(5);
            this.Panel.ItemLines[1][1].GetColumn().ShouldBeEqual(4);
            this.Panel.ItemLines[1][1].GetRow().ShouldBeEqual(4);


            var rect = this.Panel.ItemLines[0][1].GetBounds();
            rect.X.ShouldBeEqual(this.Panel.ColumnDefinitions[0].AbsoluteValue).ShouldBeEqual(10);
            //   rect.Y.ShouldBeEqual(this.Panel.RowDefinitions[0].AbsoluteValue + this.Panel.RowDefinitions[1].AbsoluteValue + this.Panel.RowDefinitions[2].AbsoluteValue + this.Panel.RowDefinitions[3].AbsoluteValue).ShouldBeEqual(230);
            //    rect.Height.ShouldBeEqual(100);
            rect.Width.ShouldBeEqual(200);

            rect = this.Panel.ItemLines[1][1].GetBounds();
            rect.X.ShouldBeEqual(this.Panel.ColumnDefinitions[0].AbsoluteValue + this.Panel.ColumnDefinitions[1].AbsoluteValue + this.Panel.ColumnDefinitions[2].AbsoluteValue + this.Panel.ColumnDefinitions[3].AbsoluteValue).ShouldBeEqual(230);
            //  rect.Y.ShouldBeEqual(this.Panel.RowDefinitions[0].AbsoluteValue + this.Panel.RowDefinitions[1].AbsoluteValue + this.Panel.RowDefinitions[2].AbsoluteValue + this.Panel.RowDefinitions[3].AbsoluteValue).ShouldBeEqual(230);
            //  rect.Height.ShouldBeEqual(100);
            rect.Width.ShouldBeEqual(200);
        }


    }
}
*/