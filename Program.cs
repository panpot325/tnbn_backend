using System;
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
        G.Sub_LogWrite(@$"プログラム起動開始 {Settings.Default.Prg_Ver}");
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

        G.Sub_LogWrite(@$"【Sel_監視設定】 {KanshiSettei.SQL}");
        KanshiSettei.Dump();

        G.Sub_LogWrite(@$"【Sel_加工ワークデータタイプ】 {WorkDataTypes.SQL}");
        WorkDataTypes.Dump();

        G.Sub_LogWrite(@"【船番一覧データ作成】（省略）");
        G.Sub_LogWrite(@"【ブロック一覧データ作成】（省略）");
        G.Sub_LogWrite(@"【部材舷一覧データ作成】（省略）");

        Application.Run(new Form1());
        G.Sub_LogWrite(@"Main終了");
        
    }
}