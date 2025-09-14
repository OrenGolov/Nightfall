using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    void Start()
    {
        // Initialization if needed
    }

    void Update()
    {
        // Any updates if needed
    }

    private void OnTriggerEnter(Collider other)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            // Transitioning from main scene to cave (index 1) or new scene (index 2)
            PersistentObjectManager.SetGold(CoinBehavior.coinsCounter);

            // Check the portal name or tag to determine destination
            if (gameObject.CompareTag("Portal"))
            {
                SceneManager.LoadScene(1); // Cave scene
            }
            else if (gameObject.CompareTag("PortalFight"))
            {
                SceneManager.LoadScene(2); // New scene
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            // Transitioning back from cave to main scene
            SceneManager.LoadScene(0); // Main scene
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            // Transitioning back from new scene to main scene
            SceneManager.LoadScene(0); // Main scene
        }
    }
}
