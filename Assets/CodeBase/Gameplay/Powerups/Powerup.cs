using UnityEngine;
using Common;

namespace SpaceShooter
{
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Powerup : Entity
    {

        private void OnTriggerEnter2D(Collider2D collision)
        {
            PowerupPicker picker = collision.transform.root.GetComponent<PowerupPicker>();

            if (picker != null && Player.Instance.ActiveShip)
            {
                OnPickedUp(picker);
                Destroy(gameObject);
            }
        }


        protected abstract void OnPickedUp(PowerupPicker picker);
    }
}