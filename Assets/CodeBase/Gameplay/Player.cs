using UnityEngine;
using Common;
using Infrastructure;

namespace SpaceShooter
{
    public class Player : Singleton<Player>
    {
        public static SpaceShip SelectedSpaceship;

        [SerializeField] int m_NumLives;
        public int LivesLeft => m_NumLives;

        [SerializeField] SpaceShip m_Spaceship;
        public SpaceShip ActiveShip => m_Spaceship;

        [SerializeField] SpaceShip m_PlayerShipPrefab;
        // Когда игрок выбирает корабль в главном меню, ему дается этот корабль.
        // Если игра запущена для тестирования, вернется указанный префаб
        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceship == null)
                {
                    return m_PlayerShipPrefab;
                }
                return SelectedSpaceship;
            }
        }

        private FollowCamera m_CameraController;
        private MovementController m_MovementController;
        private Transform m_SpawnPoint;

        public FollowCamera CameraController => m_CameraController;

        public void Construct(FollowCamera followCamera, MovementController movementController, Transform spawnPoint)
        {
            m_CameraController = followCamera;
            m_MovementController = movementController;
            m_SpawnPoint = spawnPoint;
        }

        private int m_Score;
        public int Score => m_Score;

        private int m_Kills;
        public int Kills => m_Kills;

        private EmitEventOnDestroy m_ExplosionAnimationEnd;

        private void Start()
        {
            Respawn();
        }

        #region Public API
        public void AddKill()
        {
            m_Kills++;
        }

        public void AddScore(int score)
        {
            m_Score += score;
        }
        #endregion

        private void OnShipDeath()
        {
            m_MovementController.enabled = false;
            m_ExplosionAnimationEnd = ExplosionController.Instance.InstanciateExplosion(m_Spaceship.transform);
            m_ExplosionAnimationEnd.DestroyEvent.AddListener(OnDeathAnimationEnd);
        }

        private void OnDeathAnimationEnd()
        {
            m_NumLives--;
            m_ExplosionAnimationEnd.DestroyEvent.RemoveListener(OnDeathAnimationEnd);
            if (m_NumLives > 0) Respawn();

        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Spaceship = newPlayerShip.GetComponent<SpaceShip>();
            m_Spaceship.EventOnDeath.AddListener(OnShipDeath);
            m_CameraController.SetTarget(newPlayerShip.transform);
            m_MovementController.enabled = true;
        }
    }
}