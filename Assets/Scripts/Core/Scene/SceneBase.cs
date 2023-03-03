namespace Escaping.Core
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// シーンの親クラス
    /// </summary>
    public class SceneBase : MonoBehaviour
    {
        private bool m_IsActive;

        /// <summary>
        /// Unity のアップデート処理
        /// </summary>
        public interface IUnityUpdate
        {
            /// <summary>
            /// アップデート処理
            /// </summary>
            public void OnUpdate();
        }

        /// <summary>
        /// シーンロード時の処理
        /// </summary>
        public interface ISceneLoader
        {
            /// <summary>
            /// ロード中の処理
            /// </summary>
            public UniTask OnLoading();
        }

        /// <summary>
        /// 現在アクティブなシーン
        /// </summary>
        public static Scene CurrentScene { get; private set; }

        /// <summary>
        /// シーン遷移
        /// </summary>
        /// <param name="sceneName">読み込むシーン名</param>
        public void SceneTransition(string sceneName)
        {
            m_IsActive = false;
            Fade.Instance.FadeOut();
            Fade.Instance.AddOnFadeFinished((isFadeIn) => {
                if (!isFadeIn)
                {
                    SceneLoad(sceneName);
                }
                else
                {
                    m_IsActive = true;
                }
            });
        }

        private async void SceneLoad(string sceneName)
        {
            await Loading.Create("Prefabs/Core/LoadingImage", Game.Instance.transform);
            Loading.Instance.Show();

            SceneManager.LoadScene(sceneName);
        }

        private async void Awake()
        {
            if (Game.Instance == null)
            {
                await Game.Create("Prefabs/Core/Game");
                CurrentScene = SceneManager.GetActiveScene();
                SceneLoad(CurrentScene.name);
            }
            else
            {
                ISceneLoader load;
                if ((load = this as ISceneLoader) != null)
                {
                    await load.OnLoading();
                }

                Loading.Instance.Hide();
                Fade.Instance.FadeIn();

                CurrentScene = SceneManager.GetActiveScene();
            }
        }

        private void Update()
        {
            if (!m_IsActive)
            {
                return;
            }

            if (this is IUnityUpdate)
            {
                var update = this as IUnityUpdate;
                update.OnUpdate();
            }
        }
    }
}
