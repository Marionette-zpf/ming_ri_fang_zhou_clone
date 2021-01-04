using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager.Res
{
    /// <summary>
    /// Date    2021/1/1 19:00:35
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>

    public static class ResManager
    {
        public delegate void ReleaseLoader(Loader loader);

        public static event ReleaseLoader WhenReleaseLoader;

        private static Dictionary<string, Loader> g_loaderCacheMap = new Dictionary<string, Loader>();

        public static Loader LoadSceneAsync(string url, LoadSceneMode loadSceneMode = LoadSceneMode.Single, Loader.LoadedHandle loadedHandle = null, Loader.ProcessHandle processHandle = null)
        {
            return LoadAssetAsync(url, () => new SceneLoader(loadSceneMode), loadedHandle, processHandle);
        }

        public static Loader LoadAssetAsync(string url, Loader.LoadedHandle loadedHandle = null, Loader.ProcessHandle processHandle = null)
        {
            return LoadAssetAsync(url, () => new AssetLoader(), loadedHandle, processHandle);
        }

        private static Loader LoadAssetAsync(string url, Func<Loader> loaderCreator, Loader.LoadedHandle loadedHandle = null, Loader.ProcessHandle processHandle = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                return default;
            }

            if (g_loaderCacheMap.TryGetValue(url, out var loader))
            {
                switch (loader.State)
                {
                    case Loader.LoaderState.Loading:
                        loader.LoadedHandleAction += loadedHandle;
                        loader.ProcessHandleAction += processHandle;
                        break;
                    case Loader.LoaderState.Finish:
                        loadedHandle?.Invoke(loader);
                        processHandle?.Invoke(loader, 1);
                        break;
                    default:
                        //todo log
                        break;
                }
            }
            else
            {
                loader = loaderCreator.Invoke();
                loader.Init(url, loadedHandle, processHandle);
                loader.Load();

                g_loaderCacheMap.Add(url, loader);
            }

            return loader;
        }

        public static T LoadAsset<T>(string url)
        {
            if (!g_loaderCacheMap.TryGetValue(url, out var loader))
            {
                Debug.LogError("un cache asset");
                return default;
            }

            return loader.Get<T>();
        }

        public static void ReleaseAsset(string url)
        {
            if (g_loaderCacheMap.TryGetValue(url, out var loader))
            {
                WhenReleaseLoader?.Invoke(loader);
                loader.Release();
                g_loaderCacheMap.Remove(url);
            }
        }

        public static void ClearCache(HashSet<string> mask = null)
        {
            List<string> releaseAssets = new List<string>();

            foreach (var loader in g_loaderCacheMap.Values)
            {
                if (mask != null && mask.Contains(loader.Url))
                {
                    continue;
                }

                releaseAssets.Add(loader.Url);
            }

            for (int i = 0; i < releaseAssets.Count; i++)
            {
                ReleaseAsset(releaseAssets[i]);
            }

            Resources.UnloadUnusedAssets();
        }
    }
}