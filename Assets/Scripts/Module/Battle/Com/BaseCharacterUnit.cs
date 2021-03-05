using System;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/2/14 15:13:08
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public partial class BaseCharacterUnit : BaseUnit
    {
        public const string KEY_POWER = "Power";
        public override bool IsEnemy => false;

        protected NumberCom m_power;
        protected BaseUnit m_target;

        public virtual void Initialize(Vector2Int point, UnitDir dir)
        {
            Initialize();

            CurrentPoint = point;
            CurrentDir = dir;
        }

        protected override void Initialize()
        {

            m_stateMachine.AddState(UnitStateEnum.BATTLE, new BaseBattleStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.IDLE, new BaseIdleStateRunner(m_stateMachine))
              .SetPrimaryState(UnitStateEnum.IDLE);

            m_stateMachine.RegisterStateRunner(UnitStateEnum.BATTLE, OnBattleStateUpdateHandle);
            m_stateMachine.RegisterStateRunner(UnitStateEnum.IDLE, OnIdleStateUpdateHandle);
        }

        protected override void StateMachineOnChangeState(UnitStateEnum pre, UnitStateEnum curr)
        {
            switch (curr)
            {
                case UnitStateEnum.IDLE:
                    break;
                case UnitStateEnum.DEAD:
                    m_animatorCom.SetAnimation(CHAR_DIE_ANIMATION_NAME);
                    break;
                case UnitStateEnum.BATTLE:
                    m_attackCD.Complete();
                    break;
            }
        }

        protected virtual void OnBattleStateUpdateHandle()
        {
            if (m_attackCD.IsComplete)
            {
                m_animatorCom.SetAnimationAndLoopOnEnd(CHAR_ATTACK_BEGIN_ANIMATION_NAME, g_attack_animation_link);
                m_attackCD.ReStart();
            }

            if (m_animatorCom.GetCurAnimationName() == CHAR_ATTACK_ANIMATION_NAME)
            {
                return;
            }

            if (!InAttackRange(m_target.CurrentPoint) || m_target.CurrentState == UnitStateEnum.DEAD)
            {
                m_stateMachine.EnterState(UnitStateEnum.IDLE);
            }
        }

        protected virtual void OnIdleStateUpdateHandle()
        {
            var targets = SearchTargets(m_targetUnitIsEnemy);

            if (targets.Count == 0)
                return;

            int closerIndex = 0;
            float distance = Vector3.Distance(targets[0].transform.position, transform.position);
            for (int i = 1; i < targets.Count; i++)
            {
                var updateDis = Vector3.Distance(targets[i].transform.position, transform.position);
                if (updateDis < distance)
                {
                    closerIndex = i;
                    distance = updateDis;
                }
            }
            m_target = targets[closerIndex];
            m_stateMachine.EnterState(UnitStateEnum.BATTLE);
        }

        protected override void AnimationStateStartHandle(Spine.TrackEntry trackEntry)
        {
            if (trackEntry.animation.name == CHAR_ATTACK_ANIMATION_NAME)
            {
                OnEnterAttackAnimation();
            }
        }

        protected override void AnimationStateCompleteHandle(Spine.TrackEntry trackEntry)
        {
            if (trackEntry.animation.name == "Die")
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnEnterAttackAnimation() { }
    }

    /// <summary>
    /// Config
    /// </summary>
    public partial class BaseCharacterUnit
    {
        public const string CHAR_ATTACK_ANIMATION_NAME = "Attack";
        public const string CHAR_ATTACK_BEGIN_ANIMATION_NAME = "Attack_Begin";
        public const string CHAR_IDLE_ANIMATION_NAME = "Idle";
        public const string CHAR_DIE_ANIMATION_NAME = "Die";

        protected static readonly string[] g_attack_animation_link = new string[] { CHAR_ATTACK_ANIMATION_NAME, "Attack_End", "Idle" };
    }
}