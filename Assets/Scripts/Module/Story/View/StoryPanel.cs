using Config;
using DG.Tweening;
using EasyUI.Ext;
using EasyWork.Utilities;
using Extend.System;
using Helper;
using Manager;
using Manager.Res;
using Module.Story.Cache;
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
        private DialogFragment m_dialogFragent;
        private Queue<CharIconData> m_iconDataQueue = new Queue<CharIconData>();

        private Tween m_dialogTween;

        private bool m_nextDialog = false;
        private bool m_isWaiting => !m_dialogTween.IsPlaying();

        protected override void OnEnter(params object[] param)
        {
            m_dialogFragent = param.Get<DialogFragment>();
            if (m_dialogFragent == null)
            {
                return;
            }

            ECoroutine.StartCoroutine(StartDialog(m_dialogFragent));
        }

        private IEnumerator StartDialog(DialogFragment m_storyInfo)
        {
            for (int i = 0; i < m_storyInfo.DialogConfigs.Count; i++)
            {
                m_nextDialog = false;

                var dialogCfg = m_storyInfo.DialogConfigs[i];

                SetDialog(dialogCfg);

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

        private void ItemRender(int index, BaseStoryCharCom com, CharInfo data)
        {
            AddressableOpCache.Get(com.image_char)
                              .Set(loader => com.image_char.sprite = loader.Get<Sprite>(), data.IconUrl);
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

        private void SetDialog(DialogConfig dialogInfo)
        {
            m_charInfos.Clear();
            if(dialogInfo.CharacterPainting != null && dialogInfo.CharacterPainting.Count > 0)
            {
                for (int i = 0; i < dialogInfo.CharacterPainting.Count; i++)
                {
                    m_charInfos.Add(new CharInfo() { IconUrl = dialogInfo.CharacterPainting[i], Gray = dialogInfo.GreyPainting[i] });
                }
            }

            Switch(m_charInfos);

            var characterCfg = CharacterDao.Inst.GetCfg(dialogInfo.CharacterId);

            text_char.text = characterCfg.Name;
            text_dialog.text = string.Empty;

            m_dialogTween?.Kill();
            m_dialogTween = text_dialog.DOText(dialogInfo.Dialog, dialogInfo.Dialog.Length * 0.05f).SetEase(Ease.Linear).SetAutoKill(false);
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

public static class AddressableOpCache
{
    private static Dictionary<object, AddressableOp> g_cacheMap = new Dictionary<object, AddressableOp>();

    public static AddressableOp Get(object key)
    {
        if (!g_cacheMap.TryGetValue(key, out AddressableOp addressableOp))
        {
            addressableOp = new AddressableOp();
            g_cacheMap.Add(key, addressableOp);
        }

        return addressableOp;
    }
}

public class AddressableOp
{
    private Loader m_loader;

    private Loader.LoadedHandle m_loadedHandlel;

    public void Set(Loader.LoadedHandle loadedHandle, string url)
    {
        if (m_loader != null && m_loader.State == Loader.LoaderState.Loading)
        {
            m_loader.LoadedHandleAction -= m_loadedHandlel;
        }

        m_loader = ResManager.LoadAssetAsync(url);

        if (m_loader.State == Loader.LoaderState.Finish)
        {
            loadedHandle?.Invoke(m_loader);
        }
        else
        {
            m_loadedHandlel = loadedHandle;
            m_loader.LoadedHandleAction += m_loadedHandlel;
        }
    }
}