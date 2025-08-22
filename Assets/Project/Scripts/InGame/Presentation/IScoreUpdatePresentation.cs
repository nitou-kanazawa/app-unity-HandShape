using Cysharp.Threading.Tasks;

namespace Project.InGame.Presentation
{
    public interface IScoreUpdatePresentation
    {
        UniTask PlayScoreUpdateAsync(int newScore);
    }
}