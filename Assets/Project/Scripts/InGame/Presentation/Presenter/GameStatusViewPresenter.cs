using System;
using UnityEngine.UI;
using R3;
using Project.InGame.Presentation;
using Project.InGame.Game;

namespace Project
{
    public class GameStatusViewPresenter : IDisposable
    {
        private GameStatusView _view;
        private GameProcess _model;
        private CompositeDisposable _disposables = new ();

        public GameStatusViewPresenter(GameProcess model, GameStatusView view)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _view = view ?? throw new ArgumentNullException(nameof(view));
        }

        public void Initialize()
        {
            // バインド
            _model.SetCountRP
                .Subscribe(setCount => _view.SetCountText.text = $"{setCount}")
                .AddTo(_disposables);
            _model.ScoreRP
                .Subscribe(score => _view.ScoreText.text = $"{score}")
                .AddTo(_disposables);
        }


        public void Dispose()
        {
            _view = null;
            _model = null;
            _disposables.Dispose();
        }
    }
}
