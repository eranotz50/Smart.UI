using System.Windows;
using System.Windows.Controls;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;


namespace Smart.UI.Tests.PanelsTests
{
    public class OutModeTest : TestBases.PanelTestBase<FlexCanvas>
    {
        public Border Bord;

        public override void SetUp()
        {
            base.SetUp();
            this.Bord = new Border();
        }

        public void PanelOutModeTest()
        {
            this.Panel.CanvasWidth = 2000;
            this.Panel.CanvasHeight = 2000;
            this.Panel.AddChild(Bord);
            Bord.SetPlace(100, 100, 200, 200);
            this.UpdateLayout();
            Bord.GetBounds().ShouldBeEqual(new Rect(100, 100, 200, 200));
            Bord.SetPlace(1200, 1200, 100, 100);
            this.UpdateLayout();
            Bord.GetBounds().ShouldBeEqual(new Rect(1200, 1200, 200, 200));
            this.Panel.OutMode = OutMode.Cut;
            


        }

    }
}
