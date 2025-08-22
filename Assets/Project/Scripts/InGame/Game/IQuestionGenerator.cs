using Project.InGame.Data;

namespace Project.InGame.Game
{
    public interface IQuestionGenerator
    {
        GameQuestion GenerateQuestion();
    }

    public struct GameQuestion
    {
        public HandPairData correctPair;
        public HandSignData[] choices;
        public int correctAnswerIndex;

        public GameQuestion(HandPairData correctPair, HandSignData[] choices, int correctAnswerIndex)
        {
            this.correctPair = correctPair;
            this.choices = choices;
            this.correctAnswerIndex = correctAnswerIndex;
        }
    }
}