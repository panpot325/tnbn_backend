using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

// ReSharper disable ConvertIfStatementToSwitchStatement
// ReSharper disable InvertIf

namespace BackendMonitor;

/// <summary>
/// Response
/// </summary>
public partial class Form1 {
    /// <summary>
    /// @読出ビット_レスポンス処理
    /// </summary>
    public void ReadBitResponse() {
        G.LogWrite(@"【読出ビット_レスポンス処理】");
        G.LogWrite($"受信Cmd.読込データ: {ResponseMessage.ReadData}");

        //監視用受信Cmd
        MonitorMessage.Set(_unit);

        //テスト用
        ReadBitResponseTest();
        //Timer1.Enabled = true;
        //Command1.Enabled = true;
        //return;
        
        switch (MonitorMessage.RequestBit) {
            //稼動終了
            case C.REQ_STP:
                Command1.Enabled = false;
                Process_Stop(); //稼動終了
                break;
            //稼動開始
            case C.REQ_STA:
                Command1.Enabled = false;
                WorkKeyRequest(); //稼動データキー取得
                break;
            //船番要求
            case C.REQ_SNO:
                Command1.Enabled = false;
                SnoIndexWrite();
                break;
            //ブロック要求、部材要求、データ要求、ミラー要求
            case C.REQ_BLK:
            case C.REQ_BZI:
            case C.REQ_DAT:
            case C.REQ_MIR:
                Command1.Enabled = false;
                DataKeyRequest(); //要求データキー取得
                break;
            //回転要求
            case C.REQ_SPI:
            case "00000000":
                Timer1.Enabled = true;
                Command1.Enabled = true;
                break;
        }
    }

    /// <summary>
    /// @書込ビット_レスポンス処理
    /// </summary>
    public void WriteBitResponse() {
        G.LogWrite(@"【書込ビット_レスポンス処理】");
        Timer1.Enabled = true;
    }

    /// <summary>
    /// @読出ワード_レスポンス処理
    /// </summary>
    public void ReadWordResponse() {
        G.LogWrite(@"【読出ワード_レスポンス処理】");
        KeyMessage.Set();

        RevertRecvKey(_unit); //要求データキーの戻し作業

        switch (MonitorMessage.RequestBit) {
            //稼動開始
            case C.REQ_STA:
                Process_Start(); //稼動開始
                break;
            //ブロック要求
            case C.REQ_BLK:
                BlkIndexWrite(); //ブロック名一覧書込
                break;
            //部材要求
            case C.REQ_BZI:
                BziIndexWrite(); //部材名一覧書込
                break;
            case C.REQ_DAT: //データ要求
            case C.REQ_MIR: //ミラー要求
                WorkDataWrite(); //加工ワークデータ書込 
                break;
        }
    }

    /// <summary>
    /// @書込ワード_レスポンス処理
    /// </summary>
    public void WriteWordResponse() {
        G.LogWrite(@"【書込ワード_レスポンス処理】");
        if (_finish) {
            switch (MonitorMessage.RequestBit) {
                case C.REQ_DAT:
                case C.REQ_MIR:
                case C.REQ_SPI:
                    CheckKey.Sno = RequestKey.Sno;
                    CheckKey.Blk = RequestKey.Blk;
                    CheckKey.Bzi = RequestKey.Bzi;
                    CheckKey.Pcs = RequestKey.Pcs;
                    _sendCnt++;
                    break;
            }

            SendData(MonitorMessage.RequestBit switch {
                C.REQ_SNO => SnoIndexDoneCmd(_unit), //船番一覧書込完了コマンド
                C.REQ_BLK => BlkIndexDoneCmd(_unit), //ブロック名一覧書込完了コマンド
                C.REQ_BZI => BziIndexDoneCmd(_unit), //部材名一覧書込完了コマンド
                C.REQ_DAT or C.REQ_MIR or C.REQ_MIR => RequestDataKeyCmd(_unit), //要求データキーの取得コマンド
                _ => ""
            });
        }
        else {
            SendData(MonitorMessage.RequestBit switch {
                C.REQ_SNO => SnoIndexWriteCmd(_unit), //船番一覧書込コマンド
                C.REQ_BLK => BlkIndexWriteCmd(_unit), //ブロック名一覧書込コマンド
                C.REQ_BZI => BziIndexWriteCmd(_unit), //部材名一覧書込コマンド
                C.REQ_DAT or C.REQ_MIR or C.REQ_SPI => WorkDataWriteCmd(_unit), //加工ワークデータ書込コマンド
                _ => ""
            });
        }
    }

    /// <summary>
    /// @書込ワード_レスポンス処理_クリア
    /// </summary>
    public void ClearWordResponse() {
        G.LogWrite(@"【書込ワード_レスポンス処理_クリア】");

        if (_gListClrState == 1) {
            SendData(SnoIndexClearCmd(_unit)); //船番一覧クリアコマンド
        }
        else if (_gListClrState == 2) {
            SendData(BlkIndexClearCmd(_unit)); //ブロック名一覧クリアコマンド
        }
        else {
            _clrFinish = true;
            Timer1.Enabled = true;
        }
    }
}