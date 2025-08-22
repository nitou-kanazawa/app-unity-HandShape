using System.Threading;
using Cysharp.Threading.Tasks;
using Project.InGame.Data;
using Project.InGame.Presentation;
using R3;
using UnityEngine;

namespace Project.InGame.Game
{
    public class GameDemo : MonoBehaviour
    {
        [SerializeField] private InGameConfig gameConfig;
        
        private HandMatchingGameManager gameManager;
        private PresentationManager presentationManager;
        private CancellationTokenSource cancellationTokenSource;

        private void Start()
        {
            if (gameConfig == null)
            {
                Debug.LogError("InGameConfig is not assigned!");
                return;
            }

            InitializeGame();
            StartGameAsync().Forget();
        }

        private void InitializeGame()
        {
            presentationManager = PresentationFactory.CreateDummyPresentationManager();
            gameManager = GameFactory.CreateDummyGame(gameConfig, seed: 12345);
            cancellationTokenSource = new CancellationTokenSource();
            
            SubscribeToGameEvents();
        }

        private void SubscribeToGameEvents()
        {
            GameEvents.OnGameStarted.Subscribe(_ => 
            {
                Debug.Log("[GameDemo] Game Started!");
            }).AddTo(this);

            GameEvents.OnScoreChanged.Subscribe(score => 
            {
                Debug.Log($"[GameDemo] Score Changed: {score}");
            }).AddTo(this);

            GameEvents.OnAnswerSubmitted.Subscribe(isCorrect => 
            {
                var result = isCorrect ? "Correct" : "Incorrect";
                Debug.Log($"[GameDemo] Answer: {result}");
            }).AddTo(this);

            GameEvents.OnTimeUpdated.Subscribe(time => 
            {
                Debug.Log($"[GameDemo] Time: {time:F1}s");
            }).AddTo(this);

            GameEvents.OnGameEnded.Subscribe(result => 
            {
                Debug.Log($"[GameDemo] Game Ended! Final Score: {result.finalScore}, " +
                         $"Correct: {result.correctCount}, Incorrect: {result.incorrectCount}, " +
                         $"Play Time: {result.playTime:F1}s");
            }).AddTo(this);
        }

        private async UniTaskVoid StartGameAsync()
        {
            try
            {
                Debug.Log("[GameDemo] Starting Hand Matching Game Demo...");
                
                var result = await gameManager.StartGameAsync(cancellationTokenSource.Token);
                
                Debug.Log($"[GameDemo] Demo completed with result: {result.finalScore} points");
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("[GameDemo] Game was cancelled");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[GameDemo] Error during game: {ex.Message}");
            }
        }

        private void OnDestroy()
        {
            cancellationTokenSource?.Cancel();
            cancellationTokenSource?.Dispose();
            presentationManager?.Dispose();
            GameEvents.Dispose();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("[GameDemo] Space pressed - Restarting game...");
                RestartGame();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("[GameDemo] Escape pressed - Stopping game...");
                StopGame();
            }
        }

        private void RestartGame()
        {
            StopGame();
            InitializeGame();
            StartGameAsync().Forget();
        }

        private void StopGame()
        {
            cancellationTokenSource?.Cancel();
        }

        [ContextMenu("Create Sample Config")]
        private void CreateSampleConfig()
        {
            Debug.Log("[GameDemo] Please create InGameConfig ScriptableObject manually through Create > Project > InGame > InGameConfig");
        }
    }
}