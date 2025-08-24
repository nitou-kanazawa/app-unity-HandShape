using UnityEngine;
using TMPro;

namespace Project.InGame.Presentation
{
    public sealed class GameStatusView : ViewBase
    {
        [SerializeField] TextMeshProUGUI _timeText;
        [SerializeField] TextMeshProUGUI _setCountText;
        [SerializeField] TextMeshProUGUI _scoreText;

        public TextMeshProUGUI TimeText => _timeText;
        public TextMeshProUGUI SetCountText => _setCountText;
        public TextMeshProUGUI ScoreText => _scoreText;

        public override void Initialize()
        {
            _timeText.text = "";   
            _setCountText.text = "0";
            _scoreText.text = "0";
        }
    }
}
