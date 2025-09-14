using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Player's maximum health
    public int currentHealth;   // Player's current health
    public Slider healthBar;    // Health bar slider

    void Start()
    {
        // Initialize health and the health bar
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health
        healthBar.value = currentHealth; // Update the health bar

        Debug.Log($"Player took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Lost!"); // Game Over logic
        // Add game over mechanics here (e.g., restart level, show death screen)
    }
}
