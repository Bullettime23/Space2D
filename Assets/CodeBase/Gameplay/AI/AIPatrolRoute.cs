using UnityEngine;
using System.Collections.Generic;

namespace SpaceShooter
{
    /// <summary>
    /// Скрипт скрипт для патрулирования по заданным координатам
    /// </summary>
    public class AIPatrolRoute : MonoBehaviour
    {
        [SerializeField] private List<Vector3> m_Positions;
        public List<Vector3> Positions => m_Positions;

        public Vector3 NextPosition;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if (collision.)
        }
    }
}