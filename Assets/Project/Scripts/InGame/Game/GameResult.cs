using Nitou.TinyProcess;

namespace Project.InGame.Game
{
    public sealed class GameResult : CompleteResult
    {
        public int Score { get; }
        public int CorrectCount { get; }
        public int IncorrectCount { get; }

        public GameResult(int score) : base()
        {
            Score = score;
        }
    }
}
