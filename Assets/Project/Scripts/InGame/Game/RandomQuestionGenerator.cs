using System;
using System.Collections.Generic;
using System.Linq;
using Project.InGame.Data;
using UnityEngine;

namespace Project.InGame.Game
{
    public class RandomQuestionGenerator : IQuestionGenerator
    {
        private readonly HandPairData[] handPairs;
        private readonly System.Random random;

        public RandomQuestionGenerator(HandPairData[] handPairs, int? seed = null)
        {
            this.handPairs = handPairs ?? throw new ArgumentNullException(nameof(handPairs));
            if (handPairs.Length == 0)
                throw new ArgumentException("Hand pairs must not be empty", nameof(handPairs));
                
            random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
        }

        public GameQuestion GenerateQuestion()
        {
            var correctPair = handPairs[random.Next(handPairs.Length)];
            var choices = GenerateChoices(correctPair);
            var correctAnswerIndex = Array.IndexOf(choices, correctPair.MyHandSign);
            
            return new GameQuestion(correctPair, choices, correctAnswerIndex);
        }

        private HandSignData[] GenerateChoices(HandPairData correctPair)
        {
            var allMyHandSigns = handPairs.Select(p => p.MyHandSign).Distinct().ToArray();
            var choices = new List<HandSignData> { correctPair.MyHandSign };
            
            var availableWrongChoices = allMyHandSigns.Where(sign => sign != correctPair.MyHandSign).ToList();
            
            while (choices.Count < 3 && availableWrongChoices.Count > 0)
            {
                var randomIndex = random.Next(availableWrongChoices.Count);
                choices.Add(availableWrongChoices[randomIndex]);
                availableWrongChoices.RemoveAt(randomIndex);
            }
            
            while (choices.Count < 3)
            {
                choices.Add(correctPair.MyHandSign);
            }
            
            return ShuffleArray(choices.ToArray());
        }

        private HandSignData[] ShuffleArray(HandSignData[] array)
        {
            var shuffled = new HandSignData[array.Length];
            Array.Copy(array, shuffled, array.Length);
            
            for (int i = shuffled.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
            }
            
            return shuffled;
        }
    }
}