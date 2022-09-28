using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class Reload : Action
    {
        public override IEnumerator Execute()
        {
            bool reloaded = _controller.Reload();
            if (reloaded) _result = Status.Success;
            else _result = Status.Failure;

            yield return null;
        }
    }
}
