using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.share;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor.type.index;

/// <summary>
/// @船番名一覧リスト
/// </summary>
public class SnoIndex {
    /// Static Instance
    private static readonly SnoIndex _instance = new();

    /// Constants
    private const string SQL = " SELECT sno FROM tnbn_kakowk_data" +
                               " WHERE sno NOT IN (SELECT sno FROM tnbn_fin_ship_mst)" +
                               " GROUP BY sno" +
                               " ORDER BY sno";

    /// Property
    public static List<string> List => _instance._list;

    public static int Count => List.Count;
    public static bool Exist => Count > 0;

    /// Member
    private readonly List<string> _list;

    /// <summary>
    /// Private Constructor
    /// </summary>
    private SnoIndex() {
        _list = [];
        using var reader = PgConnect.Read(SQL);
        while (reader.Read()) {
            _list.Add(reader.GetString(0).PadRight(6, ' '));
        }

        PgConnect.Close();
        _list.AddRange(Enumerable.Repeat(new string(' ', 6), C.SNO_MAX - _list.Count).ToList());
    }

    /// <summary>
    /// Dump
    /// </summary>
    public static void Dump() {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/SnoIndex.txt", false, Encoding.UTF8);
        var i = 1;
        foreach (var data in List) {
            sw.WriteLine($"{i++:00} : {data}");
        }
    }
}