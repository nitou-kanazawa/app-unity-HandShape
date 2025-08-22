using UnityEngine;

namespace Project.InGame.Data
{
    [CreateAssetMenu(fileName = "HandPairData", menuName = "Project/InGame/HandPairData")]
    public class HandPairData : ScriptableObject
    {
        [SerializeField] private HandSignData yourHandSign;
        [SerializeField] private HandSignData myHandSign;
        [SerializeField] private int baseScore = 10;

        public HandSignData YourHandSign => yourHandSign;
        public HandSignData MyHandSign => myHandSign;
        public int BaseScore => baseScore;
    }
}