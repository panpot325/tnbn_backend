using System.Windows.Forms;
using G = BackendMonitor.share.Globals;

namespace BackendMonitor;

/// <summary>
/// Form1 Load
/// </summary>
public partial class Form1 {
    /// <summary>
    /// Form_Load
    /// </summary>
    private void Form_Load() {
        G.LogWrite("【Form_Load】");
        G.LogWrite($"Timer1.Interval : {Timer1.Interval}");
        G.LogWrite($"Timer2.Interval : {Timer2.Interval}");
        RemoveSystemMenu();
        Timer2_Timer();
    }

    /// <summary>
    /// Abort
    /// </summary>
    private void Abort() {
        G.LogWrite("【Command1_Click】");

        Timer1.Enabled = false;
        var result = MessageBox.Show(@"単板ラインの監視を中断しますか？",
            @"確認",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);

        if (result == DialogResult.Yes) {
            Close();
            return;
        }

        Timer1.Enabled = true;
    }

    /// <summary>
    /// Stop
    /// </summary>
    private void Stop() {
        _melsecPort.Stop();
        var result = MessageBox.Show(
            @"終了します",
            @"確認",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);

        Close();
    }

    /// <summary>
    /// RemoveSystemMenu
    /// </summary>
    private static void RemoveSystemMenu() {
        G.LogWrite("【Remove System Menu】");
    }

    /// <summary>
    /// SetText
    /// </summary>
    /// <param name="text"></param>
    /// <param name="text1"></param>
    private void SetText(string text, string text1) {
        Text = text;
        Text1.Text = text1;
    }
}