using Module.Battle.Com;
using Spine.Unity;
using System;
using UnityEngine;

namespace Module
{
    /// <summary>
    /// Date    2021/2/14 14:58:22
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class AnimatorCom : BaseAnimatorCom
    {
        private SkeletonAnimation SkeletonAnimation
        {
            get
            {
                if(m_skeletonAnimation == null)
                {
                    m_skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();
                }

                return m_skeletonAnimation;
            }
        }

        private SkeletonAnimation m_skeletonAnimation;

        public override void RegisterEvent(string eventName, Action call)
        {

        }

        public override void SetAnimation(string animation)
        {
            SkeletonAnimation.state.SetAnimation(0, animation, true);
        }

    }
}