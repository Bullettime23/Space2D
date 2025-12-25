
using UnityEngine;
using System.Collections.Generic;


namespace SpaceShooter
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        /// <summary>
        /// Spaceship rigidbody mass
        /// </summary>
        [Header("Spaceship")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Acceleration power
        /// </summary>
        [SerializeField] private float m_Thrust;
        public float Thrust
        {
            get { return m_Thrust; }
            set { m_Thrust = value; }
        }

        /// <summary>
        /// Spinning power
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Max linear speed
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;
        public float MaxLinearVelocity
        {
            get { return m_MaxLinearVelocity; }
            set { m_MaxLinearVelocity = value; }
        }

        /// <summary>
        /// Max angular speed in deg/sec
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;

        [SerializeField] private Sprite m_PreviewImage;
        public Sprite PreviewImage => m_PreviewImage;

        /// <summary>
        /// Saved link to the rigid body
        /// </summary>
        private Rigidbody2D m_Rigid;

        #region Public API
        /// <summary>
        /// Access to the thrust of the ship -1.0 to +1.0
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Access to the Torque of the ship -1.0 to +1.0
        /// </summary>
        public float TorqueControl { get; set; }

        private static HashSet<SpaceShip> m_Ships;
        public static HashSet<SpaceShip> Ships => m_Ships;

        public static void Reset()
        {
            m_Ships.Clear();
        }
        #endregion


        #region Unity Events
        protected override void Start()
        {
            base.Start();

            if (m_Ships == null)
                m_Ships = new HashSet<SpaceShip>();

            m_Ships.Add(this);

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitiateOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();

            UpdateEnergyRegen();
        }

        public void OnPlayerMove(Vector2 move)
        {
            ThrustControl = move.y;
            TorqueControl = -move.x;
        }

        #endregion

        /// <summary>
        /// Метод добавления сил кораблю для движения
        /// </summary>
        private void UpdateRigidBody()
        {
            ///Направление на фиксированное время между кадрами (реальное время может отличаться, на силу ускорения на контроллер)
            m_Rigid.AddForce(transform.up * Time.fixedDeltaTime * m_Thrust * ThrustControl, ForceMode2D.Force);

            /// Трение и ограничение по скорости
            m_Rigid.AddForce(-m_Rigid.linearVelocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            /// Вращение
            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);


            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        #region Shooting

        [SerializeField] private Turret[] m_Turrets;
        public Turret[] Turrets => m_Turrets;
        public void Shoot(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode) m_Turrets[i].Fire();
            }
        }

        [SerializeField] private float m_MaxEnergy;
        public float MaxEnergy => m_MaxEnergy;
        [SerializeField] private float m_EnergyRegen;
        [SerializeField] private int m_MaxAmmo;
        public int MaxAmmo => m_MaxAmmo;

        private float m_PrimaryEnergy;
        public float Energy => m_PrimaryEnergy;
        private float m_SecondaryAmmo;
        public float SecondaryAmmo => m_SecondaryAmmo;
        private void InitiateOffensive()
        {
            m_PrimaryEnergy = m_MaxEnergy;
            m_SecondaryAmmo = m_MaxAmmo;
        }

        public void AddEnergy(int energy)
        {
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_SecondaryAmmo = Mathf.Clamp(m_SecondaryAmmo + ammo, 0, m_MaxAmmo);
        }
        private void UpdateEnergyRegen()
        {
            m_PrimaryEnergy += Time.fixedDeltaTime * m_EnergyRegen;
            m_PrimaryEnergy = Mathf.Clamp(m_PrimaryEnergy, 0, m_MaxEnergy);
        }

        public bool DrawAmmo(int ammo)
        {
            if (m_SecondaryAmmo >= ammo)
            {
                m_SecondaryAmmo -= ammo;
                return true;
            }

            return false;
        }

        public bool DrawEnergy(int energy)
        {
            if (m_PrimaryEnergy >= energy)
            {
                m_PrimaryEnergy -= energy;
                return true;
            }

            return false;
        }

        public void AssignWeapons(TurretProperties props)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadout(props);
            }
        }
        #endregion


        protected override void OnDeath()
        {
            ExplosionController.Instance.CreateExplosion(transform);
            m_Ships.Remove(this);
            base.OnDeath();
        }
    }
}