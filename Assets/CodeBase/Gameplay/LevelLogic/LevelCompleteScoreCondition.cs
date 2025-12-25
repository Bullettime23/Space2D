using UnityEngine;

namespace SpaceShooter
{
    public class LevelCompleteScoreCondition : LevelCondition
    {
        [SerializeField] private int m_Score;
        public override bool IsCompleted
        {
            get
            {
                if (Player.Instance.ActiveShip != null)
                {
                    return Player.Instance.Score >= m_Score;
                }
                return false;
            }
        }
    }
}