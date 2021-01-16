using EasyWork.Extend.Utilities;
using GameEvent;
using Key;
using Module.Battle.Com;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle
{
    /// <summary>
    /// Date    2021/1/5 16:08:49
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public partial class BattleModule : BaseModule
    {
        public const string UNIT_MAP_POINT = "UNIT_MAP_POINT";

        private LevelMapInfo m_mapInfo;
        private Dictionary<Vector2Int, List<BaseUnit>> m_unitMap = new Dictionary<Vector2Int, List<BaseUnit>>();

        protected override void OnInit()
        {
            Register(GameKey.DATA_LEVEL_MAP, () => m_mapInfo);

            EEventUtil.Subscribe<UnitMoveEvent>(UnitMoveHandle);
        }

        public List<BaseUnit> GetUnitsByPoint(Vector2Int point)
        {
            if(m_unitMap.TryGetValue(point, out var units))
            {
                return units;
            }

            return null;
        }

        private void UnitMoveHandle(UnitMoveEvent obj)
        {
            if (!m_unitMap.TryGetValue(obj.PrePoint, out var units))
            {
                return;
            }

            units.Remove(obj.Unit);

            if(!m_unitMap.TryGetValue(obj.CurrentPoint, out units))
            {
                units.Add(obj.Unit);
            }
        }
    }
}