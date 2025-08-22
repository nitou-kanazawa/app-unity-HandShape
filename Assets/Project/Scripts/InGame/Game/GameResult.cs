namespace Project.InGame.Game
{
    public struct GameResult
    {
        public int finalScore;
        public int correctCount;
        public int incorrectCount;
        public float playTime;

        public GameResult(int finalScore, int correctCount, int incorrectCount, float playTime)
        {
            this.finalScore = finalScore;
            this.correctCount = correctCount;
            this.incorrectCount = incorrectCount;
            this.playTime = playTime;
        }
    }
}