using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelBuilder : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_PlayerHUDPrefab;
        [SerializeField] private GameObject m_PalyerGUIPrefab;
        [SerializeField] private GameObject m_BackgroundPrefab;
        [SerializeField] private GameObject m_VirtualGamepadPrefab;

        //Utility
        [SerializeField] private GameObject m_ExplosionControllerPrefab;
        [SerializeField] private GameObject m_SoundFXManagerPrefab;
        [SerializeField] private GameObject m_AudioManager;
        [SerializeField] private GameObject m_EventSystemPrefab;


        [Header("Dependencies")]
        [SerializeField] private PlayerSpawner m_PlayerSpawner;
        [SerializeField] private LevelProperties m_LevelProps;

        private void Awake()
        {
            Instantiate(m_SoundFXManagerPrefab);
            AudioManager audioManager = Instantiate(m_AudioManager).GetComponent<AudioManager>();
            audioManager.PlayLevelMusic(m_LevelProps);

            Instantiate(m_ExplosionControllerPrefab);
            Instantiate(m_EventSystemPrefab);

            Player player = m_PlayerSpawner.Spawn();

            Instantiate(m_PlayerHUDPrefab);
            GameObject playerGUI = Instantiate(m_PalyerGUIPrefab);
#if UNITY_ANDROID
            Instantiate(m_VirtualGamepadPrefab, playerGUI.transform);
#endif
            GameObject background = Instantiate(m_BackgroundPrefab);
            background.AddComponent<SyncTransform>().SetTarget(player.CameraController.transform);
        }

        private void OnDestroy()
        {
            //Удалить все референсы на корабли
            SpaceShip.Reset();
        }
    }
}
