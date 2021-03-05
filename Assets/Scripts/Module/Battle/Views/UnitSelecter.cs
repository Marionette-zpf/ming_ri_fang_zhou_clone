using EasyWork.Extend.Utilities;
using GameEvent;
using Module.Battle.Com;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Module.Battle.Views
{
    /// <summary>
    /// Date    2021/3/5 12:53:32
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class UnitSelecter : MonoBehaviour
    {
        public GameObject UnitFront;
        public GameObject UnitBack;

        public float CD = 5.0f;

        private Button m_button;
        private Image m_cdMask;

        private CountDownCom m_cdCountDown;

        private bool m_layoutComplete = true;

        void Awake()
        {
            m_button = GetComponent<Button>();
            m_cdMask = transform.Find("CD").GetComponent<Image>();

            m_button.onClick.AddListener(OnClickHandle);

            m_cdCountDown = new CountDownCom();
            m_cdCountDown.SetCount(CD);
            m_cdCountDown.Complete();

            EEventUtil.Subscribe<UnitLayoutComplete>(LayoutCompleteHandle);
        }


        private void Update()
        {
            m_cdMask.fillAmount = m_cdCountDown.Percent;
        }

        private void OnClickHandle()
        {
            if(!m_cdCountDown.IsComplete || !m_layoutComplete)
            {
                return;
            }

            m_layoutComplete = false;
            EEventUtil.Dispatch(new UnitLayoutEvent() { FrontUnitObj = UnitFront , BackUnitObj = UnitBack });
        }


        private void LayoutCompleteHandle()
        {
            m_layoutComplete = true;
            m_cdCountDown.ReStart();
        }
    }
}