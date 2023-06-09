namespace Escaping.Core
{
    using DG.Tweening;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// ロード表示クラス
    /// </summary>
    public class Loading : SingletonMonoBehaviour<Loading>
    {
        private Image m_LoadingImage;

        /// <summary>
        /// ロード画面を表示
        /// </summary>
        public void Show()
        {
            m_LoadingImage.enabled = true;
            gameObject.SetActive(true);

            // TODO: ロード中の表示が確定したらそれに変更
            m_LoadingImage.rectTransform
                .DOLocalRotate(new Vector3(0, 0, 360f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart);
        }

        /// <summary>
        /// ロード画面を非表示
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
