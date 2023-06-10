namespace Escaping.TitleScene
{
    using Cysharp.Threading.Tasks;
    using Escaping.Core;
    using Escaping.Core.UI;
    using UnityEngine;

    /// <summary>
    /// タイトルシーンの管理クラス
    /// </summary>
    public class TitleSceneManager : SceneBase, SceneBase.ISceneLoader
    {
        [SerializeField]
        private GameButton m_StartButton;

        [SerializeField]
        private GameButton m_ExitButton;

        /// <inheritdoc/>
        public UniTask OnLoading()
        {
            ButtonSetting();

            return UniTask.CompletedTask;
        }

        private void ButtonSetting()
        {
            m_StartButton.AddPressed(() => {
                SceneTransition(SceneName.Game);
            });

            m_ExitButton.AddPressed(() => {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }
    }
}
