namespace Project.InGame.Presentation
{
    public static class PresentationFactory
    {
        public static PresentationManager CreateDummyPresentationManager()
        {
            return new PresentationManager(
                new DummyCorrectAnswerPresentation(),
                new DummyIncorrectAnswerPresentation(),
                new DummyScoreUpdatePresentation(),
                new DummyQuestionTransitionPresentation()
            );
        }

        public static PresentationManager CreateCustomPresentationManager(
            ICorrectAnswerPresentation correctAnswer = null,
            IIncorrectAnswerPresentation incorrectAnswer = null,
            IScoreUpdatePresentation scoreUpdate = null,
            IQuestionTransitionPresentation questionTransition = null)
        {
            return new PresentationManager(
                correctAnswer,
                incorrectAnswer,
                scoreUpdate,
                questionTransition
            );
        }
    }
}