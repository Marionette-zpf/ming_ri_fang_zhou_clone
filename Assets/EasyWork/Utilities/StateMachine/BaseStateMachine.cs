using System;
using System.Collections.Generic;
using UnityEngine;

namespace EasyWork.Utilities
{
    /// <summary>
    /// Date    2020/12/31 15:56:22
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class BaseStateMachine<T>
    {
        /// <summary>
        /// param1:原先状态 param2:改变状态
        /// </summary>
        public event Action<T, T> OnChangeState;

        public T CurrentState => m_curStateRunner.State;

        private Dictionary<T, BaseStateRunner<T>> m_stateMap = new Dictionary<T, BaseStateRunner<T>>();

        private BaseStateRunner<T> m_curStateRunner;

        public virtual BaseStateMachine<T> AddState(T state, BaseStateRunner<T> stateRunner, bool overrideState = false)
        {
            if (m_stateMap.ContainsKey(state))
            {
                if (overrideState)
                {
                    m_stateMap[state] = stateRunner;
                }
                return this;
            }
            m_stateMap.Add(state, stateRunner);
            return this;
        }

        public virtual void SetPrimaryState(T primaryState)
        {
            if(!m_stateMap.TryGetValue(primaryState, out var stateRunner))
            {
                Debug.LogError($"primaryState:{primaryState} not exit");
                return;
            }

            m_curStateRunner = stateRunner;
            m_curStateRunner.EnterState();
        }

        public virtual void EnterState(T state)
        {
            if (!m_stateMap.TryGetValue(state, out var stateRunner) || stateRunner == m_curStateRunner)
            {
                Debug.LogError($"curState:{m_curStateRunner}, enterState:{state}");
                return;
            }

            OnChangeState?.Invoke(CurrentState, state);

            m_curStateRunner.ExitState();
            m_curStateRunner = stateRunner;
            m_curStateRunner.EnterState();
        }

        public virtual void UpdateStateMachine()
        {
            m_curStateRunner?.UpdateState();
        }

        public virtual void RegisterStateRunner(T state, Action action)
        {
            if (!m_stateMap.TryGetValue(state, out var stateRunner))
            {
                return;
            }

            stateRunner.OnUpdate += action;
        }

        public virtual void UnregisterStateRunner(T state, Action action)
        {
            if (!m_stateMap.TryGetValue(state, out var stateRunner))
            {
                return;
            }

            stateRunner.OnUpdate -= action;
        }
    }

    public abstract class BaseStateRunner<T> 
    {
        public event Action OnUpdate;

        public abstract T State { get; }

        private BaseStateMachine<T> m_stateMachine;

        public BaseStateRunner(BaseStateMachine<T> stateMachine)
        {
            m_stateMachine = stateMachine;
        }


        public virtual void EnterState()
        {
            OnEnterState();
        }

        public virtual void ExitState()
        {
            OnExitState();
        }

        public virtual void UpdateState()
        {
            OnUpdateState();
            OnUpdate?.Invoke();
        }

        protected virtual void OnEnterState() { }
        protected virtual void OnExitState() { }
        protected virtual void OnUpdateState() { }

        protected void ChangeState(T state)
        {
            m_stateMachine.EnterState(state);
        }
    }
}