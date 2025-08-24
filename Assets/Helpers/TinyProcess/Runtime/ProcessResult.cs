
namespace Nitou.TinyProcess
{
    /// <summary>
    /// 結果データの基底クラス．
    /// </summary>
    public abstract class ProcessResult {
        public bool IsSuccess => this is CompleteResult;
        public bool IsCanceled => this is CancelResult;
    }

    /// <summary>
    /// 正常に完了した時の結果データ．
    /// </summary>
    public class CompleteResult : ProcessResult { }

    /// <summary>
    /// キャンセル時の結果データ．
    /// </summary>
    public class CancelResult : ProcessResult { }
}
