using System.Linq;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour {
    private PlayerHealth playerHealth;
    public int damage;
    private GameObject explosion;
    private Rigidbody2D rb;
    new private BoxCollider2D collider;

    public Rigidbody2D RB
    {
        get { return rb; }
    }
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        collider = gameObject.GetComponent<BoxCollider2D>();
        Map.AddEnemyBullet(this);
        Destroy(gameObject, 4f);
        playerHealth = FindObjectOfType<PlayerHealth>();
        explosion = Resources.Load<GameObject>("Prefabs/Effects/BulletExplosionEffect1");
    }

    public float? CollisionTime(Collider2D col)
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3)rb.velocity.normalized);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, collider.size, collider.transform.eulerAngles.z, rb.velocity.normalized).Where(c => c.collider == col).ToArray();

        if (hits.Length > 0)
        {
            Vector2 dif = hits[0].point - (Vector2)transform.position;
            return dif.magnitude / rb.velocity.magnitude;
        }

        return null;
    }

    // Detects if the collision is an object Foreground or player
    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
            playerHealth.decreaseHealth(damage);
        
        if (!other.CompareTag("Enemy") &&
            !other.CompareTag("Bullet") &&
            !other.CompareTag("Boss") &&
            !other.CompareTag("Ignore") &&
            !other.CompareTag("Hydra") &&
            !other.CompareTag("Hole") &&
            !other.CompareTag("EnemyBullet")) {
            Destroy(gameObject);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }

    void OnDestroy()
    {
        Map.RemoveEnemyBullet(this);
    }
}
