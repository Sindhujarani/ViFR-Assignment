using UnityEngine;

/// <summary>
/// Spawns interactable objects at defined positions.
/// Accepts an ARRAY of prefabs — so tomorrow you can add
/// 25 different objects just by dragging them into the
/// Inspector. Zero code changes needed.
/// </summary>
public class CubeManager : MonoBehaviour
{
    [Header("Interactable Objects")]
    [Tooltip("Add any prefab here that has an IInteractable component")]
    [SerializeField] private GameObject[] interactablePrefabs;

    [Header("Spawn Points")]
    [SerializeField]
    private Vector3[] spawnPositions = new Vector3[]
    {
        new Vector3(-2.5f, 0.5f, 5f),
        new Vector3( 0f,   0.5f, 5f),
        new Vector3( 2.5f, 0.5f, 5f)
    };

    private void Start()
    {
        SpawnObjects();
    }

    /// <summary>
    /// Spawns one object per spawn point.
    /// Randomly picks from the prefabs array.
    /// Randomly picks one as the correct object.
    /// </summary>
    private void SpawnObjects()
    {
        // Guard clause — nothing to spawn
        if (interactablePrefabs == null
            || interactablePrefabs.Length == 0)
        {
            Debug.LogError("CubeManager: No prefabs assigned!");
            return;
        }

        // Randomly decide which spawn point has correct object
        int correctIndex = Random.Range(0, spawnPositions.Length);

        for (int i = 0; i < spawnPositions.Length; i++)
        {
            // Pick a random prefab from the array
            GameObject prefab = interactablePrefabs[
                Random.Range(0, interactablePrefabs.Length)];

            // Spawn it
            GameObject obj = Instantiate(
                prefab,
                spawnPositions[i],
                Quaternion.identity
            );

            obj.name = $"Interactable_{i}";

            // Get the CubeInteractable component
            CubeInteractable interactable = obj
                .GetComponent<CubeInteractable>();

            if (interactable != null)
            {
                // Give it a random color
                Color randomColor = new Color(
                    Random.Range(0.2f, 0.9f),
                    Random.Range(0.2f, 0.9f),
                    Random.Range(0.2f, 0.9f)
                );

                interactable.SetColor(randomColor);

                // Mark one as correct
                interactable.isCorrect = (i == correctIndex);
            }
        }
    }
}
