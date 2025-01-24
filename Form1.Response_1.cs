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
        var state = WorkStates.List[_unit];

        state.End_Count++;
        state.EndTime = WorkState.NowTime;
        state.Update_Stop(); //稼動実績WKを更新終了

        ReadLogFile(_unit); //ログファイル読込
        WriteLogFile(_unit); //ログファイル出力

        state.Start_Count = 0;
        state.EndTime = WorkState.CLR_TIME;

        Timer1.Enabled = true;
    }

    /// <summary>
    /// @稼動開始
    /// </summary>
    public void Process_Start() {
        Log.WriteLine(@"稼動開始");
        var state = WorkStates.List[_unit]; // 加工ワークデータから取得
        var recState = WorkStates.RecState(_unit);
        ; // 稼動実績WKから取得

        state.Fetch_WorkData();
        state.Start_Count++;
        state.YMD = WorkState.NowDate;
        state.StrTime = WorkState.NowTime;
        state.StrTime2 = WorkState.NowTime;


        if (state.YMD == recState.YMD
            && state.SNO.Trim() == recState.SNO.Trim()
            && state.BLK.Trim() == recState.BLK.Trim()
            && state.BZI.Trim() == recState.BZI.Trim()
            && state.PCS.Trim() == recState.PCS.Trim()) {
            state.CNT = recState.CNT;
            state.StrTime = recState.StrTime;
            state.KAD_TIME = recState.KAD_TIME;

            if (recState.EndTime != WorkState.CLR_TIME) {
                // ReSharper disable once InvalidXmlDocComment
                /**
                 * 停止処理後同じものを再度稼働させた場合
                 */
                state.Update_ReStart(); //稼動実績WKを更新
            }
            else {
                // ReSharper disable once InvalidXmlDocComment
                /**
                 * 稼動中で同じものを再度稼働させた場合（プログラム再起動時など）
                 */
                state.StrTime2 = recState.StrTime2;
            }
        }
        else {
            //WorkState.Update_Clear(_unit); //稼動実績WKをクリア
            state.Update_Start(); //稼動実績WKを初期化
        }

        Timer1.Enabled = true;
    }

    /// <summary>
    /// @要求データキーの戻し作業
    /// </summary>
    /// <param name="unit"></param>
    public void SetRequestKey(int unit) {
        Log.Sub_LogWrite($@"【要求データキーの戻し作業】受信Cmd.読込データ:{ResponseMessage.ReadData}】");

        var ascString = G.AscToString(ResponseMessage.ReadData);
        Log.Sub_LogWrite(ascString);

        var reverseString = G.ReverseString(ascString, 32);
        Log.Sub_LogWrite("変換した文字を2文字ずつ配列に代入し");
        Log.Sub_LogWrite($@"配列単位で文字を並び替え正しい文字列に並びかえ後: {reverseString}");

        var sno = G.Mid(reverseString, 1, 6);
        var blk = G.Mid(reverseString, 7, 8);
        var bzi = G.Mid(reverseString, 15, 16);
        var pcs = G.Mid(reverseString, 31, 2);
        Log.Sub_LogWrite($"装置({unit}).SNO:{sno}.BLK:{blk}.BZI:{bzi}.PCS:{pcs}");

        if (MonitorMessage.RequestBit == C.REQ_STA) {
            StatusKey.Init(sno, blk, bzi, pcs);
        }
        else {
            RequestKey.Init(sno, blk, bzi, pcs);
        }
    }
}