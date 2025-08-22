using Project.InGame.Data;
using UnityEngine;

namespace Project.InGame.Game
{
    public static class GameFactory
    {
        public static HandMatchingGameManager CreateGame(InGameConfig config, IPlayerInput playerInput = null, int? seed = null)
        {
            if (config == null)
                throw new System.ArgumentNullException(nameof(config));
            
            if (config.HandPairs == null || config.HandPairs.Length == 0)
                throw new System.ArgumentException("InGameConfig must have at least one hand pair", nameof(config));

            var input = playerInput ?? new DummyPlayerInput(seed);
            var questionGenerator = new RandomQuestionGenerator(config.HandPairs, seed);
            
            return new HandMatchingGameManager(input, questionGenerator, config);
        }
        
        public static HandMatchingGameManager CreateDummyGame(InGameConfig config, int? seed = null)
        {
            return CreateGame(config, new DummyPlayerInput(seed), seed);
        }
    }
}