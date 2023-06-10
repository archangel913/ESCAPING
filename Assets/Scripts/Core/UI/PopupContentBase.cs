namespace Escaping.Core.UI
{
    using UnityEngine;

    /// <summary>
    /// ポップアップに表示するコンテンツ
    /// </summary>
    public class PopupContentBase : MonoBehaviour
    {
        private RectTransform m_RectTransform;

        /// <summary>
        /// ポップアップを開くときの処理
        /// </summary>
        public interface IOpenPopup
        {
            /// <summary>
            /// ポップアップを開くときの処理
            /// </summary>
            void OnOpenBegin();

            /// <summary>
            /// ポップアップを開いた後の処理
            /// </summary>
            void OnOpenEnd();
        }

        /// <summary>
        /// ポップアップが閉るときの処理
        /// </summary>
        public interface IClosePopup
        {
            /// <summary>
            /// ポップアップが閉るときの処理
            /// </summary>
            void OnCloseBegin();

            /// <summary>
            /// ポップアップが閉じたときの処理
            /// </summary>
            void OnCloseEnd();
        }

        /// <summary>
        /// Unity のアップデート処理
        /// </summary>
        public interface IUnityUpdate
        {
            /// <summary>
            /// アップデート処理
            /// </summary>
            void OnUpdate();
        }

        /// <summary>
        /// コンテンツのサイズをポップアップに合わせる
        /// </summary>
        /// <param name="size">ポップアップのサイズ</param>
        /// <returns>変更倍率</returns>
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
