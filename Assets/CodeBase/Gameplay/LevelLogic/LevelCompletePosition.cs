using UnityEngine;

namespace SpaceShooter
{
    public class LevelCompletePosition : LevelCondition
    {
        [SerializeField] private float m_Radius;

        public override bool IsCompleted
        {
            get
            {
                if (Player.Instance.ActiveShip == null) return false;

                return m_Radius >= Vector3.Distance(Player.Instance.ActiveShip.transform.position, transform.position);
            }
        }

#if UNITY_EDITOR
        private Color m_GizmoColor = new Color(0, 1, 0, 0.3f);

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = m_GizmoColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }
#endif
    }
}
