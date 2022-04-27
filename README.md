# コーディングスタイルを見せるためのプロジェクト
## プロジェクトの説明
「蚊を殺すゲーム」の技術検証のために始めたプロジェクトで、今はコーディングスタイルを見せるプロジェクトです。
本番プロジェクトである「蚊を殺すゲーム」のプレイ動画はこちらを見てください。
- https://imgur.com/a/XhuC57r

## コーディングスタイルの特徴
1. パフォーマンスを意識して実装し、蚊の数が数千を超えても問題なく起動可能
2. 仕様の変更や追加に対応できるコードの実装
    1. オブジェクト指向を守り、変更や追加、テストなどに容易
    2. Scriptable Obejct Event アーキテクチャを使い、Dependencyを最小限に

## プロジェクトの構成
### 2.5Dの実装
1. 概要
    1. 2Dの平面を立体的に組み合わせることで2.5Dを演出
    2. 各2D平面はRoomの単位で表れている
    3. playerと蚊はそのRoomの中を移動したり、他のRoomに移動できる
    4. PlayerのRoom移動は「RoomChannel」のScriptableObjectでEventListener アーキテクチャで管理される。
2. 使用Script 説明
    1. Room 
        - 各Roomの大きさと隣接のRoom情報を保管する。
    2. RoomCameraController
        - 各Roomの動きを制御する
    3. RoomChannel
        - PlayerのRoom間の移動をEvent形式で伝える。
        - 拡張性と独立性を保つための SO Event Systemを使用
### 蚊の動きの実装
1. 概要
    1. 蚊が自由にRoomを移動したり、空間に出たりすることを制御する。
    2. 他の蚊をよける、天井から離れるなど、複数の行動を追加＆変更が容易にできるような設計
2. 使用 Script 説明
    1. MosqBrain 
        - 蚊の移動を制御する
        - 蚊が各Roomに移動する時にEventを発動
    2. MosqMovement 
        - 蚊の動きをまとめて実行する
        - すべてのIMosqDirectionをまとめて実行する
    3. IMosqDirection 
        - 細かい蚊の動きを実装するクラスにつける
    5. IRoomEventListener 
        - 蚊がRoomに移動するEventの管理
