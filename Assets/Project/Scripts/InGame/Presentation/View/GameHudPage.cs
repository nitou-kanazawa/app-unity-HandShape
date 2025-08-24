using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Project.InGame.Presentation
{
    public sealed class GameHudPage : MonoBehaviour
    {
        [Header("View components")]
        [SerializeField] private HandSignView _masterHandView;
        [SerializeField] private HandSignView _playerHandView;
        [SerializeField] private HandSignChoiceSetView _handChoiceSetView;
        [SerializeField] private GameStatusView _gameStatusView;
        [SerializeField] private JudgeView _judgeView;

        public HandSignView MasterHandView => _masterHandView;
        public HandSignView PlayerHandView => _playerHandView;

        /// <summary>
        /// 
        /// </summary>
        public HandSignChoiceSetView HandChoiceSetView => _handChoiceSetView;

        /// <summary>
        /// ステータス表示用のView．
        /// </summary>
        public GameStatusView GameStatusView => _gameStatusView;

        public JudgeView JudgeView => _judgeView;

        [Header("Effects")]
        [SerializeField] private PlayableDirector _effectDirector;

        /// <summary>
        /// 
        /// </summary>
        public PlayableDirector EffectDirector => _effectDirector;


        /// ----------------------------------------------------------------------------
        #region Public Method

        public void Initialize()
        {
            _masterHandView.Initialize();
            _playerHandView.Initialize();
            _handChoiceSetView.Initialize();
            _gameStatusView.Initialize();
            _judgeView.Initialize();

            // 最初は非表示
            HandChoiceSetView.Hide();
            MasterHandView.ClearHandSign();
            PlayerHandView.ClearHandSign();
        }

        public UniTask PlayEffectAsync()
        {
            if (_effectDirector == null)
            {
                return UniTask.CompletedTask;
            }
            var tcs = new UniTaskCompletionSource();
            void OnStopped(PlayableDirector director)
            {
                director.stopped -= OnStopped;
                tcs.TrySetResult();
            }
            _effectDirector.time = 0;
            _effectDirector.stopped += OnStopped;
            _effectDirector.Play();
            return tcs.Task;
        }
        #endregion
    }
}
