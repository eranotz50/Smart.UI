using System;
using System.Net;
using System.Windows;

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.PanelsTests
{
    [TestClass]
    public class Pos3DTest:SilverlightTest
    {

        [TestMethod]
        public void TestDoubleInfinity()
        {
            var d = 0.0;
            d.IsValid().ShouldBeTrue();
            d = double.NaN;
            d.IsValid().ShouldBeFalse();
            d = 1;
            d.IsValid().ShouldBeTrue();
            d = double.NegativeInfinity;
            d.IsValid().ShouldBeFalse();
        }

        [TestMethod]
        public void TestPointToInfinity()
        {
            var p = new Point();
            p.X.ShouldBeEqual(0.0);
            p.HasInfinity().ShouldBeFalse();
            p.Y.ShouldBeEqual(0.0);
            p = p.GetInfinity();
            p.X.ShouldBeEqual(double.PositiveInfinity);
            p.Y.ShouldBeEqual(double.PositiveInfinity);
            p.HasInfinity().ShouldBeTrue();
            p.X = 0;
            p.HasInfinity().ShouldBeTrue();
            p.Y = 0;
            p.HasInfinity().ShouldBeFalse();
            p = default(Point);
            p.HasInfinity().ShouldBeFalse();
        }

        [TestMethod]
        public void TestPos3D()
        {
            var p = new Pos3D();
            p.HasZ.ShouldBeFalse();
            p.X.ShouldBeEqual(0.0);
            p.NotEmpty.ShouldBeFalse();
            p.Y = 10;
            p.NotEmpty.ShouldBeTrue();
            p = new Pos3D(new Point(10, 20));
            p.NotEmpty.ShouldBeTrue();
            p.HasZ.ShouldBeFalse();
            p.HasInfinity().ShouldBeTrue();
            p.X.ShouldBeEqual(10.0);
            p.Y.ShouldBeEqual(20.0);
        }

        [TestMethod]
        public void CenterCoords()
        {
            var p = new Pos3D(110.0,220.0);
            var size = new Size(100, 200);
            p.Centered.ShouldBeFalse();
            p.ToPointWithSize(size).ShouldBeEqual(p.ToPoint());
            p.Centered = true;
            p.ToPointWithSize(size).ShouldBeEqual(new Point(60.0,120.0));
            p = new Pos3D(110.0, 220.0,0.0,true);
            p.ToPointWithSize(size).ShouldBeEqual(new Point(60.0, 120.0));            
        }

        [TestMethod]
        public void TestEqual()
        {
            var p1 = new Pos3D(5, 10, 15, true);
            var p2 = new Pos3D(5, 10, 15, true);
            p1.Equals(p2).ShouldBeTrue();
            p1.Centered = false;
            p1.Equals(p2).ShouldBeFalse();
            p2.Centered = false;
            p1.Equals(p2).ShouldBeTrue();
            p1.Z = 10;
            p1.Equals(p2).ShouldBeFalse();
        }

        [TestMethod]
        public void TestPos3DConverterAllParams()
        {
            var conv = new Pos3DConverter();
            var p = (Pos3D)conv.ConvertFrom(null, null, "0,45.5,68.6");
            p.X.ShouldBeEqual(0.0);
            p.Y.ShouldBeEqual(45.5);
            p.Z.ShouldBeEqual(68.6);
            p.NotEmpty.ShouldBeTrue();
            p.HasZ.ShouldBeTrue();
            p.HasXY().ShouldBeTrue();
            p.HasXYZ().ShouldBeTrue();
            p.ToPoint().ShouldBeEqual(new Point(0.0, 45.5));
            p.ToPointWithSize(new Size(200, 200)).ShouldBeEqual(new Point(0.0,45.5));
            p = (Pos3D)conv.ConvertFrom(null, null, "0,45.5,68.6,true");
            p.X.ShouldBeEqual(0.0);
            p.Y.ShouldBeEqual(45.5);
            p.Z.ShouldBeEqual(68.6);
            p.ToPoint().ShouldBeEqual(new Point(0.0, 45.5));            
            p.ToPointWithSize(new Size(200, 200)).ShouldBeEqual(new Point(-100.0, -54.5));
        }

        [TestMethod]
        public void TestPos3DConverterPartOfParams()
        {
            var conv = new Pos3DConverter();
            var p = (Pos3D)conv.ConvertFrom(null, null, "1,2");
            p.X.ShouldBeEqual(1.0);
            p.Y.ShouldBeEqual(2.0);
            p = (Pos3D)conv.ConvertFrom(null, null, "1;2");
            p.X.ShouldBeEqual(1.0);
            p.Y.ShouldBeEqual(2.0);
            p.HasXY().ShouldBeTrue();
            p.HasXYZ().ShouldBeFalse();
            p.NotEmpty.ShouldBeTrue();
            p.HasZ.ShouldBeFalse();
            p = (Pos3D)conv.ConvertFrom(null, null, "10");
            p.X.ShouldBeEqual(10.0);
            p.Y.ShouldBeEqual(double.PositiveInfinity);
            p.HasXY().ShouldBeFalse();
            p.IsValid().ShouldBeFalse();
            p.X.ShouldBeEqual(10.0);
            p.HasZ.ShouldBeFalse();
            p.HasXY().ShouldBeFalse();
            p.NotEmpty.ShouldBeFalse();
            p.HasXYZ().ShouldBeFalse();
        }

        [TestMethod]
        public void PlusOverrideTest()
        {
            var p1 = new Pos3D(1, 1, 1);
            var p2 = new Pos3D(2, 2, 2);
            
            var res = p1 + 1;
            res.ShouldBeEqual(new Pos3D(2,2,2));
            res = p1 + p2;
            res.ShouldBeEqual(new Pos3D(3,3,3));
            p1.HasZ = false;
            res = p1 + p2;
            var expected = new Pos3D(3, 3, 1) {HasZ = false};
            //res.ShouldBeEqual(expected);
            res.Equals(expected).ShouldBeTrue();            
        }

        [TestMethod]
        public void MinusOverrideTest()
        {
            var p1 = new Pos3D(1, 1, 1);
            var p2 = new Pos3D(2, 2, 2);

            var res = p1 - 1;
            p1.ShouldBeEqual(new Pos3D(1, 1, 1));
            res.ShouldBeEqual(new Pos3D(0, 0, 0));
            res = p1 - p2;
            p1.ShouldBeEqual(new Pos3D(1, 1, 1));
            res.ShouldBeEqual(new Pos3D(-1, -1, -1));
            p1.HasZ = false;
            res = p1 - p2;
            var expected = new Pos3D(-1, -1, 1) { HasZ = false };
            res.Equals(expected).ShouldBeTrue();
            //res.ShouldBeEqual(expected);
        }
        [TestMethod]
        public void MultipleOverrideTest()
        {
            var p1 = new Pos3D(1, 1, 1);
            var p2 = new Pos3D(2, 2, 2);

            var res = p1 * 4;
            res.ShouldBeEqual(new Pos3D(4, 4, 4));
            res = p1 * p2;
            res.ShouldBeEqual(new Pos3D(2, 2, 2));
            p1.HasZ = false;
            res = p1 * p2;
            var expected = new Pos3D(2, 2, 1) { HasZ = false };
            //res.ShouldBeEqual(expected);
            res.Equals(expected).ShouldBeTrue();            
        }
        /// <summary>
        /// Потом доделать деление
        /// </summary>
        [TestMethod]
        public void DivideOverrideTest()
        {
            var p1 = new Pos3D(1, 1, 1);
            var p2 = new Pos3D(2, 2, 2);

            var res = p1 / 2;
            res.ShouldBeEqual(new Pos3D(0.5, 0.5, 0.5));
            res = p1 / p2;
            res.ShouldBeEqual(new Pos3D(0.5, 0.5, 0.5));
            p1.HasZ = false;
            res = p1 / p2;
            var expected = new Pos3D(0.5, 0.5, 1) { HasZ = false };
            res.Equals(expected).ShouldBeTrue();
            //res.ShouldBeEqual(expected);
            p1.HasZ = true;
            /*
            res = p1/0;
            //res.ShouldBeEqual(new Pos3D(1,1,1));
            res.Equals(new Pos3D(1, 1, 1)).ShouldBeTrue();
             */
            
        }

        [TestMethod]
        public void TestDirections()
        {
            var p1 = new Pos3D(1, 1, 1);
            var p2 = new Pos3D(3, 3, 3);           
            p1.DirectionTo(p1).ShouldBeEqual(new Pos3D(0,0,0));
            //var p = p1.DirectionTo(p2);
            //(p1.DirectionTo(p2) * p2.Length).Round().ShouldBeEqual(p2);
            (p1.DirectionTo(p2) * p2.Length).Round().Equals(p2).ShouldBeTrue();
            
        }

    }
}
