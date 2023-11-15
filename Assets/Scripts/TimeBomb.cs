using UnityEngine;
using System.Collections;

public class TimeBomb : MonoBehaviour
{
    public float explosionDuration = 2f; // Duration before explosion
    public float explosionRadius = 2f; // Radius of the explosion
    public GameObject explosionEffect; // Particle effect or visual for the explosion

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ball")) // Assuming the ball has a "Ball" tag
        {
            ActivateTimeBomb(other.gameObject);
            Destroy(gameObject); // Remove the power-up after activation
        }
    }

    private void ActivateTimeBomb(GameObject ball)
    {
        StartCoroutine(CountdownToExplosion(ball));
    }

    private IEnumerator CountdownToExplosion(GameObject ball)
    {
        yield return new WaitForSeconds(explosionDuration);

        Explode(ball.transform.position);
    }

    private void Explode(Vector2 explosionPosition)
    {
        // Play explosion visual or particle effect
        Instantiate(explosionEffect, explosionPosition, Quaternion.identity);

        // Get all colliders in the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

        foreach (Collider2D col in colliders)
        {
            if (col.CompareTag("Player")) // Assuming the paddle has a "Player" tag
            {
                // Apply the shockwave effect or disable the opponent's paddle
                Paddle opponentPaddle = col.GetComponent<Paddle>();
                if (opponentPaddle != null)
                {
                    // Apply shockwave effect or disable the paddle (adjust as needed)
                    opponentPaddle.ApplyShockwave();
                }
            }
        }
    }
}

