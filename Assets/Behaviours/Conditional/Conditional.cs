using System.Collections;
using System.Collections.Generic;
using Brain;
using UnityEngine;

namespace Behaviours.Conditional
{
    public abstract class Conditional : Behaviour
    {
        [SerializeField] protected AgentBrain _brain;
        
        public override IEnumerator Execute()
        {
            // Conditionals succeed if a Condition is true and fail if a Condition is false

            yield return null;
        }

        protected abstract bool Condition();
    }
}

