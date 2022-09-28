using UnityEngine;

public class HydraHealth : BossHealth {
    public bool invulnerable;
    private int originalHealth;
    public bool scream1, scream2;

    new void Start() {
        base.Start();
        originalHealth = health;
        scream1 = true;
        scream2 = true;
    }

    void Update() {
        if (health <= originalHealth * 0.8 && scream1) {
            gameObject.GetComponent<Animator>().SetTrigger("scream");
            scream1 = false;
        } else {
            if (health <= originalHealth * 0.5 && scream2) {
                gameObject.GetComponent<Animator>().SetTrigger("scream");
                scream2 = false;
            } else if (health <= 0) {
                GetComponent<Animator>().SetTrigger("defeated");
            }
        }
    }

    public new void TakeDamage(int damage) {
        if(!invulnerable)
            health -= damage;
    }
}
