using System.Collections.Generic;
using UnityEngine;

namespace Project.InGame.Data
{
    public sealed class HandSignContainer
    {
        private HandSignDataSO[] _handSigns;

        public IEnumerable<IHandSignData> HandSigns => _handSigns;


        /// <summary>
        /// 初期化処理 - Resourcesフォルダから <see cref="HandSignDataSO"/> を一式取得
        /// </summary>
        /// <param name="resourcePath">Resourcesフォルダ内のパス（例: "HandSigns"）</param>
        public void Initialize(string resourcePath = "HandSigns")
        {

            // Resourcesフォルダから HandSignDataSO を全て読み込み
            _handSigns = Resources.LoadAll<HandSignDataSO>(resourcePath);

            if (_handSigns == null || _handSigns.Length == 0)
            {
                Debug.LogWarning($"HandSignDataSO not found in Resources/{resourcePath}");
                return;
            }
        }
    }
}
