namespace Escaping.Core
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// �t�F�[�h�\���N���X
    /// </summary>
    public class Fade : SingletonMonoBehaviour<Fade>
    {
        private Image m_FadeImage;

        private UnityEvent<bool> m_OnFadeFinished;

        /// <summary>
        /// �t�F�[�h�C�����J�n
        /// </summary>
        /// <param name="fadeTime">�t�F�[�h�C���ɂ����鎞��</param>
        public void FadeIn(float fadeTime = 0.5f)
        {
            m_FadeImage.color = Color.black;
            m_FadeImage
                .DOColor(Color.clear, fadeTime)
                .SetEase(Ease.Linear)
                .OnComplete(() => m_OnFadeFinished?.Invoke(true));
            gameObject.SetActive(true);
        }

        /// <summary>
        /// �t�F�[�h�A�E�g���J�n
        /// </summary>
        /// <param name="fadeTime">�t�F�[�h�A�E�g�ɂ����鎞��</param>
        public void FadeOut(float fadeTime = 0.5f)
        {
            m_FadeImage.color = Color.clear;
            m_FadeImage
                .DOColor(Color.black, fadeTime)
                .SetEase(Ease.Linear)
                .OnComplete(() => m_OnFadeFinished?.Invoke(false));
            gameObject.SetActive(true);
        }

        /// <summary>
        /// �t�F�[�h�������ɌĂ΂�鏈����ǉ�
        /// </summary>
        /// <param name="action">�t�F�[�h�������Ɏ��s����</param>
        public void AddOnFadeFinished(UnityAction<bool> action)
        {
            m_OnFadeFinished ??= new UnityEvent<bool>();

            m_OnFadeFinished.AddListener(action);
        }

        /// <summary>
        /// �t�F�[�h�������ɌĂ΂�鏈�����폜
        /// </summary>
        public void RemoveOnFadeFinished()
        {
            m_OnFadeFinished.RemoveAllListeners();
        }

        /// <summary>
        /// �t�F�[�h�p�w�i�𓧖��ɂ���
        /// </summary>
        public void Hide()
        {
            m_FadeImage.color = Color.clear;
            enabled = false;
        }

        /// <summary>
        /// �t�F�[�h�p�w�i�����ɂ���
        /// </summary>
        public void Show()
        {
            m_FadeImage.color = Color.black;
            enabled = true;
        }

        /// <inheritdoc/>
        protected override void OnCreatedInstance()
        {
            m_FadeImage = GetComponent<Image>();
        }
    }
}