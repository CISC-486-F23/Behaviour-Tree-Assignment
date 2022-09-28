using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Conditional
{
    public class EnemyClose : Conditional
    {
        protected override bool Condition()
        {
            return _brain.EnemyClose;
        }
    }
}
