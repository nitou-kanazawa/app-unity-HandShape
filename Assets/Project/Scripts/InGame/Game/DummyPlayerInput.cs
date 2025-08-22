using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.InGame.Game
{
    public class DummyPlayerInput : IPlayerInput
    {
        private readonly System.Random random;
        private CancellationTokenSource cancellationTokenSource;

        public DummyPlayerInput(int? seed = null)
        {
            random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        }

        public async UniTask<int> WaitForPlayerSelectionAsync(int[] availableChoices)
        {
            if (availableChoices == null || availableChoices.Length == 0)
                throw new ArgumentException("Available choices must not be null or empty", nameof(availableChoices));

            cancellationTokenSource = new CancellationTokenSource();
            
            try
            {
                var delay = random.Next(500, 3000);
                await UniTask.Delay(delay, cancellationToken: cancellationTokenSource.Token);
                
                var selectedChoice = availableChoices[random.Next(availableChoices.Length)];
                
                Debug.Log($"[DummyPlayerInput] Selected choice: {selectedChoice} from {string.Join(", ", availableChoices)}");
                
                return selectedChoice;
            }
            catch (OperationCanceledException)
            {
                Debug.Log("[DummyPlayerInput] Selection was cancelled");
                return availableChoices[0];
            }
            finally
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        public void Cancel()
        {
            cancellationTokenSource?.Cancel();
        }
    }
}