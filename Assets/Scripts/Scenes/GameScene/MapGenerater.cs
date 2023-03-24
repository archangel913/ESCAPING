namespace Escaping.GameScene
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Escaping.Core.FileSystems;
    using UnityEngine;

    /// <summary>
    /// マップ生成クラス
    /// </summary>
    public class MapGenerater : MonoBehaviour
    {
        [SerializeField]
        private int m_FloorSize = 31;

        [SerializeField]
        private Transform m_FloorParent;

        [SerializeField]
        private Transform m_WallParent;

        [SerializeField]
        private int m_LoopNum = 3;

        /// <summary>
        /// マップ初期化
        /// </summary>
        /// <returns>Mapクラス</returns>
        public async UniTask<Map> Init()
        {
            var floor = await FileLoader.LoadAssetAsync<GameObject>("Prefabs/GameScene/Floor");
            var wall = await FileLoader.LoadAssetAsync<GameObject>("Prefabs/GameScene/Wall");

            floor.transform.localScale = new Vector3(m_FloorSize, 1, m_FloorSize);
            Instantiate(floor, new Vector3(m_FloorSize / 2, 0, m_FloorSize / 2), Quaternion.identity, m_FloorParent);

            var map = GenerateMaze();
            var map2d = map.GetMap();

            for (int x = 0; x < m_FloorSize; ++x)
            {
                for (int z = 0; z < m_FloorSize; ++z)
                {
                    if (map2d[x, z] == MapStructure.Wall)
                    {
                        Instantiate(wall, new Vector3(x, 1, z), Quaternion.identity, m_WallParent);
                    }
                }
            }

            return map;
        }

        private Map GenerateMaze()
        {
            var map = new MapStructure[m_FloorSize, m_FloorSize];

            for (int x = 0; x < m_FloorSize; ++x)
            {
                for (int z = 0; z < m_FloorSize; ++z)
                {
                    if (x == 0 || z == 0 || x == m_FloorSize - 1 || z == m_FloorSize - 1)
                    {
                        map[x, z] = MapStructure.Path;
                    }
                    else
                    {
                        map[x, z] = MapStructure.Wall;
                    }
                }
            }

            var candidate = new List<Vector2Int>();
            var start = new Vector2Int(1, 1);

            while (start.x % 2 != 0)
            {
                start.x = Random.Range(2, m_FloorSize - 2);
            }

            while (start.y % 2 != 0)
            {
                start.y = Random.Range(2, m_FloorSize - 2);
            }

            map[start.x, start.y] = MapStructure.Path;

            candidate.Add(start);
            Vector2Int? canNullDir;
            Vector2Int dir;
            Vector2Int pos;

            while (candidate.Count > 0)
            {
                pos = candidate[0];
                candidate.RemoveAt(0);

                while (true)
                {
                    canNullDir = GenerateOnePath(map, pos);

                    if (canNullDir is null)
                    {
                        break;
                    }

                    dir = (Vector2Int)canNullDir;
                    map[pos.x + dir.x, pos.y + dir.y] = MapStructure.Path;
                    map[pos.x + (dir.x * 2), pos.y + (dir.y * 2)] = MapStructure.Path;

                    if (pos.x % 2 == 0 && pos.y % 2 == 0)
                    {
                        candidate.Add(pos);
                    }

                    pos = new Vector2Int(pos.x + (dir.x * 2), pos.y + (dir.y * 2));
                }
            }

            if (Random.Range(0, 2) % 2 == 0)
            {
                map[m_FloorSize - 2, m_FloorSize - 3] = MapStructure.Path;
            }
            else
            {
                map[m_FloorSize - 3, m_FloorSize - 2] = MapStructure.Path;
            }

            GenerateLoop(map);

            return new Map(map, m_FloorSize);
        }

        private Vector2Int? GenerateOnePath(MapStructure[,] map, Vector2Int pos)
        {
            var next = new List<Vector2Int>();
            var vectors = new List<Vector2Int>() {
                new Vector2Int(1, 0),
                new Vector2Int(0, 1),
                new Vector2Int(-1, 0),
                new Vector2Int(0, -1),
            };

            foreach (var v in vectors)
            {
                if (map[pos.x + v.x, pos.y + v.y] == MapStructure.Wall &&
                    map[pos.x + (v.x * 2), pos.y + (v.y * 2)] == MapStructure.Wall)
                {
                    next.Add(v);
                }
            }

            if (next.Count > 0)
            {
                int index = Random.Range(0, next.Count);
                return next[index];
            }

            return null;
        }

        private void GenerateLoop(MapStructure[,] map)
        {
            for (int i = 0; i < m_LoopNum; ++i)
            {
                int x = Random.Range(3, m_FloorSize - 3);
                int y;

                while (true)
                {
                    y = Random.Range(3, m_FloorSize - 3);

                    if (((x % 2 == 0 && y % 2 == 1) || (x % 2 == 1 && y % 2 == 0)) && map[x, y] == MapStructure.Wall)
                    {
                        break;
                    }
                }

                map[x, y] = MapStructure.Path;
            }
        }
    }
}
