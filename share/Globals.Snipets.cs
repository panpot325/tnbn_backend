using System;
using System.Net;
using System.Net.Sockets;
using BackendMonitor.Properties;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor.share;

/// <summary>
/// Globals
/// </summary>
public partial class Globals {
    /// <summary>
    /// ユニットコードの取得
    /// </summary>
    public static string UnitCode(int unit) {
        return unit switch {
            C.UNIT_2 => C.UNIT_CODE_2,
            C.UNIT_3 => C.UNIT_CODE_3,
            C.UNIT_5 => C.UNIT_CODE_5,
            _ => ""
        };
    }

    /// <summary>
    /// ユニット名の取得
    /// </summary>
    public static string UnitName(int unit) {
        return unit switch {
            C.UNIT_2 => "仮付け装置",
            C.UNIT_3 => "２０電極溶接",
            C.UNIT_5 => "歪み矯正機",
            _ => ""
        };
    }

    /// <summary>
    /// ユニット名の取得（短縮）
    /// </summary>
    public static string UnitShortName(int unit) {
        return unit switch {
            C.UNIT_2 => "仮付",
            C.UNIT_3 => "20電",
            C.UNIT_5 => "矯正",
            _ => ""
        };
    }

    /// <summary>
    /// 稼働時間内か
    /// </summary>
    /// <param name="endTime"></param>
    /// <returns>稼働時間内であればtrue</returns>
    public static bool IsUsableTime(string endTime) {
        if (string.IsNullOrEmpty(endTime)) {
            endTime = Settings.Default.End_Time;
        }

        return string.CompareOrdinal(DateTime.Now.ToString("HH:mm:ss"), endTime) <= 0;
    }

    /// <summary>
    /// ホスト名取得
    /// </summary>
    /// <returns></returns>
    public static string GetHostName() {
        return Dns.GetHostName();
    }

    /// <summary>
    /// IPアドレス取得
    /// </summary>
    /// <returns></returns>
    public static string GetIpAddress() {
        foreach (var ip in Dns.GetHostAddresses(GetHostName())) {
            if (ip.AddressFamily == AddressFamily.InterNetwork) {
                return ip.ToString();
            }
        }

        return "";
    }
}