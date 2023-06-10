namespace Escaping.Core.UI
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// �e�L�X�g�\���|�b�v�A�b�v
    /// </summary>
    public class TextPopupContent : PopupContentBase
    {
        [SerializeField]
        private TextMeshProUGUI m_Text;

        /// <summary>
        /// �\���e�L�X�g
        /// </summary>
        public string Text
        {
            get => m_Text.text;
            set => m_Text.text = value;
        }
    }
}
