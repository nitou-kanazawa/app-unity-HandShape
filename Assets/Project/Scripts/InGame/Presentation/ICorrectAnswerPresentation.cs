using Cysharp.Threading.Tasks;

namespace Project.InGame.Presentation
{
    public interface ICorrectAnswerPresentation
    {
        UniTask PlayCorrectAnimationAsync(int addedScore);
    }
}