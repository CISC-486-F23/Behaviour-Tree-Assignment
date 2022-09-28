using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Decorator
{
    public class Failure : Decorator
    {
        public override IEnumerator Execute()
        {
            // Failure always fails
            yield return null;
        }
    }
}
