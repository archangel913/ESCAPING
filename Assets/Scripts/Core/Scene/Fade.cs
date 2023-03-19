namespace Escaping.Core
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    /// <summary>
    /// フェード表示クラス
    /// </summary>
    public class Fade : SingletonMonoBehaviour<Fade>
    {
        private Image m_FadeImage;

        private UnityEvent<bool> m_OnFadeFinished;

        /// <summary>
        /// フェードインを開始
        /// </summary>
        /// <param name="fadeTime">フェードインにかける時間</param>
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
        /// フェードアウトを開始
        /// </summary>
        /// <param name="fadeTime">フェードアウトにかける時間</param>
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
        /// フェード完了時に呼ばれる処理を追加
        /// </summary>
        /// <param name="action">フェード完了時に実行処理</param>
        public void AddOnFadeFinished(UnityAction<bool> action)
        {
            m_OnFadeFinished ??= new UnityEvent<bool>();

            m_OnFadeFinished.AddListener(action);
        }

        /// <summary>
        /// フェード完了時に呼ばれる処理を削除
        /// </summary>
        public void RemoveOnFadeFinished()
        {
            m_OnFadeFinished.RemoveAllListeners();
        }

        /// <summary>
        /// フェード用背景を透明にする
        /// </summary>
        public void Hide()
        {
            m_FadeImage.color = Color.clear;
            enabled = false;
        }

        /// <summary>
        /// フェード用背景を黒にする
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