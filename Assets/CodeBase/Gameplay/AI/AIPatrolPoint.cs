using UnityEngine;

namespace SpaceShooter
{
    public class AIPatrolPoint : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        public float Radius => m_Radius;

        private readonly static Color GizmoColor = new Color(0.75f, 0.05f, 0.05f, 0.3f);

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = GizmoColor;
            Gizmos.DrawSphere(transform.position, m_Radius);
        }
#endif
    }
}