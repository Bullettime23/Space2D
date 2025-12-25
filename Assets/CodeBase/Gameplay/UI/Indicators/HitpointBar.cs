using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class HitpointBar : MonoBehaviour
    {
        [SerializeField] private Image m_Image;

        private float lastHitPoints;

        private void Update()
        {
            if (Player.Instance.ActiveShip == null) return;

            float hitpoints = (float)Player.Instance.ActiveShip.CurrentHitPoints / (float)Player.Instance.ActiveShip.InitialHitPoints;

            if (hitpoints != lastHitPoints)
            {
                lastHitPoints = hitpoints;
                m_Image.fillAmount = lastHitPoints;
            }
        }
    }
}