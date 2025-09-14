using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialKeyBehavior : MonoBehaviour
{
    public GameObject player;  // The player object
    public GameObject keyObject;  // Reference to the special key object
    public float pickupDistance = 4f;  // Pickup distance is 4 units
    public AudioSource keyPickupSound;  // Optional sound to play on pickup

    public RawImage[] crosshairs;
    private Color originalCrosshairColor;

    public Text pickupText;

    private bool keyPickedUp = false;

    void Start()
    {
        if (crosshairs.Length > 0)
        {
            originalCrosshairColor = crosshairs[0].color;
        }

        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }

        if (PersistentObjectManager.hasSpecialKey)
        {
            Debug.Log("Key has already been collected, disabling key object.");
            keyObject.SetActive(false);  // Hide the key if already picked up
        }
        else
        {
            Debug.Log("Key is available for pickup.");
        }
    }

    void Update()
    {
        if (!keyPickedUp)
        {
            HandleKeyPickup();
        }
    }

    private void HandleKeyPickup()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == keyObject)
            {
                float distance = Vector3.Distance(player.transform.position, keyObject.transform.position);
                if (distance < pickupDistance)
                {
                    pickupText.text = "Press 'P' to pick up Special Key";
                    pickupText.gameObject.SetActive(true);
                    ChangeCrosshairColor(Color.red);
                    Debug.Log("Player is within range to pick up the key.");

                    if (Input.GetKeyDown(KeyCode.P))
                    {
                        PickupKey();
                    }
                }
                else
                {
                    ResetCrosshairAndText();
                }
            }
            else
            {
                ResetCrosshairAndText();
            }
        }
        else
        {
            ResetCrosshairAndText();
        }
    }

    void PickupKey()
    {
        if (keyPickupSound != null)
        {
            keyPickupSound.Play();
        }

        keyPickedUp = true;
        keyObject.SetActive(false);

        PersistentObjectManager.SetHasSpecialKey(true);  // Updates the key state
        Debug.Log("Special key picked up, state updated in PersistentObjectManager.");

        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }

        ChangeCrosshairColor(originalCrosshairColor);
    }

    void ResetCrosshairAndText()
    {
        if (pickupText != null)
        {
            pickupText.gameObject.SetActive(false);
        }

        ChangeCrosshairColor(originalCrosshairColor);
    }

    void ChangeCrosshairColor(Color newColor)
    {
        foreach (RawImage crosshair in crosshairs)
        {
            if (crosshair != null)
            {
                crosshair.color = newColor;
            }
        }
    }
}
