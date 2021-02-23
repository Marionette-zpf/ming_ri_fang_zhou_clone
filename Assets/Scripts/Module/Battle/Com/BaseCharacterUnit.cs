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
    public class BaseCharacterUnit : BaseUnit
    {
        public const string KEY_POWER = "Power";

        public override bool IsEnemy => false;

        protected NumberCom m_power;

        private List<BaseUnit> m_targetUnits = new List<BaseUnit>();

        protected virtual Func<BaseUnit, bool> TargetUnitFilter => unit => unit.IsEnemy;

        protected override void OnAttack(List<BaseUnit> units)
        {

            m_targetUnits.Clear();
            foreach (var unit in units)
            {
                if (TargetUnitFilter.Invoke(unit))
                {
                    m_targetUnits.Add(unit);
                }
            }

            HandleTargets(m_targetUnits);
        }

        protected virtual void HandleTargets(List<BaseUnit> units)
        {

        }
    }
}