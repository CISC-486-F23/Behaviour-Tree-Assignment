using UnityEngine;

public class EnemyHealth : MonoBehaviour {
    public int health;

    public void Start()
    {
        Map.AddEnemy(this);
    }
    
    public void decreaseHealth(int damage) {
        health -= damage;
    }
}
