using UnityEngine;

namespace SpaceShooter
{
    public class PowerupSpeed : Powerup
    {
        [SerializeField] private PowerupProperties m_PowerupProperties;

        protected override void OnPickedUp(PowerupPicker picker)
        {
            picker.ApplySpeedBoost(m_PowerupProperties);
        }
    }
}