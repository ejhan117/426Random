using UnityEngine;
using System.Collections;

public class ReverseControlPowerup : MonoBehaviour
{
    public float reverseDuration = 5f; // Duration of the reverse control effect

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Assuming the player has a "Player" tag
        {
            ActivateReverseControl(other.gameObject);
            Destroy(gameObject); // Remove the power-up after activation
        }
    }

    private void ActivateReverseControl(GameObject player)
    {
        StartCoroutine(ReverseControlEffect(player));
    }

    private IEnumerator ReverseControlEffect(GameObject player)
    {
        // Get the opponent's paddle script
        Paddle opponentPaddle = GetOpponentPaddle(player);

        // Apply the reverse controls effect
        opponentPaddle.ReverseControls();

        // Wait for the specified duration
        yield return new WaitForSeconds(reverseDuration);

        // Revert the opponent's controls after the duration
        opponentPaddle.ReverseControls();
    }

    // Function to get the opponent's paddle script
    private Paddle GetOpponentPaddle(GameObject currentPlayer)
    {
        Paddle opponentPaddle = null;

        if (currentPlayer.CompareTag("Player"))
        {
            // Assuming the opponent's paddle script is attached to the "Opponent" GameObject
            opponentPaddle = GameObject.Find("Opponent").GetComponent<Paddle>();
        }
        return opponentPaddle;
    }
}


