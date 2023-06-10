namespace Escaping.GameScene
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core;
    using Escaping.GameScene.Enemy;
    using UnityEngine;

    /// <summary>
    /// ゲームシーン管理クラス
    /// </summary>
    public class GameSceneManager :
        SceneBase,
        SceneBase.IUnityUpdate,
        SceneBase.ISceneLoader
    {
        [SerializeField]
        private MapGenerator m_MapGenerator;

        [SerializeField]
        private Player m_Player;

        [SerializeField]
        private EnemyManager m_EnemyManager;

        private Map m_Map;

        /// <inheritdoc/>
        public async UniTask OnLoading()
        {
            m_Map = await m_MapGenerator.Init();
            await UniTask.Delay(1000);
            m_Player.Init();
            await m_EnemyManager.InstantiateEnemy(m_Map);
        }

        /// <inheritdoc/>
        public void OnUpdate()
        {
            m_Player.Movement();
            m_EnemyManager.EnemyMovement();
        }
    }
}
