using System;
using BackendMonitor.Properties;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor;

/// <summary>
/// Timer
/// </summary>
public partial class Form1 {
    /// <summary>
    /// Timer2_Timer
    /// </summary>
    private void Timer2_Timer() {
        G.LogWrite("【Timer2_Timer】");

        if (!G.IsUsableTime(Settings.Default.End_Time)) {
            G.LogWrite($"処理終了時間の為、プログラムを終了:{DateTime.Now:HH:mm:ss}");
            Close();
        }

        _unit = 1;
        Timer1.Enabled = false;
        Timer2.Enabled = false;

        SetText($@"{Settings.Default.Prg_Ver} 単板ライン 接続処理実施中...", @"単板ラインに接続処理開始");
        G.LogWrite(
            $"Winsock Protocol:{KanshiSettei.Protocol} " +
            $"RemoteHost:{KanshiSettei.RemoteHost} " +
            $"RemotePort:{KanshiSettei.RemotePort} " +
            $"W_LocalPort:{KanshiSettei.LocalPort}"
        );

        //Ethernetインタフェースユニットと接続
        if (!_melsecPort.Start()) {
            Timer2.Enabled = true;
            Command1.Enabled = true;
            return;
        }

        Timer1.Enabled = true;
    }

    /// <summary>
    /// Timer1_Timer
    /// </summary>
    private void Timer1_Timer() {
        G.LogWrite("【Timer1_Timer】");

        string cmd;
        if (_clrFinish) {
            SetText($@"{Settings.Default.Prg_Ver} 単板ライン 監視中...", $@"単板ライン 監視中...  装置No:{_unit}");
            G.LogWrite(@"Timer1 単板ライン 監視中...");
            Command1.Enabled = true;

            _unit = _unit switch {
                C.UNIT_2 => C.UNIT_3,
                C.UNIT_3 => C.UNIT_5,
                C.UNIT_5 => C.UNIT_2,
                _ => C.UNIT_2
            };
            cmd = RecvBitCmd(_unit);
            Console.WriteLine(@$"【読出ビットコマンド】:{_unit}】 :{cmd}");
            _gCmd = "";
        }
        else {
            SetText(@"単板ライン 一覧ｸﾘｱを開始します...", @"単板ライン 一覧ｸﾘｱを開始します...");
            G.LogWrite(@"Timer1 単板ライン 一覧ｸﾘｱを開始します...");

            _itrnClrCnt = C.SNO_MAX;
            _unit = C.UNIT_2;
            
            //船番一覧クリアコマンド(装置No)
            cmd = SnoIndexClearCmd(_unit);
            _gCmd = cmd;
        }

        if (string.IsNullOrEmpty(cmd)) {
            return;
        }

        Timer1.Enabled = false;
        SendData(cmd);
    }
}