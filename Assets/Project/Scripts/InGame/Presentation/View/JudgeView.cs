using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Project.InGame.Presentation
{
    public class JudgeView : ViewBase
    {
        [SerializeField] private TextMeshProUGUI _judgeText;


        public override void Initialize()
        {
            Hide();
        }

        public void SetJudgeText(string text)
        {
            _judgeText.text = text;
        }
    }
}
