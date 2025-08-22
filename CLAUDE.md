# Unity Hand Matching Game - 開発仕様書

## プロジェクト概要
YourHand（ゲーム側）の形にあわせて、MyHand（プレイヤー側）が選択肢から対応する形を選ぶマッチングゲーム。
制限時間内にできるだけ高いスコアを獲得することを目指す。

## 技術要件

### 開発環境
- **Unity Version**: Unity 6000.2.0f1
- **必要パッケージ**: 
  - UniTask
  - R3
- **Git管理**: 
  - メインブランチ: `main`
  - 作業ブランチ: `feature`
  - PR形式で開発進行

### フォルダ構成
```
Assets/Project
├── Scripts/InGame/
│   ├── Game/           # ゲームロジック
│   ├── Data/           # ScriptableObject
│   └── Presentation/   # 演出インターフェース
└── ...
```

## ゲーム仕様

### 基本ルール
1. YourHand（ゲーム側）が表示する形を確認
2. MyHand（プレイヤー側）が3つの選択肢から対応する形をタップ/クリックで選択
3. 正解時にスコア加算、プレイヤーが回答するか5秒経過で次の問題へ
4. 制限時間（40秒）内でのスコア最大化を目指す

### 画面構成
- **画面左部**: YourHand（相手の手の形）表示エリア
- **画面下央**: MyHand選択肢（3つのボタン）
- **画面上部**: 現在のスコアと残り時間表示

### ゲーム進行
- **制限時間**: 40秒（InGameConfigで管理）
- **問題切り替え**: プレイヤー回答後すぐ、または5秒経過で自動切り替え
- **タイムアウト処理**: 5秒経過時は不正解扱いで次の問題へ

## データ設計

### ScriptableObject構成

#### HandSignData
```csharp
[CreateAssetMenu]
public class HandSignData : ScriptableObject
{
    public string signName;
    public Sprite signSprite;
    public int signId;
}
```

#### HandPairData  
```csharp
[CreateAssetMenu]
public class HandPairData : ScriptableObject
{
    public HandSignData yourHandSign;    // ゲーム側の形
    public HandSignData myHandSign;      // プレイヤー側の対応する形
    public int baseScore;                // 基本スコア
}
```

#### InGameConfig
```csharp
[CreateAssetMenu]
public class InGameConfig : ScriptableObject
{
    public float gameTimeLimit = 40f;           // ゲーム制限時間
    public float questionTimeLimit = 5f;        // 問題制限時間
    public HandPairData[] handPairs;            // 使用する手のペア配列
}
```

## システム設計

### メインゲームクラス
```csharp
public class HandMatchingGameManager
{
    public async UniTask<GameResult> StartGameAsync(InGameConfig config)
    {
        // ゲーム実行ロジック
        // GameResultを返す非同期処理
    }
}
```

### GameResult
```csharp
public struct GameResult
{
    public int finalScore;        // 最終スコア
    public int correctCount;      // 正解数
    public int incorrectCount;    // 不正解数
    public float playTime;        // プレイ時間
}
```

### 演出インターフェース設計
ゲームロジックと演出を分離するため、以下のインターフェースを定義：

```csharp
// 正解時演出
public interface ICorrectAnswerPresentation
{
    UniTask PlayCorrectAnimationAsync(int addedScore);
}

// 不正解時演出
public interface IIncorrectAnswerPresentation  
{
    UniTask PlayIncorrectAnimationAsync();
}

// スコア更新演出
public interface IScoreUpdatePresentation
{
    UniTask PlayScoreUpdateAsync(int newScore);
}

// 問題切り替え演出
public interface IQuestionTransitionPresentation
{
    UniTask PlayQuestionTransitionAsync();
}
```

### イベント通知システム（UniRx使用）
演出完了を待たないイベント通知用：

```csharp
public static class GameEvents
{
    public static readonly Subject<int> OnScoreChanged = new();
    public static readonly Subject<bool> OnAnswerSubmitted = new(); // true: correct, false: incorrect
    public static readonly Subject<float> OnTimeUpdated = new();
    public static readonly Subject<Unit> OnGameStarted = new();
    public static readonly Subject<GameResult> OnGameEnded = new();
}
```

## 実装方針

### MVP（最小実行可能プロダクト）範囲
- ✅ 基本的なマッチングゲーム機能
- ✅ スコアシステム（基本スコアのみ）
- ✅ 制限時間システム
- ✅ 演出インターフェース（ダミー実装）
- ❌ 連続ボーナス（今回は実装しない）
- ❌ 追加評価システム（今回は実装しない）

### ダミー演出実装
各演出インターフェースに対してダミー実装クラスを提供：
```csharp
public class DummyCorrectAnswerPresentation : ICorrectAnswerPresentation
{
    public async UniTask PlayCorrectAnimationAsync(int addedScore)
    {
        Debug.Log($"Correct! +{addedScore} points");
        await UniTask.Delay(100); // 最小限の待機
    }
}
```

### 拡張性への配慮
- ScriptableObjectによる柔軟なデータ管理
- 演出インターフェースによる機能分離
- UniTaskによる非同期処理
- R3によるイベント管理
- 設定値の一元管理（InGameConfig）

## 開発タスク

### Phase 1: コア実装
1. データ構造定義（ScriptableObject）
2. ゲームロジック実装（HandMatchingGameManager）
3. UI基盤実装
4. 演出インターフェース定義とダミー実装

### Phase 2: 統合とテスト
1. 全体統合テスト
2. パフォーマンス確認
3. バグ修正

### 注意事項
- 演出部分は後から追加実装予定のため、演出完了を待つ設計を重視
- UniTaskとR3を積極活用
- 拡張性を考慮した設計
- コードの可読性と保守性を重視