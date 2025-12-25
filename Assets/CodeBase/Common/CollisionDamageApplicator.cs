using SpaceShooter;
using UnityEngine;

namespace Common
{
    public class CollisionDamageApplicator : MonoBehaviour
    {
        public static string IgnoreTag = "WorldBoundary";

        [SerializeField] private float m_VelocityDamageModifier;

        [SerializeField] private float m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == IgnoreTag) return;

            SpaceShip self = transform.root.GetComponent<SpaceShip>();
            SpaceShip collisionShip = collision.transform.GetComponentInParent<SpaceShip>();

            if (self != null && collisionShip != null && collisionShip.TeamId == self.TeamId) return;

            Destructible destructable = transform.root.GetComponent<Destructible>();

            if (destructable != null)
            {
                destructable.ApplyDamage((int)m_DamageConstant + (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
            }
        }
    }
}