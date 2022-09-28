using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Conditional
{
    public class OnTrap : Conditional
    {
        protected override bool Condition()
        {
            return _brain.OnTrap;
        }
    }
}