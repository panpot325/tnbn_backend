using System.IO;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.share;

namespace BackendMonitor.type.singleton;

/// <summary>
/// @監視設定クラス
/// </summary>
public class KanshiSettei {
    public const string SQL = "SELECT * FROM tnbn_kanshi_settei";
    
    /// Static Instance
    private static readonly KanshiSettei _instance = new();

    // Members
    private readonly int _interval;
    private readonly short _wProtocol;
    private readonly string _wRemoteHost;
    private readonly int _wRemotePort;
    private readonly int _wLocalPort;
    private readonly int _cInterval;
    private readonly int _interval2;

    // Property
    public static int Interval => _instance._interval;
    public static int Protocol => _instance._wProtocol;
    public static string RemoteHost => _instance._wRemoteHost;
    public static int RemotePort => _instance._wRemotePort;
    public static int LocalPort => _instance._wLocalPort;
    public static int IntervalC => _instance._cInterval;
    public static int Interval2 => _instance._interval2;
    public static string ServerName => RemoteHost.Replace("\\", "\\\\").Replace("'", "\'");

    /// <summary>
    /// Private Constructor
    /// </summary>
    private KanshiSettei() {
        var row = PgOpen.PgSelect(SQL).Rows[0];
        _interval = (int)row["t_interval"];
        _wProtocol = (short)row["w_protocol"];
        _wRemoteHost = (string)row["w_remotehost"];
        _wRemotePort = (int)row["w_remoteport"];
        _wLocalPort = (int)row["w_localport"];
        _cInterval = (int)row["c_interval"];
        _interval2 = (int)row["t_interval2"];
    }

    /// <summary>
    /// Private Dump
    /// </summary>
    public static void Dump() {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/KanshiSettei.txt", false, Encoding.UTF8);
        sw.WriteLine($"Interval : {Interval}");
        sw.WriteLine($"Protocol : {Protocol}");
        sw.WriteLine($"RemoteHost : {RemoteHost}");
        sw.WriteLine($"RemotePort : {RemotePort}");
        sw.WriteLine($"LocalPort : {LocalPort}");
        sw.WriteLine($"IntervalC : {IntervalC}");
        sw.WriteLine($"Interval2 : {Interval2}");
        sw.WriteLine($"Setting : {ServerName}");
    }
}