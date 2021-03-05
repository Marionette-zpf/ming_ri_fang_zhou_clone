using EasyWork.Extend.Utilities;
using EasyWork.Utilities;
using Module.Battle.Com;
using System;
using UnityEngine;

namespace Module.Battle.Bullet
{
    /// <summary>
    /// Date    2021/2/24 10:33:35
    /// Name    A12771\Administrator
    /// Desc    desc
    /// </summary>
    public class BaseBullet : MonoBehaviour 
    {
        private Transform m_targetTrans;
        private BaseUnit m_target;
        private Action<BaseUnit> m_onCollision;

        private Transform m_transform;

        private float m_speed;
        private float m_damage;

        private void Awake()
        {
            m_transform = transform;
        }

        public BaseBullet SetTarget(BaseUnit target, Action<BaseUnit> onCollision = null)
        {
            gameObject.SetActive(true);

            m_target = target;
            m_targetTrans = target.transform;
            m_onCollision = onCollision;

            return this;
        }

        public BaseBullet Init(Vector3 position, float value, float damage)
        {
            m_transform.position = position;
            m_speed = value;
            m_damage = damage;
            return this;
        }

        private void Update()
        {
            var dir = (m_targetTrans.position - m_transform.position).normalized;
            m_transform.position += dir * m_speed * Time.deltaTime;
        }

        public void OnTriggerEnter(Collider other)
        {
            m_onCollision?.Invoke(other.GetComponent<BaseUnit>());
            m_onCollision = null;

            m_target.DoDamage(m_damage);

            gameObject.SetActive(false);
        }
    }
}