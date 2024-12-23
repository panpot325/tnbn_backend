using BackendMonitor.Properties;
using G = BackendMonitor.share.Globals;

namespace BackendMonitor.type.singleton;

/// <summary>
/// 受信メッセージクラス
/// </summary>
public class ResponseMessage : RCmd {
    /// Static Instance
    private static readonly ResponseMessage _instance = new();

    /// Static Property
    public static string Sh => _instance.SH;
    public static string FinishCode => _instance.Finish_Code;
    public static string IllegalCode => _instance.Illegal_Code;
    public static string ReadData => _instance.Read_Data;

    /// <summary>
    /// Set
    /// </summary>
    public static ResponseMessage Set(string buff) {
        string sh, finishCode, illegalCode, readData;
        switch (Settings.Default.MC_Protocol) {
            case "3E":
                sh = SCmd.ResponseHeader;
                finishCode = G.Mid(buff, 19, 2);
                illegalCode = finishCode == "00" ? "00" : G.Mid(buff, 19, 4);
                readData = finishCode == "00" ? G.Mid(buff, 23) : "";
                break;
            default:
                sh = G.Mid(buff, 1, 2);
                finishCode = G.Mid(buff, 3, 2);
                illegalCode = finishCode == "5B" ? G.Mid(buff, 5, 2) : "00";
                readData = finishCode == "00" ? G.Mid(buff, 5) : "";
                break;
        }

        _instance.Set(sh, finishCode, illegalCode, readData);
        return _instance;
    }

    /// <summary>
    /// Init
    /// </summary>
    public static ResponseMessage Init() {
        _instance.Clear();

        return _instance;
    }

    /// <summary>
    /// Private Constructor
    /// </summary>
    private ResponseMessage() {
    }
}