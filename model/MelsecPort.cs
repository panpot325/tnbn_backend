using System.Text;
using System.Threading;
using System.Windows.Forms;
using BackendMonitor.type.singleton;

// ReSharper disable InvertIf
// ReSharper disable ConvertIfStatementToReturnStatement

namespace BackendMonitor.model;

/// <summary>
/// Primary Constructor
/// </summary>
public class MelsecPort(Control mWindowHandle, string serverIpAddress, int serverPort, int readTimeout) {
    /* 通信ハンドル */
    private Thread _mMelsecThread;
    private MTcpClient _mMelsecComm;

    /* コールバック */
    public delegate void ConnectEventHandler();

    public event ConnectEventHandler MOnConnect;

    public delegate void RecvEventHandler(string teleLog);

    public event RecvEventHandler MOnRecv;

    /* 制御フラグ */
    private bool _mIsStopCommand;

    /// <summary>
    /// 開始処理
    /// </summary>
    public bool Start() {
        // 通信設定
        _mMelsecComm = new MTcpClient();
        if (!_mMelsecComm.SetConnectInfo(serverIpAddress, serverPort, readTimeout)) {
            return false;
        }

        // 三菱PLC 通信スレッド起動
        _mIsStopCommand = false;
        _mMelsecThread = new Thread(MelsecThread);
        _mMelsecThread.Start();

        // 正常起動
        return true;
    }

    /// <summary>
    /// 終了処理
    /// </summary>
    public void Stop() {
        _mIsStopCommand = true;
    }

    public void Send(string cmd) {
        _mMelsecComm.Send(Encoding.ASCII.GetBytes(cmd));
    }

    /// <summary>
    /// 通信スレッド
    /// </summary>
    public void MelsecThread() {
        while (!_mIsStopCommand) {
            // 未接続時は接続する
            if (!_mMelsecComm.m_connected) {
                _mMelsecComm.Connect();
            }

            // 接続している場合
            if (_mMelsecComm.m_connected) {
                // 読出ONの場合
                var recvCommData = new byte[256];
                var recvSize = _mMelsecComm.Receive(recvCommData);
                // 応答解析
                ParseRecvData(recvCommData, recvSize);
            }

            // 読出周期だけスリープ
            Thread.Sleep(10);
        }

        // 終了処理
        if (_mMelsecComm.m_connected) {
            _mMelsecComm.Disconnect();
        }

        // 終了通知
        if (mWindowHandle != null && MOnConnect != null) {
            mWindowHandle.Invoke(MOnConnect);
        }
    }

    /// <summary>
    /// データ解析
    /// </summary>
    private void ParseRecvData(byte[] recvData, int recvSize) {
        //ヘッダ部のデータが足りない場合はエラー
        if (recvSize < 1) {
            return;
        }

        // 受信通知
        if (mWindowHandle != null && MOnRecv != null) {
            //Console.WriteLine("受信データ　" + recvData);
            //Console.WriteLine(@"受信データ　" + new ASCIIEncoding().GetString(recvData));
            mWindowHandle.Invoke(MOnRecv, new object[1] { new ASCIIEncoding().GetString(recvData, 0 ,recvSize) });
        }
    }
}