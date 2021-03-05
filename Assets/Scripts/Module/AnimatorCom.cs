using Module.Battle.Com;
using Spine;
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
    public class AnimatorCom : MonoBehaviour
    {
        public SkeletonAnimation SkeletonAnimation
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


        public void SetAnimation(string animation, bool loop = true)
        {
            SkeletonAnimation.state.SetAnimation(0, animation, loop);
        }


        public string GetCurAnimationName()
        {
            return SkeletonAnimation.AnimationName;
        }

        public void SetAnimation(string animation, params string[] appendAnimations)
        {
            SkeletonAnimation.state.SetAnimation(0, animation, false);
            for (int i = 0; i < appendAnimations.Length; i++)
            {
                SkeletonAnimation.state.AddAnimation(0, appendAnimations[i], false, 0);
            }
        }

        public void SetAnimationAndLoopOnEnd(string animation, params string[] appendAnimations)
        {
            SkeletonAnimation.state.SetAnimation(0, animation, false);
            for (int i = 0; i < appendAnimations.Length; i++)
            {
                SkeletonAnimation.state.AddAnimation(0, appendAnimations[i], i == appendAnimations.Length - 1, 0);
            }
        }
    }
}