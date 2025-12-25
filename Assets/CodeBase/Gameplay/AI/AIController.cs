using UnityEngine;
using Common;


namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol,
            PatrolRoute,
            Attack,
        }

        [SerializeField] private AIBehaviour m_Behavior;

        [SerializeField] private AIPatrolPoint[] m_PatrolPoints;
        [SerializeField] private AIPatrolPoint m_PatrolPointPrefab;
        [SerializeField] private float m_PatrolPointTime;

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0f, 1f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        [SerializeField] private float m_ShootDistance;

        [SerializeField] private float m_ColliderWidth;


        private Vector3 m_MovePosition;

        private SpaceShip m_Ship;

        private Destructible m_SelectedTarget;
        private Rigidbody2D m_SelectedTargetRigidBody;
        private Projectile m_PrimaryProjectile;
        private AIBehaviour m_PrevBehaviour;

        private AIPatrolPoint m_CurrentPatrolPoint;
        private int m_PatrolPointIndex;
        private bool m_IsPatrolPointCreated = false;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;
        private Timer m_PatrolPointTimer;
        private void Start()
        {
            m_Ship = GetComponent<SpaceShip>();


            if (m_PatrolPoints.Length == 0 || m_PatrolPoints[0] == null)
            {
                AIPatrolPoint newPoint = Instantiate(m_PatrolPointPrefab, m_Ship.transform.position, Quaternion.identity);
                AIPatrolPoint[] patrolPoints = { newPoint };
                m_PatrolPoints = patrolPoints;
                m_IsPatrolPointCreated = true;
            }

            foreach (Turret turret in m_Ship.Turrets)
            {
                if (turret.Mode == TurretMode.Primary)
                {
                    Turret primaryWeapon = turret;
                    m_PrimaryProjectile = primaryWeapon.Props.ProjectilePrefab.GetComponent<Projectile>();
                    break;
                }

            }
            InitTimers();

            Gizmos.color = new Color(0, 1f, 1f, 0.5f);
        }

        private void Update()
        {
            UpdateTimers();
            UpdateAI();
        }

        private void OnDestroy()
        {
            if(m_IsPatrolPointCreated == true && m_PatrolPoints[0] != null)
            {
                Destroy(m_PatrolPoints[0].gameObject);
            } 
        }

        private void UpdateAI()
        {
            if (m_Behavior == AIBehaviour.Null)
            {
                return;
            }

            if (m_Behavior == AIBehaviour.Patrol || m_Behavior == AIBehaviour.PatrolRoute)
            {
                UpdateBehaviourPatrol();
            }

            if (m_Behavior == AIBehaviour.Attack)
            {
                UpdateBehaviourAttack();
            }

        }

        private void UpdateBehaviourPatrol()
        {
            ActionFindNewAttackTarget();
            if (m_Behavior != AIBehaviour.Patrol && m_Behavior != AIBehaviour.PatrolRoute)
                return;
            ActionFindNewMovePosition();
            ActionEvadeCollision();
            ActionControlShip();
        }

        private void UpdateBehaviourAttack()
        {
            // Приблизиться к цели
            ActionFindNewMovePosition();
            // Зайти на цель
            ActionEvadeCollision();
            ActionControlShip();

            // Снаряды попадут?

            // Открыть огонь
            if (IsAimTook() == true)
                ActionFire();
            // Цель уничтожена?
            ActionOnTargetDestroyed();
            // Преследовать цель
            // Вернуться к патрулированию

        }

        #region Movement and Patrol
        private void ActionFindNewMovePosition()
        {
            if (m_SelectedTarget != null)
            {
                SetBehaviour(AIBehaviour.Attack);
            }

            if (m_Behavior == AIBehaviour.Attack)
            {
                m_MovePosition = MakeLead();
            }

            if (m_Behavior == AIBehaviour.PatrolRoute)
            {
                if (m_PatrolPointTimer.IsFinish == true)
                {
                    m_PatrolPointIndex++;

                    if (m_PatrolPointIndex == m_PatrolPoints.Length)
                        m_PatrolPointIndex = 0;

                    m_PatrolPointTimer.Start(m_PatrolPointTime);
                }
                m_CurrentPatrolPoint = m_PatrolPoints[m_PatrolPointIndex];

                PatrolCurrentPoint();

            }

            if (m_Behavior == AIBehaviour.Patrol)
            {
                m_CurrentPatrolPoint = m_PatrolPoints[0];
                PatrolCurrentPoint();
            }
        }

        private void PatrolCurrentPoint()
        {
            if (m_CurrentPatrolPoint != null)
            {
                bool isInsidePatrolZone = (m_CurrentPatrolPoint.transform.position - transform.position).magnitude < m_CurrentPatrolPoint.Radius;

                if (isInsidePatrolZone == true)
                {
                    if (m_RandomizeDirectionTimer.IsFinish == true)
                    {

                        Vector2 newPoint = Random.onUnitSphere * m_CurrentPatrolPoint.Radius + m_CurrentPatrolPoint.transform.position;

                        m_MovePosition = newPoint;

                        m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                    }


                }
                else
                {
                    m_MovePosition = m_CurrentPatrolPoint.transform.position;
                }
            }
        }

        private static float MAX_ANGLE = 45.0f;
        private static float MAX_ATTACK_ANGLE = 90.0f;

        /// <summary>
        /// Рассчитать угол поворота корабля к цели, с учетом максимально возможного угла поворота
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static float CalculateAlignTorqeNormalized(Vector3 targetPosition, Transform ship, bool limit = true)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = limit ? Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE : Mathf.Clamp(angle, -MAX_ATTACK_ANGLE, MAX_ATTACK_ANGLE) / MAX_ATTACK_ANGLE;

            return -angle;
        }

        private int m_EvadeTurnSign;
        private Timer m_EvadeTurnTimer;
        private void ActionEvadeCollision()
        {
            if (m_EvadeTurnTimer.IsFinish)
            {
                m_EvadeTurnSign = Mathf.RoundToInt(Random.Range(0, 1)) == 1 ? 1 : -1;
                m_EvadeTurnTimer.Start(5);
            }

            if (Physics2D.CircleCast(m_Ship.transform.position, m_ColliderWidth, m_Ship.transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = m_Ship.transform.position + m_EvadeTurnSign * transform.right * 100.0f;
            }
        }

        #endregion
        private void ActionControlShip()
        {
            if (m_Behavior == AIBehaviour.Attack)
            {
                float thrust = m_NavigationLinear;

                if (Vector3.Distance(m_Ship.transform.position, m_MovePosition) <= m_ShootDistance)
                    thrust = 0;

                m_Ship.ThrustControl = thrust;

                m_Ship.TorqueControl = CalculateAlignTorqeNormalized(m_MovePosition, m_Ship.transform, false) * m_NavigationAngular;
            }
            else
            {
                // Чем меньше расстояние, тем меньше скорость
                float distanceMultiplyer = Mathf.Clamp(Vector3.Distance(m_Ship.transform.position, m_MovePosition), 0.2f, 1f);

                m_Ship.ThrustControl = m_NavigationLinear * distanceMultiplyer;

                m_Ship.TorqueControl = CalculateAlignTorqeNormalized(m_MovePosition, m_Ship.transform) * m_NavigationAngular;

            }
        }
        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinish == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();
                m_FindNewTargetTimer.Start(m_FindNewTargetTime);
            }

            if (m_SelectedTarget != null)
            {
                SetBehaviour(AIBehaviour.Attack);
            }
        }

        /// <summary>
        /// Нужно, чтобы корабль направлял оружие по курсу цели, с упреждением
        /// </summary>
        private Vector3 MakeLead()
        {
            if (m_SelectedTarget == null)
            {
                return m_MovePosition;
            }

            m_SelectedTargetRigidBody = m_SelectedTarget.transform.root.GetComponent<Rigidbody2D>();
            Vector3 targetPosition = m_SelectedTarget.transform.position;
            Vector3 targetShipVelocity = m_SelectedTarget.GetComponent<Rigidbody2D>().linearVelocity;

            float interceptionTime = Vector3.Distance(m_Ship.transform.position, targetPosition) /
                (m_PrimaryProjectile.Velocity * m_PrimaryProjectile.transform.up - targetShipVelocity).magnitude;

            Vector3 lockPosition = targetPosition +
                new Vector3(m_SelectedTargetRigidBody.linearVelocity.x, m_SelectedTargetRigidBody.linearVelocity.y, 0) * interceptionTime;

            return lockPosition;
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = 500f;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                Destructible d = v as Destructible;

                if (d.GetComponent<SpaceShip>() == m_Ship) continue;

                if (d.TeamId == Destructible.TeamIdNeutral) continue;

                if (d.TeamId == m_Ship.TeamId) continue;

                float dist = Vector2.Distance(m_Ship.transform.position, d.transform.position);

                if (dist <= maxDist)
                {
                    maxDist = dist;
                    potentialTarget = d;
                }
            }

            return potentialTarget;

        }

        private bool IsAimTook()
        {
            Vector3 distance = m_MovePosition - m_Ship.transform.position;
            float angle = Mathf.Acos(Vector3.Dot(distance.normalized, m_Ship.transform.up));

            return Mathf.Abs(angle) < 0.05f;
        }
        private void ActionFire()
        {
            if (m_Behavior == AIBehaviour.Attack && m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinish == true)
                {
                    m_Ship.Shoot(TurretMode.Primary);
                    m_FireTimer.Start(m_ShootDelay);
                }
            }
        }
        private void ActionOnTargetDestroyed()
        {
            if (m_SelectedTarget == null)
                m_Behavior = AIBehaviour.PatrolRoute;
        }


        #region Behaviour

        public void SetBehaviour(AIBehaviour nextBehaviour)
        {
            m_PrevBehaviour = m_Behavior;
            m_Behavior = nextBehaviour;
        }

        public void RetrunPrevBehaviour(AIBehaviour nextBehaviour)
        {
            m_Behavior = m_PrevBehaviour;
        }
        public void SetPatrolBehaviourPoint(AIPatrolPoint point)
        {
            SetBehaviour(AIBehaviour.Patrol);
            m_CurrentPatrolPoint = point;
        }

        public void SetPatrolBehaviourRoute(AIPatrolPoint[] points)
        {
            m_PatrolPointIndex = 0;
            SetBehaviour(AIBehaviour.PatrolRoute);
            m_PatrolPoints = points;
        }
        #endregion



        #region Timers
        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);
            m_FireTimer = new Timer(m_ShootDelay);
            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
            m_EvadeTurnTimer = new Timer(5);
            m_PatrolPointTimer = new Timer(m_PatrolPointTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);
            m_EvadeTurnTimer.RemoveTime(Time.deltaTime);
            m_FireTimer.RemoveTime(Time.deltaTime);
            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
            m_PatrolPointTimer.RemoveTime(Time.deltaTime);
        }
        #endregion

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (m_Ship != null && m_MovePosition != null)
            {
                Gizmos.DrawRay(m_Ship.transform.position, m_Ship.transform.up * Vector3.Distance(m_Ship.transform.position, m_MovePosition));
                Gizmos.DrawSphere(m_Ship.transform.position, m_ColliderWidth / 2);
            }

        }
#endif
    }
}