using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;
        public TurretProperties Props => m_TurretProperties;

        private float m_RefireTimer;

        public bool CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        #region Update
        private void Start()
        {
            m_Ship = gameObject.GetComponentInParent<SpaceShip>();

        }

        private void Update()
        {
            if (m_RefireTimer > 0)
                m_RefireTimer -= Time.deltaTime;
        }
        #endregion

        #region Public API
        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (m_RefireTimer > 0) return;

            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;
            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            //Разместить снаряд там же, где корабль и повернуть его по курсу корабля
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;

            projectile.SetParent(m_Ship);

            m_RefireTimer = m_TurretProperties.FireRate;

            if (m_TurretProperties.LaunchSFX != null) SoundFXManager.Instance.PlaySoundFXClip(m_TurretProperties.LaunchSFX, transform, 0.1f, 2);
        }

        // Задает параметры оружия, при подборе бонусов, например
        public void AssignLoadout(TurretProperties turretProperties)
        {
            if (m_Mode != turretProperties.Mode) return;

            m_RefireTimer = 0;
            m_TurretProperties = turretProperties;
        }
        #endregion
    }
}