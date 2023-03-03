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
        private GenerateMap m_GenerateMap;

        [SerializeField]
        private Player m_Player;

        public async UniTask OnLoading()
        {
            m_GenerateMap.Init();
            await UniTask.Delay(1000);
            m_Player.Init();
        }

        public void OnUpdate()
        {
            m_Player.Movement();
        }
    }
}
