namespace VRCampusTour.Utils
{
    public static class Constants
    {
        // Scene Names
        public const string SCENE_WELCOME = "WelcomeScene";
        public const string SCENE_WALKTHROUGH = "CampusWalkthrough";
        public const string SCENE_OUTRO = "OutroScene";

        // Tags
        public const string TAG_PLAYER = "Player";
        public const string TAG_INTERACTABLE = "Interactable";
        public const string TAG_NAVIGATION = "Navigation";
        public const string TAG_INFO = "Info";

        // Layers
        public const string LAYER_UI = "UI";
        public const string LAYER_HOTSPOT = "Hotspot";

        // UI
        public const float FADE_DURATION = 1.5f;
        public const float GAZE_DURATION = 2f;
        public const float INFO_PANEL_ANIMATION_DURATION = 0.3f;

        // Audio
        public const float DEFAULT_VOLUME = 0.7f;
        public const float TRANSITION_AUDIO_FADE = 0.5f;

        // VR
        public const float RAYCAST_DISTANCE = 100f;
        public const float HOTSPOT_SCALE_MULTIPLIER = 1.2f;
    }

    public static class AnimationKeys
    {
        public const string SHOW = "Show";
        public const string HIDE = "Hide";
        public const string PULSE = "Pulse";
    }
}