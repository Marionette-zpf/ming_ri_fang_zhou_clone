using System;

namespace EasyWork.Utilities
{
    public class EEvent<T>
    {
        public event Action<T> m_paramEvent;
        public event Action m_event;
        public void Subscribe(Action<T> eventHandler)
        {
            m_paramEvent += eventHandler;
        }

        public void UnSubscribe(Action<T> eventHandler)
        {
            m_paramEvent -= eventHandler;
        }

        public void Subscribe(Action eventHandler)
        {
            m_event += eventHandler;
        }

        public void UnSubscribe(Action eventHandler)
        {
            m_event -= eventHandler;
        }

        public void Dispatch(T param)
        {
            m_paramEvent?.Invoke(param);
        }

        public void Dispatch()
        {
            m_event?.Invoke();
        }
    }


}
