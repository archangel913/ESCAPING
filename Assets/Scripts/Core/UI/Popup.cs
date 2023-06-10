namespace Escaping.Core.UI
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Escaping.Core.FileSystems;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// �|�b�v�A�b�v�N���X
    /// </summary>
    public class Popup : MonoBehaviour
    {
        private const string PrefabPath = "Prefabs/UI/Popup";
        private const float TweenTime = 0.3f;

        [Header("Popup")]
        [SerializeField]
        private TextMeshProUGUI m_Title;

        [SerializeField]
        private Transform m_ContentsParent;

        [SerializeField]
        private GameButton m_PositiveButton;

        [SerializeField]
        private GameButton m_NegativeButton;

        [SerializeField]
        private GameButton m_OtherButton;

        private PopupContentBase m_Content;
        private Size m_Size;

        private uint m_ID;

        /// <summary>
        /// �{�^���̎��
        /// </summary>
        public enum ButtonType
        {
            /// <summary>
            /// �|�W�e�B�u
            /// </summary>
            Positive,

            /// <summary>
            /// �l�K�e�B�u
            /// </summary>
            Negative,

            /// <summary>
            /// ���̑�
            /// </summary>
            Other,
        }

        /// <summary>
        /// �|�b�v�A�b�v�^�C�g��
        /// </summary>
        public string Title
        {
            get => m_Title.text;
            set => m_Title.text = value;
        }

        /// <summary>
        /// �|�b�v�A�b�v�𐶐�
        /// </summary>
        /// <param name="prefab">�R���e���c�̃v���n�u</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public static async UniTask<Popup> InstantiatePopup(PopupContentBase prefab)
        {
            return await InstantiatePopup(prefab, Size.Midium);
        }

        /// <summary>
        /// �|�b�v�A�b�v�𐶐�
        /// </summary>
        /// <param name="prefab">�R���e���c�̃v���n�u</param>
        /// <param name="size">�|�b�v�A�b�v�̃T�C�Y</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public static async UniTask<Popup> InstantiatePopup(PopupContentBase prefab, Size size)
        {
            var popup = await InstantiatePopup();
            popup.SetSize(size);
            popup.m_ID = PopupManager.Instance.AddPopup(popup);

            var content = Instantiate(prefab, popup.m_ContentsParent);
            popup.m_Content = content;

            return popup;
        }

        /// <summary>
        /// �|�b�v�A�b�v�𐶐�
        /// </summary>
        /// <param name="prefabPath">�v���n�u�̃p�X</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public static async UniTask<Popup> InstantiatePopup(string prefabPath)
        {
            var prefab = FileLoader.LoadAssetAsync<PopupContentBase>(prefabPath);
            return await InstantiatePopup(await prefab, Size.Midium);
        }

        /// <summary>
        /// �|�b�v�A�b�v�𐶐�
        /// </summary>
        /// <param name="prefabPath">�v���n�u�̃p�X</param>
        /// <param name="size">�|�b�v�A�b�v�̃T�C�Y</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public static async UniTask<Popup> InstantiatePopup(string prefabPath, Size size)
        {
            var prefab = FileLoader.LoadAssetAsync<PopupContentBase>(prefabPath);
            return await InstantiatePopup(await prefab, size);
        }

        /// <summary>
        /// �^�C�g����ݒ�
        /// </summary>
        /// <param name="title">�^�C�g��</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public Popup SetTitle(string title)
        {
            Title = title;

            return this;
        }

        /// <summary>
        /// �T�C�Y��ݒ�
        /// </summary>
        /// <param name="size">�T�C�Y</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public Popup SetSize(Size size)
        {
            m_Size = size;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = m_Size.GetSize();

            return this;
        }

        /// <summary>
        /// �T�C�Y���擾
        /// </summary>
        /// <returns>�T�C�Y</returns>
        public Size GetSize()
        {
            return m_Size;
        }

        /// <summary>
        /// �R���e���c���擾
        /// </summary>
        /// <typeparam name="T">�R���e���c�̌^</typeparam>
        /// <returns>�R���e���c</returns>
        public T GetContent<T>()
            where T : PopupContentBase
        {
            return m_Content as T;
        }

        /// <summary>
        /// �R���e���c���擾
        /// </summary>
        /// <returns>�R���e���c</returns>
        public PopupContentBase GetContent()
        {
            return m_Content;
        }

        /// <summary>
        /// �|�b�v�A�b�v���J��
        /// </summary>
        public void Open()
        {
            if (m_Content is PopupContentBase.IOpenPopup open)
            {
                open.OnOpenBegin();
            }

            transform.localScale = Vector2.zero;
            transform
                .DOScale(Vector2.one, TweenTime)
                .SetEase(Ease.OutBack)
                .OnComplete(() => {
                    if (m_Content is PopupContentBase.IOpenPopup open)
                    {
                        open.OnOpenEnd();
                    }
                });
        }

        /// <summary>
        /// �|�b�v�A�b�v�����
        /// </summary>
        public void Close()
        {
            if (m_Content is PopupContentBase.IClosePopup close)
            {
                close.OnCloseBegin();
            }

            transform.localScale = Vector2.one;
            transform
                .DOScale(Vector2.zero, TweenTime)
                .SetEase(Ease.InBack)
                .OnComplete(() => {
                    if (m_Content is PopupContentBase.IClosePopup close)
                    {
                        close.OnCloseEnd();
                    }

                    PopupManager.Instance.RemovePopup(m_ID);
                });
        }

        /// <summary>
        /// �{�^����ǉ�
        /// </summary>
        /// <param name="text">�{�^���ɕ\������e�L�X�g</param>
        /// <param name="type">�{�^���̎��</param>
        /// <param name="action">�N���b�N���̏���</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public Popup AddButton(string text, ButtonType type, UnityAction action)
        {
            var button = GetButton(type);

            if (button is null)
            {
                return this;
            }

            button.AddPressed(action);
            button.Text = text;
            button.gameObject.SetActive(true);

            return this;
        }

        /// <summary>
        /// ����{�^����ǉ�
        /// </summary>
        /// <param name="onClicked">�N���b�N���̏���</param>
        /// <param name="type">�{�^���̎��</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public Popup AddCloseButton(UnityAction onClicked = null, ButtonType type = ButtonType.Negative)
        {
            return AddButton("����", type, () => {
                onClicked?.Invoke();
                Close();
            });
        }

        /// <summary>
        /// OK�{�^����ǉ�
        /// </summary>
        /// <param name="onClicked">�N���b�N���̏���</param>
        /// <param name="type">�{�^���̎��</param>
        /// <returns>�|�b�v�A�b�v</returns>
        public Popup AddOKButton(UnityAction onClicked = null, ButtonType type = ButtonType.Positive)
        {
            return AddButton("OK", type, () => {
                onClicked?.Invoke();
                Close();
            });
        }

        private static async UniTask<Popup> InstantiatePopup()
        {
            var prefab = FileLoader.LoadAssetAsync<Popup>(PrefabPath);
            return Instantiate(await prefab, Game.Instance.PopupParent);
        }

        private GameButton GetButton(ButtonType type)
        {
            return type switch {
                ButtonType.Positive => m_PositiveButton,
                ButtonType.Negative => m_NegativeButton,
                ButtonType.Other => m_OtherButton,
                _ => null,
            };
        }

        /// <summary>
        /// �|�b�v�A�b�v�̃T�C�Y�N���X
        /// </summary>
        public class Size
        {
            private Vector2 m_Size;

            private Size()
            {
            }

            /// <summary>
            /// �������T�C�Y
            /// </summary>
            public static Size Small
            {
                get
                {
                    Size size = new Size();
                    size.m_Size = new Vector2(400, 300);
                    return size;
                }
            }

            /// <summary>
            /// �W���T�C�Y
            /// </summary>
            public static Size Midium
            {
                get
                {
                    Size size = new Size();
                    size.m_Size = new Vector2(700, 450);
                    return size;
                }
            }

            /// <summary>
            /// �傫���T�C�Y
            /// </summary>
            public static Size Large
            {
                get
                {
                    Size size = new Size();
                    size.m_Size = new Vector2(1000, 600);
                    return size;
                }
            }

            /// <summary>
            /// Vector2�ŃT�C�Y���擾
            /// </summary>
            /// <returns>�T�C�Y</returns>
            public Vector2 GetSize()
            {
                return m_Size;
            }

            /// <summary>
            /// �R���e���c�̃T�C�Y���擾
            /// </summary>
            /// <param name="size">�|�b�v�A�b�v�̃T�C�Y</param>
            /// <returns>�R���e���c�̃T�C�Y</returns>
            public Vector2 GetContentSize()
            {
                var size = GetSize();
                var ret = Vector2.zero;
                ret.x = size.x - 20;
                ret.y = size.y - 110;

                return ret;
            }
        }
    }
}
