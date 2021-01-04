using Config;
using EasyUI;
using Manager.Res;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    /// <summary>
    /// Date    2020/12/21 17:32:02
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public static partial class PanelManager
    {

        private static Stack<BasePanel> g_panelStack = new Stack<BasePanel>();
        private static Dictionary<string, BasePanel> g_cachePanel = new Dictionary<string, BasePanel>();

        public static UIRoot G_Root; 

        public static void Open(string name, params object[] param)
        {
            if(G_Root == null)
            {
                Debug.LogError($"the ui root cannot be empty");
                return;
            }

            if (IsInStack(name))
            {
                Debug.LogError($"the panel:{name} has in stack");
                return;
            }

            if (g_cachePanel.TryGetValue(name, out var panel))
            {
                ChangeTopPanel(panel, param);
                return;
            }

            var cfg = ResBinderDao.Inst.GetCfg(name);
            if(cfg == null)
            {
                return;
            }

            ResManager.LoadAssetAsync(cfg.Url, loader =>
            {
                var go = loader.Get<GameObject>();
                var panelGo = GameObject.Instantiate(go);

                var basePanel = panelGo.GetComponent<BasePanel>();

                if (basePanel == null)
                {
                    Debug.LogError($"unfind panelCom from panel:{basePanel.PanelName}");
                    return;
                }

                basePanel.Init();

                ChangeTopPanel(basePanel, param);
            });
        }

        public static void CloseTopPanel(bool cache = true)
        {
            if(IsEmptyStack())
            {
                Debug.LogError("panel stack is empty");
                return;
            }

            var popPanel = g_panelStack.Pop();
            popPanel.Close();

            if(!IsEmptyStack())
            {
                g_panelStack.Peek().Resume();
            }

            if (cache)
            {
                g_cachePanel.Add(popPanel.PanelName, popPanel);
            }
            else
            {
                var cfg = ResBinderDao.Inst.GetCfg(popPanel.PanelName);
                ResManager.ReleaseAsset(cfg.Key);

                GameObject.Destroy(popPanel.gameObject);
            }
        }

        public static void ReleaseCache()
        {
            while (!IsEmptyStack())
            {
                var popPanel = g_panelStack.Pop();
                var cfg = ResBinderDao.Inst.GetCfg(popPanel.PanelName);
                ResManager.ReleaseAsset(cfg.Key);

                GameObject.Destroy(popPanel.gameObject);
            }
        }

        private static void ChangeTopPanel(BasePanel basePanel, params object[] vo)
        {
            if(!IsEmptyStack())
            {
                g_panelStack.Peek().Pause();
            }

            basePanel.Open(vo);

            g_panelStack.Push(basePanel);
            G_Root.AddChild(basePanel);
        }

        private static bool IsEmptyStack()
        {
            return g_panelStack.Count == 0;
        }

        private static bool IsInStack(string name)
        {
            foreach (var panel in g_panelStack)
            {
                if(Equals(panel.PanelName, name))
                {
                    return true;
                }
            }

            return false;
        }
    }
}