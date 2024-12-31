using System.Collections.Generic;
using System.IO;

// ReSharper disable FunctionRecursiveOnAllPaths
// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming
namespace BackendMonitor.type;

/// <summary>
/// Logクラス
/// </summary>
public partial class Log {
    public string SNO { get; set; } //
    public string BLK { get; set; } //
    public string BZI { get; set; } //
    public string PCS { get; set; } //
    public string L { get; set; } //
    public string B { get; set; } //
    public string Tmax { get; set; } //
    public string Maisu { get; set; } //
    public string Honsu { get; set; } //
    public string YMD { get; set; } //
    public string StrTime { get; set; } //
    public string EndTime { get; set; } //
    public string TotTime { get; set; } //
    public string KadTime { get; set; } //
    public string StpTime { get; set; } //

    /// <summary>
    /// Constructor
    /// </summary>
    public Log() {
    }

    public static List<Log> List = [];
    public static int Count => List.Count;
    public static bool Exists => Count > 0;

    /// <summary>
    /// Clear
    /// </summary>
    /// <returns></returns>
    public static List<Log> Clear() {
        List = [];
        return List;
    }

    /// <summary>
    /// Add
    /// </summary>
    /// <param name="log"></param>
    /// <returns></returns>
    public static List<Log> Add(Log log) {
        List.Add(log);
        return List;
    }

    /// <summary>
    /// 最後の要素を削除
    /// </summary>
    /// <returns></returns>
    public static List<Log> RemoveLast() {
        if (Exists) {
            List.RemoveAt(Count - 1);
        }

        return List;
    }

    /// <summary>
    /// Read
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static List<Log> Read(string filePath) {
        var isHeader = false;
        List = [];
        using var reader = new StreamReader(filePath);
        while (!reader.EndOfStream) {
            var line = reader.ReadLine();
            if (!isHeader) {
                isHeader = true;
                continue;
            }

            if (line == null) continue;
            var values = line.Split(',');
            var log = new Log {
                SNO = Trim(values[0]),
                BLK = Trim(values[1]),
                BZI = Trim(values[2]),
                PCS = Trim(values[3]),
                L = Trim(values[4]),
                B = Trim(values[5]),
                Tmax = Trim(values[6]),
                Maisu = Trim(values[7]),
                Honsu = Trim(values[8]),
                YMD = Trim(values[9]),
                StrTime = Trim(values[10]),
                EndTime = Trim(values[11]),
                TotTime = Trim(values[12]),
                KadTime = Trim(values[13]),
                StpTime = Trim(values[14])
            };

            List.Add(log);
        }

        return List;
    }

    /// <summary>
    /// Write
    /// </summary>
    /// <param name="filePath"></param>
    public static void Write(string filePath) {
        var csvList = new CsvList<string> {
            "船番",
            "ブロック",
            "部材",
            "舷",
            "長",
            "幅",
            "厚",
            "板枚数",
            "Ｌ本数",
            "年月日",
            "開始",
            "終了",
            "ﾄｰﾀﾙ時間",
            "稼動時間",
            "停止時間"
        };

        using var writer = new StreamWriter(filePath, false);
        writer.WriteLine(string.Join(",", csvList));
        foreach (var log in List) {
            writer.WriteLine(string.Join(",", [
                log.SNO,
                log.BLK,
                log.BZI,
                log.PCS,
                log.L,
                log.B,
                log.Tmax,
                log.Maisu,
                log.Honsu,
                log.YMD,
                log.StrTime,
                log.EndTime,
                log.TotTime,
                log.KadTime,
                log.StpTime
            ]));
        }
    }

    /// <summary>
    /// CreateDir
    /// </summary>
    /// <param name="path"></param>
    public static void CreateDir(string path) {
        if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
        }
    }

    /// <summary>
    /// TrimDoubleQuotationMarks
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private static string Trim(string target) {
        return target.Trim(['"']);
    }

    /// <summary>
    /// AddDoubleQuotationMarks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private class CsvList<T> : List<string> {
        // orverride不可
        public new List<string> Add(string item) {
            base.Add("\"" + item + "\"");

            return this;
        }
    }
}