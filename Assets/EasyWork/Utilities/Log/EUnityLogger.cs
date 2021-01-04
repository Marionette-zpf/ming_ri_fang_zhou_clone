using UnityEngine;

namespace EasyWork.Utilities
{
    public class EUnityLogger : IELogger
    {
        public void Log(string context)
        {
            Debug.Log(context);
        }

        public void LogError(string context)
        {
            Debug.LogError(context);
        }

        public void LogWarning(string context)
        {
            Debug.LogWarning(context);
        }
    }

}
