namespace Escaping.Core
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// �t�F�[�h�\���N���X
    /// </summary>
    public class Fade : SingletonMonoBehaviour<Fade>
    {
        private Image m_FadeImage;

        private bool m_Fading = false;
        private float m_FadeTime;
        private float m_FadeTimer = 0.0f;
        private bool m_IsFadeIn = true;

        private bool m_BeforeFading = false;

        private UnityEvent<bool> m_OnFadeBegin;
        private UnityEvent<bool> m_OnFadeFinished;

        /// <summary>
        /// �t�F�[�h�C�����J�n
        /// </summary>
        /// <param name="fadeTime">�t�F�[�h�C���ɂ����鎞��</param>
        public void FadeIn(float fadeTime = 0.5f)
        {
            m_FadeTimer = 0.0f;
            enabled = true;
            m_Fading = true;
            m_FadeTime = fadeTime;
            m_IsFadeIn = true;
            m_FadeImage.color = Color.black;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// �t�F�[�h�A�E�g���J�n
        /// </summary>
        /// <param name="fadeTime">�t�F�[�h�A�E�g�ɂ����鎞��</param>
        public void FadeOut(float fadeTime = 0.5f)
        {
            m_FadeTimer = 0.0f;
            enabled = true;
            m_Fading = true;
            m_FadeTime = fadeTime;
            m_IsFadeIn = false;
            m_FadeImage.color = Color.clear;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// �t�F�[�h���������s
        /// �� �X�V�������ŌĂ�
        /// </summary>
        public void Run()
        {
            m_BeforeFading = m_Fading;

            if (!m_Fading)
            {
                return;
            }
            else if (!m_BeforeFading)
            {
                m_BeforeFading = true;
                m_OnFadeBegin?.Invoke(m_IsFadeIn);
            }

            m_FadeTimer += Time.deltaTime;
            float alpha = m_FadeTimer / m_FadeTime;

            if (alpha >= 1.0f)
            {
                alpha = 1.0f;
                m_Fading = false;

                m_OnFadeFinished?.Invoke(m_IsFadeIn);

                if (m_IsFadeIn)
                {
                    gameObject.SetActive(false);
                }
            }

            if (m_IsFadeIn)
            {
                alpha = 1.0f - alpha;
            }

            m_FadeImage.color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }

        /// <summary>
        /// �t�F�[�h�J�n���ɌĂ΂�鏈����ǉ�
        /// </summary>
        /// <param name="action">�t�F�[�h�J�n���Ɏ��s���鏈��</param>
        public void OnFadeBegin(UnityAction<bool> action)
        {
            m_OnFadeBegin ??= new UnityEvent<bool>();

            m_OnFadeBegin.AddListener(action);
        }

        /// <summary>
        /// �t�F�[�h�J�n���ɌĂ΂�鏈�����폜
        /// </summary>
        public void RemoveOnFadeBegin()
        {
            m_OnFadeBegin.RemoveAllListeners();
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
        /// �t�F�[�h�p�w�i�������ɂ���
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