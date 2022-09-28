using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Control
{
    public class Sequence : Control
    {
        public override IEnumerator Execute()
        {
            // What status should Sequence have while children are executing?
            
            // Reset the status of all children
            foreach (Behaviour child in Children) child.Reset();
            
            // Sequences executes each of its children until one of them fails
            // If no child fails, then Sequence succeeds
            /*foreach (...)
            {
                
            }*/

            yield return null;
        }
    }
}

