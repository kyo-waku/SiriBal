# SiriBal
Serious Balloon

## ビルドターゲット

buildフォルダをビルド成果物の保存先にすると、gitコミット対象からは外してくれるようにしています。
buildフォルダを個人で作ってね

## Firebaseのビルド

このアプリケーションでは User Authentication のために、 Google Firebase Authentication を使っています。
Firebase を使用したアプリケーションをビルドする場合、以下の点に注意してください。

- APIKeyを適切に配置する（コミットしない）
    Google Firebase は BaaS (Backend as a Service) です。このような Web サービスには接続用 API Key が必要です。しかし、 API Key をむやみに公開すると、その API Key を使って大量のアクセスを受けた際に、所有者のアカウントに対して多額の費用が請求されることがあります。そのため、 API Key を含むファイルは厳重に管理され、 GitHub のような Public アクセス可能なところにおいてはいけません。API Key の管理者から適切に譲り受け、正しく取り扱いましょう。


- Cocoapodsをインストールする
    Firebase は Xcode ビルド時にのパッケージマネージャーとして Cocoapods を使用しています。事前にインストールしてください。Macの場合、はじめから Ruby はインストールされていますので、いきなり以下のコマンドから初めて大丈夫です。心配な方は gem が入っていることを確認してください。 `gem --version` で確認可能です。
    ```shell
    $$ sudo gem install -n usr/local/bin cocoapods
    ```
    これで Cocoapods のインストールが完了します。続いて、
    ```shell
    $$ pod setup
    ```
    として、 Cocoapods の環境をセットアップしてください。これにて完了です。