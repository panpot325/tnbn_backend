using System.Linq;
using System.Text;
using BackendMonitor.share;

namespace BackendMonitor.type.singleton;

/// <summary>
/// @加工ワークデータクラス
/// </summary>
public class WorkData {
    /// Static Instance
    private static readonly WorkData _instance = new();

    /// Instance Getter
    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public static WorkData I => _instance;

    public static bool Exist => _instance._exist;

    /// <summary>
    /// Private Constructor
    /// </summary>
    private WorkData() {
    }

    /// Members
    public string SNO; //

    public string BLK; //
    public string BZI; //
    public string PCS; //
    public byte Gr1; //
    public byte Gr2; //
    public byte Gr3; //
    public byte Gr4; //
    public byte Gr5; //

    public byte Lk1; //
    public byte Lk2; //
    public byte Lk3; //
    public byte Lk4; //
    public byte Lk5; //

    public decimal L; //
    public decimal B; //
    public decimal Tmax; //

    public decimal T1; //
    public decimal T2; //
    public decimal T3; //
    public decimal T4; //
    public decimal T5; //

    public decimal It1; //
    public decimal It2; //
    public decimal It3; //
    public decimal It4; //

    public decimal Sp1; //
    public decimal Sp2; //
    public decimal Sp3; //
    public decimal Sp4; //
    public decimal Sp5; //

    public decimal Lh1; //
    public decimal Lh2; //
    public decimal Lh3; //
    public decimal Lh4; //
    public decimal Lh5; //

    public decimal Lt1; //
    public decimal Lt2; //
    public decimal Lt3; //
    public decimal Lt4; //
    public decimal Lt5; //

    public decimal Ll1; //
    public decimal Ll2; //
    public decimal Ll3; //
    public decimal Ll4; //
    public decimal Ll5; //

    public byte Wl1; //
    public byte Wl2; //
    public byte Wl3; //
    public byte Wl4; //
    public byte Wl5; //

    public decimal Is1; //

    public decimal Stp1; //
    public decimal Stp2; //
    public decimal Stp3; //
    public decimal Stp4; //
    public decimal Stp5; //

    public byte Org; //

    public int Yoteibi_Kari; //
    public int Yoteibi_Hon; //
    public int Yoteibi_Kyosei; //
    public int Jissibi_Kari; //
    public int Jissibi_Hon; //
    public int Jissibi_Kyosei; //

    public byte Status_Kari; //
    public byte Status_Hon; //
    public byte Status_Kyosei; //

    public int Create_Date; //
    public int Create_Syain; //
    public int Update_Date; //
    public int Update_Syain; //

    public byte Chg_Flg; //

    private bool _exist;

    /// <summary>
    /// Fetch
    /// </summary>
    public WorkData Fetch(string sno, string blk, string bzi, string pcs) {
        _exist = false;
        var i = 0;
        var j = 4;
        var types = WorkDataTypes.List
            .Select(value => value.Nyu_Tani == 1 ? true : false)
            .ToList();

        var sb = new StringBuilder();
        sb.Append(" SElECT sno, blk, bzi, pcs,");
        sb.Append(" gr1, gr2, gr3, gr4, gr5,");
        sb.Append(" lk1, lk2, lk3, lk4, lk5,");
        sb.Append(" l, b, tmax,");
        sb.Append(" t1, t2, t3, t4, t5,");
        sb.Append(" it1, it2, it3, it4,");
        sb.Append(" sp1, sp2, sp3, sp4, sp5,");
        sb.Append(" lh1, lh2, lh3, lh4, lh5,");
        sb.Append(" lt1, lt2, lt3, lt4, lt5,");
        sb.Append(" ll1, ll2, ll3, ll4, ll5,");
        sb.Append(" wl1, wl2, wl3, wl4, wl5,");
        sb.Append(" is1,");
        sb.Append(" stp1, stp2, stp3, stp4, stp5,");
        sb.Append(" org");
        sb.Append(" FROM tnbn_kakowk_data");
        sb.Append($" WHERE sno = '{sno}'");
        sb.Append($" AND blk = '{blk}'");
        sb.Append($" AND bzi = '{bzi}'");
        sb.Append($" AND pcs = '{pcs}'");
        using var reader = PgConnect.Read(sb.ToString());
        while (reader.Read()) {
            SNO = reader.GetString(i++);
            BLK = reader.GetString(i++);
            BZI = reader.GetString(i++);
            PCS = reader.GetString(i++);

            Gr1 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Gr2 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Gr3 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Gr4 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Gr5 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);

            Lk1 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Lk2 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Lk3 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Lk4 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Lk5 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);

            L = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            B = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Tmax = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            T1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            T2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            T3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            T4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            T5 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            It1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            It2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            It3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            It4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Sp1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Sp2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Sp3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Sp4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Sp5 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Lh1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lh2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lh3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lh4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lh5 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Lt1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lt2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lt3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lt4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Lt5 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Ll1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Ll2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Ll3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Ll4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Ll5 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Wl1 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Wl2 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Wl3 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Wl4 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            Wl5 = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);

            Is1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Stp1 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Stp2 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Stp3 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Stp4 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;
            Stp5 = types[j++] ? reader.GetDecimal(i++) : reader.GetDecimal(i++) * 10;

            Org = types[j++] ? reader.GetByte(i++) : (byte)(reader.GetByte(i++) * 10);
            _exist = true;
        }

        PgConnect.Close();

        return this;
    }

    /// <summary>
    /// @ステータス更新
    /// </summary>
    public static void UpdateStatus(int status, int unit, string sno, string blk, string bzi, string pcs) {
        var sb = new StringBuilder();
        sb.Append("UPDATE tnbn_kakowk_data SET");

        switch (unit) {
            case 2: {
                sb.Append($" status_kari = {status}");
                if (status == 2) {
                    sb.Append(", jissibi_kari = to_number(to_char(current_timestamp,'YYYYMMDD'), '00000000')");
                    sb.Append(", jissijkn_kari = to_number(to_char(current_timestamp,'HH24MISS'), '00000000')");
                }

                break;
            }
            case 3: {
                sb.Append($" status_hon = {status}");
                if (status == 2) {
                    sb.Append(", jissibi_hon = to_number(to_char(current_timestamp,'YYYYMMDD'), '00000000')");
                    sb.Append(", jissijkn_hon = to_number(to_char(current_timestamp,'HH24MISS'), '00000000')");
                }

                break;
            }
            default: {
                sb.Append($" status_kyosei = {status}");
                if (status == 2) {
                    sb.Append(", jissibi_kyosei = to_number(to_char(current_timestamp,'YYYYMMDD'), '00000000')");
                    sb.Append(", jissijkn_kyosei = to_number(to_char(current_timestamp,'HH24MISS'), '00000000')");
                }

                break;
            }
        }

        sb.Append($" WHERE sno = '{sno}'");
        sb.Append($" AND blk = '{blk}'");
        sb.Append($" AND bzi = '{bzi}'");
        sb.Append($" AND pcs = '{pcs}'");

        var rowsAffected = PgConnect.Update(sb.ToString());
        Log.WriteLine(@"rowsAffected = " + rowsAffected);
    }
}