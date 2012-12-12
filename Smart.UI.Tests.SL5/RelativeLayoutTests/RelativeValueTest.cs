using System;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.RelativeLayoutTests
{
    [TestClass]
    public class RelativeValueTest : SilverlightTest
    {
        public FlexCanvas Canv = new FlexCanvas();
        public FlexGrid Grid = new FlexGrid();

        [TestInitialize]
        public void SetUp()
        {

        }


        [TestMethod]
        public void RealativeValueBasicTest()
        {
            var r = new RelativeValue("*");
            Assert.IsTrue(r.Stars.Equals(1));
            Assert.IsTrue(double.IsInfinity(r.Value));

            r = new RelativeValue("0.3*");
            Assert.IsTrue(r.Stars.Equals(0.3));
            Assert.IsTrue(double.IsInfinity(r.Value));

            r.StarLength = 100;
            Assert.IsTrue(r.IsStar);
            Assert.IsTrue(r.Stars.Equals(0.3));
            Assert.IsTrue(r.Value.Equals(30));
            r.Stars = 1;
            r = new RelativeValue("0.3*");
            Assert.IsTrue(r.StaredLength(100).Equals(30));
        }

        [TestMethod]
        public void RelativeValueAdvTest()
        {
            var r = new RelativeValue(500);
            var len = 1000;
            r.SetStarsAsPart(len);
            Assert.AreEqual(0.5, r.Stars);
            r.AddAbsolute(500.0);
            Assert.AreEqual(1.0, r.Stars);
        }

        [TestMethod]
        public void RelativeValueConstrains()
        {
            var r = new RelativeValue(500);
            r.ApplyConstrains(0,501);
            Assert.AreEqual(500,r.Value);
            r.ApplyConstrains(0,400);
            Assert.AreEqual(400, r.Value);
            r.ApplyConstrains(700, 800);
            Assert.AreEqual(700, r.Value);
            
            r = new RelativeValue(0.5,1000);
            r.ApplyConstrains(0, 501);
            Assert.AreEqual(500, r.Value);
            Assert.AreEqual(0.5,r.Stars);            

            r.ApplyConstrains(0, 400);
            Assert.AreEqual(400, r.Value);
            Assert.AreEqual(0.4, r.Stars);

            r.ApplyConstrains(700, 800);
            Assert.AreEqual(700, r.Value);
            Assert.AreEqual(0.7, r.Stars);

            r = new RelativeValue(0.5, 1000);
            
            r.ApplyConstrains(0, 400, false);
            Assert.AreEqual(400, r.Value);
            Assert.AreEqual(0.5, r.Stars);
            Assert.AreEqual(800, r.StarLength);  
        }

        [TestMethod]
        public void RelativeValueOperatorsTest()
        {
            var a = new RelativeValue("0.6*");
            var b = new RelativeValue("0.2*");
            var c = a + b;
            Assert.AreEqual(0.8, c.Stars);
            a = new RelativeValue(500);
            b = new RelativeValue(700);
            c = a + b;
            Assert.AreEqual(1200, c.Value);
            a = new RelativeValue(0.5, 1000);
            b = new RelativeValue(500);
            c = a + b;
            Assert.AreEqual(1000, c.Value);
            Assert.AreEqual(1.0, c.Stars);
        }

        [TestMethod]
        public void RelativeConverterTest()
        {
            var conv = new RelativeValueConverter();
            var r = conv.ConvertFrom("0.3*") as RelativeValue;            
            Assert.IsTrue(r.IsStar);            
            Assert.AreEqual(r.Stars, 0.3);
            r.StarLength = 100;
            Assert.AreEqual(r.Value, 30);
            var str = conv.ConvertTo(null, null, r, typeof (String));
            Assert.AreEqual(str,"0.3*");
            r = conv.ConvertFrom("900") as RelativeValue;
            Assert.AreEqual(r.Value, 900);
            Assert.IsFalse(r.IsStar);
            str = conv.ConvertTo(null, null, r, typeof(string));
            Assert.AreEqual(str,"900");   
        }   

        [TestMethod]
        public void InfinityTest()
        {
            var r = new RelativeValue(double.PositiveInfinity);
            r.AbsoluteValue.ShouldBeEqual(double.PositiveInfinity);
            r.AbsoluteValue += 10;
            r.AbsoluteValue.ShouldBeEqual(double.PositiveInfinity);
            r.AbsoluteValue -= 10;
            r.AbsoluteValue.ShouldBeEqual(double.PositiveInfinity);            
        }


    }
}