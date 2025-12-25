using UnityEngine;
using Common;

namespace SpaceShooter
{
    /// <summary>
    /// Для большого астероида
    /// Чтобы созавать несколько маленьких в месте разлома
    /// </summary>
    public class AsteroidFragile : Destructible
    {
        [SerializeField] private Transform m_AsteroidPrefab;
        [SerializeField] private int m_MinFragments;
        [SerializeField] private int m_MaxFragments;
        [SerializeField] private float m_MaxFragmentVelocity;
        [SerializeField] private ImpactEffect m_EffectOnDestory;

        override protected void OnDeath()
        {
            SpawnAsteroidFragments();
            ExplosionController.Instance.PlayEffect(m_EffectOnDestory ,transform);
            base.OnDeath();
        }

        private void SpawnAsteroidFragments()
        {
            int fragmentsNumber = Random.Range(m_MinFragments, m_MaxFragments);

            for (int i = 0; i < fragmentsNumber; i++)
            {
                var asteroid = Instantiate(m_AsteroidPrefab, new Vector3(transform.position.x + i * 5, transform.position.y + i * 5, transform.position.z), Quaternion.identity);

                Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();

                rb.linearVelocity = new Vector2(Random.Range(-m_MaxFragmentVelocity, m_MaxFragmentVelocity), Random.Range(--m_MaxFragmentVelocity, m_MaxFragmentVelocity));

                /// Вращение
                rb.angularVelocity = Random.Range(-30, 30);
            }
        }
    }
}