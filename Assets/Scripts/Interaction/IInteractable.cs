namespace VRCampusTour.Interaction
{
    /// <summary>
    /// Interface for all interactable objects
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Called when the object is interacted with (clicked/gazed)
        /// </summary>
        void OnInteract();

        /// <summary>
        /// Called when gaze enters the object
        /// </summary>
        void OnGazeEnter();

        /// <summary>
        /// Called when gaze exits the object
        /// </summary>
        void OnGazeExit();

        /// <summary>
        /// Returns true if the object can be interacted with
        /// </summary>
        bool IsInteractable();
    }
}