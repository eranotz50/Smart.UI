using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Events;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Events;
using Smart.Classes.Subjects;
using Smart.TestExtensions;

namespace Smart.UI.Tests.DictionariesTests
{
    [TestClass]
    public class EventManagerTest
    {        
        public SmartEventManager EventManager;
        public String Str;
        public int Q;

        [TestInitialize]
        public void SetUp()
        {
            this.EventManager = new SmartEventManager();
            this.Str = "";
            this.Q = 0;
            Animator.TestInit();
        }

        protected void StrPlusHandler(String pl)
        {
            this.Str += pl;
        }

        [TestMethod]
        public void AddRemoveHandlerTests()
        {
            const string ev = "e1";
            this.EventManager.AddEventHandler<String>(ev, this.StrPlusHandler).OnNext("1");
            this.Str.ShouldBeEqual("1");
            this.EventManager.RunEvent(ev,"2");
            this.Str.ShouldBeEqual("12");
            this.EventManager.AddObserver(ev,new SimpleSubject<string>(i=>Str+="|"+i)).OnNext("3");
            this.Str.ShouldBeEqual("123|3");
            this.EventManager.RunEvent(ev, "4");
            this.Str.ShouldBeEqual("123|34|4");
            this.EventManager.RemoveHandler<String>(ev,StrPlusHandler);
            this.EventManager.RunEvent(ev, "5");
            this.Str.ShouldBeEqual("123|34|4|5");            
        }

        [TestMethod]
        public void AddContinuesHandlerTests()
        {
            const string ev = "e1";
            var k = 0;            
            var sub = this.EventManager.AddEventCallback<String,double>(ev, this.CallBack);
            sub.DoOnCompleted = () => k = 100;            
            sub.OnNext("1");
            this.Str.ShouldBeEqual("1");
            this.EventManager.RunEvent(ev, "2");
            this.Str.ShouldBeEqual("12");
            k.ShouldBeEqual(0);
            Animator.PlusOneSecond();
            k.ShouldBeEqual(0);            
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            Animator.PlusOneSecond();
            k.ShouldBeEqual(100);            
        }

        protected SimpleSubject<double> CallBack(String args)
        {
            this.StrPlusHandler(args);
            return new Animation(new TimeSpan(0,0,0,3)).Go();
        }



        [TestCleanup]
        public void CleanUp()
        {
            this.EventManager = null;
        }
    }
}
