using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Project.InGame.Data;
using R3;

namespace Project.InGame.Game
{
    public class HandMatchingGameManager
    {
        private readonly IPlayerInput playerInput;
        private readonly IQuestionGenerator questionGenerator;
        private readonly InGameConfig config;
        
        private int currentScore;
        private int correctCount;
        private int incorrectCount;
        private float startTime;
        
        private CancellationTokenSource gameCancellationTokenSource;

        public HandMatchingGameManager(IPlayerInput playerInput, IQuestionGenerator questionGenerator, InGameConfig config)
        {
            this.playerInput = playerInput ?? throw new ArgumentNullException(nameof(playerInput));
            this.questionGenerator = questionGenerator ?? throw new ArgumentNullException(nameof(questionGenerator));
            this.config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async UniTask<GameResult> StartGameAsync(CancellationToken cancellationToken = default)
        {
            InitializeGame();
            gameCancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            
            try
            {
                GameEvents.OnGameStarted.OnNext(Unit.Default);
                
                var gameTask = RunGameLoopAsync(gameCancellationTokenSource.Token);
                var timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(config.GameTimeLimit), cancellationToken: gameCancellationTokenSource.Token);
                
                await UniTask.WhenAny(gameTask, timeoutTask);
                
                return CreateGameResult();
            }
            finally
            {
                CleanupGame();
            }
        }

        private void InitializeGame()
        {
            currentScore = 0;
            correctCount = 0;
            incorrectCount = 0;
            startTime = UnityEngine.Time.time;
            
            GameEvents.OnScoreChanged.OnNext(currentScore);
        }

        private async UniTask RunGameLoopAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var question = questionGenerator.GenerateQuestion();
                var choiceIds = Array.ConvertAll(question.choices, choice => choice.SignId);
                
                var answerTask = playerInput.WaitForPlayerSelectionAsync(choiceIds);
                var timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(config.QuestionTimeLimit), cancellationToken: cancellationToken);
                
                var (hasValue, selectedChoiceId) = await UniTask.WhenAny(answerTask, timeoutTask);
                
                bool isCorrect = false;
                if (hasValue == 0)
                {
                    var selectedIndex = Array.IndexOf(choiceIds, selectedChoiceId);
                    isCorrect = selectedIndex == question.correctAnswerIndex;
                }
                
                await ProcessAnswer(isCorrect, question.correctPair.BaseScore);
                
                if (cancellationToken.IsCancellationRequested)
                    break;
            }
        }

        private async UniTask ProcessAnswer(bool isCorrect, int baseScore)
        {
            if (isCorrect)
            {
                correctCount++;
                currentScore += baseScore;
            }
            else
            {
                incorrectCount++;
            }
            
            GameEvents.OnAnswerSubmitted.OnNext(isCorrect);
            GameEvents.OnScoreChanged.OnNext(currentScore);
            
            await UniTask.Delay(100);
        }

        private GameResult CreateGameResult()
        {
            var playTime = UnityEngine.Time.time - startTime;
            return new GameResult(currentScore, correctCount, incorrectCount, playTime);
        }

        private void CleanupGame()
        {
            var result = CreateGameResult();
            GameEvents.OnGameEnded.OnNext(result);
            
            playerInput?.Cancel();
            gameCancellationTokenSource?.Cancel();
            gameCancellationTokenSource?.Dispose();
            gameCancellationTokenSource = null;
        }
    }
}