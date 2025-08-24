using System.Collections.Generic;
using UnityEngine;

namespace Project.InGame.Data
{
    public sealed class HandSignPairContainer
    {
        private HandPairDataSO[] _handSignPairs;

        public IEnumerable<IHandPairData> HandSignPairs => _handSignPairs;


        /// <summary>
        /// 初期化処理 - Resourcesフォルダから <see cref="HandPairDataSO"/> を一式取得
        /// </summary>
        public void Initialize(string resourcePath = "HandSignPairs")
        {

            // Resourcesフォルダから HandSignDataSO を全て読み込み
            _handSignPairs = Resources.LoadAll<HandPairDataSO>(resourcePath);

            if (_handSignPairs == null || _handSignPairs.Length == 0)
            {
                Debug.LogWarning($"HandPairDataSO not found in Resources/{resourcePath}");
                return;
            }
        }

    }
}
