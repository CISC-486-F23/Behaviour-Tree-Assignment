using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Conditional
{
    public class TargetVisible : Conditional
    {
        protected override bool Condition()
        {
            return _brain.TargetVisible;
        }
    }
}

