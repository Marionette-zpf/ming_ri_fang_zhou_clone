namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/2/14 15:11:25
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class BasePassivityUnit : BaseEnemyUnit
    {
        protected BaseUnit m_battleUnit;

        public void SetBattleUnit(BaseUnit unit)
        {
            m_battleUnit = unit;
            m_stateMachine.EnterState(UnitStateEnum.BATTLE);
        }
    }
}