using System;
using System.Timers;
using System.Windows.Forms;
using BackendMonitor.model;
using BackendMonitor.Properties;
using BackendMonitor.type;

// ReSharper disable MemberCanBeMadeStatic.Local
namespace BackendMonitor;

public partial class Form1 : Form {
    private readonly MelsecPort _melsecPort;

    private int _unit; //装置No どの装置を監視しているか判断するための変数
    private int _iter; //@itrnCnt
    private bool _finish; //@Finish
    private int _clrIter; //@itrnClrCnt
    private bool _clrFinish; //@ClrFinish
    private int _clrState; //@gListClrState 0:一覧ｸﾘｱ完了、1:船番一覧ｸﾘｱ、2:ﾌﾞﾛｯｸ一覧ｸﾘｱ
    private int _sendCnt; //@送信完了Cnt

    /// <summary>
    /// Form1 Constructor
    /// </summary>
    public Form1() {
        InitComponent();
        _melsecPort = new MelsecPort(this,
            Settings.Default.PLC_Host,
            Settings.Default.PLC_Port,
            Settings.Default.PLC_Timeout);
        _melsecPort.MOnRecv += OnRecv;
        _melsecPort.MOnConnect += OnDisconnect;
        _clrFinish = false; //起動時にクリア処理を行う
        _clrState = 1;
        _unit = 1; // 装置No = 1
    }

    /// <summary>
    /// Load Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_Load(object sender, EventArgs e) {
        Log.Sub_LogWrite("【Form_Load】");
        Form_Load();
    }

    /// <summary>
    /// Click Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Command1_Click(object sender, EventArgs e) {
        Log.Sub_LogWrite("【Command1_Click】");
        Abort();
    }

    /// <summary>
    /// Timer Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Timer1_Elapsed(object sender, ElapsedEventArgs e) {
        Timer1_Timer();
    }

    /// <summary>
    /// Timer Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Timer2_Elapsed(object sender, ElapsedEventArgs e) {
        Timer2_Timer();
    }

    /// <summary>
    /// Click Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button1_Click_1(object sender, EventArgs e) {
    }

    /// <summary>
    /// Closing Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
        Log.Sub_LogWrite("【Form_Unload】");
        if (_melsecPort.IsStop()) {
            return;
        }

        _melsecPort.Stop();
        MessageBox.Show(
            @"終了します",
            @"確認",
            MessageBoxButtons.OK,
            MessageBoxIcon.Exclamation,
            MessageBoxDefaultButton.Button2);
    }

    /// <summary>
    /// Closed Event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
    }

    /// <summary>
    /// 通信スレッドからの受信通知
    /// </summary>
    private void OnRecv(string strTeleLog) {
        DataArrival(strTeleLog);
    }

    /// <summary>
    /// 通信スレッドからの受信通知
    /// </summary>
    private void OnDisconnect() {
    }

    /// <summary>
    /// 船番一覧書込要求
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button2_Click(object sender, EventArgs e) {
        SendData(SnoIndexRequestCmd(2));
    }

    /// <summary>
    /// ブロック名一覧書込要求
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void button3_Click(object sender, EventArgs e) {
        SendData(BlkIndexRequestCmd(2));
    }

    /// <summary>
    /// 部材名一覧書込要求
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button4_Click(object sender, EventArgs e) {
        SendData(BziIndexRequestCmd(2));
    }

    /// <summary>
    /// ワークデータ書込要求
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button5_Click(object sender, EventArgs e) {
        SendData(WorkDataRequestCmd(2));
    }

    /// <summary>
    /// 稼働開始
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button6_Click(object sender, EventArgs e) {
        SendData(WorkStartRequestCmd(2));
    }

    /// <summary>
    /// 稼働開始
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void button7_Click(object sender, EventArgs e) {
        SendData(WorkStopRequestCmd(2));
    }
}