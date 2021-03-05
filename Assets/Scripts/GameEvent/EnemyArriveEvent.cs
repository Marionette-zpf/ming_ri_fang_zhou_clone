using Module.Battle.Com;
using Module.Battle.Views;
using UnityEngine;

namespace GameEvent
{
    /// <summary>
    /// Date    2021/1/6 13:12:45
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public struct EnemyArriveEvent
    {
        public BaseEnemyUnit EnemyUnit;
    }

    public struct UnitMoveEvent
    {
        public Vector2Int PrePoint;
        public Vector2Int CurrentPoint;
        public BaseUnit Unit;
    }

    public struct UnitLayoutEvent
    {
        public GameObject FrontUnitObj;
        public GameObject BackUnitObj;
    }

    public struct UnitLayoutComplete
    {
        
    }
}