using EasyWork.Extend.Utilities;
using EasyWork.Utilities;
using Extend.Unity;
using GameEvent;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Module.Battle.Com
{
    /// <summary>
    /// Date    2021/1/5 16:12:24
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public abstract class BaseUnit : MonoBehaviour, IModuleBinder<BattleModule>
    {
        public const string KEY_HEALTH = "Health";
        public const string KEY_BODY_COLLIDER = "BodyCollider";
        public const string KEY_ANIMATOR = "AnimatorCom";
        public const string KEY_UNIT = "BaseUnit";
        public const string KEY_ATTACK_CD = "CountDownCom";

        public UnitStateEnum CurrentState => m_stateMachine.CurrentState;
        public UnitDir CurrentDir { get; set; }
        public Vector2Int CurrentPoint {
            get => m_currPoint;
            set
            {
                if (m_currPoint == value) return;
                EEventUtil.Dispatch(new UnitMoveEvent() { PrePoint = m_currPoint, CurrentPoint = value, Unit = this });
                m_currPoint = value;
            }
        }


        protected BaseUnitStateMachine<UnitStateEnum> m_stateMachine;
        protected BaseAnimatorCom m_animatorCom;
        protected Collider2D m_bodyCollider;
        protected Collider2D m_reactionCollider;
        protected LevelMapInfoExt m_mapInfoExt;
        protected Vector2Int m_currPoint;

        protected NumberCom m_health = new NumberCom();
        protected NumberCom m_attackRate = new NumberCom();

        protected CountDownCom m_attackCD = new CountDownCom();

        protected Dictionary<UnitDir, Vector2Int[]> m_attackRange = new Dictionary<UnitDir, Vector2Int[]>();


        protected virtual void Awake()
        {
            m_stateMachine = new BaseUnitStateMachine<UnitStateEnum>();
            m_bodyCollider = transform.Find("BodyObj").GetComponent<Collider2D>();
            m_reactionCollider = transform.Find("ReactionObj").GetComponent<Collider2D>();

            m_stateMachine.DataInterface.Register(KEY_HEALTH, () => m_health);
            m_stateMachine.DataInterface.Register(KEY_BODY_COLLIDER, () => m_bodyCollider);
            m_stateMachine.DataInterface.Register(KEY_ANIMATOR, () => m_animatorCom);
            m_stateMachine.DataInterface.Register(KEY_ATTACK_CD, () => m_attackCD);

            m_stateMachine.DataInterface.Register(KEY_UNIT, () => this);

            m_bodyCollider.OnTriggerEnter2DAsObservable()
                          .Subscribe(OnBodyTriggerEnterHandle);

            m_bodyCollider.OnTriggerExit2DAsObservable()
                          .Subscribe(OnBodyTriggerExitHandle);

            m_attackCD.SetCount(1 / m_attackRate.Current);
        }

        

        public virtual void SetAttackRange(Vector2Int[] attackPoints)
        {
            m_attackRange.Clear();

            for (UnitDir dir = UnitDir.EAST; dir <= UnitDir.NORTH; dir++)
            {
                m_attackRange.Add(dir, new Vector2Int[attackPoints.Length]);

                for (int i = 0; i < attackPoints.Length; i++)
                {
                    m_attackRange[dir][i] = dir.GetTrans(attackPoints[i]);
                }
            }
        }

        public virtual void DODamage(float damage)
        {
            if(m_stateMachine.CurrentState == UnitStateEnum.DEAD)
            {
                return;
            }

            m_health.Current -= damage;

            if(m_health.Current == 0)
            {
                m_stateMachine.EnterState(UnitStateEnum.DEAD);
                return;
            }

            if (m_stateMachine.CurrentState == UnitStateEnum.ATTACK || m_stateMachine.CurrentState == UnitStateEnum.HURT)
            {
                return;
            }

            m_stateMachine.EnterState(UnitStateEnum.HURT);
        }

        protected virtual TileTypeEnum TargetAttackTileType => TileTypeEnum.LOAD;
        protected virtual bool AttackState => CurrentState == UnitStateEnum.MOVE || CurrentState == UnitStateEnum.IDLE;

        protected virtual IEnumerator AttackCoroutine()
        {
            while (true)
            {
                if (!m_attackCD.IsComplete) yield return null;
                if (!AttackState) yield return null;

                var attackRange = m_attackRange[CurrentDir];

                for (int i = 0; i < attackRange.Length; i++)
                {
                    var point = attackRange[i];
                    var tile = m_mapInfoExt.MapInfo.GetTile(point);

                    if(TargetAttackTileType != tile.Type)
                    {
                        continue;
                    }

                    OnAttack(this.Module().GetUnitsByPoint(point));
                }

                yield return null;
            }
        }

        protected virtual void OnAttack(List<BaseUnit> units)
        {

        }

        protected virtual void OnBodyTriggerEnterHandle(Collider2D other)
        {

        }

        protected virtual void OnBodyTriggerExitHandle(Collider2D other)
        {

        }

    }

    public class BaseCharacterUnit : BaseUnit
    {
        public const string KEY_POWER = "Power";

        protected NumberCom m_power;
    }


    public class BaseEnemyUnit : BaseUnit
    {
        public const string KEY_PATH = "PathInfo";
        public const string KEY_SPEED = "MoveSpeed";
        public const string KEY_AROUNT_CHAR = "AroundChar";

        public const float MINI_DIS = 0.1f;

        protected NumberCom m_speed;
        protected UnitPathExt m_path;

        protected List<BaseCharacterUnit> m_aroundChar = new List<BaseCharacterUnit>();


        protected override void Awake()
        {
            base.Awake();

            m_stateMachine.DataInterface.Register(KEY_PATH, () => m_path);
            m_stateMachine.DataInterface.Register(KEY_SPEED, () => m_speed);
            m_stateMachine.DataInterface.Register(KEY_AROUNT_CHAR, () => m_aroundChar);

            m_stateMachine.AddState(UnitStateEnum.MOVE, new BaseMoveStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.ATTACK, new BaseAttackStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.DEAD, new BaseDeadStateRunner(m_stateMachine))
                          .AddState(UnitStateEnum.HURT, new BaseHurtRunner(m_stateMachine))
                          .SetPrimaryState(UnitStateEnum.MOVE);
        }


        public void SetPath(UnitPathExt pathInfo)
        {
            m_path = pathInfo;
        }

    }

    public class BaseUnitStateMachine<T> : BaseStateMachine<T>
    {
        public event Action<T> OnEnterState;

        public DataInterface DataInterface { get; } = new DataInterface();

        public override void EnterState(T state)
        {
            base.EnterState(state);
            OnEnterState?.Invoke(state);
        }
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

        public BaseHurtRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) { }

        protected override void OnEnterState()
        {
            GetData<BaseAnimatorCom>(BaseUnit.KEY_ANIMATOR).SetAnimation("Hurt");
        }
    }

    public class BaseBattleStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.BATTLE;

        protected CountDownCom m_attackCD;

        public BaseBattleStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine)
        {
            m_attackCD = GetData<CountDownCom>(BaseUnit.KEY_ATTACK_CD);
        }

        protected override void OnEnterState()
        {
            m_attackCD.ReStart();
        }

        public override void UpdateState()
        {
            if (m_attackCD.IsComplete)
            {
                GetData<BaseAnimatorCom>(BaseUnit.KEY_ANIMATOR).SetAnimation("Hurt");
                m_attackCD.ReStart();
            }
        }
    }

    public class BaseMoveStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.MOVE;

        protected UnitPathExt m_path;
        protected BaseUnit m_unitObj;
        protected NumberCom m_speed;
        protected Transform m_trans;

        protected Vector3 m_preDir = Vector3.zero;

        protected int m_targetPointIndex = 0;

        protected float m_curJourney = 0;

        public BaseMoveStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) 
        {
            m_path = GetData<UnitPathExt>(BaseEnemyUnit.KEY_PATH);
            m_speed = GetData<NumberCom>(BaseEnemyUnit.KEY_SPEED);
            m_unitObj = GetData<BaseUnit>(BaseUnit.KEY_UNIT);

            m_trans = m_unitObj.transform;
        }

        protected override void OnUpdateState()
        {
            if(m_curJourney > m_path.Length())
            {
                EEventUtil.Dispatch(new EnemyArriveEvent() { EnemyUnit = m_unitObj as BaseEnemyUnit });
                return;
            }

            var moveSize = m_speed.Current * Time.deltaTime;
            m_curJourney += moveSize;
            m_trans.position = m_path.GetWSPosByJourney(m_curJourney);

            m_unitObj.CurrentPoint = m_path.GetPointByJourney(m_curJourney);
        }

        protected override void OnEnterState()
        {
            GetData<BaseAnimatorCom>(BaseUnit.KEY_ANIMATOR).SetAnimation("Move");
            GetData<CountDownCom>(BaseUnit.KEY_ATTACK_CD).ReStart();
        }
    }

    public class BaseAttackStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.ATTACK;

        public BaseAttackStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) { }

        protected override void OnEnterState()
        {
            GetData<BaseAnimatorCom>(BaseUnit.KEY_ANIMATOR).SetAnimation("Attack");
        }
    }

    public class BaseDeadStateRunner : BaseUnitStateRunner<UnitStateEnum>
    {
        public override UnitStateEnum State => UnitStateEnum.DEAD;

        public BaseDeadStateRunner(BaseStateMachine<UnitStateEnum> stateMachine) : base(stateMachine) { }

        protected override void OnEnterState()
        {
            GetData<BaseAnimatorCom>(BaseUnit.KEY_ANIMATOR).SetAnimation("Dead");
        }
    }

    public abstract class BaseAnimatorCom
    {
        public abstract void SetAnimation(string animation);

        public abstract void RegisterEvent(string eventName, Action call);
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

    public class CountDownCom
    {
        public bool IsComplete => m_current <= 0;

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
        NORTH
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
                    return new Vector2Int(-origin.x, origin.y);
                case UnitDir.SOUTH:
                    return new Vector2Int(origin.y, -origin.x);
                case UnitDir.NORTH:
                    return new Vector2Int(-origin.y, origin.x);
                default:
                    break;
            }

            return default;
        }
    }

    public class BasePassivityUnit : BaseEnemyUnit
    {
        protected BaseUnit m_battleUnit;

        public void SetBattleUnit(BaseUnit unit)
        {
            m_battleUnit = unit;
            m_stateMachine.EnterState(UnitStateEnum.BATTLE);
        }
    }
}