namespace Escaping.Core.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// ポップアップ管理クラス
    /// </summary>
    public sealed class PopupManager : Singleton<PopupManager>
    {
        private SortedList<uint, Popup> m_Popups;
        private uint m_MaxID;

        /// <summary>
        /// ポップアップを追加
        /// </summary>
        /// <param name="popup">ポップアップ</param>
        /// <returns>ポップアップID</returns>
        public uint AddPopup(Popup popup)
        {
            m_Popups ??= new SortedList<uint, Popup>(1);

            if (m_Popups.Count == 0)
            {
                Game.Instance.ShowPopup();
            }

            m_Popups.Add(++m_MaxID, popup);

            return m_MaxID;
        }

        /// <summary>
        /// ポップアップを削除
        /// </summary>
        /// <param name="popupId">ポップアップID</param>
        /// <returns>削除に成功したか</returns>
        public bool RemovePopup(uint popupId)
        {
            try
            {
                var popup = m_Popups[popupId];
                Object.Destroy(popup.gameObject);
            }
            catch (KeyNotFoundException)
            {
                return false;
            }

            bool success = m_Popups.Remove(popupId);

            if (m_Popups.Count == 0)
            {
                Game.Instance.HidePopup();
            }

            return success;
        }

        /// <summary>
        /// 更新処理
        /// </summary>
        public void Update()
        {
            foreach (var popup in m_Popups)
            {
                if (popup.Value.GetContent() is PopupContentBase.IUnityUpdate update)
                {
                    update.OnUpdate();
                }
            }
        }
    }
}
