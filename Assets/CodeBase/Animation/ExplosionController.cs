using SpaceShooter;
using UnityEngine;
using Common;
using Infrastructure;

namespace SpaceShooter
{
    public class ExplosionController : Singleton<ExplosionController>
    {
        [SerializeField] private Transform m_ExplosionPrefab;

        public EmitEventOnDestroy InstanciateExplosion(Transform trfm)
        {
            EmitEventOnDestroy explosion = Instantiate(m_ExplosionPrefab, transform.InverseTransformPoint(trfm.position), trfm.rotation, transform).transform.GetComponent<EmitEventOnDestroy>();
            return explosion;
        }

        public void CreateExplosion(Transform trfm)
        {
            Instantiate(m_ExplosionPrefab, transform.InverseTransformPoint(trfm.position), trfm.rotation, transform).GetComponent<Explosion>();
        }

        public void PlayEffect(ImpactEffect effect, Transform trfm)
        {
            Instantiate(effect, transform.InverseTransformPoint(trfm.position), trfm.rotation, transform);

        }
    }

}