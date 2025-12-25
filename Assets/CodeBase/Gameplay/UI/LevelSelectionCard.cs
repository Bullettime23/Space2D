using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LevelSelectionButton : MonoBehaviour
    {
        [SerializeField] private LevelProperties m_LevelProperties;
        [SerializeField] private TextMeshProUGUI m_LevelName;
        [SerializeField] private Image m_LevelPreview;
        [SerializeField] private TextMeshProUGUI m_LevelDescription;

        private void Start()
        {
            if (m_LevelProperties == null) return;

            m_LevelName.text = m_LevelProperties.Title;
            m_LevelDescription.text = m_LevelProperties.SceneDescription;
            m_LevelPreview.sprite = m_LevelProperties.PrevieImage;
        }
        public void SelectLevel()
        {
            SceneManager.LoadScene(m_LevelProperties.SceneName);
        }
    }
}