using UnityEngine;

public class FlashManager : MonoBehaviour
{
    [SerializeField] private GameObject corpse; // Assign the corpse GameObject in the Inspector
    [SerializeField] private WorldGenerator worldGenerator; // Reference to the WorldGenerator script
    [SerializeField] private CanvasGroup flashCanvasGroup; // UI CanvasGroup for the flash effect
    [SerializeField] private float flashDuration = 0.5f; // Duration of the flash effect

    private bool isFlashing = false;

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            ShootRaycast();
        }
    }

    private void ShootRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject == corpse)
            {
                Debug.Log("Raycast hit the corpse!");
                TriggerFlash();
            }
        }
    }

    private void TriggerFlash()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashCoroutine());
        }
    }

    private System.Collections.IEnumerator FlashCoroutine()
    {
        isFlashing = true;
        float elapsedTime = 0f;

        // Fade in to white
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            flashCanvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / (flashDuration / 2));
            yield return null;
        }

        elapsedTime = 0f;

        // Fade out to transparent
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            flashCanvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / (flashDuration / 2));
            yield return null;
        }

        isFlashing = false;
    }
}