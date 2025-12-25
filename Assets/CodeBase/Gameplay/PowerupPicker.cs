using System.Collections.Generic;
using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class PowerupPicker : MonoBehaviour
    {
        private SpaceShip m_Ship;

        private List<PowerupProperties> m_Powerups = new List<PowerupProperties>();

        private void Start()
        {
            m_Ship = GetComponent<SpaceShip>();
        }

        private void Update()
        {
            for (int i = 0; i < m_Powerups.Count; i++)
            {
                PowerupProperties powerup = m_Powerups[i];

                powerup.Timer.RemoveTime(Time.deltaTime);

                if (powerup.Timer.IsFinish)
                {
                    if (powerup.PowerupType == PowerupType.Speedboost)
                        RemoveSpeedBoost(m_Powerups[i]);

                    m_Powerups.Remove(powerup);
                    SoundFXManager.Instance.PlaySoundFXClip(powerup.AudioEnd, m_Ship.transform, 0.5f, 2);
                }
            }
        }

        #region Public APi
        public void ApplySpeedBoost(PowerupProperties props)
        {
            props.Timer = new Timer(props.Duration);

            if (props.PowerupType == PowerupType.Speedboost)
            {
                m_Powerups.Add(props);

                m_Ship.Thrust += props.BonusSpeed;
                m_Ship.MaxLinearVelocity += props.BonusSpeed;
                SoundFXManager.Instance.PlaySoundFXClip(props.AudioStart, m_Ship.transform, 0.5f, props.Duration);
            }
        }

        private void RemoveSpeedBoost(PowerupProperties props)
        {
            if (props.PowerupType == PowerupType.Speedboost)
            {
                m_Ship.Thrust -= props.BonusSpeed;
                m_Ship.MaxLinearVelocity -= props.BonusSpeed;
            }
        }

        public void ApplyStatBoost(PowerupStats stats)
        {

            if (stats.Type == PowerupStats.EffectType.AddEnergy)
                m_Ship.AddEnergy((int)stats.Value);
            if (stats.Type == PowerupStats.EffectType.AddAmmo)
                m_Ship.AddAmmo((int)stats.Value);
        }

        public void ApplyWeapon(TurretProperties weapon)
        {
            m_Ship.AssignWeapons(weapon);

            if (weapon.Mode == TurretMode.Secondary)
                m_Ship.AddAmmo(m_Ship.MaxAmmo);
            if (weapon.Mode == TurretMode.Primary)
                m_Ship.AddEnergy((int)m_Ship.MaxEnergy);
        }
        #endregion
    }
}