using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Singleton that handles all on-screen text feedback.
/// Cubes call this to show messages — keeps UI logic
/// completely separate from game logic.
/// </summary>
public class UIManager : MonoBehaviour
{
    // Singleton instance
    public static UIManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI feedbackText;

    [Header("Settings")]
    [SerializeField] private float displayDuration = 2f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        // Singleton pattern — only one UIManager allowed
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    /// <summary>
    /// Shows a message on screen for a short duration.
    /// Called by CubeInteractable.
    /// </summary>
    public void ShowMessage(string message, Color color)
    {
        feedbackText.text = message;
        feedbackText.color = color;
        feedbackText.gameObject.SetActive(true);

        // Reset timer if message is already showing
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }

    // Hide the text after a delay
    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        feedbackText.gameObject.SetActive(false);
    }
}