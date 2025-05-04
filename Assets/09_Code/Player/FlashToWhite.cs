using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlashToWhite : MonoBehaviour
{
    [SerializeField] private float flashDuration = 0.5f;
    [SerializeField] private WorldGenerator worldGenerator; // Reference to the WorldGenerator script
    [SerializeField] private GameObject corpse; // Assign the corpse GameObject in the Inspector

    private bool isFlashing = false;
    private Color originalBackgroundColor;

    private void Start()
    {
        // Ensure the Main Camera is assigned and has a valid background color
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not found! Ensure a camera is tagged as 'MainCamera'.");
            return;
        }

        // Store the original background color of the camera
        originalBackgroundColor = Camera.main.backgroundColor;
    }

    private void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            ShootRaycast();
        }
    }

    private void ShootRaycast()
    {
        // Check if Camera.main is null
        if (Camera.main == null)
        {
            Debug.LogError("Main Camera is not found! Ensure a camera is tagged as 'MainCamera'.");
            return;
        }

        // Shoot a raycast from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Visualize the raycast in the Scene view
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 1f); // Draws a red ray for 1 second

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the raycast hit the corpse
            if (corpse == null)
            {
                Debug.LogError("Corpse GameObject is not assigned in the Inspector!");
                return;
            }

            if (hit.collider != null && hit.collider.gameObject == corpse)
            {
                // Trigger the flash effect and regenerate the world
                TriggerFlash();
                RestartScene();
            }
        }
    }

    private void TriggerFlash()
    {
        if (!isFlashing)
        {
            StartCoroutine(FlashAndRegenerateCoroutine());
        }
    }

    private IEnumerator FlashAndRegenerateCoroutine()
    {
        isFlashing = true;
        float elapsedTime = 0f;

        // Fade in to white
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (flashDuration / 2);
            Camera.main.backgroundColor = Color.Lerp(originalBackgroundColor, Color.white, t);
            yield return null;
        }

        elapsedTime = 0f;

        // Regenerate the world during the flash
        

        // Fade out to the original background color
        while (elapsedTime < flashDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (flashDuration / 2);
            Camera.main.backgroundColor = Color.Lerp(Color.white, originalBackgroundColor, t);
            yield return null;
        }

        isFlashing = false;
    }

    private void RestartScene()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}