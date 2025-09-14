using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehavior : MonoBehaviour
{
    CharacterController controller;
    AudioSource footStep;
    AudioSource audioSource;     // Added to play weapon pickup sound
    float speed = 15;
    float angularSpeed = 150;
    public GameObject PlayerCamera;
    public Text pickText;
    public Text TwoWeaponsError;  // Text to display error message when trying to pick up another weapon
    public Text coinErrorText;    // Text to display "need more coins" error message

    public AudioClip weaponPickupSound;  // Add this to assign pickup sound

    // All weapons
    public GameObject Axe_DS;
    public GameObject Axe_DS_Player;
    public GameObject Axe_OS;
    public GameObject Axe_OS_Player;
    public GameObject Sword_DH;
    public GameObject Sword_DH_Player;
    public GameObject Sword_OH;
    public GameObject Sword_OH_Player;
    public GameObject Bow;
    public GameObject Bow_Player;
    public GameObject Arrows;
    public GameObject Arrows_Player;   // Add this for arrows in the player's inventory

    private Dictionary<GameObject, string> weaponMap; // Map environment weapons to their names
    private Dictionary<GameObject, GameObject> weaponInPlayerMap; // Map environment weapons to player-held weapons

    public RawImage[] crosshairs; // Array to store multiple crosshair images
    private Color originalCrosshairColor; // Store the original color

    void Start()
    {
        controller = GetComponent<CharacterController>();
        controller.slopeLimit = 60f;
        footStep = GetComponent<AudioSource>();
        audioSource = GetComponent<AudioSource>(); // Initialize AudioSource for sound

        if (crosshairs.Length > 0)
        {
            originalCrosshairColor = crosshairs[0].color;
        }

        // Map environment weapons to their names and corresponding player-held weapons
        weaponMap = new Dictionary<GameObject, string>()
        {
            { Axe_DS, "Axe_DS" },
            { Axe_OS, "Axe_OS" },
            { Sword_DH, "Sword_DH" },
            { Sword_OH, "Sword_OH" },
            { Bow, "Bow" }
        };

        weaponInPlayerMap = new Dictionary<GameObject, GameObject>()
        {
            { Axe_DS, Axe_DS_Player },
            { Axe_OS, Axe_OS_Player },
            { Sword_DH, Sword_DH_Player },
            { Sword_OH, Sword_OH_Player },
            { Bow, Bow_Player }
        };

        // Ensure error texts are hidden at the start
        if (coinErrorText != null) coinErrorText.gameObject.SetActive(false);
        if (TwoWeaponsError != null) TwoWeaponsError.gameObject.SetActive(false);

        // Initialize coin counter UI
        CoinBehavior coinBehaviorInstance = FindObjectOfType<CoinBehavior>();
        if (coinBehaviorInstance != null)
        {
            coinBehaviorInstance.UpdateCoinText();
        }
        else
        {
            Debug.LogWarning("CoinBehavior instance not found in the scene. Make sure it is properly set up.");
        }

    }

    void Update()
    {
        HandleMovement();
        HandleWeaponPickup();
    }

    private bool IsWeaponPickedUp()
    {
        // Check if any weapon is currently picked up by the player
        foreach (var weapon in PersistentObjectManager.hasWeapon)
        {
            if ((weapon.Key == "Bow" || weapon.Key == "Arrow") && weapon.Value)
            {
                return true;
            }
            if (weapon.Value && weapon.Key != "Arrow") // Ignore arrow here since it's linked with the bow
            {
                return true;
            }
        }
        return false;
    }

    private void HandleWeaponPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(PlayerCamera.transform.position, PlayerCamera.transform.forward, out hit))
        {
            if (weaponMap.ContainsKey(hit.collider.gameObject))
            {
                GameObject weapon = hit.collider.gameObject;
                string weaponName = weaponMap[weapon];
                GameObject playerWeapon = weaponInPlayerMap[weapon];

                // Check if the player is within 10 units of the weapon
                float distance = Vector3.Distance(PlayerCamera.transform.position, weapon.transform.position);
                if (distance < 10)
                {
                    pickText.text = "Press 'P' to pick up " + weapon.name;
                    pickText.gameObject.SetActive(true);

                    // Change all crosshair images to red
                    ChangeCrosshairColor(Color.red);

                    if (Input.GetKeyDown(KeyCode.P)) // Trigger only on pressing 'P'
                    {
                        // Check if the player has enough coins
                        if (CoinBehavior.coinsCounter < 5)
                        {
                            ShowCoinErrorMessage("Not Enough Collected Coins !");
                            return;
                        }

                        // Check if the player is already holding a different weapon
                        if (IsWeaponPickedUp() && !PersistentObjectManager.hasWeapon[weaponName])
                        {
                            ShowErrorMessage("You cannot pick two different weapons!");
                            return;
                        }

/*                        // Deduct 5 coins
                        CoinBehavior.coinsCounter -= 5;
                        PersistentObjectManager.SetGold(CoinBehavior.coinsCounter);

                        // Immediately update the UI
                        CoinBehavior instance = FindObjectOfType<CoinBehavior>();
                        if (instance != null)
                        {
                            instance.UpdateCoinText(); // Force UI to reflect the change
                        }
                        else
                        {
                            Debug.LogWarning("No active CoinBehavior instance found to update UI.");
                        }
*/
                        // Play pickup sound
                        audioSource.PlayOneShot(weaponPickupSound);

                        // Weapon pickup logic
                        weapon.SetActive(false);
                        playerWeapon.SetActive(true);

                        // Update persistent state for the weapon
                        PersistentObjectManager.SetHasWeapon(weaponName, true);
                        PersistentObjectManager.SetHasWeaponInCave(weaponName, false);

                        if (weaponName == "Bow")
                        {
                            Arrows.SetActive(false);
                            Arrows_Player.SetActive(true);

                            PersistentObjectManager.SetHasWeapon("Arrow", true);
                            PersistentObjectManager.SetHasWeaponInCave("Arrow", false);
                        }
                    }

                }
                else
                {
                    pickText.gameObject.SetActive(false);
                    ChangeCrosshairColor(originalCrosshairColor);
                }
            }
            else
            {
                pickText.gameObject.SetActive(false);
                ChangeCrosshairColor(originalCrosshairColor);
            }
        }
        else
        {
            pickText.gameObject.SetActive(false);
            ChangeCrosshairColor(originalCrosshairColor);
        }
    }




    private void ShowErrorMessage(string message)
    {
        TwoWeaponsError.text = message;
        TwoWeaponsError.gameObject.SetActive(true);
        CancelInvoke(nameof(HideErrorText));  // Cancel any previous hides to restart the timer
        Invoke(nameof(HideErrorText), 3f);    // Hide the message after 3 seconds
    }

    private void ShowCoinErrorMessage(string message)
    {
        coinErrorText.text = message;
        coinErrorText.gameObject.SetActive(true);
        CancelInvoke(nameof(HideCoinErrorText));  // Cancel any previous hides to restart the timer
        Invoke(nameof(HideCoinErrorText), 2f);    // Hide the message after 2 seconds
    }

    private void HideCoinErrorText()
    {
        coinErrorText.gameObject.SetActive(false);
    }

    private void HideErrorText()
    {
        TwoWeaponsError.gameObject.SetActive(false);
    }

    private void ChangeCrosshairColor(Color newColor)
    {
        foreach (RawImage crosshair in crosshairs)
        {
            crosshair.color = newColor;
        }
    }

    private void HandleMovement()
    {
        float dz, dx, rotationAboutY, rotationAboutX;

        // Rotate player and camera
        rotationAboutX = -Input.GetAxis("Mouse Y") * angularSpeed * Time.deltaTime;
        PlayerCamera.transform.Rotate(rotationAboutX, 0, 0);

        rotationAboutY = Input.GetAxis("Mouse X") * angularSpeed * Time.deltaTime;
        transform.Rotate(0, rotationAboutY, 0);

        // Move player
        dz = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        dx = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        Vector3 motion = new Vector3(dx, -0.2f, dz);
        motion = transform.TransformDirection(motion);
        controller.Move(motion);

        // Play footstep sound if player is moving
        if (!(Mathf.Abs(motion.x) < 0.001 && Mathf.Abs(motion.z) < 0.001))
        {
            if (!footStep.isPlaying)
            {
                footStep.Play();
            }
        }
    }
}
