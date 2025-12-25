using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Обрабатывает ввод от Unity Player Input System и вызывает методы SpaceShip
    /// </summary>
    public class MovementController : MonoBehaviour
    {
        private PlayerControls m_PlayerControls;

        private Vector2 lastMove;

        private void Awake()
        {
            m_PlayerControls = new PlayerControls();
        }
        private void Update()
        {
            if (Player.Instance.ActiveShip != null)
            {
                Vector2 moveActionValue = m_PlayerControls.Player.Move.ReadValue<Vector2>();
                if (moveActionValue != lastMove)
                {
                    Player.Instance.ActiveShip.OnPlayerMove(moveActionValue);
                    lastMove = moveActionValue;
                }

                if (m_PlayerControls.Player.AttackPrimary.triggered)
                    Player.Instance.ActiveShip.Shoot(TurretMode.Primary);

                if (m_PlayerControls.Player.AttackSecondary.triggered)
                    Player.Instance.ActiveShip.Shoot(TurretMode.Secondary);
            }
        }

        private void OnEnable()
        {
            m_PlayerControls.Enable();
        }

        private void OnDisable()
        {
            m_PlayerControls.Disable();
        }
    }
}