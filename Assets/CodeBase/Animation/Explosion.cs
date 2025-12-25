using UnityEngine;
using UnityEngine.Events;
using Common;

namespace SpaceShooter
{
    public class Explosion : MonoBehaviour
    {
        [SerializeField] private Transform m_ExplosionPrefab;
        [SerializeField] private UnityEvent m_AnimationEnd;
        public UnityEvent AnimationEnd => m_AnimationEnd;

        private EmitEventOnDestroy m_AnimationObjectDestroy;

        public void PlayExplodeAnimation(Transform transform)
        {
            var explosionAnimation = Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);
             m_AnimationObjectDestroy = explosionAnimation.GetComponent<EmitEventOnDestroy>();

            if (m_AnimationObjectDestroy != null)
            {
                m_AnimationObjectDestroy.DestroyEvent.AddListener(OnAnimationEnd);
            }
        }
        
        private void OnAnimationEnd()
        {
            m_AnimationObjectDestroy.DestroyEvent.RemoveListener(OnAnimationEnd);
            m_AnimationEnd.Invoke();
        }
    }

}
