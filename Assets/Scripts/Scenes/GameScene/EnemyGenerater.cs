namespace Escaping
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core.FileSystems;
    using Escaping.GameScene;
    using UnityEngine;

    /// <summary>
    /// 敵生成クラス
    /// </summary>
    public class EnemyGenerater : MonoBehaviour
    {
        [SerializeField]
        private Transform m_EnemyParent;

        [SerializeField]
        private int m_NumberOfEnemy = 2;

        /// <summary>
        /// 敵生成処理
        /// </summary>
        /// <param name="map">マップ情報</param>
        /// <returns>void</returns>
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
