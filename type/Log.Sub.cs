using System;
using System.IO;
using System.Text;
using BackendMonitor.Properties;

namespace BackendMonitor.type;

/// <summary>
/// Logクラス
/// </summary>
public partial class Log {
    /// <summary>
    /// Sub_LogWrite
    /// </summary>
    /// <param name="message"></param>
    public static void Sub_LogWrite(string message) {
        if (Settings.Default.Log_Write != 1) return;
        using var sw = new StreamWriter($"{Settings.Default.Log_Path}/{Settings.Default.Log_File}", true,
            Encoding.UTF8);
        sw.WriteLine($"{DateTime.Now:yyyy/MM/dd HH:mm:ss}\t{message}");
    }

    /// <summary>
    /// WriteLine
    /// </summary>
    /// <param name="message"></param>
    public static void WriteLine(string message) {
        Console.WriteLine(message);
    }
}