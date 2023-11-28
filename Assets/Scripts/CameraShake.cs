using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private float shakeDuration = 0.4f; // Duration of the shake effect
    private float shakeMagnitude = 0.15f; // Magnitude of the shake

    private Vector3 originalPosition;
    private float currentShakeDuration;

    public RectTransform canvasTransform;
    private Vector3 canvasPos;

    void Awake()
    {
        originalPosition = transform.position; // Store the original position of the camera
        canvasPos = canvasTransform.anchoredPosition;
    }

    void Update()
    {
        if (currentShakeDuration > 0)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeMagnitude;
            canvasTransform.anchoredPosition = canvasPos + Random.insideUnitSphere * shakeMagnitude;
            currentShakeDuration -= Time.deltaTime;
        }
        else
        {
            currentShakeDuration = 0f;
            transform.position = originalPosition; // Reset the position after shaking
            canvasTransform.anchoredPosition = canvasPos;
        }
    }

    public void TriggerShake()
    {
        currentShakeDuration = shakeDuration; // Trigger the shake effect
    }
}
