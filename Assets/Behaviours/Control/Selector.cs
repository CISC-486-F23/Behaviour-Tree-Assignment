using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Control
{
    public class Selector : Control
    {
        public override IEnumerator Execute()
        {
            // What status should Selector have while children are executing?
            
            // Reset the status of all children
            foreach (Behaviour child in Children) child.Reset();
            
            // Selector executes each of its children until one of them succeeds
            // If no child succeeds, then Selector fails
            /*foreach (...)
            {
                
            }*/

            yield return null;
        }
    }
}
