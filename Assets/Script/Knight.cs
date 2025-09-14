using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Knight : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    public GameObject target; // Reference to the Player
    bool startMoving = false;
    bool isDead = false; // New flag for death state
    public LineRenderer line;
    public Slider HealthBar;
    int maxHealth = 100;
    int currentHealth = 100;
    int damage = 34;

    // Optional: Predefined DeathType (0 for backward, 1 for forward)
    public int deathType = -1; // -1 means randomize by default

    // Damage logic variables
    public int damageToPlayer = 10; // Damage to deal to the player per contact

    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (isDead) return; // Stop all updates if the knight is dead

        if (startMoving)
        {
            // Navigate toward the player
            agent.SetDestination(target.transform.position);
            line.positionCount = agent.path.corners.Length;
            line.SetPositions(agent.path.corners);
        }

        // Stop moving when close enough to the player
        if (startMoving && Vector3.Distance(transform.position, target.transform.position) < 3f)
        {
            animator.SetInteger("Status", 0);
            agent.isStopped = true;
            startMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            startMoving = true;
            animator.SetInteger("Status", 1);
        }
    }

    // Trigger-based damage when the knight's collider touches the player
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("Knight touched the player!");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Deal damage to the player
                playerHealth.TakeDamage(damageToPlayer);
                Debug.Log($"Knight damaged the player! Player's current health: {playerHealth.currentHealth}");
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on the player!");
            }
        }
    }

    public void DoDamage()
    {
        if (isDead) return; // Don't take damage if already dead

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HealthBar.value = 0;
            Die(); // Trigger death logic
        }
        else
        {
            HealthBar.value = currentHealth / (float)maxHealth;
        }
    }

    private void Die()
    {
        isDead = true; // Set dead state

        // Set DeathType (0 = backward, 1 = forward)
        if (deathType == -1) // Randomize only if not manually set
        {
            deathType = Random.Range(0, 2);
        }
        animator.SetInteger("DeathType", deathType);

        animator.SetTrigger("Die"); // Trigger the death animation
        agent.isStopped = true; // Stop movement
        agent.enabled = false; // Disable NavMeshAgent
        line.enabled = false; // Disable line rendering
    }
}
