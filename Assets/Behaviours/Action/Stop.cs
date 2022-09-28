using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public class Stop : Action
    { 
        public override IEnumerator Execute()
        {
            _result = Status.Running;

            _controller.Stop();
            
            _result = Status.Success;

            yield return null;
        }
    }
}