using UnityEngine;

namespace SpaceShooter
{
    public class PowerupWeapons : Powerup
    {
        [SerializeField] private TurretProperties m_TurretProperties;
        protected override void OnPickedUp(PowerupPicker picker)
        {
            picker.ApplyWeapon(m_TurretProperties);
        }
    }
}