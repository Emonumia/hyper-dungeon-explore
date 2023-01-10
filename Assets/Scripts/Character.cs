using UnityEngine;

public class Character : MonoBehaviour
{
    public VisualizeTile standingOnTile;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;
    //public GameObject prefab;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(25);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
