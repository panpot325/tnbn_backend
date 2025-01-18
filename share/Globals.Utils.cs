using System.IO;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.type;
using BackendMonitor.type.singleton;

namespace BackendMonitor.share;

/// <summary>
/// Globals
/// </summary>
public partial class Globals {
    /// <summary>
    /// DebugWrite
    /// </summary>
    /// <param name="message"></param>
    public static void DebugWrite(string message) {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/{Settings.Default.Dev_File}", true,
            Encoding.UTF8);
        sw.WriteLine(message);
    }

    /// <summary>
    /// ファイルの内容をクリアする
    /// </summary>
    public static void ClearDebugFile() {
        var fileName = $"{Settings.Default.Dev_Path}/{Settings.Default.Dev_File}";
        if (!File.Exists(fileName)) {
            using (File.Create(fileName)) {
            }
        }

        using var fs = new FileStream(fileName, FileMode.Open);
        fs.SetLength(0);
    }

    /// <summary>
    /// 3EMcProtocolDataのデバッグ
    /// </summary>
    /// <param name="mode"></param>
    /// <param name="sData"></param>
    public static void ConsoleWriteMcProtocolData(string mode, string sData) {
        if (mode == "W") {
            if (Settings.Default.MC_Protocol == "3E") {
                Log.WriteLine(@"----------------------------------------------------------");
                Log.WriteLine(@" 送信ヘッダ：" + sData.Substring(0, 4), false);
                Log.WriteLine(sData.Substring(22, 4) == "0401" ? " READ" : " WRITE", false);
                Log.WriteLine(@"：" + sData.Substring(22, 4), false);
                Log.WriteLine(sData.Substring(26, 4) == "0000" ? " WORD" : " BIT", false);
                Log.WriteLine(@" " + sData.Substring(26, 4), false);
                Log.WriteLine(@" デバイス：" + sData.Substring(30, 2), false);
                Log.WriteLine(@" アドレス：" + sData.Substring(32, 6), false);
                Log.WriteLine(@" 点数：" + sData.Substring(38, 4), false);
                Log.WriteLine(@" データ：" + sData.Substring(42), false);
            }
        }

        if (mode == "R") {
            if (Settings.Default.MC_Protocol == "3E") {
                Log.WriteLine(@" 受信ヘッダ：" + sData.Substring(0, 4), false);
                Log.WriteLine(@" レスポンスヘッダ：" + SCmd.ResponseHeader, false);
                Log.WriteLine(@" コード：" + sData.Substring(18, 4), false);
                Log.WriteLine(@" データ：" + sData.Substring(22));
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
        using (var sw = new StreamWriter($"{Settings.Default.Dev_Path}/{Settings.Default.Dev_File}", true,
                   Encoding.UTF8)) {
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