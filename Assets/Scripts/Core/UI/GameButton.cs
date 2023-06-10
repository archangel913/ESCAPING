namespace Escaping.Core.UI
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// ゲームで使用するボタン
    /// </summary>
    public class GameButton :
        MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler
    {
        private const float TweenTime = 0.1f;
        private const float TweenSize = 0.9f;

        [SerializeField]
        private Image m_Image;

        [SerializeField]
        private TextMeshProUGUI m_Text;

        private UnityEvent m_OnClick;
        private bool m_IsSetActions;

        /// <summary>
        /// 表示するテキスト
        /// </summary>
        public string Text
        {
            get => m_Text.text;
            set => m_Text.text = value;
        }

        /// <summary>
        /// ボタンの画像
        /// </summary>
        public Sprite Image
        {
            set => m_Image.sprite = value;
        }

        /// <summary>
        /// テキストを表示するかを設定
        /// </summary>
        /// <param name="isActive">テキストを表示するか</param>
        /// <returns>インスタンス</returns>
        public GameButton SetTextActive(bool isActive)
        {
            m_Text.gameObject.SetActive(isActive);

            return this;
        }

        /// <summary>
        /// クリック時の処理を追加
        /// </summary>
        /// <param name="onClicked">クリック時の処理</param>
        /// <param name="showWarning">警告を表示するか</param>
        /// <returns>インスタンス</returns>
        public GameButton AddPressed(UnityAction onClicked, bool showWarning = true)
        {
#if UNITY_EDITOR
            if (showWarning && m_IsSetActions)
            {
                Debug.LogWarning("Actions are set at the button already");
            }
#endif

            m_OnClick ??= new UnityEvent();
            m_OnClick.AddListener(onClicked);
            m_IsSetActions = true;

            return this;
        }

        /// <summary>
        /// クリック時の処理を削除
        /// </summary>
        /// <returns>インスタンス</returns>
        public GameButton RemovePressed()
        {
            m_OnClick.RemoveAllListeners();
            m_IsSetActions = false;

            return this;
        }

        /// <inheritdoc/>
        public void OnPointerDown(PointerEventData eventData)
        {
            transform.DOScale(new Vector2(TweenSize, TweenSize), TweenTime);
        }

        /// <inheritdoc/>
        public void OnPointerUp(PointerEventData eventData)
        {
            transform.DOScale(Vector2.one, TweenTime);
        }

        /// <inheritdoc/>
        public void OnPointerClick(PointerEventData eventData)
        {
            m_OnClick?.Invoke();
        }
    }
}
