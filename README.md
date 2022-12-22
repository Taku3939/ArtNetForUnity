![Unity version](https://img.shields.io/badge/Unity-2021.3.11f1-blue.svg)

# ArtNetReceiver

Unity 内で ArtNet でデータを簡易に受信と録画するためのプロジェクト

## Operation Check

qlc+, TouchDesigner, DasLight4 での動作確認は行いました

## Usage

ArtNetClient を任意なオブジェクトにアタッチし、Port を設定する（デフォルトで 6454）.

以下のようなコードを書く

```C# : データ受信のサンプルコード
public class Sample : MonoBehaviour
{
    [SerializeField] private ArtNetClient artNetClient;
    private void Start()
    {
        artNetClient.onDataReceived += EventHandler;
    }

    private void EventHandler(ArtNetData data)
    {
         if (data.OpCode == ArtNetOpCode.OpDmx)
         {
        	// ここにデータ受信時のプログラムを書く
	        data.Logger(); //Log出力用関数
         }
    }
}
```

## Recorder

ArtNetDataRecorder にパスを設定して, Unity を再生する。

録画開始は`R`、録画終了は`S`でできます。

## License

[MIT](LICENSE.md)ですがコメントくれたら作者は喜びます(_'ω'_)
