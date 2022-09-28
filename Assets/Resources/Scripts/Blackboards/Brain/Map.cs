using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    protected static Map _instance;

    public static Vector3 PlayerPosition { get; set; }
    
    // Maps are column major: x, y

    [SerializeField] protected GameObject _dialogues;
    [SerializeField] protected Tilemap _floor;
    [SerializeField] protected Transform _exit;

    protected bool[,] _walkable;

    [SerializeField] protected List<Vector3> _traps;

    [SerializeField] protected List<EnemyHealth> _enemies;
    [SerializeField] protected List<EnemyHealth> _attackingEnemies;

    [SerializeField] protected List<EnemyBulletController> _enemyBullets;

    [SerializeField] protected List<Transform> _gunPickups;
    [SerializeField] protected List<Transform> _itemPickups;
    [SerializeField] protected List<Transform> _healthPickups;
    [SerializeField] protected List<Transform> _ammoPickups;
    [SerializeField] protected List<Transform> _keyPickups;

    public static bool CanReach(Vector3 p1, Vector3 p2)
    {
        GraphNode node1 = AstarPath.active.GetNearest(p1, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(p2, NNConstraint.Default).node;

        return PathUtilities.IsPathPossible(node1, node2);
    }
    
    protected Vector3 Offset
    {
        get { return _floor.transform.position; }
    }

    public static bool Tutorial
    {
        set{ if(_instance._dialogues != null) _instance._dialogues.SetActive(value);}
    }

    public static bool Walkable((int x, int y) position)
    {
        return Walkable(position.x, position.y);
    }

    public static bool Walkable(int x, int y)
    {
        return InBounds(x, y) && _instance._walkable[x, y];
    }

    public static bool InBounds((int x, int y) position)
    {
        return InBounds(position.x, position.y);
    }
    
    public static bool InBounds(int x, int y)
    {
        return x >= 0 && x < _instance._walkable.GetLength(0) && y >= 0 && y < _instance._walkable.GetLength(1);
    }

    public static (int x, int y) Position(Vector3 position)
    {
        position += _instance.Offset;

        return (Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
    }

    public static Transform Exit
    {
        get { return _instance._exit; }
    }

    public static bool CanReachExit(Vector3 position)
    {
        return CanReach(position, _instance._exit.position);
    }

    // Enemies
    protected static List<EnemyHealth> Enemies
    {
        get
        {
            _instance._enemies = _instance._enemies.Where(e => e != null).ToList();
            return _instance._enemies;
        }
    }
    
    protected static List<EnemyHealth> AttackingEnemies
    {
        get
        {
            _instance._attackingEnemies = _instance._attackingEnemies.Where(e => e != null).ToList();
            return _instance._attackingEnemies;
        }
    }

    public static bool UnderAttack
    {
        get { return AttackingEnemies.Count > 0; }
    }

    public static bool EnemyInRange(Vector3 position, float range)
    {
        return AttackingEnemies.Any(e => (e.transform.position - position).magnitude < range);
    }
    
    public static void AddAttackingEnemies(List<EnemyHealth> enemies)
    {
        _instance._attackingEnemies = enemies;
    }

    public static void AddEnemy(EnemyHealth enemy)
    {
        if(_instance._enemies == null) _instance._enemies = new List<EnemyHealth>();
        
        _instance._enemies.Add(enemy);
    }

    public static void RemoveEnemy(EnemyHealth enemy)
    {
        if (_instance._enemies != null) _instance._enemies.Remove(enemy);
        if (_instance._attackingEnemies != null) _instance._attackingEnemies.Remove(enemy);
    }

    public static bool EnemyInMap
    {
        get
        {
            return Enemies.Count > 0;
        }
    }

    public static EnemyHealth ClosestEnemy(Vector3 position)
    {
        if(AttackingEnemies.Count > 0) return AttackingEnemies[AttackingEnemies.Select(e => (e.transform.position - position).magnitude).ToList().MinIndex()];
        return Enemies[Enemies.Select(e => (e.transform.position - position).magnitude).ToList().MinIndex()];
    }
    
    // Bullets

    public static void AddEnemyBullet(EnemyBulletController bullet)
    {
        if(!_instance._enemyBullets.Contains(bullet))_instance._enemyBullets.Add(bullet);
    }

    public static void RemoveEnemyBullet(EnemyBulletController bullet)
    {
        _instance._enemyBullets.Remove(bullet);
    }

    public static List<EnemyBulletController> EnemyBullets
    {
        get { return _instance._enemyBullets.Where(b => b != null).ToList(); }
    }

    public static bool EnemyBulletImminent(Collider2D col, float t = 1f)
    {
        return EnemyBullets.Any(b =>
        {
            float? time = b.CollisionTime(col);
            if (time.HasValue) return time < t;
            else return false;
        });
    }

    public static Rigidbody2D ClosestEnemyBullet(Vector3 position)
    {
        return EnemyBullets.Count > 0 ? EnemyBullets[EnemyBullets.Select(b => (b.transform.position - position).magnitude).ToList().MinIndex()].RB : null; 
    }

    private static bool AnyAvailable(List<Transform> items, Vector3 position)
    {
        return items.Count > 0 && items.Where(i => i != null).Any(i => CanReach(position, i.position));
    }

    private static Transform ClosestItem(List<Transform> items, Vector3 position)
    {
        items = items.Where(i => i != null && CanReach(position, i.position)).ToList();
        return AnyAvailable(items, position) ? items[items.Select(i => (i.position - position).magnitude).ToList().MinIndex()] : null;
    }

    // Guns
    
    public static bool GunAvailable(Vector3 position)
    {
        return AnyAvailable(_instance._gunPickups, position);
    }

    public static Transform ClosestGun(Vector3 position)
    {
        return ClosestItem(_instance._gunPickups, position);
    }
    
    // Items

    public static bool ItemAvailable(Vector3 position)
    {
        return AnyAvailable(_instance._itemPickups, position);
    }

    public static Transform ClosestItem(Vector3 position)
    {
        return ClosestItem(_instance._itemPickups, position);
    }
    
    // Health

    public static bool HealthAvailable(Vector3 position)
    {
        return AnyAvailable(_instance._healthPickups, position);
    }

    public static Transform ClosestHealth(Vector3 position)
    {
        return ClosestItem(_instance._healthPickups, position);
    }
    
    // Ammo

    public static bool AmmoAvailable(Vector3 position)
    {
        return AnyAvailable(_instance._ammoPickups, position);
    }

    public static Transform ClosestAmmo(Vector3 position)
    {
        return ClosestItem(_instance._ammoPickups, position);
    }
    
    // Keys

    public static bool KeyAvailable(Vector3 position)
    {
        return AnyAvailable(_instance._keyPickups, position);
    }

    public static Transform ClosestKey(Vector3 position)
    {
        return ClosestItem(_instance._keyPickups, position);
    }
    
    // Traps

    public static void AddTrap(Vector3 position)
    {
        _instance._traps.Add(position);
    }

    public static bool OnTrap(Vector3 position)
    {
        return _instance._traps.Any(t => (t - position).magnitude < 1f);
    }

    void Awake()
    {
        _instance = this;
    }
    
    void Start()
    {
        BuildWalkable();
    }

    protected void BuildWalkable()
    {
        _walkable = new bool[_floor.size.x, _floor.size.y];
        
        for (int i = 0; i < _floor.size.x; i++)
        {
            for (int j = 0; j < _floor.size.y; j++)
            {
                _walkable[i, j] = _floor.HasTile(new Vector3Int(i, j));
            }
        }
    }
}

public static class ListUtilities
{
    public static int MinIndex(this List<float> list)
    {
        int min = 0;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] < list[min]) min = i;
        }

        return min;
    }
}
