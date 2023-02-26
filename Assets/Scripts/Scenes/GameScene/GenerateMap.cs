namespace Escaping.GameScene
{
    using System.Collections.Generic;
    using Escaping.Core.FileSystems;
    using UnityEngine;

    public class GenerateMap : MonoBehaviour
    {
        [SerializeField]
        private int m_Floorsize = 31;

        [SerializeField]
        private Transform m_FloorParent;

        [SerializeField]
        private Transform m_WallParent;

        public enum Map
        {
            Wall = 0,
            Path = 1
        }

        private async void Start()
        {
            GameObject floor = await FileLoader.LoadAssetAsync<GameObject>("Prefabs/Floor");
            GameObject wall  = await FileLoader.LoadAssetAsync<GameObject>("Prefabs/Wall");

            for (int x = 0; x < m_Floorsize; ++x)
            {
                for (int z = 0; z < m_Floorsize; ++z)
                {
                    Instantiate(floor, new Vector3(x, 0, z), Quaternion.identity, m_FloorParent);
                }
            }

            Map[,] map = GenerateMaze();

            for (int x = 0; x < m_Floorsize; ++x)
            {
                for (int z = 0; z < m_Floorsize; ++z)
                {
                    if (map[x, z] == Map.Wall)
                    {
                        Instantiate(wall, new Vector3(x, 1, z), Quaternion.identity, m_WallParent);
                    }
                }
            }
        }

        private Map[,] GenerateMaze()
        {
            Map[,] map = new Map[m_Floorsize, m_Floorsize];

            for (int x = 0; x < m_Floorsize; ++x)
            {
                for (int z = 0; z < m_Floorsize; ++z)
                {
                    if (x == 0 || z == 0 || x == m_Floorsize - 1 || z == m_Floorsize - 1)
                    {
                        map[x, z] = Map.Path;
                    }
                    else
                    {
                        map[x, z] = Map.Wall;
                    }
                }
            }

            var candidate = new List<Vector2Int>();
            var start = new Vector2Int(1, 1);

            while (start.x % 2 != 0)
            {
                start.x = Random.Range(2, m_Floorsize - 2);
            }

            while (start.y % 2 != 0)
            {
                start.y = Random.Range(2, m_Floorsize - 2);
            }

            map[start.x, start.y] = Map.Path;

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
                    map[pos.x + dir.x, pos.y + dir.y] = Map.Path;
                    map[pos.x + dir.x * 2, pos.y + dir.y * 2] = Map.Path;

                    if (pos.x % 2 == 0 && pos.y % 2 == 0)
                    {
                        candidate.Add(pos);
                    }

                    pos = new Vector2Int(pos.x + dir.x * 2, pos.y + dir.y * 2);
                }
            }

            return map;
        }

        private Vector2Int? GenerateOnePath(Map[,] map, Vector2Int pos)
        {
            var next = new List<Vector2Int>();
            List<Vector2Int> vectors = new() {
                new Vector2Int(1,0),
                new Vector2Int(0,1),
                new Vector2Int(-1,0),
                new Vector2Int(0,-1)
            };

            foreach (var v in vectors)
            {
                if (map[pos.x + v.x, pos.y + v.y] == Map.Wall &&
                    map[pos.x + v.x * 2, pos.y + v.y * 2] == Map.Wall)
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
    }
}
