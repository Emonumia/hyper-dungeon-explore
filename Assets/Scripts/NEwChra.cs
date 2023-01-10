using UnityEngine;

public class NEwChra : MonoBehaviour
{
    public VisualizeTile standingOnTile;
    public int maxHealth1 = 100; // max health of prefab1
    public int maxHealth2 = 50; // max health of prefab2
    public int currentHealth1; // current health of prefab1
    public int currentHealth2; // current health of prefab2
    public HealthBar healthBar1; // health bar for prefab1
    public HealthBar healthBar2; // health bar for prefab2
    public GameObject prefab1;
    public GameObject prefab2;

    void Start()
    {
        currentHealth1 = maxHealth1;
        healthBar1.SetMaxHealth(maxHealth1);
        currentHealth2 = maxHealth2;
        healthBar2.SetMaxHealth(maxHealth2);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Take damage for prefab1
            TakeDamage(prefab1, 25);
            // Take damage for prefab2
            TakeDamage(prefab2, 15);
        }
    }

    public void TakeDamage(GameObject prefab, int damage)
    {
        if (prefab == prefab1)
        {
            currentHealth1 -= damage;
            healthBar1.SetHealth(currentHealth1);
            if (currentHealth1 <= 0)
            {
                Destroy(prefab1);
            }
        }
        else if (prefab == prefab2)
        {
            currentHealth2 -= damage;
            healthBar2.SetHealth(currentHealth2);
            if (currentHealth2 <= 0)
            {
                Destroy(prefab2);
            }
        }
    }
}

