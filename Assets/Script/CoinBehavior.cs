using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinBehavior : MonoBehaviour
{
    public GameObject coins;
    public GameObject player; // Reference to the player
    public static int coinsCounter = 0; // Static variable to track the number of coins collected
    public Text coinText; // UI Text to display the coin count
    public AudioSource coinAudioSource; // Reference to the AudioSource on the parent Coins object

    private void Start()
    {
        UpdateCoinText(); // Initialize the coin counter text
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            coinsCounter++; // Increment the static coin counter
            PersistentObjectManager.SetGold(coinsCounter); // Update persistent gold value
            UpdateCoinText(); // Update the UI to reflect the new coin count

            gameObject.SetActive(false); // Hide the coin after picking it up
            AudioSource sound = coins.GetComponent<AudioSource>();
            sound.Play();

            // Update the coin's state in PersistentObjectManager
            PersistentObjectManager.SetHasCoin(gameObject.name, false); // Mark the coin as collected

            // Play sound effect from the parent Coins object if the AudioSource is set
            if (coinAudioSource != null)
            {
                Debug.Log("Playing coin pickup sound");
                coinAudioSource.Play();
            }
            else
            {
                Debug.LogWarning("AudioSource for coin sound is not assigned.");
            }
        }
    }

    // Method to update the coin counter UI text
    public void UpdateCoinText()
    {
        if (coinText != null)
        {
            // Update the displayed text
            coinText.text = "Gold: " + coinsCounter.ToString();

            // Force refresh by toggling visibility
            coinText.enabled = false; // Temporarily hide the text
            coinText.enabled = true;  // Show the text again

            Debug.Log("Coin text updated to: " + coinsCounter);
        }
        else
        {
            Debug.LogWarning("coinText UI element is not assigned.");
        }
    }


    // Method to reset the coins counter and force a UI update
    public static void ResetCoinsAndUI()
    {
        coinsCounter = 0; // Reset the static counter
        PersistentObjectManager.SetGold(0); // Reset persistent gold value

        // Find an instance to update the UI
        CoinBehavior instance = FindObjectOfType<CoinBehavior>();
        if (instance != null)
        {
            instance.UpdateCoinText(); // Force the UI to reflect the reset
        }
        else
        {
            Debug.LogWarning("No active instance of CoinBehavior found to update UI.");
        }
    }
}
