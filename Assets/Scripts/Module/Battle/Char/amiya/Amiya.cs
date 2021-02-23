using EasyWork.Utilities;
using Manager;
using Module.Battle.Com;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Char.amiya
{
    /// <summary>
    /// Date    2021/2/16 13:55:53
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class Amiya : BaseCharacterUnit
    {
        protected override bool AttackState => true;

        private void Start()
        {
            m_attackRate.Current = 0.5f;
            m_attackCD.SetCount(1.0f / m_attackRate.Current);

            SetAttackRange(new Vector2Int[] { 
              new Vector2Int(0, 0), new Vector2Int(0, 1), new Vector2Int(0, 2)
            , new Vector2Int(1, 0), new Vector2Int(1, 1), new Vector2Int(1, 2)
            , new Vector2Int(-1, 0), new Vector2Int(-1, 1), new Vector2Int(-1, 2)
            });

            m_currPoint = new Vector2Int(4, 3);
            CurrentDir = UnitDir.WEST;

            m_mapInfoExt = LevelDataManager.DataMgr.MapInfoExt;

            m_stateMachine.AddState(UnitStateEnum.MOVE, new BaseMoveStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.ATTACK, new BaseAttackStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.HURT, new BaseHurtRunner(m_stateMachine))
              .AddState(UnitStateEnum.IDLE, new AmiayIdleStateRunner(m_stateMachine))
              .SetPrimaryState(UnitStateEnum.IDLE);

            StartCoroutine(AttackCoroutine());
        }

        protected override void HandleTargets(List<BaseUnit> units)
        {
            m_attackCD.ReStart();
            Debug.LogError("Attack");
        }

        //private void OnDrawGizmos()
        //{
        //    if (!Application.isPlaying) return;

        //    var attackRange = m_attackRange[CurrentDir];

        //    for (int i = 0; i < attackRange.Length; i++)
        //    {
        //        var point = attackRange[i] + m_currPoint;
        //        var tile = LevelDataManager.DataMgr.MapInfoExt.MapInfo.GetTile(point);

        //        Gizmos.DrawSphere(tile.CenterWorldPos, 1);
        //    }
        //}
    }

    public class AmiayIdleStateRunner : BaseStateRunner<UnitStateEnum>
    {
        public AmiayIdleStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {
        }

        public override UnitStateEnum State => UnitStateEnum.IDLE;
    }

    
}