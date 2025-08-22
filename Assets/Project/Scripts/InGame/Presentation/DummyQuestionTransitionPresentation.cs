using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project.InGame.Presentation
{
    public class DummyQuestionTransitionPresentation : IQuestionTransitionPresentation
    {
        public async UniTask PlayQuestionTransitionAsync()
        {
            Debug.Log("[QuestionTransition] Next question coming up...");
            await UniTask.Delay(200);
        }
    }
}