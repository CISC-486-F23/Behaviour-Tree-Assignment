using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviours
{
    public class Composite : Behaviour
    {
        [SerializeField] protected Behaviour _root;

        public Behaviour Root
        {
            set { _root = value; }
        }

        public override bool Contains(Behaviour other)
        {
            if (Children.Count == 0)
            {
                if (_root == other || _root.Contains(other)) return true;
            }
            else
            {
                if (Children[0] == other || Children[0].Contains(other)) return true;
            }

            return false;
        }

        public override IEnumerator Execute()
        {
            _result = Status.Running;

            if (Children.Count == 0)
            {
                yield return _root.Execute();
                _result = _root.Result;
            }
            else
            {
                yield return Children[0].Execute();
                _result = Children[0].Result;
            }
        }
        
        public override void Reset()
        {
            _result = Status.None;
            
            if(Children.Count > 0) foreach(Behaviour child in Children) child.Reset();
            else _root.Reset();
        }
    }
}