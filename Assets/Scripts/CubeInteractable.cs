using System.Collections;
using UnityEngine;

/// <summary>
/// Handles behaviour of each cube when player gazes at it.
/// Knows if it is the correct cube or a wrong one.
public class CubeInteractable : MonoBehaviour, IInteractable
{
    // Is this the correct cube?
    [HideInInspector] public bool isCorrect = false;

    // Has this cube been disabled after correct answer?
    [HideInInspector] public bool isDisabled = false;

    // Satisfies the IsDisabled property required by IInteractable
    public bool IsDisabled => isDisabled;

    private Color originalColor;
    private Renderer cubeRenderer;

    private void Awake()
    {
        cubeRenderer = GetComponent<Renderer>();
    }

    /// <summary>
    /// Called by CubeManager to assign a random color.
    /// </summary>
    public void SetColor(Color color)
    {
        originalColor = color;
        cubeRenderer.material.color = color;
    }

    /// <summary>
    /// Called by GazeController when player looks at this cube.
    /// </summary>
    public void OnGazed()
    {
        if (isDisabled) return;

        if (isCorrect)
            HandleCorrect();
        else
            HandleWrong();
    }

    // Handle correct cube logic
    private void HandleCorrect()
    {
        // Turn white
        cubeRenderer.material.color = Color.white;

        // Enable emission for glow effect
        cubeRenderer.material.EnableKeyword("_EMISSION");
        cubeRenderer.material.SetColor("_EmissionColor",
            Color.white * 2f);

        // Spawn a point light on the cube for extra glow
        GameObject lightObj = new GameObject("CorrectLight");
        lightObj.transform.position = transform.position;
        Light pointLight = lightObj.AddComponent<Light>();
        pointLight.type = LightType.Point;
        pointLight.color = Color.white;
        pointLight.intensity = 3f;
        pointLight.range = 5f;

        // Disable further interaction
        isDisabled = true;

        // Start glow pulse animation
        StartCoroutine(PulseGlow(pointLight));

        // Notify other managers
        AudioManager.Instance.PlaySuccess();
        UIManager.Instance.ShowMessage("Correct!", Color.green);
    }
    /// <summary>
    /// Pulses the glow light briefly then keeps it steady.
    /// </summary>
    private IEnumerator PulseGlow(Light glowLight)
    {
        float duration = 0.6f;
        float elapsed = 0f;

        // Pulse up
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            glowLight.intensity = Mathf.Lerp(0f, 5f, t);
            cubeRenderer.material.SetColor("_EmissionColor",
                Color.white * Mathf.Lerp(0f, 3f, t));
            yield return null;
        }

        // Stay steady at full glow
        glowLight.intensity = 3f;
        cubeRenderer.material.SetColor("_EmissionColor",
            Color.white * 2f);
    }

    // Handle wrong cube logic
    private void HandleWrong()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRed());
        UIManager.Instance.ShowMessage("Try Again", Color.red);
    }

    // Briefly flash red then return to original color
    private IEnumerator FlashRed()
    {
        cubeRenderer.material.color = Color.red;
        yield return new WaitForSeconds(0.4f);
        cubeRenderer.material.color = originalColor;
    }
}
