using UnityEngine;

namespace EasyWork.Utilities
{
    public class EUnityAssert : IEAssert
    {
        public void Assert(bool condition, string context)
        {
            Debug.Assert(condition, context);
        }
    }

}

