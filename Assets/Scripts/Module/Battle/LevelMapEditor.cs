using Module.Battle.Com;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle
{
    /// <summary>
    /// Date    2021/2/11 19:46:51
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class LevelMapEditor : MonoBehaviour
    {
        public LevelMapInfo MapInfo;

        public TileTypeEnum TileType = TileTypeEnum.OBSTACLE;
        public EditorType EditorType = EditorType.Tile;
        public int PathIndex = 0;

        public Vector3 XAix = Vector3.right;
        public Vector3 ZAix = Vector3.forward;

        void Start()
        {

        }

        private List<Vector2Int> m_editorPath = new List<Vector2Int>();

        void Update()
        {
            if (MapInfo == null) return;
            if (EditorType == EditorType.Tile)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var mousePosWS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2Int point = new Vector2Int(Mathf.CeilToInt(mousePosWS.x), Mathf.CeilToInt(-mousePosWS.z));

                    int index = point.x * MapInfo.Wdith + point.y;
                    MapInfo.MapTiles[index].Type = TileType;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var mousePosWS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2Int point = new Vector2Int(Mathf.CeilToInt(mousePosWS.x), Mathf.CeilToInt(-mousePosWS.z));
                    if(!m_editorPath.Contains(point))
                        m_editorPath.Add(point);
                }
                if (Input.GetMouseButtonDown(1))
                {
                    m_editorPath.RemoveAt(m_editorPath.Count - 1);
                }
                if (Input.GetMouseButtonDown(2))
                {
                    MapInfo.EnemyPathArray[PathIndex].Points = m_editorPath.ToArray();
                    m_editorPath.Clear();
                }
            }

        }

        private void Reset()
        {
            if(PathIndex > MapInfo.EnemyPathArray.Length - 1)
            {
                PathIndex = MapInfo.EnemyPathArray.Length - 1;
            }
        }

        [ContextMenu("Init World Pos")]
        public void InitCenterPosWS()
        {
            if (MapInfo == null) return;

            for (int i = 0; i < MapInfo.MapTiles.Length; i++)
            {
                var tile = MapInfo.MapTiles[i];
                int row = i / MapInfo.Wdith;
                int cloumn = i % MapInfo.Wdith;

                tile.Point = new Vector2Int(row, cloumn);

                tile.CenterWorldPos = Vector3.zero;

                tile.CenterWorldPos += XAix * row;
                tile.CenterWorldPos += ZAix * cloumn;
            }
        }


        private void OnDrawGizmos()
        {
            if (MapInfo == null) return;

            if (EditorType == EditorType.Tile)
            for (int i = 0; i < MapInfo.MapTiles.Length; i++)
            {
                var tile = MapInfo.MapTiles[i];

                switch (tile.Type)
                {
                    case TileTypeEnum.LOAD:
                        Gizmos.color = Color.blue;
                        break;
                    case TileTypeEnum.PLACE:
                        Gizmos.color = Color.green;
                        break;
                    case TileTypeEnum.OBSTACLE:
                        Gizmos.color = Color.red;
                        break;
                    default:
                        break;
                }
                
                Gizmos.DrawWireSphere(tile.CenterWorldPos, 0.3f);

            }

            if(EditorType == EditorType.Path)
            {
                Gizmos.color = Color.red;

                for (int j = 0; j < m_editorPath.Count; j++)
                {
                    if (j != m_editorPath.Count - 1)
                    {
                        Vector3 from = MapInfo.GetTile(m_editorPath[j]).CenterWorldPos + Vector3.up * 2;
                        Vector3 to = MapInfo.GetTile(m_editorPath[j + 1]).CenterWorldPos + Vector3.up * 2;

                        Gizmos.DrawLine(from, to);
                    }
                }
            }



            for (int i = 0; i < MapInfo.EnemyPathArray.Length; i++)
            {
                var path = MapInfo.EnemyPathArray[i];
                Gizmos.color = path.Color;
                if (i != PathIndex) continue;
                for (int j = 0; j < path.Points.Length; j++)
                {
                    if (j != path.Points.Length - 1)
                    {
                        Vector3 from = MapInfo.GetTile(path.Points[j]).CenterWorldPos + Vector3.up * 2;
                        Vector3 to = MapInfo.GetTile(path.Points[j + 1]).CenterWorldPos + Vector3.up * 2;
                        
                        Gizmos.DrawLine(from, to);
                    }
                }
            }
        }

    }

    public enum EditorType
    {
        Tile,
        Path
    }
}