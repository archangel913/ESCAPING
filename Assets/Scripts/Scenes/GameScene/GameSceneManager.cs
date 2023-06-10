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
        private MapGenerator m_GenerateMap;

        [SerializeField]
        private Player m_Player;

        public async UniTask OnLoading()
        {
            await m_GenerateMap.Init();
            m_Player.Init();
        }

        public void OnUpdate()
        {
            m_Player.Movement();
        }
    }
}
