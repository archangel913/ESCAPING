namespace Escaping.Core
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core.UI;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// �V�[���̐e�N���X
    /// </summary>
    public class SceneBase : MonoBehaviour
    {
        private bool m_IsActive;
        private bool m_IsLoaded;

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
            /// <returns>void</returns>
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
                    Loading.Instance.Show();
                    SceneManager.LoadScene(sceneName);
                }
                else
                {
                    m_IsActive = true;
                    m_IsLoaded = false;
                }

                Fade.Instance.RemoveOnFadeFinished();
            });
        }

        private async void Awake()
        {
            // �Q�[������������ĂȂ���ΐ�������
            if (Game.Instance == null)
            {
                await Game.Create("Prefabs/Core/Game");
                CurrentScene = SceneManager.GetActiveScene();
                await Loading.Create("Prefabs/Core/LoadingImage", Game.Instance.transform);
                Loading.Instance.Show();
                m_IsActive = true;
            }

            if (this is ISceneLoader load)
            {
                // ���[�h���������s
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

            // ���[�v���������s
            if (this is IUnityUpdate update)
            {
                update.OnUpdate();
            }

            PopupManager.Instance.Update();
        }
    }
}
