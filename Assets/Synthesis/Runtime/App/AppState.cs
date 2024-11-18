namespace Synthesis.App
{
    public enum AppState
    {
        None = 0,
        Initializing = 1,
        Initialized = 2,
        Ready = 3,
        LevelDataLoading = 4,
        LevelDataLoaded = 5,
        LevelDataInitializing = 6,
        LevelDataInitialized = 7,
        LevelCreating = 8,
        LevelCreated = 9,
        LevelStarting = 10,
        LevelStarted = 11,
        LevelPausing = 12,
        LevelPaused = 13,
        LevelEnding = 14,
        LevelEnded = 15,
        LevelDestroying = 16,
        LevelDestroyed = 17,
        Ending = 18,
        Ended = 19
    }
}