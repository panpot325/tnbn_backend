using System.Collections.Generic;
using BackendMonitor.type;

namespace BackendMonitor.share;

/// <summary>
/// Globals
/// @Deprecated
/// </summary>
public partial class Globals {
    public static int SnoIti; //@船番Iti
    public static int BziCnt; //@部材舷Cnt

    public static List<Log> Logs; //--ログ保管用構造体
    public static string PingRes; //pingレスポンス
}