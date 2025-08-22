using NUnit.Framework;
using Cysharp.Threading.Tasks;
using Project.InGame.Presentation;

namespace Project.InGame.Tests
{
    public class PresentationManagerTests
    {
        private PresentationManager presentationManager;
        private MockPresentations mockPresentations;

        [SetUp]
        public void SetUp()
        {
            mockPresentations = new MockPresentations();
            presentationManager = new PresentationManager(
                mockPresentations.CorrectAnswer,
                mockPresentations.IncorrectAnswer,
                mockPresentations.ScoreUpdate,
                mockPresentations.QuestionTransition
            );
        }

        [Test]
        public async void PlayCorrectAnswerAsync_CallsCorrectPresentation()
        {
            await presentationManager.PlayCorrectAnswerAsync(10);
            
            Assert.IsTrue(mockPresentations.CorrectAnswer.WasCalled);
            Assert.AreEqual(10, mockPresentations.CorrectAnswer.LastAddedScore);
        }

        [Test]
        public async void PlayIncorrectAnswerAsync_CallsIncorrectPresentation()
        {
            await presentationManager.PlayIncorrectAnswerAsync();
            
            Assert.IsTrue(mockPresentations.IncorrectAnswer.WasCalled);
        }

        [Test]
        public async void PlayScoreUpdateAsync_CallsScoreUpdatePresentation()
        {
            await presentationManager.PlayScoreUpdateAsync(50);
            
            Assert.IsTrue(mockPresentations.ScoreUpdate.WasCalled);
            Assert.AreEqual(50, mockPresentations.ScoreUpdate.LastScore);
        }

        [Test]
        public async void PlayQuestionTransitionAsync_CallsQuestionTransitionPresentation()
        {
            await presentationManager.PlayQuestionTransitionAsync();
            
            Assert.IsTrue(mockPresentations.QuestionTransition.WasCalled);
        }

        [Test]
        public void Constructor_WithNullPresentations_UsesDummyImplementations()
        {
            var manager = new PresentationManager();
            
            Assert.DoesNotThrowAsync(async () => await manager.PlayCorrectAnswerAsync(10));
            Assert.DoesNotThrowAsync(async () => await manager.PlayIncorrectAnswerAsync());
            Assert.DoesNotThrowAsync(async () => await manager.PlayScoreUpdateAsync(20));
            Assert.DoesNotThrowAsync(async () => await manager.PlayQuestionTransitionAsync());
        }

        [TearDown]
        public void TearDown()
        {
            presentationManager?.Dispose();
        }

        private class MockPresentations
        {
            public MockCorrectAnswerPresentation CorrectAnswer { get; } = new MockCorrectAnswerPresentation();
            public MockIncorrectAnswerPresentation IncorrectAnswer { get; } = new MockIncorrectAnswerPresentation();
            public MockScoreUpdatePresentation ScoreUpdate { get; } = new MockScoreUpdatePresentation();
            public MockQuestionTransitionPresentation QuestionTransition { get; } = new MockQuestionTransitionPresentation();
        }

        private class MockCorrectAnswerPresentation : ICorrectAnswerPresentation
        {
            public bool WasCalled { get; private set; }
            public int LastAddedScore { get; private set; }

            public async UniTask PlayCorrectAnimationAsync(int addedScore)
            {
                WasCalled = true;
                LastAddedScore = addedScore;
                await UniTask.CompletedTask;
            }
        }

        private class MockIncorrectAnswerPresentation : IIncorrectAnswerPresentation
        {
            public bool WasCalled { get; private set; }

            public async UniTask PlayIncorrectAnimationAsync()
            {
                WasCalled = true;
                await UniTask.CompletedTask;
            }
        }

        private class MockScoreUpdatePresentation : IScoreUpdatePresentation
        {
            public bool WasCalled { get; private set; }
            public int LastScore { get; private set; }

            public async UniTask PlayScoreUpdateAsync(int newScore)
            {
                WasCalled = true;
                LastScore = newScore;
                await UniTask.CompletedTask;
            }
        }

        private class MockQuestionTransitionPresentation : IQuestionTransitionPresentation
        {
            public bool WasCalled { get; private set; }

            public async UniTask PlayQuestionTransitionAsync()
            {
                WasCalled = true;
                await UniTask.CompletedTask;
            }
        }
    }
}