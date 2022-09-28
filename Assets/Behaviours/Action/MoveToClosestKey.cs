using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToClosestKey : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.KeyAvailable(_controller.Position))
            {
                _controller.Destination = Map.ClosestKey(_controller.Position);
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}
