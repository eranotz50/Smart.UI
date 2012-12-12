using System.Linq;
using System.Windows;
using Design.Data.ContentWall;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;
using Smart.Classes.Extensions;
using Smart.Classes.Collections;

namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class WallScrollerTest:WallTestBase
    {
        [TestMethod]
        public void ScollerSizesTest()
        {
            this.UpdateLayout();
            this.Panel.Children.ShouldContain(this.Panel.VerticalScroll);
            this.Panel.Children.ShouldContain(this.Panel.HorizontalScroll);            
            //this.Panel.VerticalScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);            
            //this.Panel.HorizontalScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);
            this.Panel.Width = 500;
            //this.Panel.Height = 500;
            this.Panel.Space.MeasureSize = new Size(500,1000);
            this.UpdateLayout();
            
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.Space.Panel.Width.ShouldBeEqual(500);
            this.Panel.HorizontalScroll.GetBounds().Width.ShouldBeEqual(500);

            //this.Panel.VerticalScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);            
            //this.Panel.HorizontalScroll.Visibility.ShouldBeEqual(Visibility.Visible);
        }

        [TestMethod]
        public void AddItemsTest()
        {
            this.Panel.LinesLength = new LineDefinition(1, 0.0);
            this.Panel.Width = 1100;
            UpdateLayout();            
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;
            this.Panel.Items = Items;
            this.Panel.CountCells().ShouldBeEqual(20);            
            this.Panel.CellsInLine = 4;
            this.Panel.BetweenLines.Value.ShouldBeEqual(10);

            
            this.Items.Count.ShouldBeEqual(20);
            this.Panel.ItemLines.FlatCount().ShouldBeEqual(20);            
            this.Panel.CountCells().ShouldBeEqual(20);
            this.Panel.ItemLines.Count.ShouldBeEqual(5);

            this.Panel.Width.ShouldBeEqual(1100);
            //this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
          

            this.Panel.ItemLines[0].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[1].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[2].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[3].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[4].Count.ShouldBeEqual(4);
            this.TestPanel.UpdateLayout();


            this.Panel.ColumnDefinitions.StarLength.ShouldBeEqual(200);
            this.Panel.RowDefinitions.Count.ShouldBeEqual(12);
            this.Panel.ColumnDefinitions.Count.ShouldBeEqual(15);         

            this.Panel.Items = Items;
            this.Panel.CellsInLine.ShouldBeEqual(4);
            this.Panel.ItemLines.Count.ShouldBeEqual(5);
            this.Panel.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;
            this.Panel.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;        
            this.UpdateLayout();

            for (var i = 0; i < 20; i++)
            {
                this.Panel.Items.Add(this.MakeItem(ContentSize.Small));
            }
            this.Panel.ColumnDefinitions.DeltaStars.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.DeltaAbs.ShouldBeEqual(100);
            this.UpdateLayout();

            this.Panel.RowDefinitions.Count.ShouldBeEqual(12);
            this.Panel.ColumnDefinitions.Count.ShouldBeEqual(30);

            this.Panel.Width.ShouldBeEqual(1100);
            this.Panel.Space.Panel.Width.ShouldBeEqual(1100);            
            this.Panel.Space.Canvas.Width.ShouldBeEqual(2200);
            this.Panel.ItemLines.Count.ShouldBeEqual(10);
            this.Panel.ItemLines.Last().Count.ShouldBeEqual(4);
            this.Panel.Items.Add(this.MakeItem(ContentSize.Large));
            this.Panel.ColumnDefinitions.StarLength.ShouldBeEqual(200);
            this.Panel.ColumnDefinitions.DeltaAbs.ShouldBeEqual(20);
            this.Panel.ColumnDefinitions.DeltaStars.ShouldBeEqual(200);
            this.UpdateLayout();


            this.Panel.ItemLines.Count.ShouldBeEqual(11);
            this.Panel.ItemLines.Last().Count.ShouldBeEqual(1);

            this.Panel.Space.Canvas.Width.ShouldBeEqual(2420);
            this.Panel.Items.Pop();
            this.UpdateLayout();

            this.Panel.Space.Canvas.Width.ShouldBeEqual(2200);
            this.Panel.Width.ShouldBeEqual(1100);
            for (var i = 0; i < 20; i++)
            {
                this.Panel.Items.Pop();
            }
            this.Panel.ColumnDefinitions.DeltaAbs.ShouldBeEqual(-100);
            this.Panel.ColumnDefinitions.DeltaStars.ShouldBeEqual(-1000);
            this.UpdateLayout();

            this.Panel.CountCells().ShouldBeEqual(20);

            this.Items.Count.ShouldBeEqual(20);
            this.Panel.ItemLines.FlatCount().ShouldBeEqual(20);
            this.Panel.CountCells().ShouldBeEqual(20);
            this.Panel.ItemLines.Count.ShouldBeEqual(5);

            this.Panel.Width.ShouldBeEqual(1100);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1100);

            this.Panel.ItemLines[0].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[1].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[2].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[3].Count.ShouldBeEqual(4);
            this.Panel.ItemLines[4].Count.ShouldBeEqual(4);
        }

        [TestMethod]
        public void ScrollbarPositioningTest()
        {
            this.Panel.LinesLength = new LineDefinition(1, 0.0);
            this.Panel.Width = 1200;
            this.Panel.Items = Items;
            this.UpdateLayout();

            this.Panel.CountCells().ShouldBeEqual(40);            
            this.Panel.ItemLines.Count.ShouldBeEqual(10);                        
            this.Panel.ColumnDefinitions.StarLength.ShouldBeEqual(100);

            this.Panel.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;
            this.Panel.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;        
          

            for (var i = 0; i < 40; i++)
            {
                this.Panel.Items.Add(this.MakeItem(ContentSize.Small));
            }           
            this.Panel.ColumnDefinitions.DeltaStars.ShouldBeEqual(1000);
            this.Panel.ColumnDefinitions.DeltaValues.ShouldBeEqual(200);
            this.UpdateLayout();

            this.Panel.Space.Canvas.Width.ShouldBeEqual(2400);
            this.Panel.Space.Panel.Width.ShouldBeEqual(1200);
            this.Panel.Space.Canvas.Height.ShouldBeEqual(1000);
            this.Panel.Space.Panel.Height.ShouldBeEqual(1000);

            this.Panel.Space.RelativeRect.X.ShouldBeEqual(0);
            this.Panel.Space.RelativeRect.Width.ShouldBeEqual(0.5);
            this.Panel.MainScroller.Position.Whole.ShouldBeEqual(1);
            this.Panel.MainScroller.Position.Start.ShouldBeEqual(0.0);
            this.Panel.MainScroller.Position.End.ShouldBeEqual(0.5);
            this.Panel.MainScroller.GetBounds().Width.ShouldBeEqual(1200);            
            var bounds = this.Panel.MainScroller.Slider.GetBounds();
            bounds.Width.ShouldBeEqual(600);
        }

  
        [TestMethod]        
        public void MoveSliderTest()
        {
            foreach (var item in Items.OfType<ContentWallPanelItem>()) item.ContentSize = ContentSize.Small;
            this.Panel.Width = 1100;            
            this.Panel.Items = Items;                                    
            this.UpdateLayout();

            this.Panel.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;
            this.Panel.RowDefinitions.PanelUpdateMode = PanelUpdateMode.Canvas;                        
            

            this.Panel.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1100, 1000));
            this.Panel.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 1100, 1000));
            
            this.Panel.MainScroller.ShouldBeEqual(this.Panel.HorizontalScroll);
            this.Panel.VerticalScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);
            this.Panel.HorizontalScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);


           
            for (var i = 0; i < 20; i++)
            {
                this.Panel.Items.Add(this.MakeItem(ContentSize.Small));
            }
            this.UpdateLayout();

            this.Panel.MainScroller.ShouldBeEqual(this.Panel.HorizontalScroll);
            this.Panel.VerticalScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);
            this.Panel.HorizontalScroll.Visibility.ShouldBeEqual(Visibility.Visible);


            this.Panel.RowDefinitions.Count.ShouldBeEqual(12);
            this.Panel.ColumnDefinitions.Count.ShouldBeEqual(30);

            this.Panel.Width.ShouldBeEqual(1100);
            this.Panel.Space.Panel.Width.ShouldBeEqual(1100);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(2200);                        
        }

       
    }
}
