namespace Escaping.Core.UI
{
    using UnityEngine;

    /// <summary>
    /// �|�b�v�A�b�v�ɕ\������R���e���c
    /// </summary>
    public class PopupContentBase : MonoBehaviour
    {
        private RectTransform m_RectTransform;

        /// <summary>
        /// �|�b�v�A�b�v���J���Ƃ��̏���
        /// </summary>
        public interface IOpenPopup
        {
            /// <summary>
            /// �|�b�v�A�b�v���J���Ƃ��̏���
            /// </summary>
            void OnOpenBegin();

            /// <summary>
            /// �|�b�v�A�b�v���J������̏���
            /// </summary>
            void OnOpenEnd();
        }

        /// <summary>
        /// �|�b�v�A�b�v����Ƃ��̏���
        /// </summary>
        public interface IClosePopup
        {
            /// <summary>
            /// �|�b�v�A�b�v����Ƃ��̏���
            /// </summary>
            void OnCloseBegin();

            /// <summary>
            /// �|�b�v�A�b�v�������Ƃ��̏���
            /// </summary>
            void OnCloseEnd();
        }

        /// <summary>
        /// Unity �̃A�b�v�f�[�g����
        /// </summary>
        public interface IUnityUpdate
        {
            /// <summary>
            /// �A�b�v�f�[�g����
            /// </summary>
            void OnUpdate();
        }

        /// <summary>
        /// �R���e���c�̃T�C�Y���|�b�v�A�b�v�ɍ��킹��
        /// </summary>
        /// <param name="size">�|�b�v�A�b�v�̃T�C�Y</param>
        /// <returns>�ύX�{��</returns>
        public Vector2 FitSize(Popup.Size size)
        {
            var contentSize = size.GetContentSize();
            var thisSize = m_RectTransform.sizeDelta;

            var ret = Vector3.one;

            ret.x = contentSize.x / thisSize.x;
            ret.y = contentSize.y / thisSize.y;

            transform.localScale = ret;

            return ret;
        }

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }
    }
}
