using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.InGame.Presentation
{
    public class DummyCorrectAnswerPresentation : ICorrectAnswerPresentation
    {
        public async UniTask PlayCorrectAnimationAsync(int addedScore)
        {
            Debug.Log($"[CorrectAnswer] +{addedScore} points! Great job!");
            await UniTask.Delay(100);
        }
    }
}