namespace Escaping.Core
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// ���[�h�\���N���X
    /// </summary>
    public class Loading : SingletonMonoBehaviour<Loading>
    {
        private Image m_LoadingImage;

        /// <summary>
        /// ���[�h��ʂ�\��
        /// </summary>
        public void Show()
        {
            m_LoadingImage.enabled = true;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// ���[�h��ʂ��\��
        /// </summary>
        public void Hide()
        {
            m_LoadingImage.enabled = false;
            gameObject.SetActive(false);
        }

        /// <summary>
        /// ���s����
        /// </summary>
        public void Run()
        {
            m_LoadingImage.transform.Rotate(0.0f, 0.0f, -180.0f * Time.deltaTime);
        }

        /// <inheritdoc/>
        protected override void OnCreatedInstance()
        {
            m_LoadingImage = GetComponent<Image>();
        }
    }
}
