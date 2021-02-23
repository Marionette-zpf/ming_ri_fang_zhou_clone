using EasyWork.Utilities;
using Module.Battle.Com;
using UnityEngine;

namespace Module.Battle.Char.Enemy_001
{
    /// <summary>
    /// Date    2021/2/14 15:33:56
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class Enemy_001 : BaseEnemyUnit
    {
        protected override void Start()
        {
            m_speed.Current = 0.5f;

            m_stateMachine.AddState(UnitStateEnum.MOVE, new BaseMoveStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.ATTACK, new BaseAttackStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.HURT, new BaseHurtRunner(m_stateMachine))
                          .SetPrimaryState(UnitStateEnum.MOVE);
        }

    }

    public class Enemy_001_MoveState : BaseMoveStateRunner
    {
        public Enemy_001_MoveState(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {

        }

        public override void EnterState()
        {
            base.EnterState();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        public override void UpdateState()
        {
            base.UpdateState();
        }
    }

}