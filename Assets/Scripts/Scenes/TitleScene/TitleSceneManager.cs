namespace Escaping.TitleScene
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// タイトルシーンの管理クラス
    /// </summary>
    public class TitleSceneManager : SceneBase, SceneBase.ISceneLoader
    {
        [SerializeField]
        private Button m_StartButton;

        [SerializeField]
        private Button m_ExitButton;

        /// <inheritdoc/>
        public UniTask OnLoading()
        {
            ButtonSetting();

            return UniTask.CompletedTask;
        }

        private void ButtonSetting()
        {
            m_StartButton.onClick.AddListener(() => {
                SceneTransition(SceneName.Game);
            });

            m_ExitButton.onClick.AddListener(() => {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }
}
