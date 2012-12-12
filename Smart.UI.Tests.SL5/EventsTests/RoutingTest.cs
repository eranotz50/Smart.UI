using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.UI.Classes.Extensions;
using Smart.TestExtensions;
using Smart.UI.Classes.Events;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.EventsTests
{
    [TestClass]
    public class RoutingTest:PanelTestBase<FlexCanvas>
    {
        public SmartCollection2D<SimplePanel> Panels;
        public int XCount = 3;
        public int YCount = 3;
        public int Q;
       
       
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Q = 0;
            this.Panels = new SmartCollection2D<SimplePanel>(3,3);
            this.Panels.Count.ShouldBeEqual(this.XCount);
            this.Panels[XCount - 1].Count.ShouldBeEqual(this.YCount);
            for (var i = 0; i < Panels.Count; i++)
            {
                var pC = Panels[i];
                for (var j = 0; j < pC.Count; j++)
                {
                    var pR = pC[j];
                    if (j == 0) this.Panel.AddChild(pR); else pC[j-1].AddChild(pR);                    
                }
            }
            this.TestPanel.UpdateLayout();
        }

        [TestMethod]
        public void SimpleRouterTest()
        {
            const string ev = "ev";
            for (var i = 0; i < Panels.Count; i++)
            {
                var pC = Panels[i];
                for (var j = 0; j < pC.Count; j++)
                {
                    var pR = pC[j];
                    pR.SmartEventManager.AddEventHandler<int>("ev", v => this.Q+=v);
                }
            }
            this.Panels.Last().Last().RaiseEvent(ev,1);
            this.Q.ShouldBeEqual(3);
        }

       
       

        [TestCleanup]
        public override void CleanUp()
        {
            this.Panels = null;
            this.Panel = null;
        }
    }
}
