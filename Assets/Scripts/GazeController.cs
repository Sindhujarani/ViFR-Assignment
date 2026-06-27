using UnityEngine;

/// <summary>
/// Fires a raycast from the camera center every frame.
/// Uses IInteractable interface so it works with ANY object —
/// cubes, spheres, coins, or 25 different objects.
/// No changes needed here when new object types are added.
/// </summary>
public class GazeController : MonoBehaviour
{
    [Header("Gaze Settings")]
    [SerializeField] private float gazeDistance = 20f;
    [SerializeField] private LayerMask interactableLayer;

    // Track which object we are currently looking at
    private IInteractable lastGazed = null;

    private void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, gazeDistance,
                            interactableLayer))
        {
            // Works for ANY object implementing IInteractable
            // Not just cubes — any of 25 objects will work here
            IInteractable interactable = hit.collider
                .GetComponent<IInteractable>();

            // Only trigger once per new object we look at
            if (interactable != null
                && interactable != lastGazed
                && !interactable.IsDisabled)
            {
                lastGazed = interactable;
                interactable.OnGazed();
            }
        }
        else
        {
            // Not looking at anything — reset
            lastGazed = null;
        }
    }
}
