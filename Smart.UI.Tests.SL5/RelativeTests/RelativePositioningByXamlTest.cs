using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.UI.Relatives;
using Smart.Classes.Arguments;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;
using Smart.UI.Tests.PanelsTests;


namespace Smart.UI.Tests.RelativeTests
{
  
    [TestClass]
    public class RelativePositioningByXamlTest : GridTestBase<SmartGrid>
    {
        public SmartGrid TestCell;
        public RelativeConverter Rel;

        [TestInitialize]
        public override void SetUp()
        {
 	        base.SetUp();
            this.Rel = new RelativeConverter();
            this.TestCell = Cells[0, 0];
            Rel.ShouldNotBeNull();
            this.TestCell.ShouldNotBeNull();
            RelativeConverter.OddActions = false;
        }

        [TestCleanup]
        public override void CleanUp()
        {
 	        base.CleanUp();
            Rel = null;
            this.TestCell = null;
        }
      
        [TestMethod]
        public void NameFinderTest()
        {
            var q = 0;
            const string name = "cell_2_2";
            var child = this.Grids.ChildByName(name);
            child.ShouldBeEqual(Cells[2, 2]);

            Grids.RemoveChild(Cells[1, 1]);
            Grids.RemoveChild(Cells[1, 2]);
            Grids.RemoveChild(child);
            TestPanel.UpdateLayout();

            child = this.Grids.ChildByName(name);
            child.ShouldBeEqual(null);

            TestPanel.UpdateLayout();
            Grids.Children.ItemsAdded.OnceIf(i => i.Name.Equals(name), i => q++);
            Grids.AddChild(Cells[1, 1]);
            q.ShouldBeEqual(0);
            Grids.AddChild(Cells[2, 2]);
            q.ShouldBeEqual(1);
            Grids.AddChild(Cells[1, 2]);
            q.ShouldBeEqual(1);
            Grids.RemoveChild(Cells[2, 2]);
            Grids.AddChild(Cells[2, 2]);
            q.ShouldBeEqual(1);            
        }

        [TestMethod]
        public void NameCatcherTest()
        {
            var q = 0;
            var r = 0;
            Action<FrameworkElement> action = i => q++;
            const string name = "cell_2_2";
            var child = this.Grids.ChildByName(name);
            child.ShouldBeEqual(Cells[2, 2]);
            Grids.CatchByName(name, action);
            q.ShouldBeEqual(1);
            Grids.RemoveChild(child);
            q.ShouldBeEqual(1);
            Grids.AddChild(child);
            q.ShouldBeEqual(1);
            Grids.RemoveChild(child);
            q.ShouldBeEqual(1);
            Grids.CatchByName(name, action);
            q.ShouldBeEqual(1);
            Grids.AddChild(child);
            q.ShouldBeEqual(2);
            Grids.RemoveChild(child);
            Grids.CatchByName(name, action, ra => r++);
            r.ShouldBeEqual(0);
            Grids.AddChild(child);
            q.ShouldBeEqual(3);
            Grids.RemoveChild(child);
            r.ShouldBeEqual(1);
            Grids.AddChild(child);
            q.ShouldBeEqual(3);
            Grids.CatchByName(name, action, ra => r++);
            q.ShouldBeEqual(4);
            r.ShouldBeEqual(1);
            Grids.RemoveChild(child);
            r.ShouldBeEqual(2);
        }

        [TestMethod]
        public void ToTest()
        {
            Relative.GetTo(TestCell).ShouldBeEqual(default(String));
            Relative.GetRelativeElement(TestCell).ShouldBeEqual(null);
            Relative.GetRelativePosition(TestCell).ShouldBeEqual(null);
            Relative.SetTo(TestCell, "Cell2");
            Relative.GetRelativeElement(TestCell).ShouldBeEqual(null);
            Relative.GetRelativePosition(TestCell).ShouldBeEqual(null);
            Relative.SetTo(TestCell, "cell_4_4");
            Relative.GetRelativeElement(TestCell).ShouldBeEqual(Cell);
            Relative.SetTo(TestCell, "cell_4_45");
            Relative.GetRelativeElement(TestCell).ShouldBeEqual(null);
            Relative.SetTo(TestCell, "cell_4_4");
            Relative.GetRelativeElement(TestCell).ShouldBeEqual(Cell);           
        }


        [TestMethod]
        public void RelativeConverterTest()
        {
            Rel.CanConvertFrom(typeof(String)).ShouldBeTrue();
            Rel.CanConvertFrom(typeof(RelativeValue)).ShouldBeFalse();
            Rel.CanConvertFrom(typeof(Point)).ShouldBeFalse();
            /*
            Rel.CanConvertTo(typeof(String)).ShouldBeTrue();
            Rel.CanConvertTo(typeof(RelativeValue)).ShouldBeFalse();
            Rel.CanConvertTo(typeof(Point)).ShouldBeFalse();
             */
            (Rel.ConvertFrom("Left" as object) as Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>).ShouldNotBeNull().ShouldBeEqual(RelativeFunctions.PosLeft);
            (Rel.ConvertFrom("Right" as object) as Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>).ShouldNotBeNull().ShouldBeEqual(RelativeFunctions.PosRight);
            (Rel.ConvertFrom("Top" as object) as Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>).ShouldNotBeNull().ShouldBeEqual(RelativeFunctions.PosTop);
            (Rel.ConvertFrom("Bottom" as object) as Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>).ShouldNotBeNull().ShouldBeEqual(RelativeFunctions.PosBottom);
        }

        [TestMethod]
        public void RelativePositioningSettersTest()
        {
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));                        
            TestCell.SetPlace(new Rect(0, 0, 100, 100));
            var str = "Right";
            Rel.CanConvertFrom(new Point().GetType()).ShouldBeFalse();            
            Rel.CanConvertFrom(str.GetType()).ShouldBeTrue();
            var act = (Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object);
            act.ShouldBeEqual(RelativeFunctions.PosRight);
            TestCell.SetRelativeTo(Cell.Name);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);
            TestCell.SetPosition(act);
            RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()).ShouldBeEqual(act);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosRight);            
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()));
            var relPos = TestCell.GetRelativePosition();
            relPos.Param1.ShouldBeEqual(TestCell);
            relPos.Action.ShouldBeEqual(act);
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            TestPanel.UpdateLayout();            
            TestCell.GetSlot().ShouldBeEqual(new Rect(500, 0, 100, 100));            
            TestCell.GetBounds().ShouldBeEqual(new Rect(500, 0, 100, 100));
            TestPanel.SetPlace(new Rect(0, 0, 100, 100));
            TestPanel.UpdateLayout();
            Cell.SetColumn(5);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));                        
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 0, 100, 100));
            TestCell.GetSlot().ShouldBeEqual(new Rect(600, 0, 100, 100));            
            str = "bottom";
            TestCell.SetPosition((Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object));
            TestPanel.UpdateLayout();
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosBottom);            
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            TestPanel.UpdateLayout();
            /*NAPILNIK
            TestCell.GetSlot().ShouldBeEqual(new Rect(600, 500, 100, 100));         
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 500, 100, 100));         
             */
        }

        [TestMethod]
        public void RelativePositioningSidesTest()
        {
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            TestCell.SetPlace(new Rect(0, 0, 100, 100));
            var str = "RightSide";
            Rel.CanConvertFrom(new Point().GetType()).ShouldBeFalse();
            Rel.CanConvertFrom(str.GetType()).ShouldBeTrue();
            var act = (Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object);
            act.ShouldBeEqual(RelativeFunctions.PosRightSide);
            TestCell.SetRelativeTo(Cell.Name);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);
            TestCell.SetPosition(act);
            RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()).ShouldBeEqual(act);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosRightSide);
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()));
            var relPos = TestCell.GetRelativePosition();
            relPos.Param1.ShouldBeEqual(TestCell);
            relPos.Action.ShouldBeEqual(act);
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            TestPanel.UpdateLayout();
            TestCell.GetSlot().ShouldBeEqual(new Rect(400, 0, 100, 100));
            TestCell.GetBounds().ShouldBeEqual(new Rect(400, 0, 100, 100));
            TestPanel.SetPlace(new Rect(0, 0, 100, 100));
            TestPanel.UpdateLayout();
            Cell.SetColumn(5);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            TestCell.GetBounds().ShouldBeEqual(new Rect(500, 0, 100, 100));
            TestCell.GetSlot().ShouldBeEqual(new Rect(500, 0, 100, 100));
            str = "BottomSide";
            TestCell.SetPosition((Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object));
            TestPanel.UpdateLayout();
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosBottomSide);
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            TestPanel.UpdateLayout();
            /*NAPILNIK
            TestCell.GetSlot().ShouldBeEqual(new Rect(600, 500, 100, 100));         
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 500, 100, 100));         
             */
        }

        //[TestMethod]

        
        [TestMethod]
        public void RelativePositioningCallbacksTest()
        {
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
            TestCell.SetPlace(new Rect(0, 0, 100, 100));
            //emulate depprop setting before adding to stage
            this.Grids.RemoveChild(TestCell);
            TestPanel.UpdateLayout();
            TestCell.Parent.ShouldBeEqual(null);
            

            //set relative position property
            var str = "Right";            
            var act = (Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object);
            act.ShouldBeEqual(RelativeFunctions.PosRight);
            TestCell.SetValue(Relative.PositionProperty,act.AsString());
            RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()).ShouldBeEqual(act);
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()));
            TestPanel.UpdateLayout();

            //set RelativeFunctions.To
            TestCell.SetValue(Relative.ToProperty, Cell.Name);
            TestCell.GetRelativeElement().ShouldBeEqual(null);
            

            //adding TestCell
            Grids.AddChild(TestCell);
            TestPanel.UpdateLayout();

            var relEl = TestCell.GetRelativeElement();
            relEl.ShouldBeEqual(Cell);
            RelativeConverter.Instance.ConvertFrom(TestCell.GetPosition()).ShouldBeEqual(act);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);
            
            //action with args
            var relPos = TestCell.GetRelativePosition();
            RelativeConverter.Instance.ConvertTo(relPos.Action).ShouldBeEqual(TestCell.GetPosition());            
            relPos.Param1.ShouldBeEqual(TestCell);
            relPos.Action.ShouldBeEqual(act).ShouldNotBeNull();


            TestCell.Parent.ShouldBeEqual(Grids);           
            relEl.GetOnArrange().ShouldNotBeNull().DoOnNext.ShouldNotBeNull();

           
           // Cell.UpdateMeasure();
           // Cell.UpdateArrange();
            //Cell.GetOnArrange().OnNext(new Args<FrameworkElement, Rect, Size>(Cell,Cell.GetSlot(),Grids.RenderSize ));
           
            TestPanel.UpdateLayout();
            
            
            Cell.GetBounds().ShouldBeEqual(new Rect(400, 400, 100, 100));
           
            TestCell.GetSlot().ShouldBeEqual(new Rect(500, 0, 100, 100));                                                           
            TestCell.GetBounds().ShouldBeEqual(new Rect(500, 0, 100, 100));
            TestPanel.SetPlace(new Rect(0, 0, 100, 100));
            TestPanel.UpdateLayout();

            Cell.SetColumn(5);
            TestPanel.UpdateLayout();
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 0, 100, 100));
            str = "bottom";
            TestCell.SetPosition((Action<Args<FrameworkElement, Rect, Size>, FrameworkElement>)Rel.ConvertFrom(str as object));
            TestCell.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosBottom);
            TestCell.GetRelativeElement().ShouldBeEqual(Cell);
            TestPanel.UpdateLayout();
            Cell.GetBounds().ShouldBeEqual(new Rect(500, 400, 100, 100));
            /* NAPILNIK
            TestPanel.UpdateLayout();
            TestCell.GetBounds().ShouldBeEqual(new Rect(600, 500, 100, 100));
            TestCell.GetSlot().ShouldBeEqual(new Rect(600, 500, 100, 100));
             */
        }

        [TestMethod]
        public void XamlLoadTest()
        {
            var canv = XamlReader.Load(@"<FlexCanvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' FlexCanvas.Place='200,200,200,200'><Rectangle FlexCanvas.Left='0' FlexCanvas.Right='0' Name='rect' Width='100' Height='100'/></FlexCanvas>") as FlexCanvas;
            Grids.AddChild(canv);            
            TestPanel.UpdateLayout();
            canv.GetPlace().ShouldBeEqual(new Rect(200, 200, 200, 200));
            canv.GetBounds().ShouldBeEqual(new Rect(200, 200, 200, 200));
        } 

        [TestMethod]
        public void LoadScrollBar()
        {
            RelativeConverter.OddActions.ShouldBeFalse();
            //Application.LoadComponent();
            var scroll = XamlReader.Load(@"
<FlexCanvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation'  DragManager.ChildrenDragCanvas='NearestParent' DragManager.ChildrenDockMode='DockEverywhere' CanvasUpdateMode='KeepCanvas' Name='Root' DragEnabled='True' Height='16' FlexCanvas.Place='0,0,1000,16' Background='White' WaitBeforeDrag='0:0:0' >
                        <!-- <Rectangle Name='Shadow'   Fill='Gray' Canvas.ZIndex='-1' Stroke='Red'/>-->
                        <Rectangle Name='Substract'  Fill='Gray'  DragManager.DragSubject='None'/>
                        <Rectangle Name='Slider' DragManager.DragSubject='Horizontal' Fill='White' FlexCanvas.Left='70' MinWidth='40'  RadiusX='10' RadiusY='10'  StrokeThickness='1'/>
                        <Triangle Name='BackwardFastArrow'  Margin='5,0,5,0'  Orientation='Left' Relative.To='Slider' Relative.Position='LeftSide'  StrokeThickness='1'  Width='15' Fill='Gray' />
                        <Triangle Name='ForwardFastArrow'  Margin='5,0,5,0' Orientation='Right' Relative.To='Slider' Relative.Position='RightSide'  StrokeThickness='1'  Width='15' Fill='Gray' />
                        <Rectangle Name='BackwardSplitter' Relative.To='BackwardFastArrow' Relative.Position='Right' Width='5' Fill='LightGray'/>
                        <Rectangle Name='ForwardSplitter'  Relative.To='ForwardFastArrow' Relative.Position='Left' Width='5' Fill='LightGray'/>
                        <Triangle Name='BackwardArrow'  Orientation='Left' FlexCanvas.Left='0'  StrokeThickness='1'  Width='15' Fill='AliceBlue' />
                        <Triangle Name='ForwardArrow'  Orientation='Right' FlexCanvas.Right='0'  StrokeThickness='1'  Width='15' Fill='AliceBlue' />
                    </FlexCanvas>
") as FlexCanvas;


            Grids.AddChild(scroll);
            var slider = scroll.ChildByName<Rectangle>("Slider").ShouldNotBeNull();            
            var bsp = scroll.ChildByName<Rectangle>("BackwardSplitter").ShouldNotBeNull();
            var fsp = scroll.ChildByName<Rectangle>("ForwardSplitter").ShouldNotBeNull();
            var bfa = scroll.ChildByName<Triangle>("BackwardFastArrow").ShouldNotBeNull();
            var ffa = scroll.ChildByName<Triangle>("ForwardFastArrow").ShouldNotBeNull();

            bfa.GetRelativeTo().ShouldBeEqual(slider.Name);
            ffa.GetRelativeTo().ShouldBeEqual(slider.Name);
            bsp.GetRelativeTo().ShouldBeEqual(bfa.Name);
            fsp.GetRelativeTo().ShouldBeEqual(ffa.Name);
            

            bfa.GetPosition().ShouldBeEqual("LeftSide");
            ffa.GetPosition().ShouldBeEqual("RightSide");
            bsp.GetPosition().ShouldBeEqual("Right");
            fsp.GetPosition().ShouldBeEqual("Left");

            bfa.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosLeftSide);
            ffa.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosRightSide);
            bsp.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosRight);
            fsp.GetRelativePosition().Action.ShouldBeEqual(RelativeFunctions.PosLeft);
            TestPanel.UpdateLayout();

            scroll.Children.ShouldHaveElementBefore(slider, bfa);
            scroll.Children.ShouldHaveElementBefore(slider, ffa);
            scroll.Children.ShouldHaveElementBefore(bfa, bsp);
            scroll.Children.ShouldHaveElementBefore(ffa, fsp);

            slider.GetOnArrange().ShouldNotBeNull();

            bfa.Margin = default(Thickness);
            ffa.Margin = default(Thickness);
            bsp.Margin = default(Thickness);
            fsp.Margin = default(Thickness);
            slider.Margin = default(Thickness);
           


            bfa.GetRelativeElement().ShouldBeEqual(slider);
            ffa.GetRelativeElement().ShouldBeEqual(slider);
            bsp.GetRelativeElement().ShouldBeEqual(bfa);
            fsp.GetRelativeElement().ShouldBeEqual(ffa);

            var sleft = slider.GetLeft().ShouldBeEqual(70);
            
            scroll.InvalidateMeasure();
            
            var s = slider.GetOnArrange();
            s.DoOnNext += i => { var a = s; };
             
            TestPanel.UpdateLayout();
           // slider.GetOnArrange().ShouldNotBeNull().ShouldBeSame(s);

            sleft = slider.GetLeft().ShouldBeEqual(70).ShouldBeEqual(slider.GetBounds().X);
            bfa.GetBounds().X.ShouldBeEqual(sleft);
            slider.ActualWidth.ShouldBeEqual(40);
            ffa.GetBounds().X.ShouldBeEqual(slider.ActualWidth + sleft-ffa.ActualWidth);

            

            Grids.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            Grids.Space.Canvas.ShouldBeEqual(new Rect(0, 0, 1000, 1000));

            scroll.GetPlace().ShouldBeEqual(new Rect(0, 0, 1000, 16));
            scroll.GetBounds().ShouldBeEqual(new Rect(0, 0, 1000, 16));
        }

        


    }
}
