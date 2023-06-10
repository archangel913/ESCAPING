namespace Escaping.Core.UI
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// �|�b�v�A�b�v�Ǘ��N���X
    /// </summary>
    public sealed class PopupManager : Singleton<PopupManager>
    {
        private SortedList<uint, Popup> m_Popups;
        private uint m_MaxID;

        /// <summary>
        /// �|�b�v�A�b�v��ǉ�
        /// </summary>
        /// <param name="popup">�|�b�v�A�b�v</param>
        /// <returns>�|�b�v�A�b�vID</returns>
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
        /// �|�b�v�A�b�v���폜
        /// </summary>
        /// <param name="popupId">�|�b�v�A�b�vID</param>
        /// <returns>�폜�ɐ���������</returns>
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
        /// �X�V����
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
