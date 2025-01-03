using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using BackendMonitor.Properties;
using BackendMonitor.share;

namespace BackendMonitor.type;

/// <summary>
/// 加工ワークータタイプコレクション
///  @Singleton Design Pattern
/// </summary>
public class WorkDataTypes {
    /// Static Instance
    private static readonly WorkDataTypes _instance = new();

    /// Static Members
    private readonly List<WorkDataType> _list;

    private readonly DataTable _table;

    // Static Property
    public static List<WorkDataType> List => _instance._list;
    public static int Count => List.Count;
    public static bool Exists => Count > 0;
    public static DataTable Table => _instance._table;

    /// Constants
    public const string SQL = "SELECT * FROM tnbn_kakowk_data_type" +
                              " WHERE dm like 'W%'" +
                              " ORDER BY dm";

    /// <summary>
    /// Private Constructor
    /// </summary>
    private WorkDataTypes() {
        _list = [];
        _table = PgOpen.PgSelect(SQL);
        foreach (DataRow row in _table.Rows) {
            _list.Add(new WorkDataType(
                (string)row["dm"],
                (string)row["ryaku"],
                (string)row["meisyo"],
                (string)row["nyu_mode"],
                (decimal)row["nyu_tani"],
                (string)row["tani"],
                (short)row["dev_tensu"],
                (decimal)row["hani_min"],
                (decimal)row["hani_max"],
                (string)row["zero_entry"],
                (string)row["keishiki"],
                (string)row["biko"],
                (string)row["karitsuke"],
                (string)row["yosetu"],
                (string)row["kyosei"],
                (string)row["def"],
                0m
            ));
        }
    }

    public static DataTable GetDataTable() {
        var table = new DataTable();
        table.Columns.Add("dm", typeof(string));
        foreach (var data in _instance._list) {
            var row = table.NewRow();
            row["dm"] = data.DM;
            table.Rows.Add(row);
        }

        return table;
    }

    /// <summary>
    /// 配列番号を取得
    /// </summary>
    /// <param name="dm"></param>
    /// <returns></returns>
    public static int DmIndex(string dm) {
        return (
            from data in List.Select((value, index) => new { value, index })
            where data.value.DM == dm
            select data.index).FirstOrDefault();
    }

    /// <summary>
    /// データタイプのSP1〜SP5より最大のデフォルト値を取得
    /// </summary>
    /// <param name="sp"></param>
    /// <returns></returns>
    public static string DefMax(int sp) {
        var types = List;
        var defMax = List[sp].Def;

        if (string.Compare(defMax, List[sp + 1].Def, StringComparison.Ordinal) < 0) {
            defMax = types[sp + 1].Def;
        }

        if (string.Compare(defMax, types[sp + 2].Def, StringComparison.Ordinal) < 0) {
            defMax = types[sp + 2].Def;
        }

        if (string.Compare(defMax, types[sp + 3].Def, StringComparison.Ordinal) < 0) {
            defMax = types[sp + 3].Def;
        }

        if (string.Compare(defMax, types[sp + 4].Def, StringComparison.Ordinal) < 0) {
            defMax = types[sp + 4].Def;
        }

        return defMax;
    }

    /// <summary>
    /// Dump List
    /// </summary>
    public static void Dump() {
        using var sw = new StreamWriter($"{Settings.Default.Dev_Path}/KakoWkDataType.txt", false, Encoding.UTF8);
        var i = 1;
        foreach (var data in List) {
            sw.Write($"{i++:000} {data.DM} {data.Ryaku} {data.Meisyo} {data.Nyu_Mode} ");
            sw.Write($"{data.Nyu_Tani} {data.Tani} {data.Dev_Tensu} {data.Hani_Min} ");
            sw.Write($"{data.Hani_Max} {data.Zero_Entry} {data.Keishiki} {data.Biko} ");
            sw.Write($"{data.Karituke} {data.Yosetu} {data.Kyosei} {data.Def} ");
            sw.WriteLine($"{data.Hani_Min2}");
        }

        sw.WriteLine(Count);
    }
}