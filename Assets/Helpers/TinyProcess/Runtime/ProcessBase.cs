using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

#if USE_R3
using R3;
#else
using UniRx;
#endif

namespace Nitou.TinyProcess
{
    /// <summary>
    /// プロセスの基底クラス．
    /// </summary>
    public abstract partial class ProcessBase : IProcess
    {
        private ImtStateMachine<ProcessBase, StateEvent> _stateMachine;

        private readonly UniTaskCompletionSource<ProcessResult> _finishedSource = new();
        private ProcessResult _resultData = null;
        private IDisposable _disposable;

        /// <summary>
        /// 終了時の通知
        /// </summary>
        public UniTask<ProcessResult> ProcessFinished => _finishedSource.Task;


        /// ----------------------------------------------------------------------------
        #region Public Method

        protected ProcessBase()
        {
            _stateMachine = new ImtStateMachine<ProcessBase, StateEvent>(this);
            {
                // ポーズ
                _stateMachine.AddTransition<RunningState, PauseState>(StateEvent.Pause);
                _stateMachine.AddTransition<PauseState, RunningState>(StateEvent.UnPause);
                // 終了
                _stateMachine.AddTransition<RunningState, EndState>(StateEvent.Complete);
                _stateMachine.AddTransition<PauseState, EndState>(StateEvent.Complete);
                _stateMachine.AddTransition<RunningState, EndState>(StateEvent.Cancel);
                _stateMachine.AddTransition<PauseState, EndState>(StateEvent.Cancel);
            }
        }

        public virtual void Dispose()
        {
            _disposable?.Dispose();
            _disposable = null;
        }

        /// <inharted/>
        public void Run()
        {
            _stateMachine.SetStartState<RunningState>();
            _stateMachine.Update();

            // 更新処理
            _disposable = Observable.EveryUpdate().Subscribe(_ => _stateMachine.Update());
        }

        /// <inharted/>
        public void Pause() => _stateMachine.SendEvent(StateEvent.Pause);

        /// <inharted/>
        public void UnPause() => _stateMachine.SendEvent(StateEvent.UnPause);

        /// <inharted/>
        public void Cancel(CancelResult cancelResult)
        {
            _stateMachine.SendEvent(StateEvent.Cancel);

            // 結果データの格納
            _resultData = cancelResult ?? new CancelResult();
        }
        #endregion


        /// ----------------------------------------------------------------------------
        #region Protected Method

        protected virtual void OnStart() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnPause() { Debug.Log("Pause"); }
        protected virtual void OnUnPause() { Debug.Log("Un Pause"); }
        protected virtual void OnEnd() { }

        /// <summary>
        /// プロセス完了イベントの発火（※派生クラス用）
        /// </summary>
        protected void TriggerComplete(CompleteResult result)
        {
            _stateMachine.SendEvent(StateEvent.Complete);

            // 結果データの格納
            _resultData = result;
        }
        #endregion
    }
}
