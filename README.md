# SiriBal
Serious Balloon

## ビルドターゲット

buildフォルダをビルド成果物の保存先にすると、gitコミット対象からは外してくれるようにしています。
buildフォルダを個人で作ってね

## コンポーネント

Firebase Auth と Analytics を入れたので、 Xcode で iOS ビルドするなら Cocoapods が必要です。

```sh
## cocoapods インストール
sudo gem install cocoapods

## cocoapods セットアップ
pod setup
```
とコマンドを叩いておいてください