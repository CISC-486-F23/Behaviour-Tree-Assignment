using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Conditional
{
    public class EnemyFar : Conditional
    {
        protected override bool Condition()
        {
            return _brain.EnemyFar;
        }
    }
}
