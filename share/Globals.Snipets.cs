using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
    /// PingCheck
    /// Todo Implements
    /// </summary>
    /// <returns></returns>
    public static bool PingCheck() {
        return true;
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

    /// <summary>
    /// ASCII → 文字 に変換
    /// </summary>
    /// <param name="dataString"></param>
    /// <returns></returns>
    public static string AscToString(string dataString) {
        var sb = new StringBuilder();
        for (var i = 0; i < dataString.Length - 1; i += 2) {
            var hex = dataString.Substring(i, 2);
            if (hex != "00") {
                sb.Append(
                    ((char)Convert.ToInt32(hex, 16)).ToString()
                );
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// 文字を2文字ずつ並びかえる
    /// </summary>
    /// <param name="dataString"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static string ReverseString(string dataString, int max = 0) {
        var sb = new StringBuilder();
        for (var i = 0; i < dataString.Length - 1; i += 2) {
            var hex = dataString.Substring(i, 2);
            if (max > 0 && i < max) {
                sb.Append(string.Concat(hex.Reverse()));
            }
        }

        return sb.ToString();
    }
}