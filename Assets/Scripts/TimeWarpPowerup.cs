using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarpPowerup : MonoBehaviour
{
    public float timeWarpDuration = 5f; // Adjust as needed
    public float timeWarpFactor = 0.5f; // Adjust as needed

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ActivateTimeWarp(other.gameObject, timeWarpDuration, timeWarpFactor);
            // Instead of destroying immediately, you can deactivate the GameObject and later reuse it.
            gameObject.SetActive(false);
        }
    }

    private void ActivateTimeWarp(GameObject player, float duration, float factor)
    {
        StartCoroutine(TimeWarpEffect(player, duration, factor));
    }

    private IEnumerator TimeWarpEffect(GameObject player, float duration, float factor)
    {
        // Disable player controls during time warp
        Player playerScript = player.GetComponent<Player>();
        playerScript.DisableControls();

        // Store the original time scale
        float originalTimeScale = Time.timeScale;

        // Slow down time
        Time.timeScale = factor;

        // Wait for the specified duration
        yield return new WaitForSecondsRealtime(duration);

        // Restore the original time scale
        Time.timeScale = originalTimeScale;

        // Enable player controls after time warp
        playerScript.EnableControls();

        // Deactivate the TimeWarp power-up GameObject
        gameObject.SetActive(false);
    }
}


