using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class LevelCompleteAllDestroyed : LevelCondition
    {
        [SerializeField] private int m_DefeatedTeamId;

        [SerializeField] private float m_CheckTime;

        private Timer m_CheckTimer;
        private bool m_IsCompleted = false;

        private void Start()
        {
            m_CheckTimer = new Timer(m_CheckTime);
        }

        private void Update()
        {
            m_CheckTimer.RemoveTime(Time.deltaTime);

            if (m_CheckTimer.IsFinish)
            {
                UpdateIsCompleted();
                m_CheckTimer.Start(m_CheckTime);
            }
        }

        private void UpdateIsCompleted()
        {
            int shipsCount = 0;

            foreach (var ship in SpaceShip.Ships)
            {
                if (ship.TeamId == m_DefeatedTeamId) 
                    shipsCount++;
            }

            Debug.Log("Ships left " + shipsCount.ToString());

            m_IsCompleted = shipsCount == 0;
        }

        public override bool IsCompleted => m_IsCompleted;
    }
}