using System;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor.type.singleton;

/// <summary>
/// 監視メッセージクラス
/// </summary>
public class MonitorMessage : RCmd {
    /// Static Instance
    private static readonly MonitorMessage _instance = new();

    /// Static Property
    public static string RequestBit => _instance.Request_Bit;

    /// <summary>
    /// Set
    /// </summary>
    public static MonitorMessage Set(int unit) {
        var readData = MapReadData(ResponseMessage.ReadData, unit); //読込データ
        Log.Sub_LogWrite($"編集後 監視用受信Cmd.読込データ: {readData}");

        var requestBit = GetRequestBit(readData, unit); //要求ビット
        Log.Sub_LogWrite($"監視用受信Cmd.要求ビット: {requestBit}");

        _instance.Set(
            ResponseMessage.Sh,
            ResponseMessage.FinishCode,
            ResponseMessage.IllegalCode,
            readData,
            requestBit
        );

        return _instance;
    }

    /// <summary>
    /// private Constructor
    /// </summary>
    private MonitorMessage() {
    }

    /// <summary>
    /// @読み込みデータ取得
    /// </summary>
    /// <param name="readData"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    private static string MapReadData(string readData, int unit) {
        return unit switch {
            //B110-B115 B63 B66
            C.UNIT_2 => G.Mid(readData, 177, 6) +
                        G.Mid(readData, 4, 1) +
                        G.Mid(readData, 7, 1),
            //B1F0-B1F5 B183 (1)
            C.UNIT_3 => G.Mid(readData, 113, 6) +
                        G.Mid(readData, 4, 1) +
                        "1",
            //B390-B395 B333 B349
            C.UNIT_5 => G.Mid(readData, 97, 6) +
                        G.Mid(readData, 4, 1) +
                        G.Mid(readData, 26, 1),
            _ => "00000000"
        };
    }

    /// <summary>
    /// @要求ビット取得
    /// </summary>
    /// <param name="readData"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    private static string GetRequestBit(string readData, int unit) {
        if (G.Mid(readData, 7, 2) == "11") {
            return WorkStates.List[unit].Start_Count == 0 ? C.REQ_STA : C.REQ_NOP;
        }

        if (G.Mid(readData, 7, 1) != "0") {
            return C.REQ_NOP;
        }

        if (WorkStates.List[unit].Start_Count > 0 || !GetEndState(unit)) {
            return C.REQ_STP;
        }

        return Convert.ToInt32(G.Mid(readData, 1, 6), 2) switch {
            0x20 => C.REQ_SNO,
            0x10 => C.REQ_BLK,
            0x08 => C.REQ_BZI,
            0x04 => C.REQ_DAT,
            0x02 => C.REQ_MIR,
            0x01 => C.REQ_SPI,
            _ => C.REQ_NOP
        };
    }

    /// <summary>
    /// @稼動実績WKから終了判定
    /// 終了していなければtrue
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static bool GetEndState(int unit) {
        WorkStates.List[unit].Fetch_Record(unit, true);
        return WorkStates.List[unit].EndState;
    }
}