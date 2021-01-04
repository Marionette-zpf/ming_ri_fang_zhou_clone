using Manager.Res;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    /// <summary>
    /// Date    2021/1/1 19:02:36
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public static class ESceneManager
    {
        public static event Action<string> SceneLoaded;

        private static Dictionary<string, PreLoadInfo> g_preLoadInfoMap = new Dictionary<string, PreLoadInfo>() 
        { 
            { "Story", new PreLoadInfo() } 
        };
        private static List<string> g_releaseAssets = new List<string>();

        public static void ConfigScenes(params string[] scene)
        {
            for (int i = 0; i < scene.Length; i++)
            {
                var sceneName = scene[i];

                if (!g_preLoadInfoMap.TryGetValue(sceneName, out var targetInfo))
                {
                    targetInfo = new PreLoadInfo();
                    g_preLoadInfoMap.Add(sceneName, targetInfo);
                }
            }
        }

        public static void LoadSceneAsync(
            string scene,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            Loader.ProcessHandle assetLoadProcessHandle = null,
            Loader.ProcessHandle sceneLoadProcessHandle = null,
            Loader.LoadedHandle sceneLoadedHandle = null)
        {
            if (!g_preLoadInfoMap.TryGetValue(scene, out var targetInfo))
            {
                Debug.LogError($"un config scene:{scene}");
                return;
            }

            //重复加载场景
            if (targetInfo.Using)
            {
                Debug.LogError($"scene:{scene} has loaded");
                return;
            }

            targetInfo.Using = true;

            //卸载当前场景
            ResManager.ReleaseAsset(scene);

            LoadSceneAssetsAsync(targetInfo, loadSceneMode, _ =>
            {
                ResManager.LoadSceneAsync(scene, loadSceneMode, loader =>
                { 
                    sceneLoadedHandle?.Invoke(loader);
                    SceneLoaded?.Invoke(scene);
                }, sceneLoadProcessHandle);
            }, assetLoadProcessHandle);
        }



        public static void AddPreloadAsset(string scene, params string[] assetUrls)
        {
            for (int i = 0; i < assetUrls.Length; i++)
            {
                AddPreloadAsset(scene, assetUrls[i]);
            }
        }

        public static void AddPreloadAsset(string scene, string assetUrl)
        {
            if (g_preLoadInfoMap.TryGetValue(scene, out var preLoadInfo) && !preLoadInfo.Using && !preLoadInfo.PreloadAssetUrls.Contains(assetUrl))
            {
                preLoadInfo.PreloadAssetUrls.Add(assetUrl);
            }
        }

        public static void RemovePreloadAsset(string scene, params string[] assetUrls)
        {
            for (int i = 0; i < assetUrls.Length; i++)
            {
                RemovePreloadAsset(scene, assetUrls[i]);
            }
        }

        public static void RemovePreloadAsset(string scene, string assetUrl)
        {
            if (g_preLoadInfoMap.TryGetValue(scene, out var preLoadInfo) && !preLoadInfo.Using && preLoadInfo.PreloadAssetUrls.Contains(assetUrl))
            {
                preLoadInfo.PreloadAssetUrls.Remove(assetUrl);
            }
        }

        private static void LoadSceneAssetsAsync(
            PreLoadInfo targetInfo,
            LoadSceneMode loadSceneMode = LoadSceneMode.Single,
            LoaderGroup.WhenAllHandle whenAllHandle = null,
            Loader.ProcessHandle processHandle = null)
        {
            g_releaseAssets.Clear();

            //卸载无用数据
            if (loadSceneMode == LoadSceneMode.Single)
            {
                foreach (var preloadInfoPair in g_preLoadInfoMap)
                {
                    var preloadInfo = preloadInfoPair.Value;

                    if (!preloadInfo.Using)
                    {
                        break;
                    }

                    preloadInfo.Using = false;

                    foreach (var url in preloadInfo.PreloadAssetUrls)
                    {
                        if (targetInfo.PreloadAssetUrls.Contains(url))
                        {
                            continue;
                        }

                        g_releaseAssets.Add(url);
                    }
                }

                //卸载数据
                for (int i = 0; i < g_releaseAssets.Count; i++)
                {
                    ResManager.ReleaseAsset(g_releaseAssets[i]);
                }
            }

            var loaderGroup = new LoaderGroup(whenAllHandle, processHandle, targetInfo.PreloadAssetUrls.ToArray());
            loaderGroup.Load();
        }
    }

    public class PreLoadInfo
    {
        public HashSet<string> PreloadAssetUrls = new HashSet<string>();
        public bool Using = false;
        public Loader SceneLoader;
    }
}