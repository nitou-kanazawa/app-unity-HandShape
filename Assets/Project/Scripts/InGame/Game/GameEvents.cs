using R3;

namespace Project.InGame.Game
{
    public static class GameEvents
    {
        public static readonly Subject<int> OnScoreChanged = new();
        public static readonly Subject<bool> OnAnswerSubmitted = new();
        public static readonly Subject<float> OnTimeUpdated = new();
        public static readonly Subject<Unit> OnGameStarted = new();
        public static readonly Subject<GameResult> OnGameEnded = new();

        public static void Dispose()
        {
            OnScoreChanged?.Dispose();
            OnAnswerSubmitted?.Dispose();
            OnTimeUpdated?.Dispose();
            OnGameStarted?.Dispose();
            OnGameEnded?.Dispose();
        }
    }
}