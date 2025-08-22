using UnityEngine;

namespace Project.InGame.Data
{
    [CreateAssetMenu(fileName = "InGameConfig", menuName = "Project/InGame/InGameConfig")]
    public class InGameConfig : ScriptableObject
    {
        [SerializeField] private float gameTimeLimit = 40f;
        [SerializeField] private float questionTimeLimit = 5f;
        [SerializeField] private HandPairData[] handPairs = new HandPairData[0];

        public float GameTimeLimit => gameTimeLimit;
        public float QuestionTimeLimit => questionTimeLimit;
        public HandPairData[] HandPairs => handPairs;
    }
}