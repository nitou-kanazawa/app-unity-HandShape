using UnityEngine;

namespace Project.InGame.Data
{
    [CreateAssetMenu(
        fileName = "InGameConfig",
        menuName = "Project/InGame/InGameConfig"
    )]
    public sealed class InGameConfigSO : ScriptableObject, IInGameConfig
    {
        [SerializeField] private float _gameTimeLimit = 40f;
        [SerializeField] private float _questionTimeLimit = 5f;
        [SerializeField] private HandPairDataSO[] _handPairs = new HandPairDataSO[0];

        public float GameTimeLimit => _gameTimeLimit;
        public float QuestionTimeLimit => _questionTimeLimit;
        public HandPairDataSO[] HandPairs => _handPairs;
    }

    public interface IInGameConfig
    {
        float GameTimeLimit { get; }
        float QuestionTimeLimit { get; }
        HandPairDataSO[] HandPairs { get; }
    }
}
