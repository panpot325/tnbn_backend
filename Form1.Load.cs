using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using BackendMonitor.type;

namespace BackendMonitor;

/// <summary>
/// Form1 Load
/// </summary>
public partial class Form1 {
    [DllImport("user32.dll")]
    private static extern bool EnableMenuItem(IntPtr hMenu, uint uIdEnableItem, uint uEnable);

    [DllImport("user32.dll")]
    public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

    internal const uint SC_CLOSE = 0x0000F060;
    internal const uint MF_GRAYED = 0x00000001;

    /// <summary>
    /// Form_Load
    /// </summary>
    private void Form_Load() {
        Log.Sub_LogWrite("【Form_Load】");
        Log.Sub_LogWrite($"Timer1.Interval : {Timer1.Interval}");
        Log.Sub_LogWrite($"Timer2.Interval : {Timer2.Interval}");
        RemoveSystemMenu();
        Timer2_Timer();
    }

    /// <summary>
    /// Abort
    /// </summary>
    private void Abort() {
        Log.Sub_LogWrite("【Command1_Click】");
        Timer1.Enabled = false;
        _melsecPort.Stop();

        var result = MessageBox.Show(@"単板ラインの監視を中断しますか？",
            @"確認",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);

        if (result == DialogResult.Yes) {
            Thread.Sleep(1000);
            Close();
            return;
        }

        _melsecPort.Start();
        Timer1.Enabled = true;
    }

    /// <summary>
    /// RemoveSystemMenu
    /// </summary>
    private void RemoveSystemMenu() {
        Log.Sub_LogWrite("【Remove System Menu】");
        var hMenu = GetSystemMenu(this.Handle, false);
        if (hMenu != IntPtr.Zero) {
            EnableMenuItem(hMenu, SC_CLOSE, MF_GRAYED);
        }
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