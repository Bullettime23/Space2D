using UnityEngine;

namespace SpaceShooter
{
   
    public class PowerupStats : Powerup
    {
        public enum EffectType
        {
            AddEnergy,
            AddAmmo,
        }

        [SerializeField] EffectType m_Type;
        public EffectType Type => m_Type;
        [SerializeField] float m_Value ;
        public float Value => m_Value;

        protected override void OnPickedUp(PowerupPicker picker)
        {
            picker.ApplyStatBoost(this);
        }
    }
}