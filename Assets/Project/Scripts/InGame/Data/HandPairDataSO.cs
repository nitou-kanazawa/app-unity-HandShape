using Alchemy.Inspector;
using UnityEngine;

namespace Project.InGame.Data
{
    /// <summary>
    /// 両手のハンドサインのペアを表すデータクラス。
    /// </summary>
    [CreateAssetMenu(
        fileName = "HandPairData",
        menuName = "Project/InGame/HandPairData"
    )]
    public sealed class HandPairDataSO : ScriptableObject, IHandPairData
    {
        [SerializeField] private HandSignDataSO _masterHandSign;
        [SerializeField] private HandSignDataSO _playerHandSign;

        [SerializeField] private int _baseScore = 10;

        public IHandSignData MasterHandSign => _masterHandSign;
        public IHandSignData PlayerHandSign => _playerHandSign;
        public int BaseScore => _baseScore;
    }
}
