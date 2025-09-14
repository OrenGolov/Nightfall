using System.Collections;
using UnityEngine;

public class BowAndArrow : MonoBehaviour
{
    public GameObject arrowOnBow; // The arrow visible on the bow
    public GameObject arrowInTarget; // The arrow that flies to the target
    public Transform arrowSpawnPoint; // The spawn position for the arrow in target
    public Camera playerCamera; // Main camera used for raycasting (aligned with crosshair)
    public float arrowSpeed = 15f; // Speed of the flying arrow
    private bool canShoot = true; // Controls whether the player can shoot
    private bool canReload = true; // Controls whether the player can reload
    private AudioSource shootAudio; // AudioSource for shooting sound

    private void Awake()
    {
        // Ensure the bow persists across scenes
        DontDestroyOnLoad(gameObject);

        // Initialize AudioSource
        shootAudio = GetComponent<AudioSource>();
        if (shootAudio == null)
        {
            shootAudio = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        // Ensure initial states
        arrowOnBow.SetActive(true);
        arrowInTarget.SetActive(false);
    }

    private void Update()
    {
        // Shoot the arrow
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            ShootArrow();
        }

        // Reload the arrow
        if (Input.GetMouseButtonDown(1) && !canShoot && canReload)
        {
            StartCoroutine(ReloadArrowWithDelay());
        }
    }

    private void ShootArrow()
    {
        if (arrowOnBow != null && arrowInTarget != null)
        {
            // Deactivate the arrow on the bow
            arrowOnBow.SetActive(false);

            // Activate the arrow in target
            arrowInTarget.SetActive(true);

            // Play the shooting sound
            if (shootAudio != null)
            {
                shootAudio.Play();
            }

            // Raycast from the center of the screen
            Vector3 targetPoint;
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); // Middle of the screen
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                targetPoint = hit.point; // Hit detected

                // Check if the arrow hit the Knight and apply damage
                Knight knight = hit.collider.GetComponent<Knight>();
                if (knight != null)
                {
                    knight.DoDamage();
                }
            }
            else
            {
                targetPoint = playerCamera.transform.position + playerCamera.transform.forward * 50f; // Default target
            }

            // Start flying the arrow
            StartCoroutine(FlyArrow(arrowInTarget, targetPoint));

            // Disable shooting until the player reloads
            canShoot = false;
        }
    }

    private IEnumerator ReloadArrowWithDelay()
    {
        // Prevent reloading while the delay is active
        canReload = false;

        // Wait for 1 second
        yield return new WaitForSeconds(1f);

        // Reactivate the arrow on the bow
        if (arrowOnBow != null && arrowInTarget != null)
        {
            arrowOnBow.SetActive(true);
            arrowInTarget.SetActive(false);
        }

        // Allow shooting again
        canShoot = true;

        // Allow reloading after the delay
        canReload = true;
    }

    private IEnumerator FlyArrow(GameObject arrow, Vector3 targetPoint)
    {
        Vector3 startPosition = arrowSpawnPoint.position;
        float distance = Vector3.Distance(startPosition, targetPoint);
        float flightTime = distance / arrowSpeed;
        float elapsedTime = 0f;

        // Set the arrow's initial position
        arrow.transform.position = startPosition;

        // Adjust rotation to point toward the target and add 90 degrees on the X-axis
        arrow.transform.rotation = Quaternion.LookRotation((targetPoint - startPosition).normalized);
        arrow.transform.Rotate(90f, 0f, 0f); // Rotate 90 degrees forward on the X-axis

        while (elapsedTime < flightTime)
        {
            elapsedTime += Time.deltaTime;
            float step = elapsedTime / flightTime;

            // Move the arrow towards the target
            arrow.transform.position = Vector3.Lerp(startPosition, targetPoint, step);

            // Rotate the arrow to align with its trajectory and maintain the 90-degree adjustment
            Vector3 direction = (targetPoint - arrow.transform.position).normalized;
            arrow.transform.rotation = Quaternion.LookRotation(direction);
            arrow.transform.Rotate(90f, 0f, 0f); // Maintain the 90-degree forward rotation

            yield return null; // Wait for the next frame
        }

        // Ensure the arrow stays at the target
        arrow.transform.position = targetPoint;
    }
}