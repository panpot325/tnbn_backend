using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BackendMonitor.Properties;
using BackendMonitor.share;
using BackendMonitor.type;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;

namespace BackendMonitor;

/// <summary>
/// Entry Point Main
/// </summary>
internal static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main() {
        AppConfig();
        Log.Sub_LogWrite(@$"プログラム起動開始 {Settings.Default.Prg_Ver}");
        using var mutex = new Mutex(false, Application.ProductName);
        if (!mutex.WaitOne(0, false)) {
            MessageBox.Show(@"既に起動中です。二重起動できません。");
            return;
        }

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        G.ClearDebugFile();
        if (!PgOpen.Connect()) {
            MessageBox.Show(@"データベース接続ができませんでした。");
            return;
        }

        Log.Sub_LogWrite(@$"【Sel_監視設定】 {KanshiSettei.SQL}");
        KanshiSettei.Dump();

        Log.Sub_LogWrite(@$"【Sel_加工ワークデータタイプ】 {WorkDataTypes.SQL}");
        WorkDataTypes.Dump();

        Log.Sub_LogWrite(@"【船番一覧データ作成】（省略）");
        Log.Sub_LogWrite(@"【ブロック一覧データ作成】（省略）");
        Log.Sub_LogWrite(@"【部材舷一覧データ作成】（省略）");

        Application.Run(new Form1());
        Log.Sub_LogWrite(@"Main終了");
    }

    /// <summary>
    /// ConfigurationManager.AppSettings
    /// </summary>
    private static void AppConfig() {
        var keys = ConfigurationManager.AppSettings.AllKeys;

        if (keys.Contains("DB_Host")) {
            Settings.Default.DB_Host = ConfigurationManager.AppSettings["DB_Host"];
        }

        if (keys.Contains("DB_Name")) {
            Settings.Default.DB_Name = ConfigurationManager.AppSettings["DB_Name"];
        }

        if (keys.Contains("DB_User")) {
            Settings.Default.DB_User = ConfigurationManager.AppSettings["DB_User"];
        }

        if (keys.Contains("DB_Pass")) {
            Settings.Default.DB_Pass = ConfigurationManager.AppSettings["DB_Pass"];
        }

        if (keys.Contains("Log_Path")) {
            Settings.Default.Log_Path = ConfigurationManager.AppSettings["Log_Path"];
        }

        if (keys.Contains("Dev_Path")) {
            Settings.Default.Dev_Path = ConfigurationManager.AppSettings["Dev_Path"];
        }

        if (keys.Contains("Log_File_Path")) {
            Settings.Default.Log_File_Path = ConfigurationManager.AppSettings["Log_File_Path"];
        }
    }
}