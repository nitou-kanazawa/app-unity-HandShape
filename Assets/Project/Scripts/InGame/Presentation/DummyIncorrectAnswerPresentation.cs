using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.InGame.Presentation
{
    public class DummyIncorrectAnswerPresentation : IIncorrectAnswerPresentation
    {
        public async UniTask PlayIncorrectAnimationAsync()
        {
            Debug.Log("[IncorrectAnswer] Wrong answer! Try again!");
            await UniTask.Delay(100);
        }
    }
}