using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestTopOpen_Cave : MonoBehaviour
{
    public Camera playerCamera;
    public float interactionRange = 10f;  // Distance within which player can interact
    public Text interactionText;          // Text for "Press 'O' to open the box"
    public Text messageText;              // Text for error messages (e.g., missing key)

    private Animator animator;
    private AudioSource sound;
    private bool chestOpen = false;

    void Start()
    {
        // Assuming Animator is on the chest_top object
        Transform chestTop = transform.Find("chest_top");
        if (chestTop != null)
        {
            animator = chestTop.GetComponent<Animator>();
        }

        sound = GetComponent<AudioSource>();

        // Initialize the topChestOpen animator parameter to false at start
        if (animator != null)
        {
            animator.SetBool("topChestOpen", false);
        }

        // Clear the interaction and message texts at the start
        if (interactionText != null)
        {
            interactionText.text = "";
        }

        if (messageText != null)
        {
            messageText.text = "";
        }

        // Debug to check if Animator is properly assigned
        if (animator != null)
        {
            Debug.Log("Animator component successfully found.");
        }
        else
        {
            Debug.LogError("Animator component is missing on the chest_top!");
        }
    }

    void Update()
    {
        HandleChestInteraction();
    }

    private void HandleChestInteraction()
    {
        RaycastHit hit;

        // Raycast from the center of the screen
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // Perform the raycast and check if it hits the chest object
        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            // Check if the ray hits the chest object (not chest_top specifically)
            if (hit.transform == transform)
            {
                Debug.Log("Raycast hit: " + hit.transform.name); // Ensure you're hitting chest

                // Calculate distance between player and chest
                float distance = Vector3.Distance(playerCamera.transform.position, transform.position);

                if (distance < interactionRange)
                {
                    // Display "Press 'O' to open the box" when within range and looking at chest
                    if (interactionText != null)
                    {
                        interactionText.text = "Press 'O' to open the box";
                        interactionText.color = Color.white;
                    }

                    // If the player presses "O" within range and raycast hits chest
                    if (!chestOpen && Input.GetKeyDown(KeyCode.O))
                    {
                        Debug.Log("Player pressed 'O'. Checking for special key...");
                        if (PersistentObjectManager.hasSpecialKey)
                        {
                            Debug.Log("Player has the special key. Opening the chest.");
                            OpenChest();
                        }
                        else
                        {
                            Debug.LogWarning("Player does not have the special key.");
                            ShowMessage("You need a Special Key to open this chest!");
                        }
                    }
                }
                else
                {
                    // If the player presses "O" but is out of range
                    if (Input.GetKeyDown(KeyCode.O))
                    {
                        Debug.Log("Player pressed 'O' but is out of range.");
                        ShowMessage("You are too far from the chest to open it!");
                    }
                    ClearInteractionText();
                }
            }
            else
            {
                ClearInteractionText();
            }
        }
        else
        {
            ClearInteractionText();
        }
    }

    // Function to open the chest_top
    void OpenChest()
    {
        if (animator != null)
        {
            Debug.Log("Triggering chest open animation.");
            animator.SetBool("topChestOpen", true);  // Trigger the chest open animation using the boolean

            if (sound != null)
            {
                sound.PlayDelayed(0.5f);  // Play sound with delay if needed
            }

            chestOpen = true;         // Mark the chest as opened
            ClearMessage();
            ClearInteractionText();
        }
        else
        {
            Debug.LogError("Animator is missing!");
        }
    }

    // Function to show a message on the screen
    void ShowMessage(string message)
    {
        if (messageText != null)
        {
            messageText.text = message;
            Debug.Log("Showing message: " + message);
            Invoke("ClearMessage", 2f); // Clear the message after 2 seconds
        }
        else
        {
            Debug.LogError("MessageText component is missing!");
        }
    }

    // Function to clear the message from the screen
    void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    // Function to clear the interaction text from the screen
    void ClearInteractionText()
    {
        if (interactionText != null)
        {
            interactionText.text = "";
        }
    }
}
