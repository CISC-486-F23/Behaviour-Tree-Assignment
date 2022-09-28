using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

public class AgentController : PlayerController
{
    public enum MovementModes
    {
        Stop,
        Reach,
        Avoid,
        Evade
    }
    
    [SerializeField] protected AIPath _path;
    [SerializeField] protected AIDestinationSetter _destinationSetter;
    [SerializeField] protected Transform _avoidPoint;
    [SerializeField] protected Transform _evadePoint;
    [SerializeField] protected Transform _target;

    [SerializeField] protected MovementModes _movementMode;
    [SerializeField] protected Transform _reach;
    [SerializeField] protected Transform _avoid;
    [SerializeField] protected Rigidbody2D _evade;

    public Vector3 Position
    {
        get { return _path.transform.position; }
    }
    
    public Transform Destination
    {
        get { return _reach; }
        set
        {
            _reach = value;
            _movementMode = MovementModes.Reach;
            _destinationSetter.target = _reach;
            _path.isStopped = false;
        }
    }

    public Transform Avoid
    {
        get { return _avoid; }
        set
        {
            _avoid = value;
            _movementMode = MovementModes.Avoid;
            _destinationSetter.target = _avoidPoint;
            _path.isStopped = false;
        }
    }

    public Rigidbody2D Evade
    {
        get { return _evade; }
        set
        {
            _evade = value;
            _movementMode = MovementModes.Evade;
            _destinationSetter.target = _evadePoint;
            _path.isStopped = false;
        }
    }

    public Transform Target
    {
        get { return _target; }
        set { _target = value; }
    }

    public void Stop()
    {
        _movementMode = MovementModes.Stop;
        _destinationSetter.target = null;
        _path.isStopped = true;
    }
    
    protected void Start()
    {
        Map.Tutorial = false;
    }
    
    protected override void Update()
    {
        base.Update();
        
        _fire = false;
        _reload = false;
    }

    protected override void UpdateValues()
    {
        if(_target == null) Aim(transform.position);
        else Aim(_target.position);

        if (_movementMode == MovementModes.Avoid)
        {
            if (_avoid != null) _avoidPoint.position = transform.position + 2f * (transform.position - _avoid.position).normalized;
            else _movementMode = MovementModes.Stop;
        }

        if (_movementMode == MovementModes.Evade)
        {
            if(_evade != null) _evadePoint.position = transform.position + 2f * (Vector3)Right(_evade.velocity, Random.Range(0f, 1f) < 0.5f).normalized;
            else _movementMode = MovementModes.Stop;
        }
        
    }

    protected static Vector2 Right(Vector2 vector, bool left = false)
    {
        return (left ? 1f : -1f) * new Vector2(-vector.y, vector.x);
    }
    
}
