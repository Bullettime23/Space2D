using TMPro;
using UnityEngine;

namespace SpaceShooter
{
    public class LivesIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private string lastLivesText;

        private void Update()
        {
            string livesText = Player.Instance.LivesLeft.ToString();

            if (lastLivesText != livesText)
            {
                lastLivesText = livesText;
                m_Text.text = lastLivesText;
            }
        }
    }
}