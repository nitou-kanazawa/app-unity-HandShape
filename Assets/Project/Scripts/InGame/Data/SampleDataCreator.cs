#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Project.InGame.Data
{
    internal static class SampleDataCreator
    {
        [MenuItem("Project/InGame/Create Sample Data")]
        public static void CreateSampleData()
        {
            CreateHandSignDataSamples();
            CreateSampleInGameConfig();
            
            Debug.Log("Sample data created! Check Assets/Project/Resources/SampleData/");
        }

        private static void CreateHandSignDataSamples()
        {
            var folder = "Assets/Project/Resources/SampleData";
            if (!AssetDatabase.IsValidFolder(folder))
            {
                AssetDatabase.CreateFolder("Assets/Project/Resources", "SampleData");
            }

            // YourHand signs
            var rockYour = CreateHandSignData("Rock_Your", 1);
            var paperYour = CreateHandSignData("Paper_Your", 2);
            var scissorsYour = CreateHandSignData("Scissors_Your", 3);

            // MyHand signs
            var rockMy = CreateHandSignData("Rock_My", 101);
            var paperMy = CreateHandSignData("Paper_My", 102);
            var scissorsMy = CreateHandSignData("Scissors_My", 103);

            // Save HandSignData assets
            AssetDatabase.CreateAsset(rockYour, $"{folder}/RockYour.asset");
            AssetDatabase.CreateAsset(paperYour, $"{folder}/PaperYour.asset");
            AssetDatabase.CreateAsset(scissorsYour, $"{folder}/ScissorsYour.asset");
            AssetDatabase.CreateAsset(rockMy, $"{folder}/RockMy.asset");
            AssetDatabase.CreateAsset(paperMy, $"{folder}/PaperMy.asset");
            AssetDatabase.CreateAsset(scissorsMy, $"{folder}/ScissorsMy.asset");

            // Create HandPairData
            var rockPair = CreateHandPairData(rockYour, rockMy, 10);
            var paperPair = CreateHandPairData(paperYour, paperMy, 15);
            var scissorsPair = CreateHandPairData(scissorsYour, scissorsMy, 20);

            AssetDatabase.CreateAsset(rockPair, $"{folder}/RockPair.asset");
            AssetDatabase.CreateAsset(paperPair, $"{folder}/PaperPair.asset");
            AssetDatabase.CreateAsset(scissorsPair, $"{folder}/ScissorsPair.asset");

            AssetDatabase.SaveAssets();
        }

        private static void CreateSampleInGameConfig()
        {
            var config = ScriptableObject.CreateInstance<InGameConfigSO>();
            
            var folder = "Assets/Project/Resources/SampleData";
            var rockPair = UnityEditor.AssetDatabase.LoadAssetAtPath<HandPairDataSO>($"{folder}/RockPair.asset");
            var paperPair = UnityEditor.AssetDatabase.LoadAssetAtPath<HandPairDataSO>($"{folder}/PaperPair.asset");
            var scissorsPair = UnityEditor.AssetDatabase.LoadAssetAtPath<HandPairDataSO>($"{folder}/ScissorsPair.asset");

            // Use reflection to set private fields
            var configType = typeof(InGameConfigSO);
            configType.GetField("gameTimeLimit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(config, 40f);
            configType.GetField("questionTimeLimit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(config, 5f);
            configType.GetField("handPairs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(config, new HandPairDataSO[] { rockPair, paperPair, scissorsPair });

            UnityEditor.AssetDatabase.CreateAsset(config, $"{folder}/SampleInGameConfig.asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        private static HandSignDataSO CreateHandSignData(string name, int id)
        {
            var data = ScriptableObject.CreateInstance<HandSignDataSO>();
            var dataType = typeof(HandSignDataSO);
            
            dataType.GetField("signName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, name);
            dataType.GetField("signId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, id);
            dataType.GetField("signSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, null); // Will be null for demo

            return data;
        }

        private static HandPairDataSO CreateHandPairData(HandSignDataSO yourHand, HandSignDataSO myHand, int score)
        {
            var data = ScriptableObject.CreateInstance<HandPairDataSO>();
            var dataType = typeof(HandPairDataSO);
            
            dataType.GetField("yourHandSign", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, yourHand);
            dataType.GetField("myHandSign", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, myHand);
            dataType.GetField("baseScore", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, score);

            return data;
        }
    }
}
#endif
