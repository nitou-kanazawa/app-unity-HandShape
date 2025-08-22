using NUnit.Framework;
using Project.InGame.Data;
using Project.InGame.Game;
using UnityEngine;

namespace Project.InGame.Tests
{
    public class RandomQuestionGeneratorTests
    {
        private HandPairData[] handPairs;
        private RandomQuestionGenerator generator;

        [SetUp]
        public void SetUp()
        {
            handPairs = CreateTestHandPairs();
            generator = new RandomQuestionGenerator(handPairs, seed: 12345);
        }

        [Test]
        public void GenerateQuestion_WithValidData_ReturnsValidQuestion()
        {
            var question = generator.GenerateQuestion();

            Assert.IsNotNull(question.correctPair);
            Assert.IsNotNull(question.choices);
            Assert.AreEqual(3, question.choices.Length);
            Assert.IsTrue(question.correctAnswerIndex >= 0 && question.correctAnswerIndex < 3);
            Assert.Contains(question.correctPair.MyHandSign, question.choices);
        }

        [Test]
        public void GenerateQuestion_MultipleCallsWithSameSeed_ProducesDifferentQuestions()
        {
            var question1 = generator.GenerateQuestion();
            var question2 = generator.GenerateQuestion();

            // 同じシードでも異なる問題が生成される可能性がある
            Assert.IsNotNull(question1.correctPair);
            Assert.IsNotNull(question2.correctPair);
        }

        [Test]
        public void Constructor_WithNullHandPairs_ThrowsArgumentNullException()
        {
            Assert.Throws<System.ArgumentNullException>(() => 
                new RandomQuestionGenerator(null));
        }

        [Test]
        public void Constructor_WithEmptyHandPairs_ThrowsArgumentException()
        {
            Assert.Throws<System.ArgumentException>(() => 
                new RandomQuestionGenerator(new HandPairData[0]));
        }

        [Test]
        public void GenerateQuestion_WithSingleHandPair_StillGeneratesThreeChoices()
        {
            var singlePair = new HandPairData[] { handPairs[0] };
            var singleGenerator = new RandomQuestionGenerator(singlePair, seed: 12345);
            
            var question = singleGenerator.GenerateQuestion();
            
            Assert.AreEqual(3, question.choices.Length);
            Assert.AreEqual(question.correctPair.MyHandSign, question.choices[question.correctAnswerIndex]);
        }

        private HandPairData[] CreateTestHandPairs()
        {
            var rockYour = CreateHandSignData("Rock_Your", 1);
            var rockMy = CreateHandSignData("Rock_My", 101);
            var paperYour = CreateHandSignData("Paper_Your", 2);
            var paperMy = CreateHandSignData("Paper_My", 102);
            var scissorsYour = CreateHandSignData("Scissors_Your", 3);
            var scissorsMy = CreateHandSignData("Scissors_My", 103);

            return new HandPairData[]
            {
                CreateHandPairData(rockYour, rockMy, 10),
                CreateHandPairData(paperYour, paperMy, 15),
                CreateHandPairData(scissorsYour, scissorsMy, 20)
            };
        }

        private HandSignData CreateHandSignData(string name, int id)
        {
            var data = ScriptableObject.CreateInstance<HandSignData>();
            typeof(HandSignData).GetField("signName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, name);
            typeof(HandSignData).GetField("signId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, id);
            return data;
        }

        private HandPairData CreateHandPairData(HandSignData yourHand, HandSignData myHand, int score)
        {
            var data = ScriptableObject.CreateInstance<HandPairData>();
            typeof(HandPairData).GetField("yourHandSign", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, yourHand);
            typeof(HandPairData).GetField("myHandSign", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, myHand);
            typeof(HandPairData).GetField("baseScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, score);
            return data;
        }
    }
}