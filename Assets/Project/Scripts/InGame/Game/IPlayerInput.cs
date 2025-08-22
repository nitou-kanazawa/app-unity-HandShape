using System;
using Cysharp.Threading.Tasks;

namespace Project.InGame.Game
{
    public interface IPlayerInput
    {
        UniTask<int> WaitForPlayerSelectionAsync(int[] availableChoices);
        void Cancel();
    }
}