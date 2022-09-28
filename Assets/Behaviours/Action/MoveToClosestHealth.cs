using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToClosestHealth : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.HealthAvailable(_controller.Position))
            {
                _controller.Destination = Map.ClosestHealth(_controller.Position);
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}