using System.Collections;
using System.Collections.Generic;
using Brain;
using UnityEngine;

namespace Behaviours.Conditional
{
    public class HasTarget : Conditional
    {
        protected override bool Condition()
        {
            return _brain.HasTarget;
        }
    }
}
