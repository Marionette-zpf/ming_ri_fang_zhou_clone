using System;
using System.Collections;
using UnityEngine;

namespace EasyWork.Utilities
{
    public class EResourceLoader : IELoader
    {
        public T Load<T>(string path) where T : UnityEngine.Object
        {
            return Resources.Load<T>(path);
        }

        public void LoadAsync<T>(string path, Action<T> complete) where T : UnityEngine.Object
        {
            var request = Resources.LoadAsync<T>(path);
            ECoroutine.StartCoroutine(LoadAsync(request, complete));
        }

        private IEnumerator LoadAsync<T>(ResourceRequest request, Action<T> complete) where T : UnityEngine.Object
        {
            yield return request;

            if (request.isDone)
            {
                complete?.Invoke(request.asset as T);
            }
        }
    }

}

