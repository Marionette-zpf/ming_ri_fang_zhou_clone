using EasyWork.Extend.Utilities;
using GameEvent;
using Module.Battle.Com;
using UnityEngine;

namespace Module.Battle
{
    /// <summary>
    /// Date    2021/3/4 11:10:58
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class LayoutUnit : MonoBehaviour
    {
        private Camera m_camera;

        [SerializeField]
        private LayerMask m_layerMask;

        private GameObject m_unit;

        private PlaceStateEnum m_stateEnum = PlaceStateEnum.SELECT_POINT;

        private Vector2Int m_selectPoint;
        private UnitDir m_selectDir;

        private Vector3 m_startMousePosition;

        private UnitLayoutEvent m_layoutEvent;

        private void Awake()
        {
            m_camera = Camera.main;

            EEventUtil.Subscribe<UnitLayoutEvent>(OnLayoutUnit);
        }

        private void OnLayoutUnit(UnitLayoutEvent obj)
        {
            m_layoutEvent = obj;

            m_unit = Instantiate(m_layoutEvent.FrontUnitObj);
        }

        private void Update()
        {
            if(m_unit == null)
            {
                return;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Clear();
                Destroy(m_unit);
            }

            if(m_stateEnum == PlaceStateEnum.SELECT_POINT)
            {
                SelectPoint();
            }
            else
            {
                SelectDir();
            }
        }

        private void SelectPoint()
        {
            var ray = m_camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit info, float.MaxValue, m_layerMask))
            {
                m_unit.transform.position = info.point;
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 point = Vector3.zero;
                point.x = Mathf.Round(info.point.x);
                point.z = Mathf.Round(info.point.z);

                m_unit.transform.position = point;

                m_selectPoint = new Vector2Int((int)point.x, -(int)point.z);
                m_stateEnum = PlaceStateEnum.SELECT_DIR;
                m_startMousePosition = m_camera.WorldToScreenPoint(m_unit.transform.position);
            }
        }

        private void SelectDir()
        {
            var unitDir = UnitDir.EAST;

            var curDir = (Input.mousePosition - m_startMousePosition).normalized;
            var cos45 = Mathf.Cos(Mathf.Deg2Rad * 45.0f);
            if(Vector3.Dot(curDir, Vector3.left) > cos45)
            {
                unitDir = UnitDir.WEST;
            }
            else if(Vector3.Dot(curDir, Vector3.up) > cos45)
            {
                unitDir = UnitDir.NORTH;
            }
            else if(Vector3.Dot(curDir, Vector3.down) > cos45)
            {
                unitDir = UnitDir.SOUTH;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var unitCom = m_unit.GetComponent<BaseCharacterUnit>();
                unitCom.Initialize(m_selectPoint, unitDir);

                if (unitDir == UnitDir.WEST)
                {
                    unitCom.Flip(0);
                }
                Clear();
            }
        }

        private void Clear()
        {
            Debug.Log("");

            m_stateEnum = PlaceStateEnum.SELECT_POINT;
            EEventUtil.Dispatch<UnitLayoutComplete>();
            m_unit = null;
            m_layoutEvent = default;
        }
    }




    public enum PlaceStateEnum
    {
        SELECT_POINT,
        SELECT_DIR
    }
}