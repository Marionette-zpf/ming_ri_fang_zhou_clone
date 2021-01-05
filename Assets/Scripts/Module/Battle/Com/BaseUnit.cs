using EasyWork.Utilities;
using System;
using UnityEngine;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/1/5 16:12:24
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract class BaseUnit : MonoBehaviour
    {
        public const string KEY_HEALTH = "Health";
        public const string KEY_POWER = "Power";
        public const string KEY_COLLIDER = "Collider";
        public const string KEY_ANIMATOR = "AnimatorCom";

        protected BaseUnitStateMachine<UnitStateEnum> m_stateMachine;
        protected BaseAnimatorCom m_animatorCom;
        protected Collider2D m_collider;

        protected NumberCom m_health;
        protected NumberCom m_attackRate;

        public BaseUnit()
        {
            m_stateMachine = new BaseUnitStateMachine<UnitStateEnum>();

            m_collider = GetComponent<Collider2D>();
        }
    }

    public class BaseCharacterUnit : BaseUnit
    {
        protected NumberCom m_power;
    }


    public class BaseEnemyUnit : BaseUnit
    {

        protected NumberCom m_speed;

        public BaseEnemyUnit()
        {
            m_stateMachine.AddState(UnitStateEnum.MOVE, new BaseMoveStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.ATTACK, new BaseAttackStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.HURT, new BaseHurtRunner(m_stateMachine))
                          .SetPrimaryState(UnitStateEnum.MOVE);

            m_stateMachine.DataInterface.Register(KEY_HEALTH, () => m_health);
            m_stateMachine.DataInterface.Register(KEY_COLLIDER, () => m_collider);
            m_stateMachine.DataInterface.Register(KEY_ANIMATOR, () => m_animatorCom);
        }

    }

    public class BaseUnitStateMachine<T> : BaseStateMachine<T>
    {
        public DataInterface DataInterface { get; } = new DataInterface();
    }

    public abstract class BaseUnitStateRunner<T> : BaseStateRunner<T>
    {
        private BaseUnitStateMachine<T> m_unitSM;

        protected BaseUnitStateRunner(BaseStateMachine<T> stateMachine) : base(stateMachine)
        {
            m_unitSM = stateMachine as BaseUnitStateMachine<T>;
        }

        protected void GetData<TData>(string key, out TData data)
        {
            m_unitSM.DataInterface.Get(key, out data);
        }

        protected TData GetData<TData>(string key)
        {
           return m_unitSM.DataInterface.Get<TData>(key);
        }
    }

    public class BaseHurtRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.HURT;

        public BaseHurtRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {

        }
    }

    public class BaseMoveStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.MOVE;

        public BaseMoveStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {

        }
    }

    public class BaseAttackStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.ATTACK;

        public BaseAttackStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {
        }
    }

    public class BaseDeadStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.DEAD;

        public BaseDeadStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {
        }
    }

    public abstract class BaseAnimatorCom
    {
        public abstract void SetAnimation(string animation);
    }

    public class NumberCom
    {
        public Action<float> OnChange;

        private float m_max;
        private float m_min;
        private float m_curValue;

        public float Current
        {
            get => m_curValue;
            set
            {
                value = Mathf.Clamp(value, m_min, m_max);
                m_curValue = value;

                OnChange?.Invoke(m_curValue);
            }
        }

        public NumberCom SetMax(uint max)
        {
            m_max = max;
            return this;
        }

        public NumberCom SetMin(uint min)
        {
            m_min = min;
            return this;
        }
    }

    public enum UnitStateEnum
    {
        WAITING,
        MOVE,
        ATTACK,
        DEAD,
        HURT
    }
}