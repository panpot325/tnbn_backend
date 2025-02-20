using System;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

// ReSharper disable MemberCanBeMadeStatic.Local

namespace BackendMonitor.type;

/// <summary>
/// 加工ワークデータタイプクラス
/// </summary>
public class WorkDataType {
    public string DM; //
    public string Ryaku; //
    public string Meisyo; //
    public string Nyu_Mode; //ANUM:英数字／NUM:数値／A:英字
    public decimal Nyu_Tani; //1／0.1 等 (NYU_MODE=NUMの時のみ使用)
    public string Tani; //
    public short Dev_Tensu; //16bit単位(16bit→1／32bit→2／・・・)
    public decimal Hani_Min; //
    public decimal Hani_Max; //
    public string Zero_Entry; //OK／NG (HANI_MIN～HANI_MAX が｢0｣を経由しない時のみ使用)
    public string Keishiki; //ASCII／16bitB／32bitB／m16bitB
    public string Biko; //
    public string Karituke; //○：使用／△：どちらでも／－：不使用
    public string Yosetu; //○：使用／△：どちらでも／－：不使用
    public string Kyosei; //○：使用／△：どちらでも／－：不使用
    public string Def;
    public decimal Hani_Min2; //

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="dm"></param>
    /// <param name="ryaku"></param>
    /// <param name="meisyo"></param>
    /// <param name="nyuMode"></param>
    /// <param name="nyuTani"></param>
    /// <param name="tani"></param>
    /// <param name="devTensu"></param>
    /// <param name="haniMin"></param>
    /// <param name="haniMax"></param>
    /// <param name="zeroEntry"></param>
    /// <param name="keishiki"></param>
    /// <param name="biko"></param>
    /// <param name="karitsuke"></param>
    /// <param name="yosetu"></param>
    /// <param name="kyosei"></param>
    /// <param name="def"></param>
    /// <param name="haniMin2"></param>
    public WorkDataType(
        string dm,
        string ryaku,
        string meisyo,
        string nyuMode,
        decimal nyuTani,
        string tani,
        short devTensu,
        decimal haniMin,
        decimal haniMax,
        string zeroEntry,
        string keishiki,
        string biko,
        string karitsuke,
        string yosetu,
        string kyosei,
        string def,
        decimal haniMin2
    ) {
        DM = dm;
        Ryaku = ryaku;
        Meisyo = meisyo;
        Nyu_Mode = nyuMode;
        Nyu_Tani = nyuTani;
        Tani = tani;
        Dev_Tensu = devTensu;
        Hani_Min = haniMin;
        Hani_Max = haniMax;
        Zero_Entry = zeroEntry;
        Keishiki = keishiki;
        Biko = biko;
        Karituke = karitsuke;
        Yosetu = yosetu;
        Kyosei = kyosei;
        Def = Nyu_Mode == C.NYU_MODE_NUM && Nyu_Tani.Equals(0.1m) ? def + "0" : def;
        Hani_Min2 = Nyu_Mode == C.NYU_MODE_NUM && Nyu_Tani.Equals(0.1m) ? haniMin2 : haniMin;
    }

    /// Property
    public string WriteData {
        get {
            var workData = GetWorkData();
            //G.DebugWrite(@$"{Ryaku} : {Keishiki.Trim()} : {workData}");
            return Keishiki.Trim() switch {
                C.KEISHIKI_ASC => FormatAsc(workData),
                C.KEISHIKI_M16 => FormatM16BitB(workData),
                C.KEISHIKI_32 => Format32BitB(workData),
                _ => FormatDefault(workData)
            };
        }
    }

    public string DeviceNumber => G.Mid(DM, 2).PadLeft(8, '0');
    public string DeviceCount => Dev_Tensu.ToString().PadLeft(2, '0');

    /// <summary>
    /// Format ASC
    /// </summary>
    /// <param name="wk"></param>
    /// <returns></returns>
    private string FormatAsc(string wk) {
        return SCmd.HexWriteData(wk);
    }

    /// <summary>
    ///  Format M16
    /// </summary>
    /// <param name="wk"></param>
    /// <returns></returns>
    private string FormatM16BitB(string wk) {
        var val = Convert.ToInt16(double.Parse(wk));
        return Convert.ToString(val, 16).PadLeft(Dev_Tensu * 4, '0');
    }

    /// <summary>
    /// Format 32
    /// </summary>
    /// <param name="wk"></param>
    /// <returns></returns>
    private string Format32BitB(string wk) {
        var val = Convert.ToInt32(double.Parse(wk));
        wk = Convert.ToString(val, 16).PadLeft(Dev_Tensu * 4, '0');
        return G.Mid(wk, 5) + G.Mid(wk, 1, 4);
    }

    /// <summary>
    /// Format
    /// </summary>
    /// <param name="wk"></param>
    /// <returns></returns>
    private string FormatDefault(string wk) {
        var val = Convert.ToInt32(double.Parse(wk));
        return Convert.ToString(val, 16).PadLeft(Dev_Tensu * 4, '0');
    }

    /// <summary>
    /// GetWorkData
    /// </summary>
    /// <returns></returns>
    private string GetWorkData() {
        var workData = WorkData.I;
        return Ryaku.Trim() switch {
            "SNO" => workData.SNO.PadRight(6, ' '),
            "BLK" => workData.BLK.PadRight(8, ' '),
            "BZI" => workData.BZI.PadRight(16, ' '),
            "PCS" => workData.PCS.PadRight(2, ' '),
            "GR1" => workData.Gr1.ToString(),
            "GR2" => workData.Gr2.ToString(),
            "GR3" => workData.Gr3.ToString(),
            "GR4" => workData.Gr4.ToString(),
            "GR5" => workData.Gr5.ToString(),
            "LK1" => workData.Lk1.ToString(),
            "LK2" => workData.Lk2.ToString(),
            "LK3" => workData.Lk3.ToString(),
            "LK4" => workData.Lk4.ToString(),
            "LK5" => workData.Lk5.ToString(),
            "L" => workData.L.ToString("F1"),
            "B" => workData.B.ToString("F1"),
            "TMAX" => workData.Tmax.ToString("F1"),
            "T1" => workData.T1.ToString("F1"),
            "T2" => workData.T2.ToString("F1"),
            "T3" => workData.T3.ToString("F1"),
            "T4" => workData.T4.ToString("F1"),
            "T5" => workData.T5.ToString("F1"),
            "IT1" => workData.It1.ToString("F1"),
            "IT2" => workData.It2.ToString("F1"),
            "IT3" => workData.It3.ToString("F1"),
            "IT4" => workData.It4.ToString("F1"),
            "SP1" => workData.Sp1.ToString("F1"),
            "SP2" => workData.Sp2.ToString("F1"),
            "SP3" => workData.Sp3.ToString("F1"),
            "SP4" => workData.Sp4.ToString("F1"),
            "SP5" => workData.Sp5.ToString("F1"),
            "LH1" => workData.Lh1.ToString("F1"),
            "LH2" => workData.Lh2.ToString("F1"),
            "LH3" => workData.Lh3.ToString("F1"),
            "LH4" => workData.Lh4.ToString("F1"),
            "LH5" => workData.Lh5.ToString("F1"),
            "LT1" => workData.Lt1.ToString("F1"),
            "LT2" => workData.Lt2.ToString("F1"),
            "LT3" => workData.Lt3.ToString("F1"),
            "LT4" => workData.Lt4.ToString("F1"),
            "LT5" => workData.Lt5.ToString("F1"),
            "LL1" => workData.Ll1.ToString("F1"),
            "LL2" => workData.Ll2.ToString("F1"),
            "LL3" => workData.Ll3.ToString("F1"),
            "LL4" => workData.Ll4.ToString("F1"),
            "LL5" => workData.Ll5.ToString("F1"),
            "WL1" => workData.Wl1.ToString("F1"),
            "WL2" => workData.Wl2.ToString("F1"),
            "WL3" => workData.Wl3.ToString("F1"),
            "WL4" => workData.Wl4.ToString("F1"),
            "WL5" => workData.Wl5.ToString("F1"),
            "IS1" => workData.Is1.ToString("F1"),
            "STP1" => workData.Stp1.ToString("F1"),
            "STP2" => workData.Stp2.ToString("F1"),
            "STP3" => workData.Stp3.ToString("F1"),
            "STP4" => workData.Stp4.ToString("F1"),
            "STP5" => workData.Stp5.ToString("F1"),
            "ORG" => workData.Org.ToString("F1"),
            _ => ""
        };
    }
}