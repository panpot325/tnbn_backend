using System;
using System.Linq;
using System.Text;
using BackendMonitor.Properties;
using C = BackendMonitor.share.Constants;

// ReSharper disable InvertIf
// ReSharper disable MemberCanBeMadeStatic.Local
// ReSharper disable ConvertIfStatementToSwitchStatement

namespace BackendMonitor.type.singleton;

/// <summary>
/// 送信コマンドクラス
/// </summary>
public class SCmd {
    /// Static Instance
    private static readonly SCmd _scmd = new();

    /// Instance Getter
    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public static SCmd I => _scmd;

    /// Private Constructor
    private SCmd() {
    }

    /// Get Instance
    public static SCmd Init() {
        return _scmd;
    }

    // Members
    private string _sh; //サブヘッダ
    private string _unitCode; //PC番号
    private string _deviceName; //デバイス名
    private string _deviceNumber; //先頭デバイス番号
    private string _deviceCount; //デバイス点数
    private string _writeData; //上書きする内容
    private string _acpuTimer; //ACPU監視タイマ
    private string _lastCode; //固定："00"

    /// 送信コマンドの取得
    public static string GetCmd() {
        return _scmd.GetSendCommand();
    }

    /// <summary>
    ///  レスポンスヘッダ
    /// </summary>
    public static string ResponseHeader =>
        _scmd._sh switch {
            C.REQ_RD_B => C.RES_RD_B,
            C.REQ_RD_W => C.RES_RD_W,
            C.REQ_WR_B => C.RES_WR_B,
            C.REQ_WR_W => C.RES_WR_W,
            _ => "00"
        };

    /// 送信コマンドの生成
    public static string GetCmd<TW, TA, TU>(
        string sh,
        string unitCode,
        TW deviceName,
        TA deviceNumber,
        TU deviceCount,
        string writeData = ""
    ) {
        return _scmd.Set(
            sh,
            unitCode,
            typeof(TW) == typeof(char)
                ? HexDeviceCode((char)(object)deviceName) + "20"
                : (string)(object)deviceName,
            typeof(TA) == typeof(int)
                ? HexDeviceCode((int)(object)deviceNumber)
                : (string)(object)deviceNumber,
            typeof(TU) == typeof(int)
                ? HexDeviceCode((int)(object)deviceCount)
                : (string)(object)deviceCount,
            writeData
        ).GetSendCommand();
    }

    /// ロパティセット
    private SCmd Set(string sh, string unitCode, string deviceName, string deviceNumber, string deviceCount,
        string writeData) {
        _sh = sh;
        _unitCode = unitCode;
        _deviceName = deviceName;
        _deviceNumber = deviceNumber;
        _deviceCount = deviceCount;
        _writeData = writeData;
        _acpuTimer = C.AC_TIMER;
        _lastCode = "00";

        return this;
    }

    /// <summary>
    /// 送信コマンドの取得
    /// </summary>
    /// <returns>送信コマンド</returns>
    private string GetSendCommand() {
        return Settings.Default.MC_Protocol switch {
            "3E" => Get3EFrameCommand(),
            _ => Get1EFrameCommand()
        };
    }

    /// 送信コマンドの取得(1Eフレーム用)
    private string Get1EFrameCommand() {
        return _sh + _unitCode + _acpuTimer +
               _deviceName.PadLeft(4, '0') +
               _deviceNumber.PadLeft(8, '0') +
               _deviceCount.PadLeft(2, '0') +
               _lastCode + _writeData;
    }

    /// 送信コマンドの取得(3Eフレーム用)
    private string Get3EFrameCommand() {
        return GetSubHeader() +
               GetAccessRoute() +
               GetRequestLength() +
               GetAcpTimer() +
               GetRequestData();
    }

    /// @HEXコード変換
    private static string HexDeviceCode(int value) {
        return value > 0
            ? Convert.ToString(value, 16)
            : "";
    }

    /**
       * ２文字ずつ文字を逆順
       * 文字をASCIIに変換
       * 16進に変換
       * String.Reverseについて
       * Stringでは返却ない、IEnumerable(Of Char)が返却される。
       * string.Concatで結合させる必要がある。
       */
    /// @書き込み用HEXコードの生成
    public static string HexWriteData(string dataString) {
        var sb = new StringBuilder();
        var sc = new StringBuilder();

        dataString = dataString.ToUpper();
        for (var i = 0; i < dataString.Length - 1; i += 2) {
            sb.Append(string.Concat(
                    dataString.Substring(i, 2).Reverse()
                )
            );
        }

        if (sb.Length < dataString.Length) {
            sb.Append(dataString[dataString.Length - 1]);
        }

        for (var i = 0; i < sb.Length; i++) {
            var asc = (int)(sb.ToString().Substring(i, 1).ToCharArray()[0]);
            sc.Append(Convert.ToString(asc, 16).PadLeft(2, '0'));
        }

        return sc.ToString();
    }

    /// サブヘッダ
    private string GetSubHeader() {
        return "5000";
    }

    /// アクセス経路
    private string GetAccessRoute() {
        return GenNetWorkNumber() + GetPcNumber() + GetReqIoNumber() + GetReqUnitNumber();
    }

    /// 要求データ長
    private string GetRequestLength() {
        return Convert.ToString(
                (GetAcpTimer() + GetRequestData()).Length, 16)
            .PadLeft(4, '0');
    }

    /// 監視タイマー
    private string GetAcpTimer() {
        return "000A";
    }

    /// 要求データ
    private string GetRequestData() {
        return GeRequestMainCmd() +
               GetRequestSubCmd() +
               GetRequestDeviceName() +
               GetRequestDeviceNumber() +
               GetRequestDeviceCount() +
               GetRequestWriteData();
    }

    /// 要求データ.コマンド (4)
    private string GeRequestMainCmd() {
        return _sh switch {
            C.REQ_RD_B or C.REQ_RD_W => "0401",
            C.REQ_WR_B or C.REQ_WR_W => "1401",
            _ => "0000"
        };
    }

    /// 要求データ.サブコマンド (4)
    private string GetRequestSubCmd() {
        return _sh switch {
            C.REQ_RD_B or C.REQ_WR_B => "0001",
            C.REQ_RD_W or C.REQ_WR_W => "0000",
            _ => "0000"
        };
    }

    /// 要求データ.デバイスコード (2)
    private string GetRequestDeviceName() {
        return _deviceName switch {
            C.DEVICE_B => "B*",
            C.DEVICE_D => "D*",
            C.DEVICE_W => "W*",
            _ => "00"
        };
    }

    /// 要求データ.デバイス番号 
    private string GetRequestDeviceNumber() {
        var deviceNumber = _deviceNumber.PadLeft(8, '0');
        return deviceNumber.Substring(deviceNumber.Length - 6);
    }

    /// 要求データ.デバイス点数 
    private string GetRequestDeviceCount() {
        return _deviceCount.PadLeft(4, '0');
    }

    /// 要求データ.書き込みデータ 
    private string GetRequestWriteData() {
        if (_deviceCount == C.TRI_BIT_DEVICE_COUNT) {
            if (_writeData == C.TRI_BIT_WRITE_OFF) {
                return "000";
            }
        }

        if (_deviceCount == C.ONE_BIT_DEVICE_COUNT) {
            if (_writeData == C.ONE_BIT_WRITE_ON) {
                return "1";
            }

            if (_writeData == C.ONE_BIT_WRITE_OFF) {
                return "0";
            }
        }

        return _writeData;
    }

    /// ネットワーク番号
    /// ユニットパラメータのネットワーク番号
    /// 自局の場合は固定値 00
    private string GenNetWorkNumber() {
        return "00";
    }

    /// PC番号
    /// ユニットパラメータのPC番号
    /// 自局の場合は固定値 FF
    private string GetPcNumber() {
        return "FF";
    }

    /// 要求先ユニットIO番号
    /// マルチドロップ接続局、マルチCPUシステム、CC-LinkIEフィールドネットワークリモートデッドユニットで設定
    /// 上記以外は固定値 03FF
    private string GetReqIoNumber() {
        return "03FF";
    }

    /// 要求先ユニットIO番号
    /// マルチドロップ接続局、マルチCPUシステム、CC-LinkIEフィールドネットワークリモートデッドユニットで設定
    /// 上記以外は固定値 00
    private string GetReqUnitNumber() {
        return "00";
    }

    ///ビット書き込みテスト
    ///送信データ：500000FF03FF000019000A14010001B*00000000011
    ///受信データ：D00000FF03FF0000040000
    public static string WriteBitCmd() {
        return "5000" + //サブヘッダ
               "00FF03FF00" + //アクセス経路
               "0019" + //要求データ長
               "000A" + //監視タイマー (4)
               "1401" + //要求データ.コマンド (4)
               "0001" + //要求データ.サブコマンド (4)
               "B*" + //要求データ.デバイスコード (2)
               "000000" + //要求データ.デバイス番号 (6)
               "0001" + //要求データ.デバイス点数 (4)
               "1" + //要求データ.書き込みデータ (1)
               "";
    }

    ///ワード書き込みテスト
    ///送信データ：500000FF03FF000020000A14010000W*0000000002AB1234CD
    ///受信データ：D00000FF03FF0000040000
    public static string WriteWordCmd() {
        return "5000" + //サブヘッダ
               "00FF03FF00" + //アクセス経路
               "0020" + //要求データ長
               "000A" + //監視タイマー (4)
               "1401" + //要求データ.コマンド (4)
               "0000" + //要求データ.サブコマンド (4)
               "W*" + //要求データ.デバイスコード (2)
               "000000" + //要求データ.デバイス番号 (6)
               "0002" + //要求データ.デバイス点数 (4)
               "AB1234CD" + //要求データ.書き込みデータ (8)
               "";
    }

    ///ビット呼び出しテスト
    ///送信データ：500000FF03FF000018000A04010001B*0000000010
    ///受信データ：D00000FF03FF00001400001000000000000000
    public static string ReadBitCmd() {
        return "5000" + //サブヘッダ
               "00FF03FF00" + //アクセス経路
               "0018" + //要求データ長
               "000A" + //監視タイマー (4)
               "0401" + //要求データ.コマンド (4)
               "0001" + //要求データ.サブコマンド (4)
               "B*" + //要求データ.デバイスコード (2)
               "000000" + //要求データ.デバイス番号 (6)
               "0010" + //要求データ.デバイス点数 (4)
               "" + //要求データ.書き込みデータ (0)
               "";
    }

    ///ワード呼び出しテスト
    ///送信データ：500000FF03FF000018000A04010000W*0000000002
    ///受信データ：D00000FF03FF00000C0000AB1234CD
    public static string ReadWordCmd() {
        return "5000" + //サブヘッダ
               "00FF03FF00" + //アクセス経路
               "0018" + //要求データ長
               "000A" + //監視タイマー (4)
               "0401" + //要求データ.コマンド (4)
               "0000" + //要求データ.サブコマンド (4)
               "W*" + //要求データ.デバイスコード (2)
               "000000" + //要求データ.デバイス番号 (6)
               "0002" + //要求データ.デバイス点数 (4)
               "" + //要求データ.書き込みデータ (0)
               "";
    }
}