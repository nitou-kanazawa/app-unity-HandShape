using Alchemy.Inspector;
using Cysharp.Threading.Tasks;
using R3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Project.InGame.Presentation
{
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class HandSignChoiceSetView : ViewBase
    {
        [AssetsOnly]
        [SerializeField] private HandSignChoiceButton _choiceButtonPrefab;

        // Ui Components
        [SerializeField] private Transform _buttonParentTransform;
        private List<HandSignChoiceButton> _choiceButtons;
        private CanvasGroup _canvasGroup;

        private static readonly int ChoiceButtonCount = 3;

        public IReadOnlyList<HandSignChoiceButton> ChoiceButtons => _choiceButtons;


        #region Public Method

        public override void Initialize()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            CreateChoiceButtons();
        }

        public void SetChoices(IReadOnlyList<Sprite> thumbnails)
        {
            for (int i = 0; i < ChoiceButtonCount; i++)
            {
                if (i < thumbnails.Count)
                {
                    _choiceButtons[i].SetThumbnail(thumbnails[i]);
                }
                else
                {
                    _choiceButtons[i].SetThumbnail(null);
                }
            }
        }

        public void ResetThumnailes()
        {
            foreach (var button in _choiceButtons)
            {
                button.SetThumbnail(null);
            }
        }

        public async UniTask<int> WaitUntilCliced(float timeoutSeconds, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // タイムアウト
            timeoutSeconds = Mathf.Max(timeoutSeconds, 0.1f);
            cts.CancelAfter(TimeSpan.FromSeconds(timeoutSeconds));

            try
            {
                // ボタンのクリックイベント
                var inpurObservable = _choiceButtons
                    .Select((button, index) => button.OnClickAsObservable.Select(_ => index))
                    .Merge();

                // 入力待機
                var result = await inpurObservable.FirstAsync(cancellationToken: cts.Token);
                return result;
            }
            catch(OperationCanceledException )
            {
                return -1;
            }
        }

        #endregion


        #region Private Method

        private void CreateChoiceButtons()
        {
            ClearButtons();

            _choiceButtons = new List<HandSignChoiceButton>(ChoiceButtonCount);

            for (int i = 0; i < ChoiceButtonCount; i++)
            {
                var button = Instantiate(_choiceButtonPrefab, _buttonParentTransform);
                button.name = $"HandSignChoiceButton_{i}";
                _choiceButtons.Add(button);
            }
        }

        private void ClearButtons()
        {
            if (_choiceButtons != null)
            {
                foreach (var button in _choiceButtons)
                {
                    if (button != null)
                    {
                        Destroy(button.gameObject);
                    }
                }
                _choiceButtons.Clear();
            }
        }
        #endregion
    }

}
