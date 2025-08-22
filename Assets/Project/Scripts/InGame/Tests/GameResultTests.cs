using NUnit.Framework;
using Project.InGame.Game;

namespace Project.InGame.Tests
{
    public class GameResultTests
    {
        [Test]
        public void Constructor_WithValidParameters_SetsAllFields()
        {
            var result = new GameResult(100, 5, 2, 30.5f);

            Assert.AreEqual(100, result.finalScore);
            Assert.AreEqual(5, result.correctCount);
            Assert.AreEqual(2, result.incorrectCount);
            Assert.AreEqual(30.5f, result.playTime);
        }

        [Test]
        public void Constructor_WithZeroValues_SetsAllFields()
        {
            var result = new GameResult(0, 0, 0, 0f);

            Assert.AreEqual(0, result.finalScore);
            Assert.AreEqual(0, result.correctCount);
            Assert.AreEqual(0, result.incorrectCount);
            Assert.AreEqual(0f, result.playTime);
        }

        [Test]
        public void Constructor_WithNegativeValues_AllowsNegativeFields()
        {
            var result = new GameResult(-10, -1, -1, -5f);

            Assert.AreEqual(-10, result.finalScore);
            Assert.AreEqual(-1, result.correctCount);
            Assert.AreEqual(-1, result.incorrectCount);
            Assert.AreEqual(-5f, result.playTime);
        }
    }
}