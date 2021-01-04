using EasyWork.Utilities;
using System.Collections;
using System.Collections.Generic;

namespace Manager.Res
{
    /// <summary>
    /// Date    2021/1/1 19:11:11
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class LoaderGroup
    {
        public delegate void WhenAllHandle(LoaderGroup loaderGroup);

        public Loader[] Loaders { get; private set; }

        public bool IsDone => m_urls.Length == m_loadedMap.Count;

        private Loader.LoadedHandle m_loadedHandle;
        private Loader.ProcessHandle m_processHandle;

        private WhenAllHandle m_whenAllHandle;

        private Dictionary<string, Loader> m_loadedMap = new Dictionary<string, Loader>();

        private string[] m_urls;

        public LoaderGroup(params string[] urls) : this(null, null, null, urls) { }

        public LoaderGroup(Loader.ProcessHandle processHandle, params string[] urls) : this(null, null, processHandle, urls) { }

        public LoaderGroup(WhenAllHandle whenAllHandle, Loader.ProcessHandle processHandle, params string[] urls) : this(whenAllHandle, null, processHandle, urls) { }

        public LoaderGroup(WhenAllHandle whenAllHandle, Loader.LoadedHandle loadedHandle, Loader.ProcessHandle processHandle, params string[] urls)
        {
            m_whenAllHandle = whenAllHandle;
            m_loadedHandle = loadedHandle;
            m_processHandle = processHandle;

            m_urls = urls;
        }

        public void Load()
        {
            ECoroutine.StartCoroutine(LoadingCoroutine());
        }

        public IEnumerator LoadingCoroutine()
        {
            Loaders = new Loader[m_urls.Length];

            for (int i = 0; i < Loaders.Length; i++)
            {
                Loaders[i] = ResManager.LoadAssetAsync(m_urls[i]);
            }

            bool complete;

            do
            {
                float process = 0.0f;

                complete = true;

                for (int i = 0; i < Loaders.Length; i++)
                {
                    var loader = Loaders[i];
                    var finish = loader.State == Loader.LoaderState.Finish;

                    complete &= finish;

                    if (finish && !m_loadedMap.ContainsKey(loader.Url))
                    {
                        m_loadedMap.Add(loader.Url, loader);
                        m_loadedHandle?.Invoke(loader);
                    }

                    process += loader.Process;

                    m_processHandle?.Invoke(null, process / Loaders.Length);
                }

                yield return null;

            } while (!complete);

            m_whenAllHandle?.Invoke(this);
        }

        public T Get<T>(string url)
        {
            if (m_loadedMap.TryGetValue(url, out var loader))
            {
                return loader.Get<T>();
            }

            //todo log
            return default;
        }

    }
}