using TMPro;
using UnityEngine;

namespace SpaceShooter
{
    public class AmmoText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_AmmoText;

        private float lastAmmo;

        private void Update()
        {
            if (Player.Instance.ActiveShip == null) return;

            float ammo = Player.Instance.ActiveShip.SecondaryAmmo;


            if (ammo != lastAmmo)
            {
                string ammoText = $"{ammo}/{Player.Instance.ActiveShip.MaxAmmo}";

                lastAmmo = ammo;
                m_AmmoText.text  = ammoText;
            }
        }
    }
}