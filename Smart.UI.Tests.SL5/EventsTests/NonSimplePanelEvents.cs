using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Events;
using Smart.UI.Panels;
using Smart.UI.Classes.Events;
using Smart.TestExtensions;
using Smart.UI.Classes.Events;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.EventsTests
{
    [TestClass]
    public class NonSimplePanelEvents:PanelTestBase<FlexCanvas>
    {
        public Canvas[] Canvases;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Canvases = new Canvas[9];
            for (var i = 0; i < 9; i++)
            {
                Canvases[i] = new Canvas() {Width = 1000 - i*100, Height = 1000 - i*100};
                if (i > 0) this.Canvases[i-1].Children.Add(Canvases[i]);                
            }
           this.Panel.AddChild(this.Canvases[0]);
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            this.Canvases = null;
        }

       
        [TestMethod]
        public void Raising()
        {
         
            //this.Canvases[0].GetBounds().Width.ShouldBeEqual(1000);
            //this.Canvases[9].GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.Panel.AddEventHandler<double>("Move", i => Canvas.SetLeft(this.Panel, Canvas.GetLeft(this.Panel) + i));            
            this.Canvases[1].AsEventsHolder().AddEventHandler<double>("Move", i=>Canvas.SetLeft(this.Canvases[1],Canvas.GetLeft(this.Canvases[1])+i));
            this.Canvases[3].AsEventsHolder().AddEventHandler<double>("Move", i => Canvas.SetLeft(this.Canvases[3], Canvas.GetLeft(this.Canvases[3]) + i));
            this.Canvases[6].AsEventsHolder().AddEventHandler<double>("Move", i => Canvas.SetLeft(this.Canvases[6], Canvas.GetLeft(this.Canvases[6]) + i));
            
            Canvas.GetLeft(this.Panel).ShouldBeEqual(0);                      
            Canvas.GetLeft(this.Canvases[0]).ShouldBeEqual(0);          
            Canvas.GetLeft(this.Canvases[1]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[2]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[3]).ShouldBeEqual(0);

            this.Canvases[8].AsEventsHolder().RaiseEvent("Move",10.0);

            var h = this.Canvases[8].GetEventsRaiser();
            h.ShouldNotBeEqual(default(IEventsHolder));
            h.HasEvent<double>("Move").ShouldBeFalse();
            
            h = this.Canvases[7].GetEventsRaiser();
            h.ShouldBeEqual(default(IEventsHolder));
            
            h = this.Canvases[6].GetEventsRaiser();
            h.ShouldNotBeEqual(default(IEventsHolder));
            h.HasEvent<double>("Move").ShouldBeTrue();

            h = this.Canvases[5].GetEventsRaiser();
            h.ShouldBeEqual(default(IEventsHolder));

            h = this.Canvases[4].GetEventsRaiser();
            h.ShouldBeEqual(default(IEventsHolder));

            h = this.Canvases[3].GetEventsRaiser();
            h.ShouldNotBeEqual(default(IEventsHolder));
            h.HasEvent<double>("Move").ShouldBeTrue();

            
            Canvas.GetLeft(this.Canvases[8]).ShouldBeEqual(0);           
            Canvas.GetLeft(this.Canvases[7]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[6]).ShouldBeEqual(10);
            Canvas.GetLeft(this.Canvases[5]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[4]).ShouldBeEqual(0);                        
            Canvas.GetLeft(this.Canvases[3]).ShouldBeEqual(10);
            Canvas.GetLeft(this.Canvases[2]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[1]).ShouldBeEqual(10);
            Canvas.GetLeft(this.Canvases[0]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Panel).ShouldBeEqual(10);
            
            
        }

        [TestMethod]
        public void RaisingRouted()
        {

            //this.Canvases[0].GetBounds().Width.ShouldBeEqual(1000);
            //this.Canvases[9].GetBounds().ShouldBeEqual(new Rect(0, 0, 100, 100));
            this.Panel.AddEventHandler<double>("Move", i => Canvas.SetLeft(this.Panel, Canvas.GetLeft(this.Panel) + i));
            this.Canvases[1].AsEventsHolder().AddEventHandler<TestHandledEvent<double>>("Move", i => Canvas.SetLeft(this.Canvases[1], Canvas.GetLeft(this.Canvases[1]) + i.Param));
            this.Canvases[3].AsEventsHolder().AddEventHandler<TestHandledEvent<double>>("Move", i =>
                                                                                                    {
                                                                                                        Canvas.SetLeft(
                                                                                                            this.
                                                                                                                Canvases
                                                                                                                [3],
                                                                                                            Canvas.
                                                                                                                GetLeft(
                                                                                                                    this
                                                                                                                        .
                                                                                                                        Canvases
                                                                                                                        [
                                                                                                                            3
                                                                                                                        ]) +
                                                                                                            i.Param);
                                                                                                        i.Handled = true;

                                                                                                    });
            this.Canvases[6].AsEventsHolder().AddEventHandler<TestHandledEvent<double>>("Move", i => Canvas.SetLeft(this.Canvases[6], Canvas.GetLeft(this.Canvases[6]) + i.Param));

            Canvas.GetLeft(this.Panel).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[0]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[1]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[2]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[3]).ShouldBeEqual(0);

            this.Canvases[8].AsEventsHolder().RaiseHandledEvent(new TestHandledEvent<double>("Move",10));

            var h = this.Canvases[8].GetEventsRaiser();
            h.ShouldNotBeEqual(default(IEventsHolder));
            h.HasEvent<TestHandledEvent<double>>("Move").ShouldBeFalse();

            h = this.Canvases[7].GetEventsRaiser();
            h.ShouldBeEqual(default(IEventsHolder));

            h = this.Canvases[6].GetEventsRaiser();
            h.ShouldNotBeEqual(default(IEventsHolder));
            h.HasEvent<TestHandledEvent<double>>("Move").ShouldBeTrue();

            h = this.Canvases[5].GetEventsRaiser();
            h.ShouldBeEqual(default(IEventsHolder));

            h = this.Canvases[4].GetEventsRaiser();
            h.ShouldBeEqual(default(IEventsHolder));

            h = this.Canvases[3].GetEventsRaiser();
            h.ShouldNotBeEqual(default(IEventsHolder));
            h.HasEvent<TestHandledEvent<double>>("Move").ShouldBeTrue();


            Canvas.GetLeft(this.Canvases[8]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[7]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[6]).ShouldBeEqual(10);
            Canvas.GetLeft(this.Canvases[5]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[4]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[3]).ShouldBeEqual(10);
            Canvas.GetLeft(this.Canvases[2]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[1]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Canvases[0]).ShouldBeEqual(0);
            Canvas.GetLeft(this.Panel).ShouldBeEqual(0);


        }

    }
}
