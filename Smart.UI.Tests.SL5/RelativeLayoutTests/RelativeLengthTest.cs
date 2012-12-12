using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.TestExtensions;
using Smart.UI.Classes.Layout;


namespace Smart.UI.Tests.RelativeLayoutTests
{
    [TestClass]
    public class RelativeLengthTest:SilverlightTest
    {
        public RelativeLength Rel;
        public string Str;

        

        #region OLD

       

        [TestMethod]
        public void ConverterMinEqual()
        {
            this.Str = "45;<=10";
            this.Rel = new RelativeLength(Str);
          //  Rel.Value.ShouldBeEqual(45);
            Rel.MaxLength.ShouldBeEqual(10);
            Rel.MinLength.ShouldBeEqual(0.0);
            Rel.ApplyConstrains();
            Rel.Value.ShouldBeEqual(10);

        }

        [TestMethod]
        public void ConverterMaxEqual()
        {
            this.Str = "45;>=20";
            this.Rel = new RelativeLength(Str);
            //Rel.Value.ShouldBeEqual(45);
            Rel.MinLength.ShouldBeEqual(20);
            /*Rel.ApplyConstrains()*/Rel.Value.ShouldBeEqual(45);
        }

        [TestMethod]
        public void ConverterMinMaxSameEqual()
        {
            this.Str = "45;<=10;>=20";
            this.Rel = new RelativeLength(Str);
         //   Rel.Value.ShouldBeEqual(45);
            Rel.MinLength.ShouldBeEqual(20);
            Rel.MaxLength.ShouldBeEqual(20);
            /*Rel.ApplyConstrains()*/Rel.Value.ShouldBeEqual(20);
        }

        [TestMethod]
        public void ConverterMinMaxEqual()
        {
            this.Str = "45;<=30;>=5";
            this.Rel = new RelativeLength(Str);
           // Rel.Value.ShouldBeEqual(45);
            Rel.MinLength.ShouldBeEqual(5);
            Rel.MaxLength.ShouldBeEqual(30);
            /*Rel.ApplyConstrains()*/Rel.Value.ShouldBeEqual(30);
        }

        [TestMethod]
        public void ConverterMinMaxEqualOtherSide()
        {
            this.Str = "45;>=5;<=30";
            this.Rel = new RelativeLength(Str);
           // Rel.Value.ShouldBeEqual(45);
            Rel.MinLength.ShouldBeEqual(5);
            Rel.MaxLength.ShouldBeEqual(30);
            /*Rel.ApplyConstrains()*/Rel.Value.ShouldBeEqual(30);
        }
        

        #endregion
        [TestMethod]
        public void RealativeLengthBasicTest()
        {
            Rel = new RelativeLength("*");
            Assert.IsTrue(Rel.Stars.Equals(1));
            Assert.IsTrue(double.IsInfinity(Rel.Value));

            Rel = new RelativeLength("0.3*");
            Assert.IsTrue(Rel.Stars.Equals(0.3));
            Assert.IsTrue(double.IsInfinity(Rel.Value));

            Rel.StarLength = 100;
            Assert.IsTrue(Rel.IsStar);
            Assert.IsTrue(Rel.Stars.Equals(0.3));
            Assert.IsTrue(Rel.Value.Equals(30));
            Rel.Stars = 1;
            Rel = new RelativeLength("0.3*");
            Assert.IsTrue(Rel.StaredLength(100).Equals(30));

            Rel = new RelativeLength("-0.3*");
            Assert.IsTrue(Rel.Stars.Equals(-0.3));
            Assert.IsTrue(double.IsInfinity(Rel.Value));
        }
    }
}
