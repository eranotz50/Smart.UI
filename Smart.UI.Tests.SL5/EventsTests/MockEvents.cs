using System;
using Smart.Classes.Interfaces;
using Smart.UI.Classes.Events;

namespace Smart.UI.Tests.EventsTests
{
    public class TestHandledEvent<T>:HandledEvent
    {
        public T Param;

        public TestHandledEvent(String name):base(name)
        {
        }

        public TestHandledEvent(String name, T param)
            : base(name)
        {
            this.Param = param;
        }
      
    }

    public class TestNamedEvent : INamed
    {
        public TestNamedEvent(String name)
        {
            this.Name = name;
        }

        #region Implementation of INamed

        public string Name { get; set; }

        #endregion
    }
}
