//using UnityEngine;

///// <summary>
///// Attached to the Main Camera.
///// Fires a raycast forward every frame from the center of the screen.
///// Whatever the ray hits is what the player is "looking at".
///// Delegates all interaction to the cube it hits.
///// </summary>
//public class GazeController : MonoBehaviour
//{
//    [Header("Gaze Settings")]
//    [SerializeField] private float gazeDistance = 20f;
//    [SerializeField] private LayerMask cubeLayer;

//    // Track which cube we are currently looking at
//    private CubeInteractable lastGazedCube = null;

//    private void Update()
//    {
//        // Fire ray from camera position in the direction camera is facing
//        Ray ray = new Ray(transform.position, transform.forward);
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit, gazeDistance, cubeLayer))
//        {
//            // Try to get CubeInteractable from the hit object
//            CubeInteractable cube = hit.collider
//                                       .GetComponent<CubeInteractable>();

//            // Only trigger once per new cube we look at
//            if (cube != null && cube != lastGazedCube)
//            {
//                lastGazedCube = cube;
//                cube.OnGazed();
//            }
//        }
//        else
//        {
//            // Not looking at any cube — reset
//            lastGazedCube = null;
//        }
//    }
//}


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