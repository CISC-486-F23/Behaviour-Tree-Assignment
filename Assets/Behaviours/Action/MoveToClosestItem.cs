using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToClosestItem : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.ItemAvailable(_controller.Position))
            {
                _controller.Destination = Map.ClosestItem(_controller.Position);
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}