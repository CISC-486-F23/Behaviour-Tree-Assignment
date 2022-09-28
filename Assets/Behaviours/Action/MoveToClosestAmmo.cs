using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class MoveToClosestAmmo : Action
    {
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Map.AmmoAvailable(_controller.Position))
            {
                _controller.Destination = Map.ClosestAmmo(_controller.Position);
                _result = Status.Success;
            }
            else _result = Status.Failure;

            yield return null;
        }
    }
}