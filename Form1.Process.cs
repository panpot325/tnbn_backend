using System;
using BackendMonitor.share;
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
            Timer1.Enabled = true;
            Command1.Enabled = true;
            return;
        }

        // *** Debug ***
        if (AppConfig.DebugMode) {
            //G.StreamWriteData("R", buff);
        }

        Log.Sub_LogWrite($@"GetData:{buff}");

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
    /// <param name="cmd"></param>
    /// <param name="pingCheck"></param>
    private void SendData(string cmd, bool pingCheck = true) {
        if (string.IsNullOrEmpty(cmd)) {
            return;
        }

        if (!pingCheck || G.PingCheck()) {
            Log.Sub_LogWrite($"SendData: {cmd}");
            _melsecPort.Send(cmd);

            //Debug Send Data
            if (AppConfig.DebugMode) {
                //G.StreamWriteData("W", cmd);
            }
        }
        else {
            SetText(@"単板ライン 接続処理実施中...", $"単板ライン{Environment.NewLine}続処理実施中...");
            Log.Sub_LogWrite(@"接続処理Msg設定 単板ライン 接続処理実施中...");

            Timer2.Enabled = true;
        }
    }
}