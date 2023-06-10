namespace Escaping.Core
{
    using UnityEngine;

    /// <summary>
    /// ゲームを管理するクラス
    /// </summary>
    public class Game : SingletonMonoBehaviour<Game>
    {
        [SerializeField]
        private GameObject m_PopupuParent;

        /// <summary>
        /// ポップアップの親
        /// </summary>
        public Transform PopupParent { get => m_PopupuParent.transform; }

        /// <summary>
        /// ポップアップを表示
        /// </summary>
        public void ShowPopup()
        {
            m_PopupuParent.SetActive(true);
        }

        /// <summary>
        /// ポップアップを非表示
        /// </summary>
        public void HidePopup()
        {
            m_PopupuParent.SetActive(false);
        }

        /// <inheritdoc/>
        protected override async void OnCreatedInstance()
        {
            if (Fade.Instance == null)
            {
                await Fade.Create("Prefabs/Core/FadeImage", transform);

                Fade.Instance.Show();
            }
        }
    }
}
