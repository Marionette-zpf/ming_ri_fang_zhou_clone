using System.Collections.Generic;
using UnityEngine;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/1/6 12:06:31
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class LevelMapInfo
    {
        public Tile[][] MapTiles;

        public EnemyPath[] EnemyPathArray;

        [HideInInspector]
        public Tile[] LoadTiles;
        [HideInInspector]
        public Tile[] PlaceTiles;

        public Tile GetTile(Vector2Int point)
        {
            return MapTiles[point.y][point.x];
        }

        public void Classify()
        {
            var loadTiles = new List<Tile>();
            var placeTiles = new List<Tile>();

            for (int i = 0; i < MapTiles.GetLength(0); i++)
            {
                for (int j = 0; j < MapTiles.GetLength(1); j++)
                {
                    switch (MapTiles[i][j].Type)
                    {
                        case TileTypeEnum.LOAD:
                            loadTiles.Add(MapTiles[i][j]);
                            break;
                        case TileTypeEnum.PLACE:
                            placeTiles.Add(MapTiles[i][j]);
                            break;
                        case TileTypeEnum.OBSTACLE:
                            break;
                        default:
                            break;
                    }
                }
            }

            LoadTiles = loadTiles.ToArray();
            PlaceTiles = placeTiles.ToArray();
        }
    }

    public class EnemyPath
    {
        public Vector2Int[] Points;
    }

    public class Tile
    {
        public TileTypeEnum Type;
        public Vector2Int Point;
        [HideInInspector]
        public Vector3 WorldPos;
    }


    public enum TileTypeEnum
    {
        LOAD,
        PLACE,
        OBSTACLE
    }
}

