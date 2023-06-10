namespace Escaping.Core.UI
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// テキスト表示ポップアップ
    /// </summary>
    public class TextPopupContent : PopupContentBase
    {
        [SerializeField]
        private TextMeshProUGUI m_Text;

        /// <summary>
        /// 表示テキスト
        /// </summary>
        public string Text
        {
            get => m_Text.text;
            set => m_Text.text = value;
        }
    }
}
