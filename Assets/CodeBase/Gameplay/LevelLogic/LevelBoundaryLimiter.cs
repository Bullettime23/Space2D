using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Работает в связке с Level Boundary
    /// Кидается на объект, который нужно ограничить
    /// </summary>
    public class LevelBoundaryLimiter : MonoBehaviour
    {
        private void Update()
        {
            if (LevelBoundary.Instance == null) return;

            var lb = LevelBoundary.Instance;
            var rad = lb.Radius;

            if (transform.position.magnitude > rad)
            {
                if (lb.LimitMode == LevelBoundary.Mode.Limit)
                {
                    transform.position = transform.position.normalized * rad;
                    return;
                }

                if (lb.LimitMode == LevelBoundary.Mode.Teleport)
                {
                    transform.position = -transform.position.normalized * rad;
                }
            }
        }
    }
}