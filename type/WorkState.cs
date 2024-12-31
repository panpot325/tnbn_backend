using System;
using System.Text;
using BackendMonitor.share;

namespace BackendMonitor.type;

/// <summary>
/// 稼動状況監視用クラス
/// </summary>
public partial class WorkState(int unit) {
    public int Start_Count; //稼動開始Cnt
    public int End_Count; //稼動終了Cnt
    public string SNO; //
    public string BLK; //
    public string BZI; //
    public string PCS; //
    public decimal L; //
    public decimal B; //
    public decimal Tmax; //Tmax
    public int Count; //本数
    public string YMD; //
    public string StrTime; //稼動開始Time
    public string StrTime2; //稼動開始Time2
    public string EndTime; //稼動終了Time
    public int KAD_TIME; //
    public int Ttl_TIME; //
    public int Stp_Time; //
    public byte CNT; //
    public bool EndState; //終了判定
    public int Unit = unit; //装置番号

    /// <summary>
    /// 初期化
    /// </summary>
    public void Clear() {
        SNO = " ";
        BLK = " ";
        BZI = " ";
        PCS = " ";
        L = 0;
        B = 0;
        Tmax = 0;
        Count = 0;
        YMD = " ";
        StrTime = "00:00:00";
        StrTime2 = "00:00:00";
        EndTime = "00:00:00";
        KAD_TIME = 0;
        Ttl_TIME = 0;
        Stp_Time = 0;
        CNT = 0;
    }

    /// <summary>
    /// Set
    /// </summary>
    /// <param name="sno"></param>
    /// <param name="blk"></param>
    /// <param name="bzi"></param>
    /// <param name="pcs"></param>
    /// <returns></returns>
    public WorkState Set(string sno, string blk, string bzi, string pcs) {
        SNO = sno;
        BLK = blk;
        BZI = bzi;
        PCS = pcs;
        return this;
    }

    /// <summary>
    /// @稼動実績WKから取得
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="endCheck"></param>
    /// <returns></returns>
    public WorkState Fetch_Record(int unit, bool endCheck = false) {
        var sb = new StringBuilder();

        sb.Append(" SElECT sno, blk, bzi, pcs, l, b, tmax, honsu, ymd,");
        sb.Append(" str_time, str_time2, end_time, kad_time, ttl_time, stp_time, cnt");
        sb.Append(" FROM tnbn_kadojisseki_wk");
        sb.Append($" WHERE taisyo = '{unit}'");

        if (!endCheck) {
            Clear();
        }

        EndState = endCheck;
        using var reader = PgConnect.Read(sb.ToString());
        while (reader.Read()) {
            SNO = reader.GetString(0).Trim();
            BLK = reader.GetString(1).Trim();
            BZI = reader.GetString(2).Trim();
            PCS = reader.GetString(3).Trim();
            L = reader.GetDecimal(4);
            B = reader.GetDecimal(5);
            Tmax = reader.GetDecimal(6);
            Count = reader.GetInt16(7);
            YMD = reader.GetString(8);
            StrTime = reader.GetString(9);
            StrTime2 = reader.GetString(10);
            EndTime = reader.GetString(11);
            if (endCheck) {
                EndState = EndTime != "00:00:00";
            }
            else {
                KAD_TIME = reader.GetInt32(12);
                Ttl_TIME = reader.GetInt32(13);
                Stp_Time = reader.GetInt32(14);
            }

            CNT = reader.GetByte(15);
        }

        PgConnect.Close();

        return this;
    }

    /// <summary>
    /// @加工ワークデータから取得
    /// </summary>
    /// <returns></returns>
    public WorkState Fetch_WorkData() {
        var count = 0;
        var sb = new StringBuilder();

        sb.Append(" SElECT l, b, tmax, lh1, lh2, lh3, lh4, lh5");
        sb.Append(" FROM tnbn_kakowk_data");
        sb.Append($" WHERE sno = '{SNO.Trim()}'");
        sb.Append($" AND blk = '{BLK.Trim()}'");
        sb.Append($" AND bzi = '{BZI.Trim()}'");
        sb.Append($" AND pcs = '{PCS.Trim()}'");

        using var reader = PgConnect.Read(sb.ToString());
        while (reader.Read()) {
            L = reader.GetDecimal(0);
            B = reader.GetDecimal(1);
            Tmax = reader.GetDecimal(2);
            if (reader.GetDecimal(3) > 0) {
                count++;
            }

            if (reader.GetDecimal(4) > 0) {
                count++;
            }

            if (reader.GetDecimal(5) > 0) {
                count++;
            }

            if (reader.GetDecimal(6) > 0) {
                count++;
            }

            if (reader.GetDecimal(7) > 0) {
                count++;
            }
        }

        Count = count;
        PgConnect.Close();
        return this;
    }

    /// <summary>
    /// @稼動実績WKを初期化
    /// </summary>
    public void Update_Init() {
        var sb = new StringBuilder();

        sb.Append("UPDATE tnbn_kadojisseki_wk SET");
        sb.Append($" sno = '{SNO.Trim()}',");
        sb.Append($" blk = '{BLK.Trim()}',");
        sb.Append($" bzi = '{BZI.Trim()}',");
        sb.Append($" pcs = '{PCS.Trim()}',");
        sb.Append($" l = {L},");
        sb.Append($" b = {B},");
        sb.Append($" tmax = {Tmax},");
        sb.Append($" honsu = {Count},");
        sb.Append($" ymd = '{YMD}',");
        sb.Append($" str_time = '{StrTime}',");
        sb.Append($" str_time2 = '{StrTime2}',");
        sb.Append($" end_time = '00:00:00',");
        sb.Append(" ttl_time = 0,");
        sb.Append(" kad_time = 0,");
        sb.Append(" stp_time = 0,");
        sb.Append(" cnt = 0");
        sb.Append($" WHERE taisyo = '{Unit}'");
        PgConnect.Update(sb.ToString());
        PgConnect.Close();
        Log.Sub_LogWrite($@"【稼動実績WKを初期化】{sb.ToString()}");
    }

    /// <summary>
    /// @稼動実績WKを更新
    /// </summary>
    public void Update_StrTime2() {
        var sb = new StringBuilder();
        sb.Append("UPDATE tnbn_kadojisseki_wk SET");
        sb.Append($" str_time2 = '{StrTime2}'");
        sb.Append($" WHERE taisyo = '{Unit}'");
        PgConnect.Update(sb.ToString());
        PgConnect.Close();
        Log.Sub_LogWrite($@"【稼動実績WKを更新】{sb.ToString()}");
    }

    /// <summary>
    /// 稼動実績WKを更新終了
    /// </summary>
    public void Update_End() {
        //稼動終了、稼動終了時の時間と稼働時間の算出
        if (Diff() < 0) {
            EndTime = "19:20:00";
        }

        Ttl_TIME = Diff();

        if (Diff2() < 0) {
            EndTime = "19:20:00";
        }

        KAD_TIME += Diff2();
        Stp_Time = Ttl_TIME - KAD_TIME;
        CNT++;

        var sb = new StringBuilder();
        sb.Append("UPDATE tnbn_kadojisseki_wk SET");
        sb.Append($" end_time = '{EndTime}',");
        sb.Append($" ttl_time = {Ttl_TIME},");
        sb.Append($" kad_time = {KAD_TIME},");
        sb.Append($" stp_time = {Stp_Time},");
        sb.Append($" cnt = {CNT}");
        sb.Append($" WHERE taisyo = '{Unit}'");
        PgConnect.Update(sb.ToString());
        PgConnect.Close();
    }

    /// <summary>
    /// Diff
    /// </summary>
    /// <returns></returns>
    private int Diff() {
        var interval　= DateTime.Parse(EndTime) - DateTime.Parse(StrTime);

        return (int)interval.TotalSeconds;
    }

    /// <summary>
    /// Diff2
    /// </summary>
    /// <returns></returns>
    private int Diff2() {
        var interval　= DateTime.Parse(EndTime) - DateTime.Parse(StrTime2);

        return (int)interval.TotalSeconds;
    }
}