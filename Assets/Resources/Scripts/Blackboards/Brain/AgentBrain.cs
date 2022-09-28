using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brain
{
    public class AgentBrain : MonoBehaviour
    {
        [SerializeField] protected AgentController _controller;
        [SerializeField] private Collider2D _collider;
        [SerializeField] protected GunsInventory _guns;
        [SerializeField] protected PlayerHealth _health;

        public bool UnderAttack
        {
            get { return Map.UnderAttack; }
        }

        public bool EnemyDanger
        {
            get { return Map.EnemyInRange(transform.position, 1.5f); }
        }

        public bool EnemyClose
        {
            get { return Map.EnemyInRange(transform.position, 3f); }
        }

        public bool EnemyFar
        {
            get { return Map.EnemyInRange(transform.position, 6f); }
        }

        public bool EnemyBulletImminent
        {
            get { return Map.EnemyBulletImminent(_collider); }
        }

        public bool GunAvailable
        {
            get { return Map.GunAvailable(transform.position); }
        }
    
        public bool HasGun
        {
            get { return _guns.HasGun; }
        }
    
        public bool CanShoot
        {
            get { return _guns.CanShoot; }
        }

        public bool AmmoFull
        {
            get { return _guns.AmmoFull; }
        }

        public bool OutOfAmmo
        {
            get { return _guns.OutOfAmmo; }
        }

        public bool HasTarget
        {
            get { return _controller.Target != null; }
        }

        public bool TargetVisible
        {
            get
            {
                if (HasTarget)
                {
                    Vector2 dif = _controller.Target.position - transform.position;
                    RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, dif.normalized, dif.magnitude, LayerMask.GetMask("Obstacle"));
                    return !hits.Any(hit => (hit.point - (Vector2)transform.position).magnitude < dif.magnitude && hit.transform.gameObject.name != "Hole-Collider");
                }

                return false;
            }
        }

        public bool HealthFull
        {
            get { return _health.FullHealth; }
        }

        public bool HealthLow
        {
            get { return _health.LowHealth; }
        }

        public bool ItemAvailable
        {
            get { return Map.ItemAvailable(transform.position); }
        }

        public bool HealthAvailable
        {
            get { return Map.HealthAvailable(transform.position); }
        }

        public bool AmmoAvailable
        {
            get { return Map.AmmoAvailable(transform.position); }
        }

        public bool KeyAvailable
        {
            get { return Map.KeyAvailable(transform.position); }
        }

        public bool OnTrap
        {
            get { return Map.OnTrap(transform.position); }
        }
    }
}