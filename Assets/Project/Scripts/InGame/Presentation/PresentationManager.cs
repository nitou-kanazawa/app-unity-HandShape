using System;
using Cysharp.Threading.Tasks;

namespace Project.InGame.Presentation
{
    public class PresentationManager : IDisposable
    {
        private readonly ICorrectAnswerPresentation correctAnswerPresentation;
        private readonly IIncorrectAnswerPresentation incorrectAnswerPresentation;
        private readonly IScoreUpdatePresentation scoreUpdatePresentation;
        private readonly IQuestionTransitionPresentation questionTransitionPresentation;

        public PresentationManager(
            ICorrectAnswerPresentation correctAnswerPresentation = null,
            IIncorrectAnswerPresentation incorrectAnswerPresentation = null,
            IScoreUpdatePresentation scoreUpdatePresentation = null,
            IQuestionTransitionPresentation questionTransitionPresentation = null)
        {
            this.correctAnswerPresentation = correctAnswerPresentation ?? new DummyCorrectAnswerPresentation();
            this.incorrectAnswerPresentation = incorrectAnswerPresentation ?? new DummyIncorrectAnswerPresentation();
            this.scoreUpdatePresentation = scoreUpdatePresentation ?? new DummyScoreUpdatePresentation();
            this.questionTransitionPresentation = questionTransitionPresentation ?? new DummyQuestionTransitionPresentation();
        }

        public async UniTask PlayCorrectAnswerAsync(int addedScore)
        {
            await correctAnswerPresentation.PlayCorrectAnimationAsync(addedScore);
        }

        public async UniTask PlayIncorrectAnswerAsync()
        {
            await incorrectAnswerPresentation.PlayIncorrectAnimationAsync();
        }

        public async UniTask PlayScoreUpdateAsync(int newScore)
        {
            await scoreUpdatePresentation.PlayScoreUpdateAsync(newScore);
        }

        public async UniTask PlayQuestionTransitionAsync()
        {
            await questionTransitionPresentation.PlayQuestionTransitionAsync();
        }

        public void Dispose()
        {
            // 将来的にIDisposableを実装する演出クラスがあれば、ここで破棄処理を行う
        }
    }
}