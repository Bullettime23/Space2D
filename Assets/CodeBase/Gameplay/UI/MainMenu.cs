using UnityEngine;

namespace SpaceShooter
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_LevelSelectionPanel;
        [SerializeField] private GameObject m_ShipSelectionPanel;
        [SerializeField] private GameObject m_MainPanel;


        private void Start()
        {
            ShowMenu();
        }
        public void ShowShipSelection()
        {
            m_ShipSelectionPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }

        public void ShowLevelSelection()
        {
            m_LevelSelectionPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }

        public void ShowMenu()
        {
            m_LevelSelectionPanel.SetActive(false);
            m_ShipSelectionPanel.SetActive(false);
            m_MainPanel.SetActive(true);
        }

        public void Quit()
        {
            Application.Quit();
        }

    }
}