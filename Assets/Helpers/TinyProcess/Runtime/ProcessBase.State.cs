
namespace Nitou.TinyProcess
{
    /// <summary>
    /// プロセスの基底クラス．
    /// </summary>
    public abstract partial class ProcessBase : IProcess
    {
        #region Inner Definition

        private enum StateEvent
        {
            // ポーズ
            Pause,
            UnPause,
            // 終了
            Complete,
            Cancel,
        }

        private abstract class StateBase : ImtStateMachine<ProcessBase, StateEvent>.State { }

        /// <summary>
        /// 実行ステート
        /// </summary>
        private sealed class RunningState : StateBase
        {
            private bool isFirstEnter = true;

            protected internal override void Enter()
            {
                if (isFirstEnter)
                {
                    Context.OnStart();
                    isFirstEnter = false;
                }
            }
            protected internal override void Update() => Context.OnUpdate();
        }

        /// <summary>
        /// ポーズステート
        /// </summary>
        private sealed class PauseState : StateBase
        {
            protected internal override void Enter()
            {
                Context.OnPause();
            }
            protected internal override void Exit()
            {
                Context.OnUnPause();
            }
        }

        /// <summary>
        /// 終了ステート
        /// </summary>
        private sealed class EndState : StateBase
        {
            protected internal override void Enter()
            {
                Context.OnEnd();
                Context._finishedSource.TrySetResult(Context._resultData);
            }
        }
        #endregion
    }
}
