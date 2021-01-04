using System;

namespace Utilities.Binder
{
    /// <summary>
    /// Date    2021/1/2 21:42:55
    /// Name    DESKTOP-H2JU0TM\icon
    /// Desc    desc
    /// </summary>
    public class DataBinder
    {

    }

    public class DataBinder<T> : DataBinder
    {
        private Func<T> m_provider;

        public DataBinder(Func<T> provider)
        {
            m_provider = provider;
        }

        public T Get()
        {
            return m_provider.Invoke();
        }
    }
}