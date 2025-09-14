using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the arrow hit the knight
        if (collision.gameObject.CompareTag("Knight"))
        {
            // Stop the arrow's movement
            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true; // Disable physics on the arrow
                rb.velocity = Vector3.zero; // Stop movement
            }

            // Attach the arrow to the knight
            transform.SetParent(collision.transform);

          //   Optional: Adjust the arrow's position/orientation on the knight
            transform.localPosition += new Vector3(0, 0.5f, 0); // Adjust as needed
            transform.localRotation = Quaternion.identity;
        }
    }
}
