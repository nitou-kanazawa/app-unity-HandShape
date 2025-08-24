using UnityEngine;

namespace Project.InGame.Data
{
    public interface IHandSignData
    {
        /// <summary>
        /// 名称．
        /// </summary>
        string SignName { get; }

        /// <summary>
        /// サムネイル画像．
        /// </summary>
        Sprite SignSprite { get; }

        /// <summary>
        /// 
        /// </summary>
        int SignId { get; }
    }


    public static class HandSignDataExtensions
    {
        public static bool IsSameSign(this IHandSignData a, IHandSignData b)
        {
            if (a == null || b == null) return false;
            return a.SignId == b.SignId;
        }
    }

}
