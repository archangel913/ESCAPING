namespace Escaping.Core.UI
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using Escaping.Core.FileSystems;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// ポップアップクラス
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
        /// ボタンの種類
        /// </summary>
        public enum ButtonType
        {
            /// <summary>
            /// ポジティブ
            /// </summary>
            Positive,

            /// <summary>
            /// ネガティブ
            /// </summary>
            Negative,

            /// <summary>
            /// その他
            /// </summary>
            Other,
        }

        /// <summary>
        /// ポップアップタイトル
        /// </summary>
        public string Title
        {
            get => m_Title.text;
            set => m_Title.text = value;
        }

        /// <summary>
        /// ポップアップを生成
        /// </summary>
        /// <param name="prefab">コンテンツのプレハブ</param>
        /// <returns>ポップアップ</returns>
        public static async UniTask<Popup> InstantiatePopup(PopupContentBase prefab)
        {
            return await InstantiatePopup(prefab, Size.Midium);
        }

        /// <summary>
        /// ポップアップを生成
        /// </summary>
        /// <param name="prefab">コンテンツのプレハブ</param>
        /// <param name="size">ポップアップのサイズ</param>
        /// <returns>ポップアップ</returns>
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
        /// ポップアップを生成
        /// </summary>
        /// <param name="prefabPath">プレハブのパス</param>
        /// <returns>ポップアップ</returns>
        public static async UniTask<Popup> InstantiatePopup(string prefabPath)
        {
            var prefab = FileLoader.LoadAssetAsync<PopupContentBase>(prefabPath);
            return await InstantiatePopup(await prefab, Size.Midium);
        }

        /// <summary>
        /// ポップアップを生成
        /// </summary>
        /// <param name="prefabPath">プレハブのパス</param>
        /// <param name="size">ポップアップのサイズ</param>
        /// <returns>ポップアップ</returns>
        public static async UniTask<Popup> InstantiatePopup(string prefabPath, Size size)
        {
            var prefab = FileLoader.LoadAssetAsync<PopupContentBase>(prefabPath);
            return await InstantiatePopup(await prefab, size);
        }

        /// <summary>
        /// タイトルを設定
        /// </summary>
        /// <param name="title">タイトル</param>
        /// <returns>ポップアップ</returns>
        public Popup SetTitle(string title)
        {
            Title = title;

            return this;
        }

        /// <summary>
        /// サイズを設定
        /// </summary>
        /// <param name="size">サイズ</param>
        /// <returns>ポップアップ</returns>
        public Popup SetSize(Size size)
        {
            m_Size = size;

            var rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = m_Size.GetSize();

            return this;
        }

        /// <summary>
        /// サイズを取得
        /// </summary>
        /// <returns>サイズ</returns>
        public Size GetSize()
        {
            return m_Size;
        }

        /// <summary>
        /// コンテンツを取得
        /// </summary>
        /// <typeparam name="T">コンテンツの型</typeparam>
        /// <returns>コンテンツ</returns>
        public T GetContent<T>()
            where T : PopupContentBase
        {
            return m_Content as T;
        }

        /// <summary>
        /// コンテンツを取得
        /// </summary>
        /// <returns>コンテンツ</returns>
        public PopupContentBase GetContent()
        {
            return m_Content;
        }

        /// <summary>
        /// ポップアップを開く
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
        /// ポップアップを閉じる
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
        /// ボタンを追加
        /// </summary>
        /// <param name="text">ボタンに表示するテキスト</param>
        /// <param name="type">ボタンの種類</param>
        /// <param name="action">クリック時の処理</param>
        /// <returns>ポップアップ</returns>
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
        /// 閉じるボタンを追加
        /// </summary>
        /// <param name="onClicked">クリック時の処理</param>
        /// <param name="type">ボタンの種類</param>
        /// <returns>ポップアップ</returns>
        public Popup AddCloseButton(UnityAction onClicked = null, ButtonType type = ButtonType.Negative)
        {
            return AddButton("閉じる", type, () => {
                onClicked?.Invoke();
                Close();
            });
        }

        /// <summary>
        /// OKボタンを追加
        /// </summary>
        /// <param name="onClicked">クリック時の処理</param>
        /// <param name="type">ボタンの種類</param>
        /// <returns>ポップアップ</returns>
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
        /// ポップアップのサイズクラス
        /// </summary>
        public class Size
        {
            private Vector2 m_Size;

            private Size()
            {
            }

            /// <summary>
            /// 小さいサイズ
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
            /// 標準サイズ
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
            /// 大きいサイズ
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
            /// Vector2でサイズを取得
            /// </summary>
            /// <returns>サイズ</returns>
            public Vector2 GetSize()
            {
                return m_Size;
            }

            /// <summary>
            /// コンテンツのサイズを取得
            /// </summary>
            /// <param name="size">ポップアップのサイズ</param>
            /// <returns>コンテンツのサイズ</returns>
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
