/*
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.UI.Panels.InfoPanels.ContentWall;

namespace Smart.UI.Tests.PanelsTests
{
    
    [TestClass]
    public class WallTest:WallTestBase
    {
       

       
        [TestMethod]
        public void ItemsAddedTest()
        {
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;
            this.Panel.CellsInLine = 4;
            this.Panel.Items = this.Items;
            this.UpdateLayout();
            this.Panel.RowDefinitions.Count.ShouldBeEqual(12);
            this.Panel.ColumnDefinitions.Count.ShouldBeEqual(15);            
            this.Panel.ItemLines.Count.ShouldBeEqual(5);
        }

        [TestMethod]
        public void PaginatorTest()
        {
            var p = Panel.Paginator as Paginator;
            p.CellsFromContentSize(ContentSize.Small).ShouldBeEqual(1);
            p.CellsFromContentSize(ContentSize.Medium).ShouldBeEqual(2);
            p.CellsFromContentSize(ContentSize.Large).ShouldBeEqual(3);
        }


        [TestMethod]
        public void OrientationChangeTest()
        {           
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;
            Panel.Orientation.ShouldBeEqual(Orientation.Horizontal);

            this.Panel.Items = Items;
            var f = this.Panel.ItemLines[4, 3];
            this.Panel.Children.Contains(f).ShouldBeTrue();            
            this.TestPanel.UpdateLayout();        
            this.Panel.ItemLines.SelectMany(l => l).Select(item => item.GetColumn()).ToList();
            this.Panel.ItemLines.SelectMany(l => l).Select(item => item.GetRow()).ToList();

            this.Panel.CountCells().ShouldBeEqual(20);
            this.Panel.CellsInLine = 4;
            this.Items.Count.ShouldBeEqual(20);
            this.Panel.ItemLines.FlatCount().ShouldBeEqual(20);
            this.Panel.CountCells().ShouldBeEqual(20);
            this.Panel.ItemLines.Count.ShouldBeEqual(5);
            
           
            f.GetColumn().ShouldBeEqual(this.NumForLine(4));
            f.GetRow().ShouldBeEqual(this.NumForLine(3));
            Panel.ColumnDefinitions.Count.ShouldBeEqual(15);            
            Panel.RowDefinitions.Count.ShouldBeEqual(12);
            
            Panel.Orientation = Orientation.Vertical;
            this.TestPanel.UpdateLayout();
            this.Panel.ItemLines[4, 3].ShouldBeEqual(f);
            f = this.Panel.ItemLines[4, 3];
            f.GetColumn().ShouldBeEqual(this.NumForLine(3));
            f.GetRow().ShouldBeEqual(this.NumForLine(4));
            Panel.ColumnDefinitions.Count.ShouldBeEqual(12);
            Panel.RowDefinitions.Count.ShouldBeEqual(15);                        
        }

      

      
        [TestMethod]
        public void RowColumnsOrientationTest()
        {

            this.Panel.CellsInLine.ShouldBeEqual(4);            
            this.Panel.Items = Items;
            this.TestPanel.UpdateLayout();
            this.Panel.OtherLineDefs.Count.ShouldBeEqual(3*this.Panel.CellsInLine);            
            this.Panel.ItemLines.OtherLineTriplets.Count.ShouldBeEqual(this.Panel.CellsInLine);
            var n = 0;
            foreach (var t in this.Panel.ItemLines.OtherLineTriplets.SelectMany(l=> new List<LineDefinition>(){l.Before,l.Content,l.After}))
            {
                t.Num.ShouldBeEqual(n);
                n++;
            }
            // Panel.UpdateLineNums();
           
            for (var i = 0; i < this.Panel.ItemLines.Count; i++)
            {
                var line = this.Panel.ItemLines[i];
                for (var j = 0; j < line.Count; j++)
                {
                    var item = line[j];
                    this.Panel.UpdateLineNums();
                    var c = this.NumForLine(i);
                    var r = this.NumForLine(j);
                    var rs = this.ToSpan(this.Panel.Paginator.CellsIn(item));                    
                    var cs = this.ToSpan(1);
                    SetSpansTest(item,  cs, rs);
                }
            }
           
            this.Panel.Orientation = Orientation.Vertical;
            this.TestPanel.UpdateLayout();
            Panel.UpdateLineNums();
           

            for (int i = 0; i < this.Panel.ItemLines.Count; i++)
            {
                var line = this.Panel.ItemLines[i];
                for (var j = 0; j < line.Count; j++)
                {
                    var item = line[j];
                    var r = this.NumForLine(i);
                    var c = this.NumForLine(j);
                    var rs = this.ToSpan(1);
                    var cs = this.ToSpan(this.Panel.Paginator.CellsIn(item));
                    SetSpansTest(item, cs,rs);
                }
            }
            
        }


        /// <summary>
        /// SPAN TEST TEMP
        /// </summary>
        /// <param name="item"></param>
        /// <param name="c"></param>
        /// <param name="cs"></param>
        /// <param name="rs"></param>
        private  void SetSpansTest(FrameworkElement item,  int cs, int rs)
        {
           // item.GetColumn().ShouldBeEqual(c);
           // item.GetRow().ShouldBeEqual(r);
            item.GetColumnSpan().ShouldBeEqual(cs);
            item.GetRowSpan().ShouldBeEqual(rs);
        }

        [TestMethod]
        public void DragTest()
        {
            this.Panel.Items = Items;
            this.UpdateLayout();
            ;
            var rect = this.Panel.Items[2].GetBounds();
            var fly  =this.GetUp(this.Panel.Items[3]);
            fly.CurrentMouse = new Point(100,100);
            fly.Pos();

           // this.Panel.Items
        }
       
     
     

     

    }
     
}
*/
/*
      [TestMethod]
      public void SetLineNumsTest()
      {
      //    foreach (var item in Items.OfType<ContentWallPanelItem>()) item.contentSize = ContentSize.Small;            
          this.Panel.Items = Items;
          Panel.UpdateLineNums();
          var f = Panel.OtherLineTriplets[0][0];            
          Panel.SetLinesForItem(f,0, this.Panel.NumForLine(0));
          f.GetColumn().ShouldBeEqual(1);
          f.GetRow().ShouldBeEqual(1);
          f.GetColumnSpan().ShouldBeEqual(1);
          f.GetRowSpan().ShouldBeEqual(4);
          Panel.Orientation = Orientation.Vertical;
          f.ShouldBeEqual(Panel.OtherLineTriplets[0][0]);
          Panel.SetLinesForItem(f, 0, this.Panel.NumForLine(0));
          f.GetColumn().ShouldBeEqual(1);
          f.GetRow().ShouldBeEqual(1);
          f.GetColumnSpan().ShouldBeEqual(4);
          f.GetRowSpan().ShouldBeEqual(1);                         
      }
      */