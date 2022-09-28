using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class Shoot : Action
    {
        public override IEnumerator Execute()
        {
            bool fired = _controller.Fire();
            if (fired) _result = Status.Success;
            else _result = Status.Failure;

            yield return null;
        }
    }
}

