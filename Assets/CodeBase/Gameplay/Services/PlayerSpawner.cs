using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private FollowCamera m_FollowCameraPrefab;
        [SerializeField] private Player m_PlayerPrefab;
        [SerializeField] private MovementController m_ShipMovementControllerPrefab;
        
        [SerializeField] private Transform m_SpawnPoint;

        public Player Spawn()
        {
            FollowCamera followCamera = Instantiate(m_FollowCameraPrefab);

            MovementController movementController = Instantiate(m_ShipMovementControllerPrefab);
            Player player = Instantiate(m_PlayerPrefab);
            player.Construct(followCamera, movementController, m_SpawnPoint);

            return player;
        }

    }
}