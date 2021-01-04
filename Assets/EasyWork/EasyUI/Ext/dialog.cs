using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using EasyWork.Utilities;

namespace Test
{
    /// <summary>
    /// Date    2020/12/23 15:10:54
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class Dialog : MonoBehaviour
    {
        public HorizontalLayoutGroup layoutGroup;

        public RectTransform loader;

        public GameObject Prefab;

        public int Count;

        public Text TxtDialog;

        public CanvasGroup m_inCanvasGroup;
        public CanvasGroup m_outCanvasGroup;

        public bool IsWaiting => !m_dialogTween.IsPlaying();

        private Queue<GameObject> m_inLayout = new Queue<GameObject>();
        private Queue<GameObject> m_outLayout = new Queue<GameObject>();

        private EPool<GameObject> m_cache = new EPool<GameObject>();

        private Tween m_dialogTween;

        [ContextMenu("Add")]
        public void Add()
        {
            for (int i = 0; i < Count; i++)
            {
                m_inLayout.Enqueue(m_outLayout.Count != 0 ? m_outLayout.Dequeue() : Instantiate(Prefab));
            }
            Layout(m_inLayout);
            Fade(m_inCanvasGroup, m_inLayout, true);
        }


        [ContextMenu("Remove")]
        public void OnRemove()
        {
            while (m_inLayout.Count != 0)
            {
                m_outLayout.Enqueue(m_inLayout.Dequeue());
            }

            Fade(m_outCanvasGroup, m_outLayout, false);
        }

        private void DoDialog(string dialog)
        {
            m_dialogTween?.Kill();
            m_dialogTween = TxtDialog.DOText(dialog, dialog.Length * 0.2f).SetEase(Ease.Linear).SetAutoKill(false);
        }

        private void Layout(Queue<GameObject> layoutObjs)
        {
            LayoutHelper.HorizontalLayout(GetComponent<RectTransform>().position, layoutObjs.Select(go=> go.GetComponent<RectTransform>()).ToList(), 200);
        }

        private void Fade(CanvasGroup canvasGroup, Queue<GameObject> fadeObjs, bool show)
        {
            foreach (var item in fadeObjs)
            {
                item.transform.SetParent(canvasGroup.transform);
            }
            canvasGroup.alpha = show ? 0.0f : 1.0f;

            var tween = canvasGroup.DOFade(show ? 1.0f : 0.0f, 0.5f).SetEase(Ease.Linear);
            tween.onComplete += () =>
            {
                canvasGroup.alpha = show ? 1.0f : 0.0f;
            };
        }
    }

    
    public static class LayoutHelper
    {
        public static void HorizontalLayout(Vector3 center, IEnumerable<RectTransform> elements, float padding)
        {
            int count = elements.Count();
            int index = 0;

            if (count == 0)
            {
                return;
            }

            int halfCount = (int)(count / 2.0f);
            var left = center - (halfCount + (count % 2 == 0 ? -0.5f : 0)) * padding * Vector3.right;

            foreach (var element in elements)
            {
                element.position = left + index * padding * Vector3.right;
                index++;
            }
        }
    }
}