using Cysharp.Threading.Tasks;
using Nitou.TinyProcess;
using Project.InGame.Data;
using Project.InGame.Presentation;
using R3;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Project.InGame.Game
{
    public sealed class GameProcess : ProcessBase
    {
        private CancellationTokenSource _cts;


        // Model
        private ReactiveProperty<int> _setCountRp = new(0);
        private ReactiveProperty<int> _scoreRp = new(0);


        /// ----------------------------------------------------------------------------
        // Property

        public IInGameConfig Config { get; }

        // Data Container
        public HandSignContainer HandSignContainer { get; }
        public HandSignPairContainer HandSignPairContainer { get; }

        // View
        public GameHudPage InGameUI { get; }

        /// <summary>
        /// 現在のセット数．
        /// </summary>
        public ReadOnlyReactiveProperty<int> SetCountRP => _setCountRp;

        /// <summary>
        /// 現在のスコア．
        /// </summary>
        public ReadOnlyReactiveProperty<int> ScoreRP => _scoreRp;


        /// ----------------------------------------------------------------------------
        // Public Method

        public GameProcess(
            IInGameConfig config,
            HandSignContainer handSignContainer,
            HandSignPairContainer handSignPairContainer,
            GameHudPage inGameUI)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            HandSignContainer = handSignContainer ?? throw new ArgumentNullException(nameof(handSignContainer));
            HandSignPairContainer = handSignPairContainer ?? throw new ArgumentNullException(nameof(handSignPairContainer));
            InGameUI = inGameUI ?? throw new ArgumentNullException(nameof(inGameUI));

            _cts = new CancellationTokenSource();
        }

        public override void Dispose()
        {
            base.Dispose();
            _setCountRp.Dispose();
            _scoreRp.Dispose();

            _cts?.Cancel();
        }


        /// ----------------------------------------------------------------------------
        #region Override Method

        protected override void OnStart()
        {
            // View初期化
            InGameUI.Initialize();

            // Presenter初期化
            var presenter = new GameStatusViewPresenter(this, InGameUI.GameStatusView);
            presenter.Initialize();
            presenter.AddTo(_cts.Token);

            // メインループ開始
            RunGameLoopAsync(_cts.Token).Forget();
        }

        protected override void OnPause()
        {
            Time.timeScale = 0.0f;
        }

        protected override void OnUnPause()
        {
            Time.timeScale = 1.0f;
        }

        #endregion


        /// ----------------------------------------------------------------------------
        // Private Method


        class SetData
        {
            public IHandSignData Master { get; set; }
            public IHandSignData Player { get; set; }
        }


        private async UniTask RunGameLoopAsync(CancellationToken cancellationToken)
        {
            Debug.Log("Game Loop Running...");

            while (!cancellationToken.IsCancellationRequested)
            {
                var setData = new SetData();

                // リセット処理（非表示化）
                await UniTask.WaitForSeconds(0.1f, cancellationToken: cancellationToken);
                _setCountRp.Value++;

                // 問題を更新
                var masterHand = HandSignContainer.HandSigns.RandomSelect(1).First();
                InGameUI.MasterHandView.SetHandSign(masterHand);
                setData.Master = masterHand;
                await UniTask.WaitForSeconds(0.5f, cancellationToken: cancellationToken);

                // プレイヤーの手札を更新
                var handSignChoices = HandSignContainer.HandSigns
                    .RandomSelect(3)
                    .ToArray();
                var thumbnails = handSignChoices
                    .Select(x => x.SignSprite)
                    .ToArray();
                InGameUI.HandChoiceSetView.SetChoices(thumbnails);
                InGameUI.HandChoiceSetView.Show();

                // プレイヤー入力待ち
                {
                    var selectedIndex = await InGameUI.HandChoiceSetView.WaitUntilCliced(3f, cancellationToken);

                    Debug.Log($"Player Selected Index: {selectedIndex}");

                    if (0 <= selectedIndex && selectedIndex < handSignChoices.Length)
                    {
                        setData.Player = handSignChoices[selectedIndex];
                        InGameUI.PlayerHandView.SetHandSign(setData.Player);
                        InGameUI.HandChoiceSetView.Show();
                    }
                }


                // 結果判定
                var targetPair = HandSignPairContainer.HandSignPairs
                    .FirstOrDefault(x => x.MasterHandSign.IsSameSign(setData.Master) && x.PlayerHandSign.IsSameSign(setData.Player));
                var isSuccess = targetPair != null;

                // 結果表示
                InGameUI.HandChoiceSetView.Hide();
                await UniTask.WaitForSeconds(1f, cancellationToken: cancellationToken);

                // 撮影エフェクト再生
                await InGameUI.PlayEffectAsync();

                if (isSuccess)
                {
                    Debug.Log($"Success! Score +{targetPair.BaseScore}");
                    _scoreRp.Value += targetPair.BaseScore;
                    InGameUI.JudgeView.SetJudgeText($"Good! <size=50>(+{targetPair.BaseScore})</size>");
                    InGameUI.JudgeView.Show();
                    await UniTask.WaitForSeconds(0.5f, cancellationToken: cancellationToken);
                }

                // 非表示化
                await UniTask.WaitForSeconds(0.3f, cancellationToken: cancellationToken);
                InGameUI.JudgeView.Hide();
                InGameUI.MasterHandView.ClearHandSign();
                InGameUI.PlayerHandView.ClearHandSign();



                await UniTask.WaitForSeconds(1f, cancellationToken: cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            TriggerComplete(CreateResultData());
        }

        private GameResult CreateResultData()
        {
            return new GameResult(10);
        }
    }
}
