using System.Collections.Generic;
using System.Reactive.Linq;
using System.Windows;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Widgets.PanelAdorners;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Tests.AdornersTests
{
    [TestClass]
    public class WidgetAdornerTest:GridTestBase<WidgetGrid>
    {
        public SmartCollection<SmartCollection<ObjectFly>> Flies;
        public int GroupQ { get; set; }
        public Dictionary<ObjectFly, int> GrCount;
        public WidgetAdorner Adorner;

        public override void SetUp()
        {
            base.SetUp();
            Flies = new SmartCollection<SmartCollection<ObjectFly>>();
            GrCount = new Dictionary<ObjectFly, int>();
            this.Adorner = new WidgetAdorner();
         
        }

        protected void StartFlights()
        {
            for (var i = 0; i < Grids.ColumnDefinitions.Count; i++)
            {
                Flies[i] = new SmartCollection<ObjectFly>();
                for (var j = 0; j < Grids.RowDefinitions.Count; j++)
                {
                    Flies[i][j] = GetUp(this.Cells[i][j]);
                }
            }
        }

        public override void CleanUp()
        {
            base.CleanUp();
            GroupQ = 0;
            GrCount = null;
            this.Adorner = null;
        }

        [TestMethod]
        public void GroupByTest()
        {
            this.StartFlights();
            var group = from f in this.Grids.UnderFlight group f by f;
            group.Chain(i =>
                            {
                                this.GroupQ++;
                                if (GrCount.ContainsKey(i.Key)) return;
                                GrCount.Add(i.Key,0);
                                i.Chain(f => GrCount[i.Key] += 1);
                            });
            this.Grids.UnderFlight.OnNext(this.Flies[0][1]);
            this.GroupQ.ShouldBeEqual(1);
            GrCount[this.Flies[0][1]].ShouldBeEqual(1);            
            this.Grids.UnderFlight.OnNext(this.Flies[0][1]);
            this.GroupQ.ShouldBeEqual(1);
            GrCount[this.Flies[0][1]].ShouldBeEqual(2);
            this.Grids.UnderFlight.OnNext(this.Flies[0][2]);
            GrCount[this.Flies[0][2]].ShouldBeEqual(1);            
            this.GroupQ.ShouldBeEqual(2);
            this.Grids.UnderFlight.OnNext(this.Flies[0][1]);
            GrCount[this.Flies[0][1]].ShouldBeEqual(3);                        
            this.GroupQ.ShouldBeEqual(2);
            this.Grids.UnderFlight.OnNext(this.Flies[0][2]);
            GrCount[this.Flies[0][2]].ShouldBeEqual(2);                        
            this.GroupQ.ShouldBeEqual(2);            
            this.Grids.UnderFlight.OnNext(this.Flies[0][3]);
            this.GroupQ.ShouldBeEqual(3);
            GrCount[this.Flies[0][3]].ShouldBeEqual(1);                                  
        }

     
        [TestMethod]
        public void AddAdornerTest()
        {
            while (Grids.Children.Count > 0) Grids.Children.Pop();
            TestPanel.UpdateLayout();
            this.Grids.Children.Add(Cell);
            this.Grids.Children.Add(Cells[0][0]);
            
            this.Cell.SetRowSpan(4).SetColumnSpan(4);
            this.Cells[0][0].SetRowSpan(4).SetColumnSpan(4);            
            this.Adorner.Rects.Count.ShouldBeEqual(0);            
            this.Grids.GetWidgetGridAdorners().Add(this.Adorner);
            TestPanel.UpdateLayout();

            var bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(400);
            bounds.Y.ShouldBeEqual(400);
            bounds.Width.ShouldBeEqual(400);
            bounds.Height.ShouldBeEqual(400);
            this.Adorner.Rects.Count.ShouldBeEqual(0);                        
            var fly = this.GetUp(Cells[0][0]);
            fly.PosSubject.DoOnNext += i=>TestPanel.UpdateLayout();
            bounds = fly.Target.GetBounds();
            bounds.X.ShouldBeEqual(0);
            bounds.Y.ShouldBeEqual(0);
            bounds.Width.ShouldBeEqual(400);
            bounds.Height.ShouldBeEqual(400);          
            fly.CurrentMouse = new Point(300,300);            
            Cell.DockingMode = DockMode.NoDock;
            Grids.OnFly(fly);
            Cell.DockingMode = DockMode.DockEverywhere;            
            var f = Grids.FindDock(fly.Target);
            f.Name.ShouldBeEqual(Cell.Name);
            Cell.DockingMode = DockMode.NoDock;
            bounds = fly.Target.GetBounds().GetIntersectionRect(Cell.GetBounds());
            bounds.Width.ShouldBeEqual(300);
            bounds.Height.ShouldBeEqual(300);
            var reg = Grids.MeasureCellsRegion(bounds);
            reg.Col.ShouldBeEqual(4);
            reg.Row.ShouldBeEqual(4);
            reg.ColSpan.ShouldBeEqual(3);
            reg.RowSpan.ShouldBeEqual(3);
            reg.Count.ShouldBeEqual(9);
            this.Adorner.Rects.Count.ShouldBeEqual(1);                                    
            bounds = fly.Target.GetBounds();
            bounds.X.ShouldBeEqual(300);
            bounds.Y.ShouldBeEqual(300);
            bounds.Width.ShouldBeEqual(400);
            bounds.Height.ShouldBeEqual(400);
            Adorner.Rects[fly].Count.ShouldBeEqual(16);
            var ocupied = Adorner.Rects[fly].Where(r => r.Fill == Adorner.OccupiedSpaceBrush).ToCollection();
            var free = Adorner.Rects[fly].Where(r => r.Fill == Adorner.FreeSpaceBrush).ToCollection();
            ocupied.Count.ShouldBeEqual(9);
            free.Count.ShouldBeEqual(7);
            fly.CurrentMouse = new Point(100, 100);
            Grids.OnFly(fly);
            TestPanel.UpdateLayout();
            Adorner.Rects[fly].Count.ShouldBeEqual(16);
            Adorner.Rects[fly].Where(r => r.Fill == Adorner.OccupiedSpaceBrush).ToCollection().Count.ShouldBeEqual(16);            
        }

    }
}
