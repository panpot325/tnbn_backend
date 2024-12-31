using BackendMonitor.type;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor;

/// <summary>
/// Process
/// </summary>
public partial class Form1 {
    /// <summary>
    /// DataArrival
    /// </summary>
    private void DataArrival(string buff) {
        if (buff.Length <= 1) {
            ResponseMessage.Init();
            return;
        }

        // *** Debug ***
        G.StreamWriteData("R", buff);
        Log.Sub_LogWrite($@"GetData:{buff}");
        // Log.WriteLine(
        // @$"Unit: {_unit} ClrFinish: {_clrFinish} GListClrState: {_gListClrState} ItrnClrCnt: {_itrnClrCnt} Finish :{_finish} ItrnCnt :{_itrnCnt}");

        ResponseMessage.Set(buff);
        if (ResponseMessage.FinishCode != "00") {
            Timer1.Enabled = true;
            Command1.Enabled = true;
            return;
        }

        if (_clrFinish) {
            switch (ResponseMessage.Sh) {
                case C.RES_RD_B: //読出ビット_レスポンス処理
                    ReadBitResponse();
                    break;
                case C.RES_RD_W: //読出ワード_レスポンス処理
                    ReadWordResponse();
                    break;
                case C.RES_WR_B: //書込ビット_レスポンス処理
                    WriteBitResponse();
                    break;
                case C.RES_WR_W: //書込ワード_レスポンス処理
                    WriteWordResponse();
                    break;
            }
        }
        else if (ResponseMessage.Sh == C.RES_WR_W) {
            ClearWordResponse(); //書込ワード_レスポンス処理_クリア
        }
    }

    /// <summary>
    /// SendData
    /// </summary>
    private void SendData(string cmd, bool gCmd = true, bool pingCheck = true) {
        if (string.IsNullOrEmpty(cmd)) {
            return;
        }

        _gCmd = gCmd ? cmd : "";
        if (!pingCheck || G.PingCheck()) {
            Log.Sub_LogWrite($"SendData: {cmd}");
            _melsecPort.Send(cmd);

            //Debug Send Data
            G.StreamWriteData("W", cmd);
        }
        else {
            SetText(@"単板ライン 接続処理実施中...", "単板ライン\n続処理実施中...");
            Log.Sub_LogWrite(@"接続処理Msg設定 単板ライン 接続処理実施中...");

            Timer2.Enabled = true;
        }
    }
}