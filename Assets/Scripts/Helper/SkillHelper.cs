using EasyWork.Extend.System;
using Module.Battle.Com;
using System;
using System.Collections.Generic;

namespace Helper
{
    /// <summary>
    /// Date    2021/1/6 10:25:44
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public static class SkillHelper
    {
        private static Dictionary<Type, BaseSkill> g_skillMap;
        
        private static void Excute<T>(BaseUnit source, BaseUnit target, float value) where T : BaseSkill
        {
            if (g_skillMap == null) Init();

            g_skillMap[typeof(T)].Excute(source, target, value);
        }

        private static void Excute<T>(BaseUnit source, BaseUnit target, params object[] parma) where T : BaseSkill
        {
            if (g_skillMap == null) Init();

            g_skillMap[typeof(T)].Excute(source, target, parma);
        }

        private static void Init()
        {
            g_skillMap = new Dictionary<Type, BaseSkill>();

            AssemblyExt.EachSubType<BaseSkill>(skillType =>
            {
                g_skillMap.Add(skillType, Activator.CreateInstance(skillType) as BaseSkill);
            });
        }
    }
}