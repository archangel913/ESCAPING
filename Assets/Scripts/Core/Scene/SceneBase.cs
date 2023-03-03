namespace Escaping.Core
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// �V�[���̐e�N���X
    /// </summary>
    public class SceneBase : MonoBehaviour
    {
        private bool m_IsActive;

        /// <summary>
        /// Unity �̃A�b�v�f�[�g����
        /// </summary>
        public interface IUnityUpdate
        {
            /// <summary>
            /// �A�b�v�f�[�g����
            /// </summary>
            public void OnUpdate();
        }

        /// <summary>
        /// �V�[�����[�h���̏���
        /// </summary>
        public interface ISceneLoader
        {
            /// <summary>
            /// ���[�h���̏���
            /// </summary>
            public UniTask OnLoading();
        }

        /// <summary>
        /// ���݃A�N�e�B�u�ȃV�[��
        /// </summary>
        public static Scene CurrentScene { get; private set; }

        /// <summary>
        /// �V�[���J��
        /// </summary>
        /// <param name="sceneName">�ǂݍ��ރV�[����</param>
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
