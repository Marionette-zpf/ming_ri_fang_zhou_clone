using Config;
using Module.Battle.Com;
using UnityEngine;

namespace Module.Battle.Char.amiya
{
    /// <summary>
    /// Date    2021/3/1 15:27:11
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class AmiyaGuard : BaseGuardCharacterUnit
    {
        protected override void Start()
        {
            base.Start();

            m_currPoint = new Vector2Int(3, 2);
            CurrentDir = UnitDir.WEST;

            SetProperties(UnitPropertiesDao.Inst.GetCfg(0), 0);
        }
    }
}