using Config;
using EasyWork.Extend.Utilities;
using GameEvent;
using Spine;
using UnityEngine;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/2/14 15:10:18
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public partial class BaseEnemyUnit : BaseUnit
    {
        protected const string ENEMY_ATTACK_ANIMATION_NAME = "Attack";
        protected const string ENEMY_IDLE_ANIMATION_NAME = "Idle";
        protected const string ENEMY_DIE_ANIMATION_NAME = "Die";

        protected string m_preMoveAnimationName;

        protected BaseCharacterUnit m_battleUnit;
        protected NumberCom m_speed;
        protected UnitPathExt m_pathExt;
        protected float m_curJourney = 0;

        protected override void Initialize()
        {
            m_stateMachine.AddState(UnitStateEnum.MOVE, new BaseMoveStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.BATTLE, new BaseBattleStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
              .SetPrimaryState(UnitStateEnum.MOVE);

            m_stateMachine.RegisterStateRunner(UnitStateEnum.MOVE, MoveStateUpdateHandle);
            m_stateMachine.RegisterStateRunner(UnitStateEnum.BATTLE, BattleStateUpdateHandle);
        }

        protected override void StateMachineOnChangeState(UnitStateEnum pre, UnitStateEnum curr)
        {
            switch (curr)
            {
                case UnitStateEnum.MOVE:
                    OnChangeDirHandle(UnitDir.NONE, m_pathExt.GetDirByJourney(m_curJourney));
                    break;
                case UnitStateEnum.ATTACK:
                    break;
                case UnitStateEnum.DEAD:
                    m_animatorCom.SetAnimation(ENEMY_DIE_ANIMATION_NAME);
                    break;
            }
        }

        protected virtual void BattleStateUpdateHandle()
        {
            if(m_battleUnit.CurrentState == UnitStateEnum.DEAD)
            {
                if(m_animatorCom.GetCurAnimationName() != ENEMY_ATTACK_ANIMATION_NAME)
                {
                    m_stateMachine.EnterState(UnitStateEnum.MOVE);
                }
                return;
            }

            if (m_attackCD.IsComplete)
            {
                m_animatorCom.SetAnimationAndLoopOnEnd(ENEMY_ATTACK_ANIMATION_NAME, ENEMY_IDLE_ANIMATION_NAME);
                m_attackCD.ReStart();
            }
        }

        protected virtual void MoveStateUpdateHandle()
        {
            if (m_curJourney >= m_pathExt.Length() - 1.5f)
            {
                EEventUtil.Dispatch(new EnemyArriveEvent() { EnemyUnit = this });
                Destroy(gameObject);
                return;
            }

            var moveSize = m_speed.Current * Time.deltaTime;
            m_curJourney += moveSize;
            transform.position = m_pathExt.GetWSPosByJourney(m_curJourney);

            CurrentDir = m_pathExt.GetDirByJourney(m_curJourney);
            CurrentPoint = m_pathExt.GetPointByJourney(m_curJourney);
        }

        protected override void AnimationStateStartHandle(TrackEntry trackEntry)
        {
            if(trackEntry.animation.name == ENEMY_ATTACK_ANIMATION_NAME)
            {
                m_battleUnit.DoDamage(m_attack.Current);
            }
        }

        protected override void OnChangeDirHandle(UnitDir preDir, UnitDir curDir)
        {
            if(m_stateMachine.CurrentState == UnitStateEnum.MOVE)
            {
                switch (CurrentDir)
                {
                    case UnitDir.EAST:
                    case UnitDir.WEST:
                        m_animatorCom.SetAnimation("Move_Loop");
                        break;
                    case UnitDir.SOUTH:
                        m_animatorCom.SetAnimation("Move_Down");
                        break;
                    case UnitDir.NORTH:
                        m_animatorCom.SetAnimation("Move_Up");
                        break;
                }
            }
        }

        protected override void AnimationStateCompleteHandle(TrackEntry trackEntry)
        {
            if(trackEntry.animation.name == "Die")
            {
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// interface
    /// </summary>
    public partial class BaseEnemyUnit
    {
        public void Initialize(UnitPropertiesConfig propertiesConfig, UnitPathExt pathInfo)
        {
            Initialize();

            SetProperties(propertiesConfig);
            m_pathExt = pathInfo;
            m_speed = new NumberCom(propertiesConfig.Speed, ZERO_FLOAT);

            CurrentPoint = m_pathExt.Tiles[0].Point;
            CurrentDir = m_pathExt.GetDirByJourney(0);
            transform.position = m_pathExt.GetStartPos();
        }

        public void EnterBattle(BaseCharacterUnit unit)
        {
            m_attackCD.Complete();
            m_battleUnit = unit;
            m_stateMachine.EnterState(UnitStateEnum.BATTLE);
        }
    }
}

