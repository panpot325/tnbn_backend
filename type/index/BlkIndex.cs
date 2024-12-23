using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.share;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor.type.index;

/// <summary>
/// @ブロック名一覧リスト
/// </summary>
public class BlkIndex {
    public static int Count;
    public static bool Exist;
    public static List<string> List;

    private const int DATA_MAX = C.BLK_MAX;

    private const string SQL = " SELECT blk FROM tnbn_kakowk_data" +
                               " WHERE sno NOT IN (SELECT sno FROM tnbn_fin_ship_mst)" +
                               " AND sno = '{0}'" +
                               " GROUP BY blk" +
                               " ORDER BY blk";

    /// <summary>
    /// Fetch
    /// </summary>
    public static void Fetch(string sno, bool padding = false) {
        List = [];
        using var reader = PgConnect.Read(string.Format(SQL, sno));
        while (reader.Read()) {
            List.Add(reader.GetString(0).PadRight(8, ' '));
        }

        PgConnect.Close();
        Count = List.Count;
        if (padding) {
            List.AddRange(Enumerable.Repeat(new string(' ', 8), DATA_MAX - List.Count).ToList());
        }

        Exist = Count > 0;
    }

    /// <summary>
    /// Dump
    /// </summary>
    public static void Dump() {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/BlkIndex.txt", false, Encoding.UTF8);
        var i = 1;
        foreach (var data in List) {
            sw.WriteLine($"{i++:00} : {data}");
        }
    }
}