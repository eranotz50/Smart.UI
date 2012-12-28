using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Tests.TestBases;
using Smart.TestExtensions;

namespace Smart.UI.Tests.PanelsTests.FlexCanvasTest
{
   
    [TestClass]
    public class FlexCanvasTest:PanelTestBase<FlexCanvas>
    {


        public Rectangle Cell;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            Cell = new Rectangle();            
            Panel.Children.Add(Cell);
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();         
            Cell = null;
        }

        [TestMethod]
        public void TestUpdate()
        {
            Cell.SetLeft(10).SetRight(10).SetTop(10).SetBottom(10);
            TestPanel.UpdateLayout();
            var bounds = Cell.GetBounds();
            bounds.X.ShouldBeEqual(10);
            bounds.Y.ShouldBeEqual(10);
            bounds.Width.ShouldBeEqual(980);
            bounds.Height.ShouldBeEqual(980);
        }

        [TestMethod]
        public void TestCanvasWidth()
        {
            var b = new Border();
            Panel.AddChild(b);
            b.SetLeft(100).SetRight(100).SetBottom(100).SetTop(100);
            UpdateLayout();

            Panel.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1000, 1000));
            b.GetBounds().ShouldBeEqual(new Rect(100, 100, 800, 800));
            Panel.CanvasWidth = 2000;
            Panel.CanvasHeight = 3000;
            UpdateLayout();

            b.GetBounds().ShouldBeEqual(new Rect(100, 100, 1800, 2800));
            Panel.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1000, 1000));


        }

        [TestMethod]
        public void CanvasOutModeTest()
        {
            TestPanel.UpdateLayout();
            Panel.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1000, 1000));  
            this.Cell.SetPlace(new Rect(1500, 1500, 100, 100));
            TestPanel.UpdateLayout();
            
            Cell.GetBounds().ShouldBeEqual(new Rect(1500, 1500,100, 100));
            this.Panel.CanvasUpdateMode = CanvasUpdateMode.KeepCanvas;
            TestPanel.UpdateLayout();

            Cell.GetBounds().ShouldBeEqual(new Rect(900, 900, 100, 100));
            this.Panel.CanvasUpdateMode = CanvasUpdateMode.AutoGrowCanvas;
            this.Panel.OutMode = OutMode.Clip;
            Panel.Space.Panel.ShouldBeEqual(new Rect(0, 0, 1000, 1000));               
            TestPanel.UpdateLayout();

            Panel.Space.Panel.ShouldBeEqual(new Rect(0,0,1000,1000));            
            Panel.Space.Canvas.Size().ShouldBeEqual(new Size(1600, 1600));
            Cell.Visibility.ShouldBeEqual(Visibility.Collapsed);
            Cell.SetPlace(new Rect(900, 900, 100, 100));
            TestPanel.UpdateLayout();
            Panel.Space.Canvas.Size().ShouldBeEqual(new Size(1000, 1000));            
        }

        [TestMethod]
        public void NegativeRelativeTest()
        {
                 var panel = XamlReader.Load(@"
             <FlexCanvas xmlns='http://schemas.microsoft.com/winfx/2006/xaml/presentation' Name='relativeCanvas'  Background='Green'>
        <Button Content ='Swap' Name='swapBtn'   FlexCanvas.Left='10' ></Button>
        <Button Content ='Swap with this' Name='swapFullBtn' FlexCanvas.Left='100'></Button>        
        <Button Name='btnTopLeft' FlexCanvas.Top='0.1*' FlexCanvas.Left='0.1*' Content='T 0.1* : L 0.1*' Width='auto' Height='auto' />
        <Button Name='btnTopMinCenter'  FlexCanvas.Top='0.1*' FlexCanvas.Center='-0.1*' Content='T 0.1 : *C -0.1*'  Width='auto' Height='auto' />
        <Button Name='btnTopCenter'  FlexCanvas.Top='0.1*' FlexCanvas.Center='0.1*' Content='T 0.1 : *C 0.1*'  Width='auto' Height='auto' />
        <Button Name='btnTopRight'  FlexCanvas.Top='0.1*' FlexCanvas.Right='0.1*' Content='T 0.1* : R 0.1*'  Width='auto' Height='auto' />

        <Button Name='btnMinMiddleLeft'  FlexCanvas.Middle='-0.1*' FlexCanvas.Left='0.1*' Content='M -0.1* : L 0.1*'  Width='auto' Height='auto' />
        <Button Name='btnMinMiddleMinCenter'  FlexCanvas.Middle='-0.1*' FlexCanvas.Center='-0.1*' Content='M -0.1* : C -0.1*'  Width='auto' Height='auto' />
        <Button Name='btnMinMiddleCenter'  FlexCanvas.Middle='-0.1*' FlexCanvas.Center='0.1*' Content='M -0.1* : C 0.1*'  Width='auto' Height='auto' />
        <Button Name='btnMinMiddleRight'  FlexCanvas.Middle='-0.1*' FlexCanvas.Right='0.1*' Content='M -0.1* : R 0.1*'  Width='auto' Height='auto' />

        <Button Name='btnMiddleLeft'  FlexCanvas.Middle='0.1*' FlexCanvas.Left='0.1*' Content='M 0.1* : L 0.1*'  Width='auto' Height='auto' />
        <Button Name='btnMiddleMinCenter'  FlexCanvas.Middle='0.1*' FlexCanvas.Center='-0.1*' Content='M 0.1* : C -0.1*'  Width='auto' Height='auto' />
        <Button Name='btnMiddleCenter'  FlexCanvas.Middle='0.1*' FlexCanvas.Center='0.1*' Content='M 0.1* : C 0.1*'  Width='auto' Height='auto' />
        <Button Name='btnMiddleRight'  FlexCanvas.Middle='0.1*' FlexCanvas.Right='0.1*' Content='M 0.1* : R 0.1*'  Width='auto' Height='auto' />


        <Button Name='btnBottomLeft'  FlexCanvas.Bottom='0.1*' FlexCanvas.Left='0.1*' Content='B 0.1* : L 0.1*' Width='auto' Height='auto' />
        <Button Name='btnBottomMinCenter' FlexCanvas.Bottom='0.1*' FlexCanvas.Center='-0.1*' Content='B 0.1* : C -0.1*'  Width='auto' Height='auto' />
        <Button Name='btnBottomCenter' FlexCanvas.Bottom='0.1*' FlexCanvas.Center='0.1*' Content='B 0.1* : C 0.1*'  Width='auto' Height='auto' />
        <Button Name='btnBottomRight' FlexCanvas.Bottom='0.1*' FlexCanvas.Right='0.1*' Content='B 0.1* : R 0.1*'  Width='auto' Height='auto' />
    </FlexCanvas>") as FlexCanvas;
            panel.ShouldNotBeNull();
            if(panel==null) throw new NullReferenceException();
            this.Panel.AddChild(panel);
            panel.SetPlace(new Rect(0, 0, 1000, 1000));
            this.UpdateLayout();
            var btnTopLeft = panel.ChildByName<Button>("btnTopLeft");
            var bounds = btnTopLeft.GetBounds();
            bounds.X.ShouldBeEqual(100);
            bounds.Y.ShouldBeEqual(100);

            var btnBottomRight = panel.ChildByName<Button>("btnBottomRight");
            bounds = btnBottomRight.GetBounds();
            bounds.X.ShouldBeEqual(1000 - bounds.Width - 100);
            bounds.Y.ShouldBeEqual(1000 - 100 - bounds.Height);

            var btnTopMinCenter = panel.ChildByName<Button>("btnTopMinCenter");
            bounds = btnBottomRight.GetBounds();
            


        }



    }
}
