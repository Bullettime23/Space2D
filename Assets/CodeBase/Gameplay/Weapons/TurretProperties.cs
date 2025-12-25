using UnityEngine;
using UnityEngine.Audio;


namespace SpaceShooter
{
    public enum TurretMode
    {
        Primary,
        Secondary
    }
    [CreateAssetMenu(fileName = "TurretProperties", menuName = "Scriptable objects/Turret properties")]
    public sealed class TurretProperties : ScriptableObject
    {
        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private Projectile m_ProjectilePrefab;
        public Projectile ProjectilePrefab => m_ProjectilePrefab;

        [SerializeField] private float m_FireRate;
        public float FireRate => m_FireRate;

        [SerializeField] private int m_EnergyUsage;
        public int EnergyUsage => m_EnergyUsage;

        [SerializeField] private int m_AmmoUsage;
        public int AmmoUsage => m_AmmoUsage;

        [SerializeField] private AudioResource m_LaunchSFX;
        public AudioResource LaunchSFX => m_LaunchSFX;

    }
}