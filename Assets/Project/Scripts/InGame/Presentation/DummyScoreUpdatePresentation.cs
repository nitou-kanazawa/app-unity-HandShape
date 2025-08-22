using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.InGame.Presentation
{
    public class DummyScoreUpdatePresentation : IScoreUpdatePresentation
    {
        public async UniTask PlayScoreUpdateAsync(int newScore)
        {
            Debug.Log($"[ScoreUpdate] Current Score: {newScore}");
            await UniTask.Delay(50);
        }
    }
}