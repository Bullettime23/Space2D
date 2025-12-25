using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] GameObject m_Panel;

        private void Start()
        {
            m_Panel.SetActive(false);
        }

        public void ShowPause()
        {
            m_Panel.SetActive(true);
            Time.timeScale = 0;
        }

        public void HidePause()
        {
            m_Panel.SetActive(false);
            Time.timeScale = 1;
        }

        public void LoadMainMenu()
        {
            m_Panel.SetActive(false);
            Time.timeScale = 1;

            // Main menu should be first scene
            SceneManager.LoadScene(0);
        }

        public void TogglePause()
        {
            if (m_Panel.activeSelf == true)
            {
                m_Panel.SetActive(false);
                Time.timeScale = 1;
                return;
            }

            m_Panel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}