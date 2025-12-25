using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Infrastructure;


namespace SpaceShooter
{
    public class LevelController : Singleton<LevelController>
    {
        public event UnityAction LevelPassed;
        public event UnityAction LevelFailed;

        [SerializeField] private LevelProperties m_Level;

        [SerializeField] private LevelCondition[] m_Conditions;

        private bool m_IsLevelCompleted = false;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        private void Start()
        {
            Time.timeScale = 1;
            m_LevelTime = 0;

            AudioManager.Instance.PlayLevelMusic(m_Level);
        }
        private void Update()
        {
            if (m_IsLevelCompleted == false)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();
            }

            if (Player.Instance.LivesLeft == 0)
                Loss();
        }

        #region Public API
        public bool HasNextLevel => m_Level.NextLevel != null;
        public void NextLevel()
        {
            if (HasNextLevel == true)
            {
                SceneManager.LoadScene(m_Level.NextLevel.SceneName);
                return;
            }

            SceneManager.LoadScene("main_menu");
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(m_Level.SceneName);
            Time.timeScale = 1;
        }
        #endregion

        private void CheckLevelConditions()
        {

            int conditionsCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted == true)
                    conditionsCompleted++;

            }

            if (conditionsCompleted == m_Conditions.Length)
            {
                m_IsLevelCompleted = true;
                Pass();
            }
        }

        private void Loss()
        {
            LevelFailed?.Invoke();
            Time.timeScale = 0;
        }

        private void Pass()
        {
            LevelPassed?.Invoke();
            Time.timeScale = 0;
        }
    }
}