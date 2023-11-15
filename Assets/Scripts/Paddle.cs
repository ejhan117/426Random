using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Paddle : MonoBehaviour
{
    public new Rigidbody2D rigidbody { get; private set; }

    public float speed = 7f;

    // Define a flag to indicate reversed controls
    private bool reversedControls = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void ResetPosition()
    {
        rigidbody.velocity = Vector2.zero;
        rigidbody.position = new Vector2(rigidbody.position.x, 0f);
    }

    public void ReverseControls()
    {
        // Reverse the direction of movement
        speed *= -1;

        // Toggle the reversedControls flag
        reversedControls = !reversedControls;
    }

    public void ApplyShockwave()
    {
        // Add code to apply the shockwave effect or disable the paddle as needed
        // For example, temporarily disable the paddle's movement
        StartCoroutine(DisablePaddleForSeconds(2f)); // Disable for 2 seconds (adjust as needed)
    }

    private IEnumerator DisablePaddleForSeconds(float duration)
    {
        // Disable the paddle's movement
        speed = 0f;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Re-enable the paddle's movement
        speed = 7f; // Reset to the default speed (adjust if needed)
    }

    private void FixedUpdate()
    {
        // Check if controls are reversed
        float verticalInput = reversedControls ? -Input.GetAxis("Vertical") : Input.GetAxis("Vertical");

        // Move the paddle
        rigidbody.velocity = new Vector2(0f, verticalInput * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            // Handle ball collision if needed
        }
    }
}

