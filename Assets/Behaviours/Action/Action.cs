using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours.Action
{
    public abstract class Action : Behaviour
    {
        [SerializeField] protected AgentController _controller;
    }
}

