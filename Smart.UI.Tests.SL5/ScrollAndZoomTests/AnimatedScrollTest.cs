using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Controls.Scrollers;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.ScrollAndZoomTests
{
    [TestClass]
    public class AnimatedScrollTest : AnimatedTestBase<FlexCanvas>
    {
        public HorizontalScrollBar Scroller;

        public override void SetUp()
        {
            base.SetUp();
            this.Scroller = new HorizontalScrollBar();
        }

        public override void CleanUp()
        {
            base.CleanUp();
        }

        [TestMethod]
        public void AnimationTest()
        {
            var howLong = new TimeSpan(0, 0, 0, 10);
            this.Ani = new Animation(howLong,AniHandler);
            //this.Scroller.UpdatePosition+=
        }

        protected void AniHandler(double val)
        {
        }

        protected void PositionUpdateTest()
        {
            //this.Ani.HowLong
        }
    }


}
