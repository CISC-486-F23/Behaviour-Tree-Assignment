using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Behaviours
{
    public class BehaviourTree : MonoBehaviour
    {
        public System.Action DrawCallback;

        [SerializeField] protected GameObject _prefab;
        
        [SerializeField] protected GameObject _compositePrefab;

        [SerializeField] protected Composite _root;
        protected Coroutine _plan;

        [SerializeField] protected Transform _compositesTransform;
        [SerializeField] protected List<Composite> _composites;
        
        [SerializeField] protected Transform _controlsTransform;
        [SerializeField] protected Transform _decoratorsTransform;
        [SerializeField] protected Transform _conditionalsTransform;
        [SerializeField] protected Transform _actionsTransform;

        public GameObject Prefab
        {
            get { return _prefab; }
        }
        
        public Composite Root
        {
            get { return _root; }
            set { _root = value; }
        }

        public Transform CompositesTransform
        {
            get { return _compositesTransform; }
        }

        public GameObject CompositePrefab
        {
            get { return _compositePrefab; }
        }
        
        public List<Composite> Composites
        {
            get { return _compositesTransform.GetComponentsInChildren<Composite>().Where(c => c.transform.parent == _compositesTransform).ToList(); }
        }

        public List<Control.Control> Controls
        {
            get { return _controlsTransform.GetComponentsInChildren<Control.Control>().ToList(); }
        }

        public List<Decorator.Decorator> Decorators
        {
            get { return _decoratorsTransform.GetComponentsInChildren<Decorator.Decorator>().ToList(); }
        }

        public List<Conditional.Conditional> Conditionals
        {
            get { return _conditionalsTransform.GetComponentsInChildren<Conditional.Conditional>().ToList(); }
        }

        public List<Action.Action> Actions
        {
            get { return _actionsTransform.GetComponentsInChildren<Action.Action>().ToList(); }
        }

        public void Start()
        {
            _plan = StartCoroutine(Execute());
        }

        IEnumerator Execute()
        {
            while (true)
            {
                _root.Reset();
                yield return _root.Execute();
                DrawCallback?.Invoke();
                yield return new WaitForSeconds(0.1f);
            }
        }

        public void CreateNewComposite()
        {
            _composites.Add(Instantiate(_compositePrefab, _compositesTransform).GetComponent<Composite>());
            _composites[_composites.Count - 1].name = _composites.Count.ToString();
        }

        public bool DeleteComposite(int index)
        {
            if (_composites[index] != _root)
            {
                Composite delete = _composites[index];

                _composites.Remove(delete);
                Destroy(delete);

                return true;
            }

            return false;
        }
    }
}