using UnityEngine;

namespace Common
{
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField] private float m_Velocity;
        public float Velocity => m_Velocity;
        [SerializeField] private float m_Lifetime;
        [SerializeField] private int m_Damage;
        [SerializeField] protected ImpactEffect m_ImpactEffectPrefab;

        protected virtual void OnHit(DestructibleBase destr) { }
        protected virtual void OnHit(Collider2D col) { }
        protected virtual void OnProjectileLifetimeEnd(Collider2D collider, Vector2 point) { }

        private Timer m_LifeTimer;

        private void Start()
        {
            m_LifeTimer = new Timer(m_Lifetime);
        }

        private void Update()
        {
            float stepLenght = Time.deltaTime * m_Velocity;
            Vector2 step = transform.up * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);

            if (hit)
            {
                OnHit(hit.collider);

                DestructibleBase dest = hit.collider.transform.root.GetComponent<DestructibleBase>();

                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);

                    OnHit(dest);

                }

                if (dest != m_Parent) OnProjectileLifetimeEnd(hit.collider, hit.point);
            }

            m_LifeTimer.RemoveTime(Time.deltaTime);

            if (m_LifeTimer.IsFinish)
                OnProjectileLifetimeEnd(hit.collider, hit.point);

            transform.position += new Vector3(step.x, step.y, 0);
        }

        private protected DestructibleBase m_Parent;
        public void SetParent(DestructibleBase parent)
        {
            m_Parent = parent;
        }

        public DestructibleBase GetParent() => m_Parent;
    }
}
