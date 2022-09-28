using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToTarget : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (_controller.Target != null)
            {
                _controller.Destination = _controller.Target;
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}