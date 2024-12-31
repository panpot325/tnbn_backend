using System.Text;
using BackendMonitor.share;
using G = BackendMonitor.share.Globals;

namespace BackendMonitor.type;

/// <summary>
/// 稼動状況監視用クラス
/// </summary>
public partial class WorkState {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    public static void Update_Clear(int unit) {
        var sb = new StringBuilder();

        sb.Append("UPDATE tnbn_kadojisseki_wk SET");
        sb.Append(" sno = ' ',");
        sb.Append(" blk = ' ',");
        sb.Append(" bzi = ' ',");
        sb.Append(" pcs = ' ',");
        sb.Append(" l = 0,");
        sb.Append(" b = 0,");
        sb.Append(" tmax = 0,");
        sb.Append(" honsu = 0,");
        sb.Append(" ymd = ' ',");
        sb.Append(" str_time = '00:00:00',");
        sb.Append(" str_time2 = '00:00:00',");
        sb.Append(" end_time = '00:00:00',");
        sb.Append($" ttl_time = 0,");
        sb.Append($" kad_time = 0,");
        sb.Append($" stp_time = 0,");
        sb.Append($" cnt = 0");
        sb.Append($" WHERE taisyo = '{unit}'");

        PgConnect.Update(sb.ToString());
        PgConnect.Close();
        Log.Sub_LogWrite($@"【稼動実績WKをクリア】{sb.ToString()}");
    }
}