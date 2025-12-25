using UnityEngine;
using Common;

namespace SpaceShooter
{
    /// <summary>
    /// Уничтожаемый объект на сцене, который может иметь хитпоинты
    /// </summary>
    public class Destructible : DestructibleBase
    {
        public static int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        protected override void Start()
        {
            base.Start();

            transform.SetParent(null);
        }
    }
}
