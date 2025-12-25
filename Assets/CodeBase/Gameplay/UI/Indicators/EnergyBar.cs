using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class EnergyBar : MonoBehaviour
    {
        [SerializeField] private Image m_Image;

        private float lastEnergy;

        private void Update()
        {
            if (Player.Instance.ActiveShip == null) return;

            float energy = (float)Player.Instance.ActiveShip.Energy / (float)Player.Instance.ActiveShip.MaxEnergy;

            if (energy != lastEnergy)
            {
                lastEnergy = energy;
                m_Image.fillAmount = lastEnergy;
            }
        }
    }
}