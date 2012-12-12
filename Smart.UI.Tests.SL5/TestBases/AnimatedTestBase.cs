using System;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;


namespace Smart.UI.Tests.TestBases
{
    
    public class AnimatedTestBase<T>:PanelTestBase<T> where T:FlexCanvas, new()
    {
        public Animation Ani;
       

        public override void SetUp()
        {
            base.SetUp();
            Animator.TestInit();
            Animator.EachFrame.OnNext(new TimeSpan(0, 0, 0, 0));  
        }

    }
}
