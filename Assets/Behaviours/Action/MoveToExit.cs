using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToExit : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.CanReachExit(transform.position))
            {
                _controller.Destination = Map.Exit;
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}
