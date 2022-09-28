using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToClosestGun : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.GunAvailable(_controller.Position))
            {
                _controller.Destination = Map.ClosestGun(_controller.Position);
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}

