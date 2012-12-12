using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;


using Smart.UI.Tests.PathTests;

namespace Smart.UI.Tests.TestBases
{
   
    public class PanelTestBase<T>:SilverlightTest,IGetterUp  where T:FlexCanvas, new()
    {
        public T Panel;

        [TestInitialize]
        public virtual void SetUp()
        {
            this.Panel = new T {Width = 1000, Height = 1000};
            TestPanel.Children.Add(Panel);
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            TestPanel.Children.Remove(this.Panel);
            this.Panel = null;
        }

        public ObjectFly GetUp(FrameworkElement element)
        {
            return this.Panel.DragManager.StartFlight(this.Panel.DragManager.BuildDragObject(element), e => element.GetBounds().TopLeft());
        }

        public void UpdateLayout()
        {
            this.TestPanel.UpdateLayout();
        }

    }
}
