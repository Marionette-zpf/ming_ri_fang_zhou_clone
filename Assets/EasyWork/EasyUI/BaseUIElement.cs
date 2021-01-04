using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EasyUI
{
    /// <summary>
    /// Date    2020/12/22 15:08:27
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract class BaseUIElement : MonoBehaviour, IPointerClickHandler
    {
        public Action<BaseUIElement, PointerEventData> ClickEvent;

        private RectTransform m_rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (m_rectTransform == null)
                {
                    m_rectTransform = GetComponent<RectTransform>();
                }

                return m_rectTransform;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent?.Invoke(this, eventData);
        }
    }
}