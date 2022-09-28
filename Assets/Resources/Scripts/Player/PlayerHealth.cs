using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    private int health;
    private bool isKill;

    [HideInInspector]
    public bool invinsible;

    private HealthBar healthBar;

    private GameObject blood1;

    private GameObject bloodHeal1;

    public int Health
    {
        get { return health; }
    }

    public bool FullHealth
    {
        get { return health == maxHealth; }
    }

    public bool LowHealth
    {
        get { return health < (maxHealth / 2); }
    }
    
    void Start() {
        health = maxHealth;
        isKill = false;

        healthBar = GameObject.Find("HealthBar").GetComponent<HealthBar>();
        healthBar.SetMaxHealth(health);

        blood1 = Resources.Load<GameObject>("Prefabs/Effects/BloodEffect1");
        bloodHeal1 = Resources.Load<GameObject>("Prefabs/Effects/HealEffect1");
    }

    void Update() {   
        if (health == 0 && !isKill) {
            isKill = true;
        }
    }

    public void decreaseHealth(int damage) {
        if (!invinsible) {
            health = Math.Max(0, health - damage);
            healthBar.SetHealth(health);
            GameObject blood = Instantiate(blood1, transform.position, transform.rotation);
            blood.transform.SetParent(transform);
            SoundManager.PlaySound("Hurt");
        }
    }

    public void IncreaseHealth(int heal) {
        health = Math.Min(maxHealth, health + heal);
        healthBar.SetHealth(health);
        GameObject blood = Instantiate(bloodHeal1, transform.position, transform.rotation);
        blood.transform.SetParent(transform);
    }

    public bool IsKill {
        get { return isKill; }
    }
}
