using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace EasyWork.Extend.System
{
    public static class AssemblyExt 
    {
        public static void EachSubType<T>(Action<Type> call)
        {
            var assembly = Assembly.GetAssembly(typeof(T));
            var types = assembly.GetTypes();

            var binderType = typeof(T);
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (binderType.IsAssignableFrom(type) && binderType != type)
                {
                    call.Invoke(type);
                }
            }
        }
    }

}
