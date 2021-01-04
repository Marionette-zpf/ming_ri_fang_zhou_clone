using EasyWork.Extend.Utilities;
using EasyWork.Utilities;
using System.Collections.Generic;

namespace Utilities.Common
{
    /// <summary>
    /// Date    2020/12/20 22:13:20
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public partial class ParamVo
    {
        private List<object> m_parmas = new List<object>();

        public void SetParams(params object[] objs)
        {
            m_parmas.Clear();

            for (int i = 0; i < objs.Length; i++)
            {
                m_parmas.Add(objs[i]);
            }
        }

        public T Get<T>(int index = 0)
        {
            if(m_parmas.Count <= index)
            {
                return default;
            }

            return (T)m_parmas[index];
        }
    }


    public partial class ParamVo
    {
        private static IEPool<ParamVo> g_parmaVoPool;

        public static ParamVo Get(params object[] objs)
        {
            if (g_parmaVoPool == null)
            {
                g_parmaVoPool = EPoolUtil.CreatePool<ParamVo>(() => new ParamVo());
            }

            var result = g_parmaVoPool.Get();
            result.SetParams(objs);
            return result;
        }
    }
}