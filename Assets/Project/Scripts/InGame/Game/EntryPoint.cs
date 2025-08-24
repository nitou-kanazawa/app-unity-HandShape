using UnityEngine;
using Project.InGame.Data;
using Cysharp.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;   
using Project.InGame.Presentation;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.InputSystem;

namespace Project.InGame.Game
{
    public class EntryPoint : MonoBehaviour
    {
        // View
        [SerializeField] GameHudPage _gameHudPage;

        [SerializeField] TextMeshProUGUI _startText;

        // Config
        [SerializeField] InGameConfigSO _inGameConfigSO;

        // Data
        private HandSignContainer _handSignContainer;
        private HandSignPairContainer _handSignPairContainer;

        async void Start()
        {
            // Resources
            _handSignContainer = new HandSignContainer();
            _handSignContainer.Initialize();
            _handSignPairContainer = new HandSignPairContainer();
            _handSignPairContainer.Initialize();

            // 
            _startText.gameObject.SetActive(true);
            _startText.text = "Click to Start";

            await UniTask.WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);

            // 
            _startText.text = "Start Game";
            await UniTask.WaitForSeconds(1);

            _startText.gameObject.SetActive(false);

            // 
            var gameProcess = new GameProcess(
                config: _inGameConfigSO,
                handSignContainer: _handSignContainer,
                handSignPairContainer: _handSignPairContainer,
                _gameHudPage);


            Debug.Log("Start GameProcess");
            gameProcess.Run();
            var result = await gameProcess.ProcessFinished;
        }

    }
}
