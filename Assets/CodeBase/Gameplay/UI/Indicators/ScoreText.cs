using TMPro;
using UnityEngine;

namespace SpaceShooter
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private string lastScoreText;

        private void Update()
        {
            string scoreText = "Score:" + Player.Instance.Score.ToString();

            if (scoreText != lastScoreText)
            {
                lastScoreText = scoreText;
                m_Text.text = lastScoreText;
            }
        }
    }
}