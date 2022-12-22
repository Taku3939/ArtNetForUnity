![Unity version](https://img.shields.io/badge/Unity-2021.3.11f1-blue.svg)

# ArtNetReceiver

Unity 内で ArtNet でデータを簡易に受信と録画するためのプロジェクト

## Operation Check

qlc+, TouchDesigner, DasLight4 での動作確認は行いました

## Usage

ArtNetClient を任意なオブジェクトにアタッチし、Port を設定する（デフォルトで 6454）.

以下のようなコードを書く
#### 受信の場合
```C# : データ受信のサンプルコード
public class ReceiveSample : MonoBehaviour
{
    [SerializeField] private ArtNetReceiver artNetReceiver;
    private void Start()
    {
        artNetReceiver.onDataReceived += EventHandler;
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

#### 送信の場合
```C# : データ送信のサンプルコード
public class SendSample : MonoBehaviour
{
    [SerializeField] private ArtNetSender sender;
    [SerializeField] [Range(0, 255)] private int[] channels = new int[512];

    private void Update()
    {
        // OpCode等を変えたい場合は引数を好きなように変えて下さい
        sender.Send(channels); 
    }
}

```

## Sample

LightCheck.unityがサンプルシーンです。
Receiver, Senderともにローカルホストを設定されていますので、Unityを開始して、Faderのチャンネルを変更することで動作を確認できます

## Recorder

ArtNetDataRecorder にパスを設定して, Unity を再生する。

録画開始は`R`、録画終了は`S`でできます。

## License

[MIT](LICENSE.md)ですがコメントくれたら作者は喜びます(_'ω'_)
