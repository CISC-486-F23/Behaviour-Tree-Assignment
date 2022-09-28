using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class EvadeClosestBullet : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.EnemyInMap)
            {
                Rigidbody2D bullet = Map.ClosestEnemyBullet(_controller.Position);
                if (bullet != null)
                {
                    _controller.Evade = bullet;
                    _result = Status.Success;
                }
                else _result = Status.Failure;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}