using System;
using Object = UnityEngine.Object;

namespace EasyWork.Utilities
{
    public interface IELoader 
    {
        T Load<T>(string path) where T : Object;

        void LoadAsync<T>(string path, Action<T> complete) where T : Object;
    }
}
