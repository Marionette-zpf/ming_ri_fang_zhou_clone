using Manager;
using UnityEngine;
using Utilities.Common;

namespace EasyUI
{
    /// <summary>
    /// Date    2020/12/21 17:32:56
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract class BasePanel : BaseUIElement
    {
        private bool m_isEnable = false;

        public virtual string PanelName => GetType().ToString();

        public virtual void Init()
        {
            OnInit();
        }

        public virtual void Open(params object[] param)
        {
            SetEnable(true);
            OnEnter(param);
        }

        public virtual void Close()
        {
            SetEnable(false);
            RemoveFromParent();
            OnExit();
        }

        public virtual void Resume()
        {
            SetEnable(true);
            OnResume();
        }

        public virtual void Pause()
        {
            SetEnable(false);
            OnPause();
        }

        public void SetEnable(bool enable)
        {
            if (enable == m_isEnable)
            {
                return;
            }
            m_isEnable = enable;

            gameObject.SetActive(m_isEnable);
        }

        public void RemoveFromParent()
        {
            RectTransform.parent = null;
        }

        protected virtual void OnInit() { }
        protected virtual void OnEnter(params object[] param) { }
        protected virtual void OnPause() { }
        protected virtual void OnResume() { }
        protected virtual void OnExit() { }
        protected virtual void OnReleadse() { }

        private void OnDestroy()
        {
            OnReleadse();
        }

    }
}