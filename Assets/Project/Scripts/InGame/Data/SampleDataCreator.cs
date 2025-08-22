using UnityEngine;

namespace Project.InGame.Data
{
    public static class SampleDataCreator
    {
        [UnityEditor.MenuItem("Project/InGame/Create Sample Data")]
        public static void CreateSampleData()
        {
            CreateHandSignDataSamples();
            CreateSampleInGameConfig();
            
            Debug.Log("Sample data created! Check Assets/Project/Resources/SampleData/");
        }

        private static void CreateHandSignDataSamples()
        {
            var folder = "Assets/Project/Resources/SampleData";
            if (!UnityEditor.AssetDatabase.IsValidFolder(folder))
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets/Project/Resources", "SampleData");
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
            UnityEditor.AssetDatabase.CreateAsset(rockYour, $"{folder}/RockYour.asset");
            UnityEditor.AssetDatabase.CreateAsset(paperYour, $"{folder}/PaperYour.asset");
            UnityEditor.AssetDatabase.CreateAsset(scissorsYour, $"{folder}/ScissorsYour.asset");
            UnityEditor.AssetDatabase.CreateAsset(rockMy, $"{folder}/RockMy.asset");
            UnityEditor.AssetDatabase.CreateAsset(paperMy, $"{folder}/PaperMy.asset");
            UnityEditor.AssetDatabase.CreateAsset(scissorsMy, $"{folder}/ScissorsMy.asset");

            // Create HandPairData
            var rockPair = CreateHandPairData(rockYour, rockMy, 10);
            var paperPair = CreateHandPairData(paperYour, paperMy, 15);
            var scissorsPair = CreateHandPairData(scissorsYour, scissorsMy, 20);

            UnityEditor.AssetDatabase.CreateAsset(rockPair, $"{folder}/RockPair.asset");
            UnityEditor.AssetDatabase.CreateAsset(paperPair, $"{folder}/PaperPair.asset");
            UnityEditor.AssetDatabase.CreateAsset(scissorsPair, $"{folder}/ScissorsPair.asset");

            UnityEditor.AssetDatabase.SaveAssets();
        }

        private static void CreateSampleInGameConfig()
        {
            var config = ScriptableObject.CreateInstance<InGameConfig>();
            
            var folder = "Assets/Project/Resources/SampleData";
            var rockPair = UnityEditor.AssetDatabase.LoadAssetAtPath<HandPairData>($"{folder}/RockPair.asset");
            var paperPair = UnityEditor.AssetDatabase.LoadAssetAtPath<HandPairData>($"{folder}/PaperPair.asset");
            var scissorsPair = UnityEditor.AssetDatabase.LoadAssetAtPath<HandPairData>($"{folder}/ScissorsPair.asset");

            // Use reflection to set private fields
            var configType = typeof(InGameConfig);
            configType.GetField("gameTimeLimit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(config, 40f);
            configType.GetField("questionTimeLimit", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(config, 5f);
            configType.GetField("handPairs", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(config, new HandPairData[] { rockPair, paperPair, scissorsPair });

            UnityEditor.AssetDatabase.CreateAsset(config, $"{folder}/SampleInGameConfig.asset");
            UnityEditor.AssetDatabase.SaveAssets();
        }

        private static HandSignData CreateHandSignData(string name, int id)
        {
            var data = ScriptableObject.CreateInstance<HandSignData>();
            var dataType = typeof(HandSignData);
            
            dataType.GetField("signName", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, name);
            dataType.GetField("signId", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, id);
            dataType.GetField("signSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(data, null); // Will be null for demo

            return data;
        }

        private static HandPairData CreateHandPairData(HandSignData yourHand, HandSignData myHand, int score)
        {
            var data = ScriptableObject.CreateInstance<HandPairData>();
            var dataType = typeof(HandPairData);
            
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