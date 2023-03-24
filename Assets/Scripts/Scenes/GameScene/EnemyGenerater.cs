using Cysharp.Threading.Tasks;
using Escaping.Core.FileSystems;
using Escaping.GameScene;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Escaping
{
    public class EnemyGenerater : MonoBehaviour
    {
        [SerializeField]
        private Transform m_EnemyParent;

        [SerializeField]
        private int m_NumberOfEnemy = 2;

        public async UniTask Init(Map map)
        {
            var enemy = await FileLoader.LoadAssetAsync<GameObject>("Prefabs/GameScene/Enemy");

            for (int i = 0; i < m_NumberOfEnemy; ++i)
            {
                int x = Random.Range(2, map.MapSize - 2);
                int z = Random.Range(2, map.MapSize - 2);

                if (map.GetStructureAt(x, z) == MapStructure.Path)
                {
                    Instantiate(enemy, new Vector3(x, 1, z), Quaternion.identity, m_EnemyParent);
                }
                else
                {
                    --i;
                    continue;
                }
            }
        }
    }
}
