using DG.Tweening;
using EasyUI.Ext;
using EasyWork.Utilities;
using Extend.System;
using Helper;
using Manager;
using Scripts.UIBase.Story;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities.Common;

namespace Module.Story.View
{
    /// <summary>
    /// Date    2020/12/24 21:36:07
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class StoryPanel : BaseStoryPanel
    {
        private StoryInfo m_storyInfo;
        private Queue<CharIconData> m_iconDataQueue = new Queue<CharIconData>();

        private Tween m_dialogTween;

        private bool m_nextDialog = false;
        private bool m_isWaiting => !m_dialogTween.IsPlaying();

        protected override void OnEnter(params object[] param)
        {
            m_storyInfo = param.Get<StoryInfo>();
            if (m_storyInfo == null)
            {
                return;
            }

            ECoroutine.StartCoroutine(StartDialog(m_storyInfo));
        }

        private IEnumerator StartDialog(StoryInfo m_storyInfo)
        {
            for (int i = 0; i < m_storyInfo.DialogInfos.Count; i++)
            {
                m_nextDialog = false;

                var dialog = m_storyInfo.DialogInfos[i];

                SetDialog(dialog);

                if(dialog.Options != null)
                {
                    SetOption(dialog.Options);
                }

                while (!m_nextDialog)
                {
                    yield return null;
                }
            }
        }

        protected override void OnInit()
        {
            m_iconDataQueue.Enqueue(new CharIconData()
            {
                CG = hlGroup_char01.GetComponent<CanvasGroup>(),
                DisLG = new LayoutGroupExt<BaseStoryCharCom, CharInfo>(hlGroup_char01, ItemRender)
            });
            m_iconDataQueue.Enqueue(new CharIconData()
            {
                CG = hlGroup_char02.GetComponent<CanvasGroup>(),
                DisLG = new LayoutGroupExt<BaseStoryCharCom, CharInfo>(hlGroup_char02, ItemRender)
            });
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (m_isWaiting)
                {
                    m_nextDialog = true;
                }
                else
                {
                    m_dialogTween.Complete();
                }
            }
        }

        private GameObject ItemFactory()
        {
            return null;
        }

        private void ItemRender(int index, BaseStoryCharCom com, CharInfo data)
        {
            com.image_char.SetIcon(data.IconUrl);
            com.image_char.color = data.Gray ? Color.gray : Color.white;
        }

        private void Switch(List<CharInfo> infos)
        {
            var data = m_iconDataQueue.Dequeue();
            Fade(data.CG, false);
            m_iconDataQueue.Enqueue(data);
            data = m_iconDataQueue.Peek();
            data.DisLG.Data = infos;
            Fade(data.CG, true);
        }

        private void Fade(CanvasGroup canvasGroup, bool show)
        {
            canvasGroup.alpha = show ? 0.0f : 1.0f;

            var tween = canvasGroup.DOFade(show ? 1.0f : 0.0f, 0.3f).SetEase(Ease.Linear);
            tween.onComplete += () =>
            {
                canvasGroup.alpha = show ? 1.0f : 0.0f;
            };
        }

        private List<CharInfo> m_charInfos = new List<CharInfo>();
        private void SetDialog(DialogInfo dialogInfo)
        {
            m_charInfos.Clear();
            var data = dialogInfo.CharIconInfo.Split(StrHelper.SPLIT_CHAR_1);

            for (int i = 0; i < data.Length; i++)
            {
                var temp = new CharInfo();
                var charInfoData = data[i].Split(StrHelper.SPLIT_CHAR_2);
                temp.Gray = charInfoData[1] == "0";
                temp.IconUrl = charInfoData[0];
                m_charInfos.Add(temp);
            }

            Switch(m_charInfos);

            text_char.text = dialogInfo.Character;
            text_dialog.text = string.Empty;

            m_dialogTween?.Kill();
            m_dialogTween = text_dialog.DOText(dialogInfo.Context, dialogInfo.Context.Length * 0.05f).SetEase(Ease.Linear).SetAutoKill(false);
        }

        private void SetOption(List<OptionInfo> options)
        {
            SetDialog(options[0].NextDialog);
        }
    }

    class CharIconData
    {
        public LayoutGroupExt<BaseStoryCharCom, CharInfo> DisLG;
        public CanvasGroup CG;
    }

    class CharInfo
    {
        public string IconUrl;
        public bool Gray;
    }
}

public static class ImageExt
{
    public static void SetIcon(this Image @this, string url)
    {

    }
}