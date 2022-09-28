using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Decorator
{
    public class Success : Decorator
    {
        public override IEnumerator Execute()
        {
            // Success always succeeds
            yield return null;
        }
    }
}
