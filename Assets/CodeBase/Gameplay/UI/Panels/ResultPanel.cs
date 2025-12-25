using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Kills;
        [SerializeField] private TextMeshProUGUI m_Score;
        [SerializeField] private TextMeshProUGUI m_Time;
        [SerializeField] private TextMeshProUGUI m_Result;
        [SerializeField] private TextMeshProUGUI m_ButtonText;

        private bool m_IsLevelPassed = false;

        private void Start()
        {
            gameObject.SetActive(false);
            LevelController.Instance.LevelFailed += OnLevelFailed;
            LevelController.Instance.LevelPassed += OnLevelPass;
        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelFailed -= OnLevelFailed;
            LevelController.Instance.LevelPassed -= OnLevelPass;
        }

        #region Public API
        public void OnButtonNextClick()
        {
            gameObject.SetActive(false);

            if (m_IsLevelPassed == true)
            {
                LevelController.Instance.NextLevel();
                return;
            }

            LevelController.Instance.RestartLevel();

        }
        #endregion

        private void OnLevelFailed()
        {
            gameObject.SetActive(true);
            CreateLevelStatistics();

            m_Result.text = "Lose";

            m_ButtonText.text = "Restart";
        }

        private void OnLevelPass()
        {
            gameObject.SetActive(true);
            CreateLevelStatistics();

            m_IsLevelPassed = true;

            m_Result.text = "Passed";

            if (LevelController.Instance.HasNextLevel == true)
            {
                m_ButtonText.text = "Next level";
            }
            else
            {
                m_ButtonText.text = "Main menu";
                AudioManager.Instance.PlayMenuMusic();
            }
        }

        private void CreateLevelStatistics()
        {
            m_Kills.text = "Kills: " + Player.Instance.Kills.ToString();
            m_Score.text = "Score: " + Player.Instance.Score.ToString();
            //Показать только целое значение
            m_Time.text = "Time: " + LevelController.Instance.LevelTime.ToString("F0");
        }
    }
}