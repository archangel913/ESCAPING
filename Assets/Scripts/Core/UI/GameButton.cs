namespace Escaping.Core.UI
{
    using DG.Tweening;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// �Q�[���Ŏg�p����{�^��
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
        /// �\������e�L�X�g
        /// </summary>
        public string Text
        {
            get => m_Text.text;
            set => m_Text.text = value;
        }

        /// <summary>
        /// �{�^���̉摜
        /// </summary>
        public Sprite Image
        {
            set => m_Image.sprite = value;
        }

        /// <summary>
        /// �e�L�X�g��\�����邩��ݒ�
        /// </summary>
        /// <param name="isActive">�e�L�X�g��\�����邩</param>
        /// <returns>�C���X�^���X</returns>
        public GameButton SetTextActive(bool isActive)
        {
            m_Text.gameObject.SetActive(isActive);

            return this;
        }

        /// <summary>
        /// �N���b�N���̏�����ǉ�
        /// </summary>
        /// <param name="onClicked">�N���b�N���̏���</param>
        /// <param name="showWarning">�x����\�����邩</param>
        /// <returns>�C���X�^���X</returns>
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
        /// �N���b�N���̏������폜
        /// </summary>
        /// <returns>�C���X�^���X</returns>
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
