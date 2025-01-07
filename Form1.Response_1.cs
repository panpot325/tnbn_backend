using System;
using BackendMonitor.type;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor;

/// <summary>
/// Response 1
/// </summary>
public partial class Form1 {
    /// <summary>
    /// @稼動終了
    /// </summary>
    public void Process_Stop() {
        var unit = _unit;
        var workState = WorkStates.List[unit];

        workState.End_Count++;
        workState.EndTime = DateTime.Now.ToString("HH:mm:ss");

        workState.Update_End(); //稼動実績WKを更新終了
        ReadLogFile(_unit); //ログファイル読込
        WriteLogFile(_unit); //ログファイル出力

        workState.Start_Count = 0;
        workState.EndTime = "00:00:00";

        Timer1.Enabled = true;
    }

    /// <summary>
    /// @稼動開始
    /// </summary>
    public void Process_Start() {
        Log.WriteLine(@"稼動開始");
        //WorkStates.List[_unit].Fetch_WorkData(); // 加工ワークデータから取得
        //WorkStates.List[0].Fetch_Record(_unit); // 稼動実績WKから取得
        //var recState = WorkStates.List[0]; // 稼動実績WKから取得

        var workState = WorkStates.List[_unit].Fetch_WorkData(); // 加工ワークデータから取得
        var recState = WorkState.Fetch(_unit); // 稼動実績WKから取得

        if (workState.YMD == recState.YMD
            && workState.SNO.Trim() == recState.SNO.Trim()
            && workState.BLK.Trim() == recState.BLK.Trim()
            && workState.BZI.Trim() == recState.BZI.Trim()
            && workState.PCS.Trim() == recState.PCS.Trim()
           ) {
            workState.StrTime = recState.StrTime;
            workState.CNT = recState.CNT;
            workState.KAD_TIME = recState.KAD_TIME;

            if (recState.EndTime != "00:00:00") {
                workState.Update_StrTime2(); //稼動実績WKを更新
            }
            else {
                workState.StrTime2 = recState.StrTime2;
            }
        }
        else {
            WorkState.Update_Clear(_unit); //稼動実績WKをクリア
            workState.Update_Init(); //稼動実績WKを初期化
        }

        Timer1.Enabled = true;
    }

    /// <summary>
    /// @要求データキーの戻し作業
    /// </summary>
    /// <param name="unit"></param>
    public void RevertRecvKey(int unit) {
        Log.Sub_LogWrite($@"【要求データキーの戻し作業】受信Cmd.読込データ:{ResponseMessage.ReadData}】");

        var ascString = G.AscToString(ResponseMessage.ReadData);
        Log.Sub_LogWrite(ascString);

        var reverseString = G.ReverseString(ascString, 32);
        Log.Sub_LogWrite("変換した文字を2文字ずつ配列に代入し");
        Log.Sub_LogWrite($@"配列単位で文字を並び替え正しい文字列に並びかえ後: {reverseString}");


        if (MonitorMessage.RequestBit == C.REQ_STA) {
            WorkStates.List[unit].Set(
                G.Mid(reverseString, 1, 6),
                G.Mid(reverseString, 7, 8),
                G.Mid(reverseString, 15, 16),
                G.Mid(reverseString, 31, 2)
            );
            Log.Sub_LogWrite(
                $"装置({unit})." +
                $"SNO:{WorkStates.List[unit].SNO}." +
                $"BLK:{WorkStates.List[unit].BLK}." +
                $"BZI:{WorkStates.List[unit].BZI}." +
                $"PCS:{WorkStates.List[unit].PCS}"
            );
        }
        else {
            RequestKey.Init(
                G.Mid(reverseString, 1, 6),
                G.Mid(reverseString, 7, 8),
                G.Mid(reverseString, 15, 16),
                G.Mid(reverseString, 31, 2)
            );
            Log.Sub_LogWrite(
                $"要求データKey." +
                $"SNO:{RequestKey.Sno}." +
                $"BLK:{RequestKey.Blk}." +
                $"BZI:{RequestKey.Bzi}." +
                $"PCS:{RequestKey.Pcs}"
            );
        }
    }
}