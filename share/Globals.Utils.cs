using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.type.singleton;

// ReSharper disable InvertIf
// ReSharper disable ConvertIfStatementToSwitchStatement
namespace BackendMonitor.share;

/// <summary>
/// Globals
/// </summary>
public partial class Globals {
    /// <summary>
    /// LogWrite
    /// </summary>
    /// <param name="message"></param>
    public static void LogWrite(string message) {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/Logs.txt", true, Encoding.UTF8);
        sw.WriteLine(message);
    }

    /// <summary>
    /// ファイルの内容をクリアする
    /// </summary>
    public static void ClearDebugFile() {
        var fileName = $"{Settings.Default.Dev_Path}/Data.txt";
        if (!File.Exists(fileName)) {
            using (File.Create(fileName)) {
            }
        }

        using var fs = new FileStream($"{Settings.Default.Dev_Path}/Data.txt", FileMode.Open);
        fs.SetLength(0);
    }

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

        if (len < 0) {
            throw new ArgumentException("引数'len'は0以上でなければなりません。");
        }

        if (len == 0) {
            len = str.Length;
        }

        if (str == null || str.Length < start) {
            return "";
        }

        return str.Length < (start + len) ? str.Substring(start - 1) : str.Substring(start - 1, len);
    }

    /// <summary>
    /// PingCheck
    /// </summary>
    /// <returns></returns>
    public static bool PingCheck() {
        return PingCheck(Settings.Default.PLC_Host, 5);
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
            var reply = ping.Send(target, 500);
            if (reply is { Status: IPStatus.Success }) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// ConsoleWriteData
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="sData"></param>
    public static void ConsoleWriteData(string mode, string sData) {
        if (mode == "W") {
            if (Settings.Default.MC_Protocol == "3E") {
                Console.WriteLine(@"----------------------------------------------------------");
                Console.Write(@" 送信ヘッダ：" + sData.Substring(0, 4));
                Console.Write(sData.Substring(22, 4) == "0401" ? " READ" : " WRITE");
                Console.Write(@"：" + sData.Substring(22, 4));
                Console.Write(sData.Substring(26, 4) == "0000" ? " WORD" : " BIT");
                Console.Write(@" " + sData.Substring(26, 4));
                Console.Write(@" デバイス：" + sData.Substring(30, 2));
                Console.Write(@" アドレス：" + sData.Substring(32, 6));
                Console.Write(@" 点数：" + sData.Substring(38, 4));
                Console.WriteLine(@" データ：" + sData.Substring(42));
            }
        }

        if (mode == "R") {
            if (Settings.Default.MC_Protocol == "3E") {
                Console.Write(@" 受信ヘッダ：" + sData.Substring(0, 4));
                Console.Write(@" レスポンスヘッダ：" + SCmd.ResponseHeader);
                Console.Write(@" コード：" + sData.Substring(18, 4));
                Console.WriteLine(@" データ：" + sData.Substring(22));
            }
        }
    }

    /// <summary>
    /// StreamWriteData
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="data"></param>
    public static void StreamWriteData(string mode, string data) {
        // ReSharper disable once ConvertToUsingDeclaration
        using (var sw = new StreamWriter($"{Settings.Default.Dev_Path}/Data.txt", true, Encoding.UTF8)) {
            if (mode == "W") {
                if (Settings.Default.MC_Protocol == "3E") {
                    sw.WriteLine("Send Data: " + data);
                    sw.Write(@" 送信ヘッダ：" + data.Substring(0, 4));
                    sw.Write(data.Substring(22, 4) == "0401" ? " READ" : " WRITE");
                    sw.Write(@"：" + data.Substring(22, 4));
                    sw.Write(data.Substring(26, 4) == "0000" ? " WORD" : " BIT");
                    sw.Write(@" " + data.Substring(26, 4));
                    sw.Write(@" デバイス：" + data.Substring(30, 2));
                    sw.Write(@" アドレス：" + data.Substring(32, 6));
                    sw.Write(@" 点数：" + data.Substring(38, 4));
                    sw.Write(@" データ：" + data.Substring(42));
                    sw.WriteLine("");
                }
            }

            if (mode == "R") {
                sw.WriteLine("Read Data: " + data);
            }
        }
    }
}