using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using BackendMonitor.Properties;

namespace BackendMonitor.share;

/// <summary>
/// Globals
/// @Deprecated
/// </summary>
public partial class Globals {
    /// <summary>
    /// Mid
    /// </summary>
    /// <param name="str"></param>
    /// <param name="start"></param>
    /// <param name="len"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string Mid(string str, int start, int len = 0) {
        if (start <= 0) {
            throw new ArgumentException("引数'start'は1以上でなければなりません。");
        }

        len = len switch {
            < 0 => throw new ArgumentException("引数'len'は0以上でなければなりません。"),
            0 => str.Length,
            _ => len
        };

        if (str == null || str.Length < start) {
            return "";
        }

        return str.Length < (start + len) ? str.Substring(start - 1) : str.Substring(start - 1, len);
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

    /// <summary>
    /// PingCheck
    /// </summary>
    /// <returns></returns>
    public static bool PingCheck() {
        return PingCheck(Settings.Default.PLC_Host, 20);
    }

    /// <summary>
    /// PingCheck
    /// </summary>
    /// <param name="target"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public static bool PingCheck(string target, int count) {
        var ping = new Ping();
        for (var i = 0; i < count; i++) {
            try {
                var reply = ping.Send(target, 500);
                if (reply is { Status: IPStatus.Success }) {
                    return true;
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        MessageBox.Show(@"Ping CheckError");
        return false;
    }
}