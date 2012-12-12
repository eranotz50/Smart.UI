using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.PanelsTests
{
   
    public class DragPanelTest : SimplePanelTest
    {

        [TestMethod]
        public void HierarchyTest()
        {
            TestPanel.Children.Add(Canvases[0]);
            Canvases[0].Children.Add(FlexCanvases[1]);
            Assert.AreSame(FlexCanvases[1], FlexCanvases[1].GetHighestDragParent<DragPanel>());
            FlexCanvases[1].GetHighestDragParent<DragPanel>().ShouldBeSame(FlexCanvases[1]);
            FlexCanvases[1].GetNearestDragParent<DragPanel>().ShouldBeSame(null);

      //      FlexCanvases[1].TopDragPanel.ShouldBeSame(FlexCanvases[1]);
            
            FlexCanvases[1].Children.Add(FlexCanvases[2]);
            FlexCanvases[1].Children.Add(FlexGrids[2]);
            FlexCanvases[1].Children.Add(Canvases[2]);
            FlexCanvases[2].GetNearestDragParent<DragPanel>().ShouldBeSame(FlexCanvases[1]);
            
            TestPanel.UpdateLayout();

       //     Assert.AreEqual(FlexCanvases[1].Name, FlexCanvases[2].TopDragPanel.Name);

            Assert.AreSame(FlexCanvases[1], FlexCanvases[2].GetHighestDragParent<DragPanel>());
            Assert.AreSame(FlexCanvases[1], FlexGrids[2].GetHighestDragParent<DragPanel>());

            Canvases[0].Children.Remove(FlexCanvases[1]);
            FlexGrids[9].Children.Add(FlexCanvases[1]);

            FlexGrids[2].Children.Add(FlexGrids[8]);
            FlexGrids[8].GetNearestDragParent<DragPanel>().ShouldBeSame(FlexGrids[2]);
            Assert.AreSame(FlexGrids[9], FlexGrids[8].GetHighestDragParent<DragPanel>());
    //        Assert.AreEqual(FlexGrids[9].Name, FlexGrids[8].TopDragPanel.Name);


            FlexGrids[6].Children.Add(FlexGrids[7]);
            Assert.AreSame(FlexGrids[6], FlexGrids[7].GetHighestDragParent<DragPanel>());

            TestPanel.UpdateLayout();
        }

        [TestMethod]
        public void UserControlPanelTest()
        {
            this.FlexCanvases[0].AddChild(this.Canvases[1]);
            this.FlexCanvases[0].AddChild(this.FlexCanvases[1]);
            this.FlexCanvases[0].AddChild(this.FlexGrids[1]);
            this.Canvases[1].Children.Add(this.FlexCanvases[2]);
            this.Canvases[1].Children.Add(this.Canvases[2]);
            this.Canvases[1].Children.Add(this.FlexGrids[2]);
            this.FlexGrids[2].AddChild(this.Canvases[3]);
            this.FlexGrids[2].AddChild(this.Controls[3]);
            this.FlexGrids[2].AddChild(this.Canvases[3]);
            this.Controls[3].Content = this.Canvases[4];
            this.Canvases[4].Children.Add(this.FlexGrids[5]);
            this.FlexGrids[5].AddChild(this.FlexGrids[6]);
            this.FlexGrids[6].AddChild(this.FlexGrids[7]);
            this.FlexGrids[7].SetDragCanvas("control");
            this.FlexGrids[7].GetNearestControlParent<DragPanel>().ShouldBeEqual(this.FlexGrids[2]);
            this.FlexGrids[7].GetParent<FrameworkElement>(s => s.Name.Equals("Control_3")).ShouldBeEqual(this.Controls[3]);
            this.FlexGrids[7].SelectFromParent<FrameworkElement, String>(s => s.Name.Equals("Control_3"), c => c.Name).ShouldBeEqual(this.Controls[3].Name);

            this.FlexGrids[7].GetNearestDragParent().ShouldBeEqual(this.FlexGrids[6]);
            this.FlexGrids[7].GetHighestDragParent<DragPanel>().ShouldBeEqual(this.FlexCanvases[0]);
            DragManager.GetDragCanvas(this.FlexGrids[7]).Execute(this.FlexGrids[7]).ShouldBeEqual(this.FlexGrids[2]);
        }



        [TestMethod]
        public void PanelAditionTest()
        {         
            FlexCanvases[0].Children.Add(new Button());
            FlexCanvases[0].Children.Add(Canvases[0]);
            FlexCanvases[0].Children.Add(Canvases[1]);
            FlexCanvases[0].Children.Add(FlexCanvases[9]);
            FlexCanvases[0].Children.Add(FlexGrids[0]);
            FlexCanvases[0].Children.Add(FlexGrids[1]);
            Assert.AreEqual(3, FlexCanvases[0].DragPanels.Count);
            Assert.AreEqual(6, FlexCanvases[0].Children.Count);
        }

        [TestMethod]
        public void AddChildEventTest()
        {
            MakeMatreshka();  
            var btn = new Button();
            SimplePanel.SetPos(btn, new Pos3D(10, 10));
            FlexCanvases[8].Children.Add(btn);
            TestPanel.UpdateLayout();
            var numAdded = 0;
            var numRemoved = 0;
            //проверяем добавление и удаление чилдрена            
            FlexCanvases[8].GetHighestDragParent<DragPanel>().ChildAdded += i => numAdded = numAdded + 1;
            FlexCanvases[8].ChildRemoved += i => numRemoved = numRemoved + 1;
            FlexCanvases[8].Children.Remove(btn);
            FlexCanvases[8].GetHighestDragParent<DragPanel>().Children.Add(btn);
            Assert.AreEqual(1, numAdded);
            Assert.AreEqual(1, numRemoved);
        }

    }
}
