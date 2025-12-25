using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{

    public class ShipSelectionCard : MonoBehaviour
    {
        [SerializeField] private MainMenu m_Menu;
        [SerializeField] private SpaceShip m_Prefab;

        [SerializeField] private TextMeshProUGUI m_ShipName;
        [SerializeField] private TextMeshProUGUI m_Hitpoints;
        [SerializeField] private TextMeshProUGUI m_Speed;
        [SerializeField] private TextMeshProUGUI m_Agility;
        [SerializeField] private Image m_Preview;

        private void Start()
        {
            if (m_Prefab != null)
            {
                m_ShipName.text = m_Prefab.Nickname;
                m_Hitpoints.text = "HP: " + m_Prefab.InitialHitPoints.ToString();
                m_Speed.text = "Speed: " + m_Prefab.MaxLinearVelocity.ToString();
                m_Agility.text = "Agility: " + m_Prefab.MaxAngularVelocity.ToString();
                m_Preview.sprite = m_Prefab.PreviewImage;
            }
        }

        public void SelectShip()
        {
            Player.SelectedSpaceship = m_Prefab;
            m_Menu.ShowMenu();
        }
    }
}