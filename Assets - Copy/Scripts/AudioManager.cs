using UnityEngine;

/// <summary>
/// Singleton that handles all audio in the game.
/// Keeps sound logic completely separate from cube logic.
/// </summary>
public class AudioManager : MonoBehaviour
{
    // Singleton instance
    public static AudioManager Instance { get; private set; }

    [Header("Audio References")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip successClip;

    private void Awake()
    {
        // Singleton pattern — only one AudioManager allowed
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Plays the success sound effect.
    /// Called by CubeInteractable when correct cube is gazed at.
    /// </summary>
    public void PlaySuccess()
    {
        if (successClip != null)
            audioSource.PlayOneShot(successClip);
    }
}