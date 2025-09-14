using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PersistentObjectManager : MonoBehaviour
{
    // Health variables
    public static int maxHealth = 100;  // Maximum health
    public static int currentHealth = 100;  // Current health
    private Slider healthBar;  // Reference to the health bar slider in the current scene

    public static PersistentObjectManager instance = null;
    public static int Gold = 0;
    public Text coinsText;

    public static bool hasSpecialKey = false;  // Track whether the player has the special key

    // Weapon persistence for all 5 weapons
    public static Dictionary<string, bool> hasWeapon = new Dictionary<string, bool>()
    {
        { "Axe_DS", false },
        { "Axe_OS", false },
        { "Sword_DH", false },
        { "Sword_OH", false },
        { "Bow", false },
        { "Arrow", false }  // Track arrows pickup state
    };

    public static Dictionary<string, bool> hasWeaponInCave = new Dictionary<string, bool>()
    {
        { "Axe_DS", true },
        { "Axe_OS", true },
        { "Sword_DH", true },
        { "Sword_OH", true },
        { "Bow", true },
        { "Arrow", true }
    };

    // Weapon game objects in hand and in the cave
    public GameObject Axe_DS_Player;
    public GameObject Axe_DS;
    public GameObject Axe_OS_Player;
    public GameObject Axe_OS;
    public GameObject Sword_DH_Player;
    public GameObject Sword_DH;
    public GameObject Sword_OH_Player;
    public GameObject Sword_OH;
    public GameObject Bow_Player;
    public GameObject Bow;
    public GameObject Arrows_Player;  // Player-held arrows object
    public GameObject Arrows;

    public Dictionary<string, GameObject> WeaponInHand = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> WeaponInCave = new Dictionary<string, GameObject>();

    public static Dictionary<string, bool> hasCoin = new Dictionary<string, bool>();
    public GameObject[] coins;

    private bool initialized = false;  // Ensure the manager only initializes once

    private void Awake()
    {
        if (instance == null)  // Runs the first time
        {
            instance = this;
            DontDestroyOnLoad(instance);  // Ensure this object persists between scenes
            InitializeHealthBar();  // Ensure the health bar is initialized when this object is created
            InitializeWeapons();  // Initialize weapons only once
            InitializeCoins();  // Initialize coins only once
            InitializeSpecialKey();  // Initialize special key
            SceneManager.sceneLoaded += OnSceneLoaded;  // Subscribe to scene load events
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(instance);
        if (coinsText != null)
        {
            coinsText.text = "Gold: " + Gold;
        }
        InitializeWeapons();  // Ensure weapons are updated
        InitializeCoins();  // Ensure coins are updated
        InitializeSpecialKey();  // Ensure special key is initialized
        InitializeHealthBar();  // Ensure the health bar is initialized when this object is created
    }
    private void InitializeHealthBar()
    {
        if (healthBar == null)
        {
            healthBar = GameObject.Find("HealthBar")?.GetComponent<Slider>();  // Find HealthBar Slider
            if (healthBar != null)
            {
                healthBar.maxValue = maxHealth;
                healthBar.value = currentHealth;
                DontDestroyOnLoad(healthBar.gameObject);  // Ensure the health bar persists across scenes
            }
        }
    }
    // Method to update health (taking damage)
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Prevent negative health
        if (healthBar != null)
        {
            healthBar.value = currentHealth;  // Update the health bar
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        Debug.Log("Player has died!");
        // Handle death (e.g., game over logic)
    }

private void InitializeCoins()
    {
        for (int i = 0; i < coins.Length; i++)
        {
            if (coins[i] == null)
            {
                continue;  // Skip any coins that have already been destroyed
            }

            string coinName = coins[i].name;

            if (!hasCoin.ContainsKey(coinName))
            {
                hasCoin.Add(coinName, true);  // Add to dictionary if not already present
            }

            coins[i].SetActive(hasCoin[coinName]);  // Set the coin's visibility based on whether it's been picked up or not
        }
    }

    private void InitializeSpecialKey()
    {
        if (hasSpecialKey)
        {
            // Special key has already been collected
        }
        else
        {
            // Special key is available for collection
        }
    }

    private void InitializeWeapons()
    {
        WeaponInHand.Clear();  // Clear old references
        WeaponInHand.Add("Axe_DS", Axe_DS_Player);
        WeaponInHand.Add("Axe_OS", Axe_OS_Player);
        WeaponInHand.Add("Sword_DH", Sword_DH_Player);
        WeaponInHand.Add("Sword_OH", Sword_OH_Player);
        WeaponInHand.Add("Bow", Bow_Player);
        WeaponInHand.Add("Arrow", Arrows_Player);

        WeaponInCave.Clear();  // Clear old references
        WeaponInCave.Add("Axe_DS", Axe_DS);
        WeaponInCave.Add("Axe_OS", Axe_OS);
        WeaponInCave.Add("Sword_DH", Sword_DH);
        WeaponInCave.Add("Sword_OH", Sword_OH);
        WeaponInCave.Add("Bow", Bow);
        WeaponInCave.Add("Arrow", Arrows);

        // Make the player's weapons persistent across scenes
        foreach (var weapon in WeaponInHand.Values)
        {
            if (weapon != null && weapon.activeSelf)  // Only make persistent if it's picked up
            {
                DontDestroyOnLoad(weapon);
            }
        }

        UpdateWeaponStates();  // Initialize weapon states
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeWeapons();  // Re-initialize all weapon references
        InitializeCoins();  // Re-initialize coin references
        UpdateWeaponStates();  // Update weapon visibility based on player possession and cave state
        InitializeSpecialKey();  // Re-initialize special key
        InitializeHealthBar();
    }

    private void UpdateWeaponStates()
    {
        foreach (var weapon in hasWeapon.Keys)
        {
            if (WeaponInHand.ContainsKey(weapon) && WeaponInHand[weapon] != null)
            {
                WeaponInHand[weapon].SetActive(hasWeapon[weapon]);  // Show weapon in player's hand
            }

            if (WeaponInCave.ContainsKey(weapon) && WeaponInCave[weapon] != null)
            {
                WeaponInCave[weapon].SetActive(hasWeaponInCave[weapon]);  // Show or hide weapon in cave
            }
        }

        if (coinsText != null)
        {
            coinsText.text = "Gold: " + Gold;  // Update the gold UI
        }
    }

    // Method to update weapon state
    public static void SetHasWeapon(string weaponName, bool value)
    {
        if (hasWeapon.ContainsKey(weaponName))
        {
            hasWeapon[weaponName] = value;

            // Automatically pick up arrows when picking up the bow
            if (weaponName == "Bow" && value)
            {
                hasWeapon["Arrow"] = true;  // Set arrows to true
            }
        }
    }

    public static void SetHasWeaponInCave(string weaponName, bool value)
    {
        if (hasWeaponInCave.ContainsKey(weaponName))
        {
            hasWeaponInCave[weaponName] = value;
        }
    }

    // Method to update gold value and refresh the UI
    public static void SetGold(int g)
    {
        Gold = g;
        if (instance != null && instance.coinsText != null)
        {
            instance.coinsText.text = "Gold: " + Gold;
        }
    }

    // Method to set the state of the special key
    public static void SetHasSpecialKey(bool value)
    {
        hasSpecialKey = value;
    }

    // Method to set the state of a coin (whether it's picked up or not)
    public static void SetHasCoin(string coinName, bool value)
    {
        if (hasCoin.ContainsKey(coinName))
        {
            hasCoin[coinName] = value;
        }
    }
}