using System.Text;
using BackendMonitor.share;

namespace BackendMonitor.type;

/// <summary>
/// 稼動状況監視用クラス
/// @Deprecated
/// </summary>
public partial class WorkState {
    /// <summary>
    /// Update_Clear
    /// @Deprecated
    /// </summary>
    /// <param name="unit"></param>
    public static void Update_Clear(int unit) {
        var sb = new StringBuilder();

        sb.Append("UPDATE tnbn_kadojisseki_wk SET");
        sb.Append(" sno = '',");
        sb.Append(" blk = '',");
        sb.Append(" bzi = '',");
        sb.Append(" pcs = '',");
        sb.Append(" l = 0,");
        sb.Append(" b = 0,");
        sb.Append(" tmax = 0,");
        sb.Append(" honsu = 0,");
        sb.Append(" ymd = '',");
        sb.Append($" str_time = '{CLR_TIME}',");
        sb.Append($" str_time2 = '{CLR_TIME}',");
        sb.Append($" end_time = '{CLR_TIME}',");
        sb.Append($" ttl_time = 0,");
        sb.Append($" kad_time = 0,");
        sb.Append($" stp_time = 0,");
        sb.Append($" cnt = 0");
        sb.Append($" WHERE taisyo = '{unit}'");

        PgConnect.Update(sb.ToString());
        PgConnect.Close();
        Log.Sub_LogWrite($@"【稼動実績WKをクリア】{sb.ToString()}");
    }

    /// <summary>
    /// Fetch
    /// @Deprecated
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static WorkState RecState(int unit) {
        var sb = new StringBuilder();
        sb.Append(" SElECT sno, blk, bzi, pcs, l, b, tmax, honsu, ymd,");
        sb.Append(" str_time, str_time2, end_time, kad_time, ttl_time, stp_time, cnt");
        sb.Append(" FROM tnbn_kadojisseki_wk");
        sb.Append($" WHERE taisyo = '{unit}'");

        var dataTable = PgOpen.PgSelect(sb.ToString());
        if (dataTable.Rows.Count == 0) {
            WorkStates.List[0].Clear();
            return WorkStates.List[0];
        }

        var row = dataTable.Rows[0];
        return new WorkState(unit) {
            Unit = unit,
            SNO = ((string)row["sno"]).Trim(),
            BLK = ((string)row["blk"]).Trim(),
            BZI = ((string)row["bzi"]).Trim(),
            PCS = ((string)row["pcs"]).Trim(),
            L = (decimal)row["l"],
            B = (decimal)row["b"],
            Tmax = (decimal)row["tmax"],
            Count = (short)row["honsu"],
            YMD = (string)row["ymd"],
            StrTime = (string)row["str_time"],
            StrTime2 = (string)row["str_time2"],
            EndTime = (string)row["end_time"],
            KAD_TIME = (int)row["kad_time"],
            Ttl_TIME = (int)row["ttl_time"],
            Stp_Time = (int)row["stp_time"],
            CNT = (short)row["cnt"],
        };
    }
}