using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Behaviours
{
    public enum Status
    {
        None,
        Success,
        Failure,
        Running
    }
    
    public abstract class Behaviour : MonoBehaviour
    {
        public Vector2 Position;

        public List<Behaviour> Children;

        [SerializeField] protected Status _result;
        public Status Result
        {
            get { return _result; }
        }

        public virtual void Reset()
        {
            _result = Status.None;
            
            foreach(Behaviour child in Children) child.Reset();
        }

        public virtual bool Contains(Behaviour other)
        {
            if (Children.Count == 0) return this == other;
            
            foreach (Behaviour child in Children)
            {
                if(child == other) return true;
                if (child.Contains(other)) return true;
            }
            
            return false;
        }

        public abstract IEnumerator Execute();

        public int AddChild(Behaviour child)
        {
            Children.Add(child);

            child.transform.parent = transform;

            return Children.Count;
        }

        public void RemoveChild(Behaviour child, Transform newParent)
        {
            if (Children.Remove(child))
            {
                child.transform.parent = newParent;
            }
        }
    }
}