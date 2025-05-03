using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FlashManager : MonoBehaviour
{
    [SerializeField] private Image flashImage; // Assign a UI Image in the Inspector
    [SerializeField] private float flashDuration = 0.5f;

    private void Start()
    {
        // Ensure the flash image starts fully transparent
        flashImage.color = new Color(1, 1, 1, 0);
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        float elapsedTime = 0f;

        // Fade in to white
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / (flashDuration / 2));
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        elapsedTime = 0f;

        // Fade out to transparent
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / (flashDuration / 2));
            flashImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}

