using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using BackendMonitor.share;
using BackendMonitor.type.singleton;

namespace BackendMonitor.model;

public class MTcpClient {
    /* 接続先サーバー情報 */
    private string _mServerAddress;
    private int _mServerPort;
    private int _mReadTimeout;
    private int _mWriteTimeout;

    /* 状態フラグ */
    public bool m_connected;

    /* TCP通信ハンドル */
    private TcpClient _mClient;
    private NetworkStream _mTcpStream;

    /// <summary>
    /// サーバー情報初期化
    /// </summary>
    /// <param name="serverAddress">サーバーアドレス</param>
    /// <param name="serverPort">サーバーポート番号</param>
    /// <param name="readTimeout">受信タイムアウト[ms]</param>
    /// <param name="writeTimeout">送信タイムアウト[ms]</param>
    /// <returns></returns>
    public bool SetConnectInfo(string serverAddress, int serverPort, int readTimeout = 1000, int writeTimeout = 1000) {
        // サーバー情報を更新
        _mServerAddress = serverAddress;
        _mServerPort = serverPort;
        _mReadTimeout = readTimeout;
        _mWriteTimeout = writeTimeout;
        // 「切断」状態に初期化
        m_connected = false;
        // エラー処理を追加してfalseを返すのがベスト
        return true;
    }

    /// <summary>
    /// サーバー接続
    /// </summary>
    /// <returns></returns>
    public bool Connect() {
        var result = false;
        try {
            // サーバーと接続
            // 接続完了するまでブロッキングする
            Console.WriteLine(
                $@"{DateTime.Now:[yyyy/MM/dd HH:mm:ss]}【TCPClient】Connect() : [{_mServerAddress}:{_mServerPort}] に接続します ...");
            _mClient = new TcpClient(_mServerAddress, _mServerPort);
            Console.WriteLine(@$"{DateTime.Now:[yyyy/MM/dd HH:mm:ss]}【TCPClient】Connect() : 接続しました");
            // 接続完了
            result = true;
            // 「接続」状態に更新
            m_connected = true;
            // ネットワークストリームを取得
            _mTcpStream = _mClient.GetStream();
            // 送受信タイムアウト時間を設定
            _mTcpStream.ReadTimeout = _mReadTimeout;
            _mTcpStream.WriteTimeout = _mWriteTimeout;
        }
        catch (Exception ex) {
            // 接続失敗
            Console.WriteLine($@"{DateTime.Now:[yyyy/MM/dd HH:mm:ss]}【TCPClient】Connect() : ERROR !!! {ex.Message}");
        }

        return result;
    }

    /// <summary>
    /// 切断処理
    /// </summary>
    public void Disconnect() {
        _mTcpStream?.Close();
        _mClient?.Close();
        m_connected = false;
    }

    /// <summary>
    /// 通信電文送信
    /// </summary>
    /// <param name="data"></param>
    public void Send(byte[] data) {
        try {
            // データ送信開始
            _mTcpStream.Write(data, 0, data.Length);

            // 送信成功
            //Console.WriteLine(@"送信データ：" + new ASCIIEncoding().GetString(data));
            Globals.ConsoleWriteData("W", new ASCIIEncoding().GetString(data));
        }
        catch (Exception ex) {
            // 送信失敗
            Console.WriteLine(@$"{DateTime.Now:[yyyy/MM/dd HH:mm:ss]}【TCPClient】Send() : ERROR !!! {ex.Message}");
            // 「切断」状態に更新
            m_connected = false;
            // クライアント初期化
            _mTcpStream?.Close();
            _mClient?.Close();
        }
    }

    /// <summary>
    /// 通信電文受信
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public int Receive(byte[] data) {
        var receiveSize = 0;
        try {
            // データ受信開始
            while (_mTcpStream.DataAvailable) {
                receiveSize += _mTcpStream.Read(data, receiveSize, data.Length);
            }

            // 受信成功
            if (receiveSize > 0) {
                //Console.WriteLine(@"受信データ：" + new ASCIIEncoding().GetString(data));
                Globals.ConsoleWriteData("R", new ASCIIEncoding().GetString(data));
            }
        }
        catch (IOException) {
            // タイムアウト
            Console.WriteLine(@$"{DateTime.Now:[yyyy/MM/dd HH:mm:ss]}【TCPClient】Receive() : 受信タイムアウト");
        }
        catch (Exception ex) {
            Console.WriteLine(
                @$"{DateTime.Now:[yyyy/MM/dd HH:mm:ss]}【TCPClient】Receive() : ERROR !!! {ex.Message}");
            // 「切断」状態に更新
            m_connected = false;
            // クライアント初期化
            _mTcpStream?.Close();
            _mClient?.Close();
            throw;
        }
        //Console.WriteLine(@" 受信バイト数：" + receiveSize);

        return receiveSize;
    }

    /// <summary>
    /// 通信電文ログ取得
    /// </summary>
    /// <param name="data">通信電文</param>
    /// <param name="size">通信電文サイズ</param>
    /// <returns>通信電文ログ</returns>
    public string MakeTeleLog(byte[] data, int size) {
        var result = "";
        for (var i = 0; i < size; i++) {
            result += $"{data[i],2:X2}";
        }

        return result;
    }
}