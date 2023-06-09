namespace Escaping.GameScene.Enemy
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core.FileSystems;
    using Escaping.GameScene;
    using UnityEngine;

    /// <summary>
    /// �G�Ǘ��N���X
    /// </summary>
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField]
        private Transform m_EnemyParent;

        [SerializeField]
        private int m_NumberOfEnemy = 2;

        /// <summary>
        /// �G�������\�b�h
        /// </summary>
        /// <param name="map">�}�b�v���</param>
        /// <returns>void</returns>
        public async UniTask InstantiateEnemy(Map map)
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

        /// <summary>
        /// �G�̈ړ����\�b�h
        /// </summary>
        public void EnemyMovement()
        {
            foreach (Transform t in m_EnemyParent)
            {
                t.GetComponent<Enemy>().Move();
            }
        }
    }
}
