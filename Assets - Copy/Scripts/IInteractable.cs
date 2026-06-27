/// <summary>
/// Interface that ANY interactable object must implement.
/// This means GazeController doesn't care if it's a cube,
/// sphere, coin or any of 25 different objects —
/// as long as it implements this interface, it will work.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// Called when the player gazes at this object.
    /// </summary>
    void OnGazed();

    /// <summary>
    /// Whether this object is disabled and
    /// can no longer be interacted with.
    /// </summary>
    bool IsDisabled { get; }
}