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
        public T CurrentState => m_curStateRunner.State;

        private Dictionary<T, BaseStateRunner<T>> m_stateMap = new Dictionary<T, BaseStateRunner<T>>();

        private IComparable<T> comparable;

        private BaseStateRunner<T> m_curStateRunner;

        private T m_primaryState;

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
                Debug.LogError("");
                return;
            }

            m_primaryState = primaryState;
            m_curStateRunner = stateRunner;

            m_curStateRunner.EnterState();
        }

        public virtual void EnterState(T state)
        {
            if (m_stateMap.TryGetValue(state, out var stateRunner) && stateRunner == m_curStateRunner)
            {
                return;
            }
            m_curStateRunner.ExitState();
            m_curStateRunner = stateRunner;
            m_curStateRunner.EnterState();
        }

        public virtual void UpdateStateMachine()
        {
            m_curStateRunner.UpdateState();
        }
    }

    public abstract class BaseStateRunner<T> 
    {
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