using TMPro;
using UnityEngine;

namespace SpaceShooter
{
    public class KillText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_Text;

        private string lastKillText;

        private void Update()
        {
            string killText = "Kill:" + Player.Instance.Kills.ToString();

            if (killText != lastKillText)
            {
                lastKillText = killText;
                m_Text.text = lastKillText;
            }
        }
    }
}