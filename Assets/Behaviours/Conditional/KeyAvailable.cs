using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Conditional
{
    public class KeyAvailable : Conditional
    {
        protected override bool Condition()
        {
            return _brain.KeyAvailable;
        }
    }
}
