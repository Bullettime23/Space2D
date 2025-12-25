using Common;
using UnityEngine;
using UnityEngine.Audio;

namespace SpaceShooter
{
    /// <summary>
    /// Скрипт должен проверять, есть ли у ракеты цель на пути, и если есть, повора
    /// чивать снаряд в сторону цели
    /// </summary>
    [RequireComponent(typeof(Projectile))]
    [RequireComponent(typeof(CircleCollider2D))]
    public class AutoMissile : MonoBehaviour
    {
        [SerializeField] private float m_DetectionRadius;
        [SerializeField] private float m_RotateMaxAngle;
        [SerializeField] private AudioResource m_LockSFX;


        private Projectile m_Projectile;
        private SpaceShip m_Target;
        private CircleCollider2D m_Collider2D;

        private void Start()
        {

            m_Projectile = GetComponent<Projectile>();
            m_Collider2D = GetComponent<CircleCollider2D>();

            m_Collider2D.radius = m_DetectionRadius;
        }


        private void Update()
        {
            if (m_Target == null)
                return;

            Rigidbody2D targetRigid = m_Target.transform.root.GetComponent<Rigidbody2D>();
            Vector3 targetPosition = m_Target.transform.position;
            Vector3 targetShipVelocity = targetRigid.linearVelocity;

            float interceptionTime = Vector3.Distance(m_Target.transform.position, targetPosition) /
                (m_Projectile.Velocity * m_Projectile.transform.up - targetShipVelocity).magnitude;

            Vector3 prediction = targetPosition +
                new Vector3(targetRigid.linearVelocity.x, targetRigid.linearVelocity.y, 0) * interceptionTime;

            Vector3 heading = prediction - transform.position;


            float angleInDegrees = Vector2.SignedAngle(transform.up, heading);

            float clampedAngle = Mathf.Clamp(angleInDegrees, -m_RotateMaxAngle, m_RotateMaxAngle);

            transform.Rotate(transform.forward, clampedAngle);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_Target != null)
                return;

            SpaceShip shipInRange = collision.transform.GetComponentInParent<SpaceShip>();

            if (shipInRange == null)
                return;

            if (shipInRange.transform.root == m_Projectile.GetParent().transform.root)
                return;

            if (shipInRange.TeamId == m_Projectile.GetParent().GetComponent<SpaceShip>().TeamId)
                return;

            m_Target = shipInRange;

            if (m_LockSFX)
                SoundFXManager.Instance.PlaySoundFXClip(m_LockSFX, transform, 0.3f, 0.5f);
        }

#if UNITY_EDITOR

        private Color m_GizmosColor = new Color(1, 0, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = m_GizmosColor;
            Gizmos.DrawSphere(transform.position, m_DetectionRadius);

        }
#endif
    }
}