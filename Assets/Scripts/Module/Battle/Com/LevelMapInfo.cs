using System;
using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/1/6 12:06:31
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    [CreateAssetMenu( menuName = "MapData")]
    public class LevelMapInfo : ScriptableObject
    {
        public const float TILE_SIZE = 1;

        public int Wdith;
        public int Height;

        public Tile[] MapTiles;

        public UnitPath[] EnemyPathArray;

        public Tile GetTile(Vector2Int point)
        {
            return MapTiles[Wdith * point.x + point.y];
        }
    }

    public class LevelMapInfoExt
    {
        public Tile[] LoadTiles;
        public Tile[] PlaceTiles;

        public LevelMapInfo MapInfo { get; private set; }

        public LevelMapInfoExt(LevelMapInfo mapInfo)
        {
            MapInfo = mapInfo;
            Classify();
        }



        private void Classify()
        {
            var loadTiles = new List<Tile>();
            var placeTiles = new List<Tile>();

            for (int i = 0; i < MapInfo.MapTiles.Length; i++)
            {
                switch (MapInfo.MapTiles[i].Type)
                {
                    case TileTypeEnum.LOAD:
                        loadTiles.Add(MapInfo.MapTiles[i]);
                        break;
                    case TileTypeEnum.PLACE:
                        placeTiles.Add(MapInfo.MapTiles[i]);
                        break;
                    case TileTypeEnum.OBSTACLE:
                        break;
                    default:
                        break;
                }
            }

            LoadTiles = loadTiles.ToArray();
            PlaceTiles = placeTiles.ToArray();
        }
    }

    public class UnitPathExt 
    {
        private LevelMapInfo m_mapInfo;

        public Tile[] Tiles;
        public UnitDir[] DirLink;

        private float m_interval;
        private float m_length;



        public UnitPathExt(UnitPath unitPath, LevelMapInfo levelMapInfo)
        {
            m_mapInfo = levelMapInfo;

            var pointLength = unitPath.Points.Length;

            Tiles = new Tile[pointLength];
            DirLink = new UnitDir[pointLength];

            for (int i = 0; i < pointLength; i++)
            {
                Tiles[i] = levelMapInfo.GetTile(unitPath.Points[i]);

                if(i != pointLength - 1)
                {
                    var point = unitPath.Points[i];
                    var nextPoint = unitPath.Points[i + 1];

                    if(nextPoint.x > point.x)
                    {
                        DirLink[i] = UnitDir.EAST;
                    }else if(nextPoint.x < point.x)
                    {
                        DirLink[i] = UnitDir.WEST;
                    }else if (nextPoint.y > point.y)
                    {
                        DirLink[i] = UnitDir.NORTH;
                    }else if(nextPoint.y < point.y)
                    {
                        DirLink[i] = UnitDir.SOUTH;
                    }
                }
                else
                {
                    DirLink[i] = DirLink[i - 1];
                }
            }
        }

        public Vector3 GetStartPos()
        {
            return Tiles[0].CenterWorldPos;
        }

        public Vector2Int GetPointByJourney(float journey)
        {
            if (journey >= Length())
            {
                return Tiles[DirLink.Length - 1].Point;
            }

            int curIndex = (int)((journey + 0.5f) / LevelMapInfo.TILE_SIZE);
            return Tiles[curIndex].Point;
        }

        public UnitDir GetDirByJourney(float journey)
        {
            if (journey >= Length())
            {
                return DirLink[DirLink.Length - 1];
            }

            int curIndex = (int)(journey / LevelMapInfo.TILE_SIZE);

            return DirLink[curIndex];
        }

        public Vector3 GetWSPosByJourney(float journey)
        {
            if (journey >= Length())
            {
                return Tiles[Tiles.Length - 1].CenterWorldPos;
            }

            int curIndex = (int)(journey / LevelMapInfo.TILE_SIZE);

            return Vector3.Lerp(Tiles[curIndex].CenterWorldPos, Tiles[curIndex + 1].CenterWorldPos, journey % LevelMapInfo.TILE_SIZE / LevelMapInfo.TILE_SIZE);
        }

        public float Length()
        {
            if (m_length != 0)
            {
                return m_length;
            }

            m_length = Tiles.Length * LevelMapInfo.TILE_SIZE;

            return m_length;
        }

        public float Interval()
        {
            if (m_interval != 0)
            {
                return m_interval;
            }

            m_interval = 1 / Tiles.Length;

            return m_interval;
        }
    }

    [Serializable]
    public class UnitPath
    {
        public Color Color = Color.white;
        public Vector2Int[] Points;
    }

    [Serializable]
    public class Tile
    {
        public TileTypeEnum Type;
        public Vector2Int Point;
        public Vector3 CenterWorldPos;
    }


    public enum TileTypeEnum
    {
        LOAD,
        PLACE,
        OBSTACLE
    }


}

