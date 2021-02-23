using System.Collections.Generic;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/2/14 15:10:18
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class BaseEnemyUnit : BaseUnit
    {
        public const string KEY_PATH = "PathInfo";
        public const string KEY_SPEED = "MoveSpeed";
        public const string KEY_AROUNT_CHAR = "AroundChar";

        public const float MINI_DIS = 0.1f;

        protected NumberCom m_speed;
        protected UnitPathExt m_path;

        protected List<BaseCharacterUnit> m_aroundChar = new List<BaseCharacterUnit>();


        protected override void Awake()
        {
            base.Awake();

            m_speed = new NumberCom();
            m_speed.Current =1f;

            m_stateMachine.DataInterface.Register(KEY_PATH, () => m_path);
            m_stateMachine.DataInterface.Register(KEY_SPEED, () => m_speed);
            m_stateMachine.DataInterface.Register(KEY_AROUNT_CHAR, () => m_aroundChar);

        }

        protected virtual void Start()
        {
            m_stateMachine.AddState(UnitStateEnum.MOVE, new BaseMoveStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.ATTACK, new BaseAttackStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
              .AddState(UnitStateEnum.HURT, new BaseHurtRunner(m_stateMachine))
              .SetPrimaryState(UnitStateEnum.MOVE);
        }


        public void SetPath(UnitPathExt pathInfo)
        {
            m_path = pathInfo;
        }

    }
}