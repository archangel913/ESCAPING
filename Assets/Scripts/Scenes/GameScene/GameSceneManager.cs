namespace Escaping.GameScene
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core;
    using UnityEngine;

    public class GameSceneManager :
        SceneBase,
        SceneBase.IUnityUpdate,
        SceneBase.ISceneLoader
    {
        [SerializeField]
        private MapGenerater m_MapGenerater;

        [SerializeField]
        private Player m_Player;

        [SerializeField]
        private EnemyGenerater m_EnemyGenerater;

        private Map m_Map;

        public async UniTask OnLoading()
        {
            m_Map = await m_MapGenerater.Init();
            m_Player.Init();
            await m_EnemyGenerater.Init(m_Map);
        }

        public void OnUpdate()
        {
            m_Player.Movement();
        }
    }
}
