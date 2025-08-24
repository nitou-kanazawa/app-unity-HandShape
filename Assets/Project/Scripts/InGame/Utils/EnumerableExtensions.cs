using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.InGame
{
    internal static class EnumerableExtensions
    {
        private static readonly Random DefaultRandom = new Random();

        /// <summary>
        /// IEnumerable<T>からランダムで指定回数抽選を行う（重複あり）
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">抽選元のコレクション</param>
        /// <param name="count">抽選回数</param>
        /// <param name="random">ランダム生成器（省略可）</param>
        /// <returns>抽選結果</returns>
        public static IEnumerable<T> RandomSelect<T>(this IEnumerable<T> source, int count, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            var list = source.ToList();
            if (list.Count == 0) yield break;

            var rng = random ?? DefaultRandom;

            for (int i = 0; i < count; i++)
            {
                yield return list[rng.Next(list.Count)];
            }
        }

        /// <summary>
        /// IEnumerable<T>からランダムで指定回数抽選を行う（重複なし）
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">抽選元のコレクション</param>
        /// <param name="count">抽選回数</param>
        /// <param name="random">ランダム生成器（省略可）</param>
        /// <returns>抽選結果</returns>
        public static IEnumerable<T> RandomSelectDistinct<T>(this IEnumerable<T> source, int count, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            var list = source.ToList();
            if (list.Count == 0) yield break;

            var actualCount = Math.Min(count, list.Count);
            var rng = random ?? DefaultRandom;

            // Fisher-Yates シャッフルアルゴリズムを使用
            for (int i = 0; i < actualCount; i++)
            {
                int j = rng.Next(i, list.Count);
                // リストの要素を交換
                (list[i], list[j]) = (list[j], list[i]);
                yield return list[i];
            }
        }

        /// <summary>
        /// 重み付きランダム抽選（重複あり）
        /// </summary>
        /// <typeparam name="T">要素の型</typeparam>
        /// <param name="source">抽選元のコレクション</param>
        /// <param name="weightSelector">重み選択関数</param>
        /// <param name="count">抽選回数</param>
        /// <param name="random">ランダム生成器（省略可）</param>
        /// <returns>抽選結果</returns>
        public static IEnumerable<T> RandomSelectWeighted<T>(this IEnumerable<T> source,
            Func<T, double> weightSelector, int count, Random random = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (weightSelector == null) throw new ArgumentNullException(nameof(weightSelector));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));

            var items = source.Select(x => new { Item = x, Weight = weightSelector(x) })
                              .Where(x => x.Weight > 0)
                              .ToList();

            if (items.Count == 0) yield break;

            var totalWeight = items.Sum(x => x.Weight);
            var rng = random ?? DefaultRandom;

            for (int i = 0; i < count; i++)
            {
                var randomValue = rng.NextDouble() * totalWeight;
                var currentWeight = 0.0;

                foreach (var item in items)
                {
                    currentWeight += item.Weight;
                    if (randomValue <= currentWeight)
                    {
                        yield return item.Item;
                        break;
                    }
                }
            }
        }
    }
}
