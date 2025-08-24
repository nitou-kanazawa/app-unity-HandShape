using Alchemy.Inspector;
using UnityEngine;

namespace Project.InGame.Data
{
    /// <summary>
    /// ハンドサインを表すデータクラス。
    /// </summary>
    [CreateAssetMenu(
        fileName = "HandSignData",
        menuName = "Project/InGame/HandSignData"
    )]
    public sealed class HandSignDataSO : ScriptableObject, IHandSignData
    {
        [SerializeField] private string _signName;
        [SerializeField] private int _signId;

        [Preview(60)]
        [SerializeField] private Sprite _signSprite;

        /// <inharted/>
        public string SignName => _signName;

        /// <inharted/>
        public Sprite SignSprite => _signSprite;

        /// <inharted/>
        public int SignId => _signId;
    }
}
