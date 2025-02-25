using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.share;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor.type.index;

/// <summary>
/// @部材名名一覧リスト
/// </summary>
public class BziIndex {
    public static int BziCount;
    public static int PcsCount;
    public static bool Exist;
    public static List<string> BziList;
    public static List<string> PcsList;

    private const int DATA_MAX = C.BZI_MAX;

    private const string SQL = " SELECT bzi, pcs FROM tnbn_kakowk_data" +
                               " WHERE sno NOT IN (SELECT sno FROM tnbn_fin_ship_mst)" +
                               " AND sno = '{0}'" +
                               " AND blk = '{1}'" +
                               " GROUP BY bzi, pcs" +
                               " ORDER BY bzi, pcs";

    /// <summary>
    /// Fetch
    /// </summary>
    public static void Fetch(string sno, string blk, bool padding = false) {
        BziList = [];
        PcsList = [];
        using var reader = PgConnect.Read(string.Format(SQL, sno, blk));
        while (reader.Read()) {
            BziList.Add(reader.GetString(0).PadRight(16, ' '));
            PcsList.Add(reader.GetString(1).PadRight(2, ' '));
        }

        PgConnect.Close();
        BziCount = BziList.Count;
        PcsCount = PcsList.Count;

        // ReSharper disable once InvertIf
        if (padding) {
            BziList.AddRange(Enumerable.Repeat(new string(' ', 16), DATA_MAX - BziList.Count).ToList());
            PcsList.AddRange(Enumerable.Repeat(new string(' ', 2), DATA_MAX - PcsList.Count).ToList());
        }

        Exist = BziCount > 0 && PcsCount > 0;
    }

    /// <summary>
    /// Dump
    /// </summary>
    public static void Dump() {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/BziIndex.txt", false, Encoding.UTF8);
        var i = 1;
        foreach (var data in BziList) {
            sw.WriteLine($"{i++:00} : {data} {PcsList[i - 1]}");
        }
    }
}