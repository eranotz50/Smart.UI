using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Controls.Scrollers;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Tests.PanelsTests;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Tests.ScrollAndZoomTests
{

  
    [TestClass]   
    public class ScrollTest:GridTestBase<SmartGrid>
    {
        public VerticalScrollBar VScroll;
        public HorizontalScrollBar HScroll;
     
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            VScroll = new VerticalScrollBar();
            HScroll = new HorizontalScrollBar();
            VScroll.Name = "VScroll";
            HScroll.Name = "HScroll";
            TestPanel.UpdateLayout();
        }


        [TestMethod]
        public void ApplyTemplateTest()
        {
            this.Grids.AddChild(this.VScroll);
            this.Grids.AddChild(this.HScroll);
            this.VScroll.Root.ShouldBeNull();
            this.HScroll.Root.ShouldBeNull();
            this.UpdateLayout();
            this.VScroll.Root.ShouldNotBeNull();
            this.HScroll.Root.ShouldNotBeNull();
            
        }



        [TestMethod]
        public void ScrollersByPanelTest()
        {
            this.Grids.Height = 1000;
            this.Grids.Width = 1000;
            this.Grids.OutMode = OutMode.Clip;
            this.Grids.SetCanvasSize(new Size(10000,10000));
            
            Grids.Children.Add(HScroll);                           
            Grids.Children.Add(VScroll);
            
            TestPanel.UpdateLayout();            
            var h = HScroll.Root.ActualHeight;
            var v = VScroll.Root.ActualWidth;
            
            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000-h, 1000, h));            
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000-v,0,v,1000));

            this.HScroll.Root.GetLocation().ShouldBeEqual(new Rect(0, 0, 1000, h));
            //this.HScroll.Root.GetBounds().ShouldBeEqual(new Rect(0, 0, 1000, h));
            this.HScroll.Root.ActualWidth.ShouldBeEqual(1000);
            this.HScroll.Root.ActualHeight.ShouldBeEqual(h);
            this.HScroll.Slider.ActualHeight.ShouldBeEqual(h);             
            this.HScroll.Slider.Width.ShouldBeEqual(100); 
            this.HScroll.Slider.ActualWidth.ShouldBeEqual(100);            
            this.HScroll.Slider.GetLeft().ShouldBeEqual(0);
            this.HScroll.Slider.ActualWidth.ShouldBeEqual(100);
            
           // this.HScroll.Slider.Height.ShouldBeEqual(h);
            this.HScroll.Slider.GetLocation().ShouldBeEqual(new Rect(0, 0, 100, h));
            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, h));



            this.VScroll.GetLocation().ShouldBeEqual(new Rect(1000 - v, 0, v, 1000));
            this.VScroll.Slider.GetSlot().ShouldBeEqual(new Rect(0, 0, v, 100));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 0, v, 100));

           
            Grids.Space.PanelChange.FireChange(Grids.Space.Panel.SetCoords(2000, 3000));
            TestPanel.UpdateLayout();

            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000 - h, 1000, h));
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000-v, 0, v, 1000));

            this.HScroll.Slider.GetLocation().ShouldBeEqual(new Rect(200, 0, 100, h));
            this.VScroll.Slider.GetLocation().ShouldBeEqual(new Rect(0, 300, v, 100));
         
            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(200, 0, 100, h));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 300, v, 100));
            
        }

        [TestMethod]
        public void UpdatePanelTest()
        {
            this.UpdateLayout();
            this.Grids.OutMode = OutMode.Clip;
            this.Grids.Width.ShouldBeEqual(1000);
            this.Grids.Height.ShouldBeEqual(1000);            
            this.Cells[0, 0].Visibility.ShouldBeEqual(Visibility.Visible);
            this.Cells[1, 1].GetBounds().ShouldBeEqual(new Rect(100,100, 100, 100));
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            this.Cell.Visibility.ShouldBeEqual(Visibility.Visible);

            this.Grids.Space.UpdatePanel(new Rect(100, 100, 100, 100));
            
            this.UpdateLayout();

            this.Grids.Width.ShouldBeEqual(100);
            this.Grids.Height.ShouldBeEqual(100);
            this.Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            this.Grids.ColumnDefinitions.Length.ShouldBeEqual(1000);
            this.Grids.ColumnDefinitions.StarLength.ShouldBeEqual(100);
            this.Grids.RowDefinitions.StarLength.ShouldBeEqual(100);
            this.Grids.RowDefinitions.Length.ShouldBeEqual(1000);
            this.Cell.Visibility.ShouldBeEqual(Visibility.Collapsed);
            this.Cells[0, 0].Visibility.ShouldBeEqual(Visibility.Collapsed);
            this.Cells[1, 1].GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));

            this.Grids.Space.UpdatePanel(new Rect(0, 0, 1000, 1000));
            this.UpdateLayout();

            this.Grids.Width.ShouldBeEqual(1000);
            this.Grids.Height.ShouldBeEqual(1000);
            this.Cells[0, 0].Visibility.ShouldBeEqual(Visibility.Visible);
            this.Cells[1, 1].GetBounds().ShouldBeEqual(new Rect(100, 100, 100, 100));
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            this.Cell.Visibility.ShouldBeEqual(Visibility.Visible);
            
        }

        
        [TestMethod]       
        public void PanelShiftByScrollersTest()
        {           
            this.Grids.Height = 1000;
            this.Grids.Width = 1000;
            this.Grids.OutMode = OutMode.Clip;
            this.Grids.SetCanvasSize(new Size(10000, 10000));

            var rel = this.Grids.Space.RelativeShift;
            rel.IsValid().ShouldBeTrue();  
          

            Grids.Children.Add(HScroll);
            Grids.Children.Add(VScroll);

            TestPanel.UpdateLayout();

            this.Grids.Space.Panel.X.ShouldBeEqual(0);
            this.Grids.Space.RelativeShift.IsValid().ShouldBeTrue();  

            var h = HScroll.Root.Height;
            var v = VScroll.Root.Width;
            
            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000-h, 1000, h));
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000 - v, 0, v, 1000));

            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 0, 100, h));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 0, v, 100));

            this.Grids.Space.Panel.X.ShouldBeEqual(0);
            rel = this.Grids.Space.RelativeShift;
            rel.IsValid().ShouldBeTrue();  


            var hfly = this.GetUp(this.HScroll.Slider);
            hfly.CurrentMouse = new Point(200,50);
            HScroll.Root.OnFly(hfly);

            this.HScroll.Position.Start.ShouldBeEqual(0.2);            
            this.VScroll.Position.Start.ShouldBeEqual(0.0);
            this.Grids.Space.Panel.X.ShouldBeEqual(2000);
            this.Grids.Space.Panel.Y.ShouldBeEqual(0);

            TestPanel.UpdateLayout();
            this.HScroll.Position.Start.ShouldBeEqual(0.2);
            this.VScroll.Position.Start.ShouldBeEqual(0.0);
            
            this.Grids.Space.Panel.X.ShouldBeEqual(2000);
            this.Grids.Space.Panel.Y.ShouldBeEqual(0);
            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(200, 0, 100, h));
           

            var vfly = this.GetUp(this.VScroll.Slider);
            vfly.CurrentMouse = new Point(0,300);
            this.Grids.Space.RelativeShift.IsValid().ShouldBeTrue();                                    
            VScroll.Root.OnFly(vfly);
            this.HScroll.Position.Start.ShouldBeEqual(0.2);
            this.VScroll.Position.Start.ShouldBeEqual(0.3);

            TestPanel.UpdateLayout();

                    
            this.Grids.Space.Panel.X.ShouldBeEqual(2000);
            this.Grids.Space.Panel.Y.ShouldBeEqual(3000);
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 300, v, 100));
         
          
            rel = this.Grids.Space.RelativeShift;
            rel.IsValid().ShouldBeTrue();                        
                        
            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000-h, 1000, h));
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000 - v, 0, v, 1000));
          

            this.HScroll.Position.Start.ShouldBeEqual(0.2);
            this.VScroll.Position.Start.ShouldBeEqual(0.3);
/*
            var backFly = this.GetUp(this.HScroll.BackwardSplitter);
            backFly.CurrentMouse = new Point(-200,50);
            HScroll.Root.OnFly(backFly);
            //this.HScroll.OtherLineTriplets.Length.ShouldBeEqual()

            this.HScroll.OtherLineTriplets.Start.ShouldBeEqual(0);
            this.HScroll.OtherLineTriplets.Length.ShouldBeNear(0.3);
            this.VScroll.OtherLineTriplets.Start.ShouldBeNear(0.3); 
            this.VScroll.OtherLineTriplets.Length.ShouldBeNear(0.1);
            this.Grids.Space.Panel.X.ShouldBeEqual(0);
            this.Grids.Space.Panel.Width.ShouldBeNear(3000);      


            TestPanel.UpdateLayout();

            this.Grids.Space.Panel.X.ShouldBeEqual(0);
            this.Grids.Space.Panel.Width.ShouldBeNear(3000);            
            this.Grids.Space.Panel.Y.ShouldBeEqual(3000);
            this.Grids.Space.Panel.Height.ShouldBeNear(1000);  

            this.HScroll.Slider.Width.ShouldBeEqual(900);
            this.VScroll.Slider.Height.ShouldBeEqual(300);            

            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 0, 900, h));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 300, v, 100));


            backFly = this.GetUp(this.VScroll.BackwardSplitter);
            backFly.CurrentMouse = new Point(50, -300);
            VScroll.Root.OnFly(backFly);

            this.HScroll.OtherLineTriplets.Start.ShouldBeEqual(0);
            this.VScroll.OtherLineTriplets.Start.ShouldBeEqual(0);
            this.HScroll.OtherLineTriplets.Length.ShouldBeNear(0.3);
            this.VScroll.OtherLineTriplets.Length.ShouldBeNear(0.3);

            TestPanel.UpdateLayout();

            this.Grids.Space.Panel.X.ShouldBeEqual(0);
            this.Grids.Space.Panel.Y.ShouldBeEqual(0);

            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 0, 900, h));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 900, v, 100));    
 */
        }

        [TestMethod]
        public void BeforaUpdateTest()
        {
            var q = 0;

            this.Grids.Height = 1000;
            this.Grids.Width = 1000;
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            this.Grids.OutMode = OutMode.Clip;
            this.Grids.SetCanvasSize(new Size(2000, 2000));
            this.Grids.Space.Shift(new Point(200, 200));
            this.Grids.BeforeArrange.DoOnNext += i => q++;
            q.ShouldBeEqual(0);
            this.Grids.UpdateLayout();
            q.ShouldBeEqual(2);
            this.Grids.UpdateLayout();
            q.ShouldBeEqual(2);
            this.Cell.InvalidateMeasure();
            this.UpdateLayout();
            q.ShouldBeEqual(2);
            this.Grids.InvalidateMeasure();
            this.UpdateLayout();
            q.ShouldBeEqual(3);
        }

        [TestMethod]
        public void MoveSliderTest()
        {
            
            this.Grids.Height = 1000;
            this.Grids.Width = 1000;
            this.Cell.GetBounds().ShouldBeEqual(new Rect(400,400,100,100));
            this.Grids.OutMode = OutMode.Clip;
            this.Grids.SetCanvasSize(new Size(2000, 2000));
            this.Grids.Space.Shift(new Point(200,200));
            this.Grids.AddChild(HScroll);
            this.Grids.AddChild(VScroll);
            var q = 0;
            this.Grids.BeforeArrange.DoOnNext += i => q++;
            this.UpdateLayout();
            q.ShouldBeEqual(4);
            this.Grids.InvalidateMeasure();            
            this.UpdateLayout();
            q.ShouldBeEqual(5);
            

            this.Grids.Space.Panel.ShouldBeEqual(new Rect(200, 200, 1000, 1000));
            this.Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 2000, 2000));
            var h = HScroll.Root.Height;
            var v = VScroll.Root.Width;
            this.Cell.GetBounds().ShouldBeEqual(new Rect(600, 600, 200, 200));

            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000 - h, 1000, h));
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000-v, 0, v, 1000));

            this.HScroll.Position.Start.ShouldBeNear(0.1);
            this.VScroll.Position.Start.ShouldBeNear(0.1);
            
            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(100,0,500,h));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 100, v, 500));



            var fly = this.GetUp(this.HScroll.Slider);
            
            fly.CurrentMouse = new Point(100,0);
            this.HScroll.Root.OnFly(fly);
            
            this.HScroll.Position.Start.ShouldBeNear(0.2);
            this.HScroll.Position.Length.ShouldBeNear(0.5);

            this.VScroll.Position.Start.ShouldBeNear(0.1);
            this.VScroll.Position.Length.ShouldBeNear(0.5);
            

            this.UpdateLayout();


            this.HScroll.Position.Start.ShouldBeNear(0.2);
            this.HScroll.Position.Length.ShouldBeNear(0.5);

            this.VScroll.Position.Start.ShouldBeNear(0.1);
            this.VScroll.Position.Length.ShouldBeNear(0.5);

            this.Grids.Space.Panel.ShouldBeEqual(new Rect(400, 200, 1000, 1000));
            this.Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 2000, 2000));

            
            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000 - h, 1000, h));
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000 - v, 0, v, 1000));
            

            this.HScroll.Slider.GetBounds().ShouldBeEqual(new Rect(200, 0, 500, h));
            this.VScroll.Slider.GetBounds().ShouldBeEqual(new Rect(0, 100, v, 500));
            

            fly = this.GetUp(this.HScroll.Slider);
            fly.CurrentMouse = new Point(100, 0);
            this.HScroll.Root.OnFly(fly);

            this.HScroll.Position.Start.ShouldBeNear(0.3);
            this.HScroll.Position.Length.ShouldBeNear(0.5);

            this.VScroll.Position.Start.ShouldBeNear(0.1);
            this.VScroll.Position.Length.ShouldBeNear(0.5);
            
            //this.Grids.Space.Canvas.RoundEquals(new Rect(600, 200, 1000, 1000)).ShouldBeTrue();
            //this.Grids.Space.Canvas.RoundEquals(new Rect(0, 0, 2000, 2000)).ShouldBeTrue(); ;

            this.UpdateLayout();

            this.Grids.Space.Panel.RoundEquals(new Rect(600, 200, 1000, 1000)).ShouldBeTrue(); ;
            this.Grids.Space.Canvas.RoundEquals(new Rect(0, 0, 2000, 2000)).ShouldBeTrue(); ;
            this.HScroll.Slider.GetBounds().RoundEquals(new Rect(300, 0, 500, h)).ShouldBeTrue(); ;
            this.VScroll.Slider.GetBounds().RoundEquals(new Rect(0, 100, v, 500)).ShouldBeTrue(); ;

            this.HScroll.GetBounds().ShouldBeEqual(new Rect(0, 1000 - h, 1000, h));
            this.VScroll.GetBounds().ShouldBeEqual(new Rect(1000 - v, 0, v, 1000));
            



            //this.HScroll.ShouldBeEqual()
        }


        [TestMethod]
        public void ShowScrollers()
        {
            this.Grids.AddChild(HScroll);
            this.Grids.AddChild(VScroll);
            this.UpdateLayout();

            HScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);
            VScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);

            this.Grids.CanvasWidth = 1200;
            this.UpdateLayout();

            HScroll.Visibility.ShouldBeEqual(Visibility.Visible);
            VScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);

            this.Grids.CanvasHeight = 1200;

            this.UpdateLayout();
            HScroll.Visibility.ShouldBeEqual(Visibility.Visible);
            VScroll.Visibility.ShouldBeEqual(Visibility.Visible);

            this.Grids.Space.GrowCanvas(new Point(-200, 0));
            this.UpdateLayout();

            this.Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 1000, 1200));
            HScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);
            VScroll.Visibility.ShouldBeEqual(Visibility.Visible);


            this.Grids.Space.GrowCanvas(new Point(0,-200));
            this.UpdateLayout();



            this.Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            HScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);
            VScroll.Visibility.ShouldBeEqual(Visibility.Collapsed);

        }


    }
     
}

