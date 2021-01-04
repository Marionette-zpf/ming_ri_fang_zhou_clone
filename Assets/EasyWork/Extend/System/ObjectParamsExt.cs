using UnityEngine;

namespace Extend.System
{
    /// <summary>
    /// Date    2021/1/1 18:49:51
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public static class ObjectParamsExt
    {
        public static T Get<T>(this object[] @this, int index = 0)
        {
            if (index < 0 || index >= @this.Length)
            {
                Debug.LogError("index out of range");
                return default;
            }

            return (T)@this[index];
        }
    }

}