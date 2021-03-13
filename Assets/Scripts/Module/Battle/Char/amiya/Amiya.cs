using Config;
using Module.Battle.Bullet;
using Module.Battle.Com;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Char.amiya
{
    public abstract class BaseShotCharacterUnit : BaseCharacterUnit
    {
        public GameObject BulletPrefab;

        protected override void OnEnterAttackAnimation()
        {
            var bullet = BulletPool.Get(BulletPrefab);
            bullet.SetTarget(m_target)
                  .Init(transform.position, 2.0f, m_attack.Current);
        }
    }

    public abstract class BaseGuardCharacterUnit : BaseCharacterUnit
    {
        protected virtual int m_maxGuardCount => 2;
        protected List<BaseUnit> m_guardUnits = new List<BaseUnit>();

        private bool m_fliped;

        protected override void StateMachineOnChangeState(UnitStateEnum pre, UnitStateEnum curr)
        {
            base.StateMachineOnChangeState(pre, curr);

            if(curr == UnitStateEnum.IDLE && m_fliped)
            {
                Flip(0.2f);
                m_fliped = false;
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            m_searchFilter += Filter;
        }

        private bool Filter(BaseUnit unit)
        {
            return !m_guardUnits.Contains(unit);
        }

        protected override void OnEnterAttackAnimation()
        {
            if(m_guardUnits.Count == 0)
            {
                return;
            }
            m_guardUnits[0].DoDamage(m_attack.Current);

            if(m_guardUnits[0].CurrentState == UnitStateEnum.DEAD)
            {
                m_guardUnits.RemoveAt(0);
            }
        }

        protected override void OnIdleStateUpdateHandle()
        {
            var targets = SearchTargets();

            if (targets.Count == 0 && m_guardUnits.Count < m_maxGuardCount)
                return;

            Guard(targets);

            m_attackCD.Complete();
            if(
                targets[0].CurrentPoint.x > CurrentPoint.x && CurrentDir == UnitDir.EAST
                || targets[0].CurrentPoint.x < CurrentPoint.x && CurrentDir == UnitDir.WEST
                )
            {
                Flip(0.2f);
                m_fliped = true;
            }

            m_stateMachine.EnterState(UnitStateEnum.BATTLE);
        }

        protected override void OnBattleStateUpdateHandle()
        {
            if (m_attackCD.IsComplete)
            {
                m_animatorCom.SetAnimationAndLoopOnEnd(CHAR_ATTACK_BEGIN_ANIMATION_NAME, g_attack_animation_link);
                m_attackCD.ReStart();
            }

            if(m_guardUnits.Count < m_maxGuardCount)
            {
                var targets = SearchTargets();

                if(targets.Count == 0 && m_guardUnits.Count == 0 && m_animatorCom.GetCurAnimationName() == CHAR_ATTACK_ANIMATION_NAME)
                {
                    m_stateMachine.EnterState(UnitStateEnum.IDLE);
                }
                else
                {
                    Guard(targets);
                }
            }
        }

        private void Guard(List<BaseUnit> targets)
        {
            int count = Mathf.Min(targets.Count, m_maxGuardCount - m_guardUnits.Count);

            for (int i = 0; i < count; i++)
            {
                m_guardUnits.Add(targets[i]);

                var target = targets[i] as BaseEnemyUnit;

                target?.EnterBattle(this);
            }
        }
    }

    /// <summary>
    /// Date    2021/2/16 13:55:53
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class Amiya : BaseShotCharacterUnit
    {
        public override void Initialize(Vector2Int point, UnitDir dir)
        {
            base.Initialize(point, dir);
            SetProperties(UnitPropertiesDao.Inst.GetCfg(0), 0);
        }
    }


}