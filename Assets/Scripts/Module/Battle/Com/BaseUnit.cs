using Config;
using EasyWork.Extend.Utilities;
using EasyWork.Utilities;
using GameEvent;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/1/5 16:12:24
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract class BaseUnit : MonoBehaviour, IModuleBinder<BattleModule>
    {
        public const float ZERO_FLOAT = 0f;

        public UnitStateEnum CurrentState => m_stateMachine.CurrentState;
        public UnitDir CurrentDir
        {
            get
            {
                return m_currentDir;
            }
            set
            {
                if(m_currentDir == value)
                {
                    return;
                }
                OnChangeDirHandle(m_currentDir, value);
                m_currentDir = value;
            }
        }

        public Vector2Int CurrentPoint 
        {
            get => m_currPoint;
            set
            {
                if (m_currPoint == value) return;
                EEventUtil.Dispatch(new UnitMoveEvent() { PrePoint = m_currPoint, CurrentPoint = value, Unit = this });
                m_currPoint = value;
            }
        }

        public virtual bool IsEnemy => true;
        protected virtual bool m_targetUnitIsEnemy => true;

        protected BaseStateMachine<UnitStateEnum> m_stateMachine;
        protected AnimatorCom m_animatorCom;
        protected Collider2D m_bodyCollider;
        protected Collider2D m_reactionCollider;
        protected LevelMapInfoExt m_mapInfoExt;
        protected Vector2Int m_currPoint;
        protected UnitDir m_currentDir = UnitDir.NONE;

        protected NumberCom m_health;
        protected NumberCom m_attackRate;
        protected NumberCom m_attack;

        protected CountDownCom m_attackCD = new CountDownCom();

        protected Dictionary<UnitDir, HashSet<Vector2Int>> m_attackRange = new Dictionary<UnitDir, HashSet<Vector2Int>>();

        protected Slider m_bloodUI;

        private List<BaseUnit> m_inAttackRangeTargets = new List<BaseUnit>();

        protected virtual void Awake()
        {
            m_stateMachine = new BaseStateMachine<UnitStateEnum>();
            m_animatorCom = transform.GetComponent<AnimatorCom>();
            m_bloodUI = GetComponentInChildren<Slider>();

            m_mapInfoExt = LevelDataManager.DataMgr.MapInfoExt;
        }

        protected virtual void Start()
        {
            m_animatorCom.SkeletonAnimation.state.Start += AnimationStateStartHandle;
            m_animatorCom.SkeletonAnimation.state.Complete += AnimationStateCompleteHandle;
            m_stateMachine.OnChangeState += StateMachineOnChangeState;
        }



        protected virtual void Update()
        {
            m_stateMachine.UpdateStateMachine();
        }


        private float m_rotateStep = 0;

        public void Flip(float time)
        {
            m_animatorCom.SkeletonAnimation.transform.DOLocalRotate(new Vector3(0, m_rotateStep += 180, 0), time);
        }

        public virtual bool InAttackRange(Vector2Int point)
        {
            return m_attackRange[CurrentDir].Contains(point);
        }

        public virtual void SetProperties(UnitPropertiesConfig propertiesConfig, int level = 0)
        {
            m_attackRate = new NumberCom(propertiesConfig.Rate + propertiesConfig.RateGrow * level, ZERO_FLOAT);
            m_health = new NumberCom(propertiesConfig.Health + propertiesConfig.HealthGrow * level, ZERO_FLOAT);
            m_attack = new NumberCom(propertiesConfig.Attack + propertiesConfig.AttackGrow * level, ZERO_FLOAT);

            m_attackCD.SetCount(1.0f / m_attackRate.Current);

            var count = propertiesConfig.Range.GetLength(0);

            Vector2Int[] attackPoints = new Vector2Int[count];

            for (int i = 0; i < count; i++)
            {
                attackPoints[i] = new Vector2Int(propertiesConfig.Range[i][0], propertiesConfig.Range[i][1]);
            }

            SetAttackRange(attackPoints);
        }

        public virtual void SetAttackRange(Vector2Int[] attackPoints)
        {
            m_attackRange.Clear();

            for (UnitDir dir = UnitDir.EAST; dir <= UnitDir.NORTH; dir++)
            {
                m_attackRange.Add(dir, new HashSet<Vector2Int>());
                for (int i = 0; i < attackPoints.Length; i++)
                {
                    m_attackRange[dir].Add(dir.GetTrans(attackPoints[i]) + m_currPoint);
                }
            }
        }

        public virtual void DoDamage(float damage)
        {
            if(m_stateMachine.CurrentState == UnitStateEnum.DEAD)
            {
                return;
            }

            m_health.Current -= damage;
            m_bloodUI.value = m_health.Precent;

            if(m_health.Current == ZERO_FLOAT)
            {
                m_stateMachine.EnterState(UnitStateEnum.DEAD);
                return;
            }
            else
            {
                //变红
            }
        }

        protected Func<BaseUnit, bool> m_searchFilter;

        protected List<BaseUnit> SearchTargets(bool isEnemy = true)
        {
            if(m_inAttackRangeTargets.Count != 0)
            {
                m_inAttackRangeTargets.Clear();
            }

            var attackRange = m_attackRange[CurrentDir];

            foreach (var point in attackRange)
            {
                var logicPoint = point;

                var uints = this.Module().GetUnitsByPoint(logicPoint);

                for (int j = 0; j < uints.Count; j++)
                {
                    var unit = uints[j];

                    if (unit.IsEnemy == isEnemy)
                    {
                        if(m_searchFilter != null)
                        {
                            if(m_searchFilter.Invoke(unit))
                            {
                                m_inAttackRangeTargets.Add(unit);
                            }
                        }
                        else
                        {
                            m_inAttackRangeTargets.Add(unit);
                        }
                    }
                }
            }

            return m_inAttackRangeTargets;
        }

        protected virtual void Initialize() { }
        protected virtual void StateMachineOnChangeState(UnitStateEnum pre, UnitStateEnum curr) { }
        protected virtual void AnimationStateStartHandle(Spine.TrackEntry trackEntry) { }
        protected virtual void AnimationStateCompleteHandle(Spine.TrackEntry trackEntry) { }
        protected virtual void OnChangeDirHandle(UnitDir preDir, UnitDir curDir) { }
    }

    public class BaseIdleStateRunner : BaseStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.HURT;
        public BaseIdleStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) { }
    }

    public class BaseBattleStateRunner : BaseStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.BATTLE;

        public BaseBattleStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {

        }
    }

    public class BaseMoveStateRunner : BaseStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.MOVE;

        public BaseMoveStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) 
        {
        }

    }

    public class BaseDeadStateRunner : BaseStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.DEAD;

        public BaseDeadStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) { }
    }

    public class NumberCom
    {
        public Action<float> OnChange;

        public float Precent => m_curValue / (m_max - m_min);

        private float m_max;
        private float m_min;
        private float m_curValue;

        public NumberCom()
        {
            m_min = float.MinValue;
            m_max = float.MaxValue;
        }
        public NumberCom(float current, float min = float.MinValue, float max = float.MaxValue)
        {
            m_min = min;
            m_max = max;

            m_curValue = current;
        }

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

    public class CountDownCom
    {
        public bool IsComplete => m_current <= 0;
        public float Percent => m_current / m_count;

        private float m_count;
        private float m_current;

        private IDisposable m_disposable;

        public CountDownCom SetCount(float count)
        {
            m_count = count;
            return this;
        }

        public void ReStart(Action complete = null)
        {
            m_current = m_count;

            m_disposable?.Dispose();
            m_disposable = Observable.EveryUpdate()
                                     .TakeWhile(_ => m_current > 0)
                                     .DoOnCompleted(()=> complete?.Invoke())
                                     .Subscribe(_ => m_current -= Time.deltaTime);
        }

        public void Complete()
        {
            m_current = 0;
        }
    }

    public enum UnitStateEnum
    {
        IDLE,
        MOVE,
        ATTACK,
        DEAD,
        HURT,
        BATTLE
    }

    public enum UnitDir
    {
        /// <summary>
        /// 东
        /// </summary>
        EAST,
        /// <summary>
        /// 西
        /// </summary>
        WEST,
        /// <summary>
        /// 南
        /// </summary>
        SOUTH,
        /// <summary>
        /// 北
        /// </summary>
        NORTH,
        NONE
    }

    public static class UnitDirCalExt
    {
        public static Vector2Int GetTrans(this UnitDir unitDir, Vector2Int origin)
        {
            switch (unitDir)
            {
                case UnitDir.EAST:
                    return origin;
                case UnitDir.WEST:
                    return new Vector2Int(origin.x, -origin.y);
                case UnitDir.SOUTH:
                    return new Vector2Int(-origin.y, origin.x);
                case UnitDir.NORTH:
                    return new Vector2Int(origin.y, -origin.x);
                default:
                    break;
            }

            return default;
        }
    }


}