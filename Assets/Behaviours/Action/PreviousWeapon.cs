using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class PreviousWeapon : Action
    {
        public override IEnumerator Execute()
        {
            _controller.ChangeWeapon(-1);

            _result = Status.Success;
            
            yield return null;
        }
    }
}