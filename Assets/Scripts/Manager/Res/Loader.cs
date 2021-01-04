using EasyWork.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Manager.Res
{
    /// <summary>
    /// Date    2021/1/1 18:57:59
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public abstract class Loader
    {
        public delegate void LoadedHandle(Loader loader);
        public delegate void ProcessHandle(Loader loader, float process);

        public enum LoaderState
        {
            None,
            Loading,
            Finish
        }

        public LoadedHandle LoadedHandleAction;
        public ProcessHandle ProcessHandleAction;

        public LoaderState State => m_curState;
        public float Process => m_process;
        public string Url => m_url;

        protected string m_url;
        protected float m_process;
        protected object m_data;

        protected LoaderState m_curState;

        public virtual void Init(string url, LoadedHandle loadedHandle = null, ProcessHandle processHandle = null)
        {
            m_url = url;
            LoadedHandleAction = loadedHandle;
            ProcessHandleAction = processHandle;

            m_curState = LoaderState.None;
        }

        public virtual void Load()
        {
            if (string.IsNullOrEmpty(m_url))
            {
                return;
            }

            //start Loading coroutine
            ECoroutine.StartCoroutine(LoadingCoroutine());
        }

        private IEnumerator LoadingCoroutine()
        {
            m_curState = LoaderState.Loading;

            SetProcessAndCall(0);
            OnLoad();

            while (m_curState == LoaderState.Loading)
            {
                OnUpdate();
                yield return null;
            }
        }

        public virtual void Release()
        {
            OnRelease();
        }

        protected abstract void OnUpdate();
        protected abstract void OnLoad();

        protected abstract void OnRelease();

        public virtual T Get<T>()
        {
            if (m_data == null)
            {
                //todo log
                return default;
            }

            T result = (T)m_data;
            if (result == null)
            {
                //todo log
                return default;
            }

            return result;
        }

        public virtual void Reset()
        {
            m_process = 0;

            m_url = string.Empty;

            m_data = null;
            LoadedHandleAction = null;
            ProcessHandleAction = null;

            m_curState = LoaderState.None;
        }

        protected void SetProcessAndCall(float process)
        {
            m_process = process;
            ProcessHandleAction?.Invoke(this, m_process);
        }

        protected void LoadComplete()
        {
            SetProcessAndCall(1);

            m_curState = LoaderState.Finish;
            LoadedHandleAction?.Invoke(this);
        }
    }


    public class SceneLoader : AssetLoader
    {
        private LoadSceneMode m_loadSceneMode;

        public SceneLoader(LoadSceneMode loadSceneMode) : base()
        {
            m_loadSceneMode = loadSceneMode;
        }

        protected override void OnLoad()
        {
            m_asyncHandle = Addressables.LoadSceneAsync(m_url, m_loadSceneMode);
        }
    }

    public class AssetLoader : Loader
    {
        protected AsyncOperationHandle m_asyncHandle;

        protected override void OnLoad()
        {
            m_asyncHandle = Addressables.LoadAssetAsync<UnityEngine.Object>(m_url);
        }

        protected override void OnRelease()
        {
            Addressables.Release(m_asyncHandle);
        }

        protected override void OnUpdate()
        {
            if (m_curState != LoaderState.Loading)
            {
                return;
            }

            if (!m_asyncHandle.IsDone)
            {
                ProcessHandleAction?.Invoke(this, m_asyncHandle.PercentComplete);
                SetProcessAndCall(m_asyncHandle.PercentComplete);
                return;
            }

            if (m_asyncHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError(m_asyncHandle.OperationException.Message);
                return;
            }

            m_data = m_asyncHandle.Result;
            LoadComplete();
        }
    }
}