using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class EvadeClosestEnemy : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.EnemyInMap)
            {
                EnemyHealth enemy = Map.ClosestEnemy(_controller.Position);
                if (enemy != null)
                {
                    _controller.Evade = enemy.gameObject.GetComponent<Rigidbody2D>();
                    _result = Status.Success;
                }
                else _result = Status.Failure;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}
