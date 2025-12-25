using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class EntitySpawner : MonoBehaviour
    {
        public enum SpawnMode
        {
            Start,
            Loop,
        }

        [SerializeField] private Entity[] m_EntityPrefabs;
        [SerializeField] private CircleArea m_Area;
        public CircleArea Area => m_Area;
        [SerializeField] private SpawnMode m_Mode;
        [SerializeField] private int m_NumSpawns;
        [SerializeField] private float m_RespawnTime;

        private float m_Timer;

        void Start()
        {
            if (m_Mode == SpawnMode.Start)
            {
                SpawnEntities();
            }

            m_Timer = m_RespawnTime;
        }

        // Update is called once per frame
        void Update()
        {
            if (m_Timer > 0)
            {
                m_Timer -= Time.deltaTime;
            }

            if (m_Mode == SpawnMode.Loop && m_Timer < 0)
            {
                SpawnEntities();

                m_Timer = m_RespawnTime;
            }
        }

        private void SpawnEntities()
        {
            for (int i = 0; i < m_NumSpawns; i++)
            {
                int index = Random.Range(0, m_EntityPrefabs.Length);

                GameObject entity = Instantiate(m_EntityPrefabs[index].gameObject);

                entity.transform.position = m_Area.GetRandomInsideZone();
            }
        }
    }
}