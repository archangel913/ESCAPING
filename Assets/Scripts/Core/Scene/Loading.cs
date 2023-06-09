namespace Escaping.Core
{
    using DG.Tweening;
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

            // TODO: ���[�h���̕\�����m�肵���炻��ɕύX
            m_LoadingImage.rectTransform
                .DOLocalRotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        /// <summary>
        /// ���[�h��ʂ��\��
        /// </summary>
        public void Hide()
        {
            m_LoadingImage.enabled = false;
            gameObject.SetActive(false);

            m_LoadingImage.transform.DOKill();
            m_LoadingImage.transform.localRotation = Quaternion.identity;
        }

        /// <inheritdoc/>
        protected override void OnCreatedInstance()
        {
            m_LoadingImage = GetComponent<Image>();
        }
    }
}
