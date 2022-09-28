using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class NextWeapon : Action
    {
        public override IEnumerator Execute()
        {
            _controller.ChangeWeapon(1);

            _result = Status.Success;
            
            yield return null;
        }
    }
}