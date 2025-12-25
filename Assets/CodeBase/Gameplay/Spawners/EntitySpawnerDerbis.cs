using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class EntitySpawnerDerbis : MonoBehaviour
    {
        [SerializeField] private Destructible[] m_DerbisPrefabs;
        [SerializeField] private CircleArea m_Area;
        [SerializeField] private int m_NumDerbis;
        [SerializeField] private float m_RandomSpeed;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            for (int i= 0; i < m_NumDerbis; i++)
            {
                SpawnDerbis();
            }

        }

        private void SpawnDerbis()
        {
            int index = Random.Range(0, m_DerbisPrefabs.Length);

            GameObject debris = Instantiate(m_DerbisPrefabs[index].gameObject);

            debris.transform.position = m_Area.GetRandomInsideZone();

            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDeath);

            Rigidbody2D rb = debris.GetComponent<Rigidbody2D>();

            if (rb != null && m_RandomSpeed > 0)
            {
                rb.linearVelocity = UnityEngine.Random.insideUnitCircle * m_RandomSpeed;
            }
        }

        private void OnDebrisDeath()
        {
            SpawnDerbis();
        }
    }
}