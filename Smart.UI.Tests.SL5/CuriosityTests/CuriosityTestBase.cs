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

namespace Smart.UI.Tests.CuriosityTests
{
    [TestClass]
    public class CuriosityTestBase:SilverlightTest
    {
        
        public TimeSpan Measure(Action test, String text)
        {

            var before = DateTime.Now.Ticks;
            var q = 100;
            for (int i = 0; i < q; i++) test();
            var after = DateTime.Now.Ticks;

            var elapsedTime = new TimeSpan(after - before);
            // MessageBox.Show(string.Format("Task took {0} milliseconds",elapsedTime.TotalMilliseconds));
            return elapsedTime;
        }
    }
}
