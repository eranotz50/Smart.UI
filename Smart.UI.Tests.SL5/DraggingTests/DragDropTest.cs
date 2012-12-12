using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;
using Smart.UI.Tests.PanelsTests;

namespace Smart.UI.Tests.DraggingTests
{
  
    [TestClass]
    public class DragDropTest :DragPanelTest
    {   
        /// <summary>
        /// Тестируем определение наиболее перекрываемого чаилда
        /// </summary>
        [TestMethod]
        public void RectangleTest()
        {
            TestPanel.Children.Add(FlexCanvases[0]);
            for (var i = 1; i < 10; i++) FlexCanvases[i].SetPanelSize(new Size(100,100));
           
            FlexCanvases[0].Children.Add(FlexCanvases[1]);
            FlexCanvases[0].Children.Add(FlexCanvases[2]);
            FlexCanvases[0].Children.Add(FlexCanvases[3]);

            FlexCanvases[1].SetValue(FlexCanvas.LeftProperty, new RelativeValue(0.0));
            FlexCanvases[1].SetValue(FlexCanvas.TopProperty, new RelativeValue(0.0));

            FlexCanvases[2].SetValue(FlexCanvas.LeftProperty, new RelativeValue(50.0));
            FlexCanvases[2].SetValue(FlexCanvas.TopProperty, new RelativeValue(50.0));

            FlexCanvases[3].SetValue(FlexCanvas.LeftProperty, new RelativeValue(100.0));
            FlexCanvases[3].SetValue(FlexCanvas.TopProperty, new RelativeValue(100.0));

            Assert.AreEqual(3, FlexCanvases[0].DragPanels.Count);

            FlexCanvases[0].InvalidateArrange();
            TestPanel.Arrange(new Rect(0, 0, 1000, 1000));
            TestPanel.UpdateLayout();

            Assert.AreEqual(new Rect(0, 0, 100, 100), FlexCanvases[1].GetBoundsTop());
            Assert.AreEqual(new Rect(50, 50, 100, 100), FlexCanvases[2].GetBoundsTop());
            Assert.AreEqual(new Rect(100, 100, 100, 100), FlexCanvases[3].GetBoundsTop());

            Assert.AreSame(FlexCanvases[1], FlexCanvases[0].FindIntersection(new Rect(21, 0, 150, 100)));
            Assert.AreSame(FlexCanvases[2], FlexCanvases[0].FindIntersection(new Rect(51, 51, 100, 100)));
            Assert.AreSame(FlexCanvases[3], FlexCanvases[0].FindIntersection(new Rect(101, 51, 100, 100)));
            Assert.AreSame(FlexCanvases[3], FlexCanvases[0].FindIntersection(new Rect(51, 100, 100, 100)));

        }

          /// <summary>
        /// Тестируем перемещение драгабельного элемента между иерархиями
        /// </summary>
        [TestMethod]
        public void FindDockTest()
        {
            MakeMatreshka();
            var dock = FlexCanvases[0].FindDock(new Rect(500, 500, 99, 99));
            FlexCanvases[5].Name.ShouldBeEqual(dock.Name);
            dock = FlexCanvases[0].FindDock(new Rect(600, 600, 99, 99));
            FlexCanvases[6].Name.ShouldBeEqual(dock.Name);
            dock = FlexCanvases[0].FindDock(new Rect(700, 700, 99, 99));
            Assert.AreEqual(FlexCanvases[7].Name, dock.Name);
            dock = FlexCanvases[0].FindDock(new Rect(800, 800, 99, 99));
            Assert.AreEqual(FlexCanvases[8].Name, dock.Name);
        }

        /// <summary>
        /// Тестируем перемещение драгабельного элемента между иерархиями
        /// </summary>     
        [TestMethod]
        public void GoUpAndDownTest()
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
            FlexCanvases[8].ChildRemoved += i => numRemoved = numRemoved+1;
            TestPanel.UpdateLayout();
            FlexCanvases[8].ChildToPanel(btn, (SimplePanel)FlexCanvases[8].GetHighestDragParent<DragPanel>());
            Assert.AreEqual(1,numAdded);
            Assert.AreEqual(1, numRemoved);
            TestPanel.UpdateLayout();
            FlexCanvases[8].GetHighestDragParent<DragPanel>().Name.ShouldBeEqual(FlexCanvases[0].Name);
            (FlexCanvases[0].Children.IndexOf(btn)>=0).ShouldBeTrue();
            SimplePanel.GetPos(btn).ShouldBeEqual(new Pos3D(new Point(810, 810)));
            FlexCanvases[0].FindDock(new Rect(810, 810, 50, 50)).Name.ShouldBeEqual(FlexCanvases[8].Name);
            FlexCanvases[0].FindDock(btn).Name.ShouldBeEqual(FlexCanvases[8].Name);
            var fly = new ObjectFly(btn) {DockMode = DockMode.DockEverywhere};
            FlexCanvases[0].DockChild(fly);
            SimplePanel.GetPos(btn).ShouldBeEqual(new Pos3D(10, 10));
            TestPanel.UpdateLayout();
        }
  
    }
}
