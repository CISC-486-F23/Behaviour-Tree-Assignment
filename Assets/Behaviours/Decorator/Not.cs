using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Decorator
{
    public class Not : Decorator
    {
        public override IEnumerator Execute()
        {
            // Not inverts the status of its lone child
            yield return null;
        }

        protected Status Invert(Status status)
        {
            if (status == Status.Success) return Status.Failure;
            if (status == Status.Failure) return Status.Success;
            return status;
        }
    }
}