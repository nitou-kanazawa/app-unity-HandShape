using System;
using NUnit.Framework;
using Cysharp.Threading.Tasks;
using Project.InGame.Game;

namespace Project.InGame.Tests
{
    public class DummyPlayerInputTests
    {
        private DummyPlayerInput dummyInput;

        [SetUp]
        public void SetUp()
        {
            dummyInput = new DummyPlayerInput(seed: 12345);
        }

        [Test]
        public async void WaitForPlayerSelectionAsync_WithValidChoices_ReturnsValidChoice()
        {
            var choices = new int[] { 1, 2, 3 };
            
            var selectedChoice = await dummyInput.WaitForPlayerSelectionAsync(choices);
            
            Assert.Contains(selectedChoice, choices);
        }

        [Test]
        public void WaitForPlayerSelectionAsync_WithNullChoices_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await dummyInput.WaitForPlayerSelectionAsync(null));
        }

        [Test]
        public void WaitForPlayerSelectionAsync_WithEmptyChoices_ThrowsArgumentException()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => 
                await dummyInput.WaitForPlayerSelectionAsync(new int[0]));
        }

        [Test]
        public async void WaitForPlayerSelectionAsync_WithSingleChoice_ReturnsThatChoice()
        {
            var choices = new int[] { 42 };
            
            var selectedChoice = await dummyInput.WaitForPlayerSelectionAsync(choices);
            
            Assert.AreEqual(42, selectedChoice);
        }

        [Test]
        public async void Cancel_DuringWait_ReturnsFirstChoice()
        {
            var choices = new int[] { 10, 20, 30 };
            
            var task = dummyInput.WaitForPlayerSelectionAsync(choices);
            dummyInput.Cancel();
            var result = await task;
            
            Assert.AreEqual(10, result);
        }

        [TearDown]
        public void TearDown()
        {
            dummyInput?.Cancel();
        }
    }
}