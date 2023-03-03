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
        private bool m_IsLoaded;

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
            Fade.Instance.AddOnFadeFinished((isFadeIn) =>
            {
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

        private void SceneLoad(string sceneName)
        {
            Loading.Instance.Show();
            SceneManager.LoadScene(sceneName);
        }

        private async void Awake()
        {
            if (Game.Instance == null)
            {
                await Game.Create("Prefabs/Core/Game");
                CurrentScene = SceneManager.GetActiveScene();
                await Loading.Create("Prefabs/Core/LoadingImage", Game.Instance.transform);
                Loading.Instance.Show();
                m_IsActive = true;
            }

            ISceneLoader load;
            if ((load = this as ISceneLoader) != null)
            {
                await load.OnLoading();
            }

            m_IsLoaded = true;

            Loading.Instance.Hide();
            Fade.Instance.FadeIn();

            CurrentScene = SceneManager.GetActiveScene();
        }

        private void Update()
        {
            if (!m_IsActive || !m_IsLoaded)
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
