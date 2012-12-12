using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Subjects;
using Smart.UI.Classes.Animations;

namespace Smart.UI.Tests.TestBases
{
    [TestClass]
    public class AnimationTestBase:SilverlightTest
    {

        public Animation Ani;
        public double Start;
        public double Q;
        public double Sum;
        public SimpleSubject<double> Source;

        [TestInitialize]
        public virtual void SetUp()
        {
            Source = new SimpleSubject<double>();
            Ani = new Animation(10.0);
            Start = 0.0;
            Animator.TestInit();
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            Start = 0.0;
            Source = null;
            Ani = null;
        }

    }
}
