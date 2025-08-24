using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace Project
{
    public class TitleSceneEntryPoint : MonoBehaviour
    {
        [SerializeField] Button _startButton;
        [SerializeField] TextMeshProUGUI _titleText;


        async void Start()
        {
            await UniTask.WaitForSeconds(0.5f);

            _titleText.enabled = true;
            await _startButton.OnClickAsync();

            Debug.Log("Start Button Clicked");
            SceneManager.LoadScene(SceneType.Game.ToString());
        }
    }

}
