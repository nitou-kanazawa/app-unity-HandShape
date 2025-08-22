using Cysharp.Threading.Tasks;

namespace Project.InGame.Presentation
{
    public interface IIncorrectAnswerPresentation
    {
        UniTask PlayIncorrectAnimationAsync();
    }
}