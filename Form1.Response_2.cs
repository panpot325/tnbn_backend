using System;
using System.Globalization;
using System.IO;
using BackendMonitor.Properties;
using BackendMonitor.type;
using BackendMonitor.type.singleton;
using C = BackendMonitor.share.Constants;

// ReSharper disable ConvertIfStatementToReturnStatement
// ReSharper disable InvertIf

namespace BackendMonitor;

/// <summary>
/// Response 2
/// </summary>
public partial class Form1 {
    /// <summary>
    /// @ログファイル読込
    /// </summary>
    /// <param name="unit"></param>
    public void ReadLogFile(int unit) {
        var workState = WorkStates.List[_unit];
        var fileName = GetFilePath();
        
        if (string.IsNullOrEmpty(fileName)) return;
        if (File.Exists(fileName)) {
            Log.Read(fileName);
        }
        else {
            Log.Clear();
        }

        var min0 = workState.Ttl_TIME / 60;
        var sec0 = workState.Ttl_TIME - min0 * 60;

        var min1 = workState.KAD_TIME / 60;
        var sec1 = workState.KAD_TIME - min1 * 60;

        var min2 = workState.Stp_Time / 60;
        var sec2 = workState.Stp_Time - min2 * 60;

        Log.Add(
            new Log {
                SNO = workState.SNO,
                BLK = workState.BLK,
                BZI = workState.BZI,
                PCS = workState.PCS,
                L = workState.L.ToString(CultureInfo.CurrentCulture),
                B = workState.B.ToString(CultureInfo.CurrentCulture),
                Tmax = workState.Tmax.ToString(CultureInfo.CurrentCulture),
                Maisu = "1",
                Honsu = workState.Count.ToString(),
                YMD = workState.YMD,
                StrTime = workState.StrTime,
                EndTime = workState.EndTime,
                TotTime = $"{min0}m {sec0}s",
                KadTime = $"{min1}m {sec1}s",
                StpTime = $"{min2}m {sec2}s"
            }
        );
    }

    /// <summary>
    /// @ログファイル出力
    /// </summary>
    /// <param name="unit"></param>
    public void WriteLogFile(int unit) {
        var fileName = GetFilePath();
        if (string.IsNullOrEmpty(fileName)) return;
        Log.CreateDir(Settings.Default.Log_Path);
        Log.Write(fileName);
    }

    /// <summary>
    /// GetFilePath
    /// </summary>
    /// <returns></returns>
    private string GetFilePath() {
        var workState = WorkStates.List[_unit];
        return _unit switch {
            C.UNIT_2 => $"{Settings.Default.Log_Path}\\{workState.SNO.Trim()}-{DateTime.Now:yyyy-MM}-仮付.csv",
            C.UNIT_3 => $"{Settings.Default.Log_Path}\\{workState.SNO.Trim()}-{DateTime.Now:yyyy-MM}-本付.csv",
            C.UNIT_5 => $"{Settings.Default.Log_Path}\\{workState.SNO.Trim()}-{DateTime.Now:yyyy-MM}-矯正.csv",
            _ => ""
        };
    }

    /// <summary>
    /// @回転処理
    /// </summary>
    public void Rotate() {
    }

    /// <summary>
    /// @ミラー反転処理
    /// </summary>
    public void Invert() {
        //現物を180度回転した値を計算

        //--- データタイプのLK1の配列番号を取得 -----
        var lk = DataTypesIndex("W715");

        //--- データタイプのWL1の配列番号を取得 -----
        var wl = DataTypesIndex("W745");

        //--- データタイプのSP1の配列番号を取得 -----
        var sp = DataTypesIndex("W72C");

        //データタイプのSP1〜SP5より最大のデフォルト値を取得
        var defMax = DataTypesDefMax(sp);

        //加工ワークデータを一時退避
        var bk = WorkData.I;
        var types = WorkDataTypes.List;

        //最終ロンジを取得
        var longi = DataTypesLastLongi(bk);

        //各項目の初期値を設定
        WorkData.I.Lk1 = bk.Lk1;
        WorkData.I.Lk2 = bk.Lk2;
        WorkData.I.Lk3 = bk.Lk3;
        WorkData.I.Lk4 = bk.Lk4;
        WorkData.I.Lk5 = bk.Lk5;

        WorkData.I.Sp1 = bk.Sp1;
        WorkData.I.Sp2 = bk.Sp2;
        WorkData.I.Sp3 = bk.Sp3;
        WorkData.I.Sp4 = bk.Sp4;
        WorkData.I.Sp5 = bk.Sp5;

        WorkData.I.Lh1 = bk.Lh1;
        WorkData.I.Lh2 = bk.Lh2;
        WorkData.I.Lh3 = bk.Lh3;
        WorkData.I.Lh4 = bk.Lh4;
        WorkData.I.Lh5 = bk.Lh5;

        WorkData.I.Lt1 = bk.Lt1;
        WorkData.I.Lt2 = bk.Lt2;
        WorkData.I.Lt3 = bk.Lt3;
        WorkData.I.Lt4 = bk.Lt4;
        WorkData.I.Lt5 = bk.Lt5;

        WorkData.I.Ll1 = bk.Ll1;
        WorkData.I.Ll2 = bk.Ll2;
        WorkData.I.Ll3 = bk.Ll3;
        WorkData.I.Ll4 = bk.Ll4;
        WorkData.I.Ll5 = bk.Ll5;

        WorkData.I.Wl1 = bk.Wl1;
        WorkData.I.Wl2 = bk.Wl2;
        WorkData.I.Wl3 = bk.Wl3;
        WorkData.I.Wl4 = bk.Wl4;
        WorkData.I.Wl5 = bk.Wl5;

        switch (longi) {
            case 5:
                //--- LH5入力有り -----
                //--- ロンジ形状を入替 -----
                WorkData.I.Lk1 = bk.Lk5;
                WorkData.I.Lk2 = bk.Lk4;
                WorkData.I.Lk3 = bk.Lk3;
                WorkData.I.Lk4 = bk.Lk2;
                WorkData.I.Lk5 = bk.Lk1;

                //--- ロンジスペースを算出 -----
                WorkData.I.Sp1 = bk.B - (bk.Sp1 + bk.Sp2 + bk.Sp3 + bk.Sp4 + bk.Sp5 + bk.Lt5);
                WorkData.I.Sp2 = bk.Sp5 - bk.Lt4 + bk.Lt5;
                WorkData.I.Sp3 = bk.Sp4 - bk.Lt3 + bk.Lt4;
                WorkData.I.Sp4 = bk.Sp3 - bk.Lt2 + bk.Lt3;
                WorkData.I.Sp5 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                //--- ロンジ高さを入替 -----
                WorkData.I.Lh1 = bk.Lh5;
                WorkData.I.Lh2 = bk.Lh4;
                WorkData.I.Lh3 = bk.Lh3;
                WorkData.I.Lh4 = bk.Lh2;
                WorkData.I.Lh5 = bk.Lh1;

                //--- ロンジ全長を入替 -----
                WorkData.I.Lt1 = bk.Lt5;
                WorkData.I.Ll2 = bk.Ll4;
                WorkData.I.Ll3 = bk.Ll3;
                WorkData.I.Ll4 = bk.Ll2;
                WorkData.I.Ll5 = bk.Ll1;

                //-- 溶接脚長を入替 -----
                WorkData.I.Wl1 = bk.Wl5;
                WorkData.I.Wl2 = bk.Wl4;
                WorkData.I.Wl3 = bk.Wl3;
                WorkData.I.Wl4 = bk.Wl2;
                WorkData.I.Wl5 = bk.Wl1;
                break;
            case 4:
                var wsp1 = (decimal)bk.B - (bk.Sp1 + bk.Sp2 + bk.Sp3 + bk.Sp4 + bk.Lt4);
                var wsu = (int)(Math.Truncate(wsp1));
                var wsp = wsp1 / wsu;

                switch (wsu) {
                    case 1:
                        WorkData.I.Lk1 = bk.Lk4;
                        WorkData.I.Lk2 = bk.Lk3;
                        WorkData.I.Lk3 = bk.Lk2;
                        WorkData.I.Lk4 = bk.Lk1;
                        WorkData.I.Lk5 = byte.Parse(types[lk + 4].Def);

                        WorkData.I.Sp1 = wsp;
                        WorkData.I.Sp2 = bk.Sp4 - bk.Lt3 + bk.Lt4;
                        WorkData.I.Sp3 = bk.Sp3 - bk.Lt2 + bk.Lt3;
                        WorkData.I.Sp4 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = bk.Lh4;
                        WorkData.I.Lh2 = bk.Lh3;
                        WorkData.I.Lh3 = bk.Lh2;
                        WorkData.I.Lh4 = bk.Lh1;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Lt1 = bk.Lt4;
                        WorkData.I.Lt2 = bk.Lt3;
                        WorkData.I.Lt3 = bk.Lt2;
                        WorkData.I.Lt4 = bk.Lt1;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Ll1 = bk.Ll4;
                        WorkData.I.Ll2 = bk.Ll3;
                        WorkData.I.Ll3 = bk.Ll2;
                        WorkData.I.Ll4 = bk.Ll1;
                        WorkData.I.Ll5 = 0;

                        WorkData.I.Wl1 = bk.Wl4;
                        WorkData.I.Wl2 = bk.Wl3;
                        WorkData.I.Wl3 = bk.Wl2;
                        WorkData.I.Wl4 = bk.Wl1;
                        WorkData.I.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    default:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = bk.Lk4;
                        WorkData.I.Lk3 = bk.Lk3;
                        WorkData.I.Lk4 = bk.Lk2;
                        WorkData.I.Lk5 = bk.Lk1;
                        if ((wsp1 - (wsp * 1)) < types[sp + 1].Hani_Min2) {
                            WorkData.I.Sp1 = decimal.Parse(types[sp].Def);
                            WorkData.I.Sp2 = wsp1 - WorkData.I.Sp1;
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp1 - (wsp * 1);
                        }

                        WorkData.I.Sp3 = bk.Sp4 - bk.Lt3 + bk.Lt4;
                        WorkData.I.Sp4 = bk.Sp3 - bk.Lt2 + bk.Lt3;
                        WorkData.I.Sp5 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = bk.Lh4;
                        WorkData.I.Lh3 = bk.Lh3;
                        WorkData.I.Lh4 = bk.Lh2;
                        WorkData.I.Lh5 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = bk.Lt4;
                        WorkData.I.Lt3 = bk.Lt3;
                        WorkData.I.Lt4 = bk.Lt2;
                        WorkData.I.Lt5 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = bk.Ll4;
                        WorkData.I.Ll3 = bk.Ll3;
                        WorkData.I.Ll4 = bk.Ll2;
                        WorkData.I.Ll5 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        ;
                        WorkData.I.Wl2 = bk.Wl4;
                        WorkData.I.Wl3 = bk.Wl3;
                        WorkData.I.Wl4 = bk.Wl2;
                        WorkData.I.Wl5 = bk.Wl1;
                        break;
                }


                break;
            case 3:
                //--- LH3入力有り(LH4=0,LH5=0) -----
                wsp1 = (decimal)(bk.B - (bk.Sp1 + bk.Sp2 + bk.Sp3 + bk.Lt3));
                wsu = (int)(Math.Truncate(wsp1 / decimal.Parse(defMax) + 1));
                wsp = wsp1 / wsu;
                switch (wsu) {
                    case 1:
                        WorkData.I.Lk1 = bk.Lk3;
                        WorkData.I.Lk2 = bk.Lk3;
                        WorkData.I.Lk3 = bk.Lk3;
                        WorkData.I.Lk4 = byte.Parse(types[lk + 3].Def);
                        WorkData.I.Lk5 = byte.Parse(types[lk + 4].Def);

                        WorkData.I.Sp1 = wsp;
                        WorkData.I.Sp2 = bk.Sp3 - bk.Lt2 + bk.Lt3;
                        WorkData.I.Sp3 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = bk.Lh3;
                        WorkData.I.Lh2 = bk.Lh2;
                        WorkData.I.Lh3 = bk.Lh1;
                        WorkData.I.Lh4 = 0;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Lt1 = bk.Lt3;
                        WorkData.I.Lt2 = bk.Lt2;
                        WorkData.I.Lt3 = bk.Lt1;
                        WorkData.I.Lt4 = 0;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Ll1 = bk.Ll3;
                        WorkData.I.Ll2 = bk.Ll2;
                        WorkData.I.Ll3 = bk.Ll1;
                        WorkData.I.Ll4 = 0;
                        WorkData.I.Ll5 = 0;

                        WorkData.I.Wl1 = bk.Wl3;
                        WorkData.I.Wl2 = bk.Wl2;
                        WorkData.I.Wl3 = bk.Wl1;
                        WorkData.I.Wl4 = byte.Parse(types[wl + 3].Def);
                        WorkData.I.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    case 2:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = bk.Lk3;
                        WorkData.I.Lk3 = bk.Lk2;
                        WorkData.I.Lk4 = bk.Lk1;
                        WorkData.I.Lk5 = byte.Parse(types[lk + 4].Def);

                        if ((wsp1 - (wsp * 1)) < types[sp + 1].Hani_Min2) {
                            WorkData.I.Sp1 = decimal.Parse(types[sp].Def);
                            WorkData.I.Sp2 = wsp1 - WorkData.I.Sp1;
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp1 - (wsp * 1);
                        }

                        WorkData.I.Sp3 = bk.Sp3 - bk.Lt2 + bk.Lt3;
                        WorkData.I.Sp4 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = bk.Lh3;
                        WorkData.I.Lh3 = bk.Lh2;
                        WorkData.I.Lh4 = bk.Lh1;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = bk.Lt3;
                        WorkData.I.Lt3 = bk.Lt2;
                        WorkData.I.Lt4 = bk.Lt1;
                        WorkData.I.Lt5 = 0;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = bk.Ll3;
                        WorkData.I.Ll3 = bk.Ll2;
                        WorkData.I.Ll4 = bk.Ll1;
                        WorkData.I.Ll5 = 0;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        ;
                        WorkData.I.Wl2 = bk.Wl3;
                        WorkData.I.Wl3 = bk.Wl2;
                        WorkData.I.Wl4 = bk.Wl1;
                        WorkData.I.Wl5 = byte.Parse(types[wl + 4].Def);
                        break;
                    default:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = byte.Parse(types[lk + 1].Def);
                        WorkData.I.Lk3 = bk.Lk3;
                        WorkData.I.Lk4 = bk.Lk2;
                        WorkData.I.Lk5 = bk.Lk1;

                        if (wsp1 < types[sp + 1].Hani_Min2
                            || wsp1 - (wsp * 2) < types[sp + 2].Hani_Min2) {
                            WorkData.I.Sp1 = decimal.Parse(types[sp].Def);
                            WorkData.I.Sp2 = decimal.Parse(types[sp + 1].Def);
                            WorkData.I.Sp3 = wsp1 - (WorkData.I.Sp1 + WorkData.I.Sp2);
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp;
                            WorkData.I.Sp3 = (wsp1 - (wsp * 2));
                        }

                        WorkData.I.Sp4 = bk.Sp3 - bk.Lt2 + bk.Lt3;
                        WorkData.I.Sp5 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = 0;
                        WorkData.I.Lh3 = bk.Lh3;
                        WorkData.I.Lh4 = bk.Lh2;
                        WorkData.I.Lh5 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = 0;
                        WorkData.I.Lt3 = bk.Lt3;
                        WorkData.I.Lt4 = bk.Lt2;
                        WorkData.I.Lt5 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = 0;
                        WorkData.I.Ll3 = bk.Ll3;
                        WorkData.I.Ll4 = bk.Ll2;
                        WorkData.I.Ll5 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        ;
                        WorkData.I.Wl2 = byte.Parse(types[wl + 1].Def);
                        WorkData.I.Wl3 = bk.Wl3;
                        WorkData.I.Wl4 = bk.Wl2;
                        WorkData.I.Wl5 = bk.Wl1;

                        break;
                }

                break;
            case 2:
                //--- LH2入力有り(LH3=0,LH4=0,LH5=0) -----
                wsp1 = (decimal)(bk.B - (bk.Sp1 + bk.Sp2 + bk.Lt2));
                wsu = (int)(Math.Truncate(wsp1 / decimal.Parse(defMax) + 1));
                wsp = wsp1 / wsu;

                switch (wsu) {
                    case 1:
                        WorkData.I.Lk1 = bk.Lk2;
                        WorkData.I.Lk2 = bk.Lk1;
                        WorkData.I.Lk3 = byte.Parse(types[lk + 2].Def);
                        WorkData.I.Lk4 = byte.Parse(types[lk + 3].Def);
                        WorkData.I.Lk5 = byte.Parse(types[lk + 4].Def);

                        WorkData.I.Sp1 = wsp;
                        WorkData.I.Sp2 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = bk.Lh2;
                        WorkData.I.Lh2 = bk.Lh1;
                        WorkData.I.Lh3 = 0;
                        WorkData.I.Lh4 = 0;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Lt1 = bk.Lt2;
                        WorkData.I.Lt2 = bk.Lt1;
                        WorkData.I.Lt3 = 0;
                        WorkData.I.Lt4 = 0;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Ll1 = bk.Ll2;
                        WorkData.I.Ll2 = bk.Ll1;
                        WorkData.I.Ll3 = 0;
                        WorkData.I.Ll4 = 0;
                        WorkData.I.Ll5 = 0;

                        WorkData.I.Wl1 = bk.Wl2;
                        WorkData.I.Wl2 = bk.Wl1;
                        WorkData.I.Wl3 = byte.Parse(types[wl + 2].Def);
                        WorkData.I.Wl4 = byte.Parse(types[wl + 3].Def);
                        WorkData.I.Wl5 = byte.Parse(types[wl + 4].Def);
                        break;
                    case 2:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = bk.Lk2;
                        WorkData.I.Lk3 = bk.Lk1;
                        WorkData.I.Lk4 = byte.Parse(types[lk + 3].Def);
                        WorkData.I.Lk5 = byte.Parse(types[lk + 4].Def);

                        if (wsp1 < types[sp + 1].Hani_Min2
                            || wsp1 - (wsp * (wsu - 1)) < types[sp + 2].Hani_Min2) {
                            WorkData.I.Sp1 = decimal.Parse(types[sp].Def);
                            WorkData.I.Sp2 = decimal.Parse(types[sp + 1].Def);
                            WorkData.I.Sp3 = wsp1 - (WorkData.I.Sp1 + WorkData.I.Sp2);
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp;
                            WorkData.I.Sp3 = wsp1 - (wsp * (wsu - 1));
                        }

                        WorkData.I.Sp4 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = 0;
                        WorkData.I.Lh3 = bk.Lh2;
                        WorkData.I.Lh4 = bk.Lh1;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = 0;
                        WorkData.I.Lt3 = bk.Lt2;
                        WorkData.I.Lt4 = bk.Lt1;
                        WorkData.I.Lt5 = 0;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = 0;
                        WorkData.I.Ll3 = bk.Ll2;
                        WorkData.I.Ll4 = bk.Ll1;
                        WorkData.I.Ll5 = 0;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        ;
                        WorkData.I.Wl2 = byte.Parse(types[wl + 1].Def);
                        WorkData.I.Wl3 = bk.Wl2;
                        WorkData.I.Wl4 = bk.Wl1;
                        WorkData.I.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    case 3:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = byte.Parse(types[lk + 1].Def);
                        WorkData.I.Lk3 = bk.Lk2;
                        WorkData.I.Lk4 = bk.Lk1;
                        WorkData.I.Lk5 = byte.Parse(types[lk + 4].Def);

                        if ((wsp1 - (wsp * 1)) < types[sp + 1].Hani_Min2) {
                            WorkData.I.Sp1 = decimal.Parse(types[sp].Def);
                            WorkData.I.Sp2 = wsp1 - WorkData.I.Sp1;
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = (wsp1 - (wsp * (wsu - 1)));
                        }

                        WorkData.I.Sp3 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = bk.Lh2;
                        WorkData.I.Lh3 = bk.Lh1;
                        WorkData.I.Lh4 = 0;
                        WorkData.I.Lh5 = 0;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = bk.Lt2;
                        WorkData.I.Lt3 = bk.Lt1;
                        WorkData.I.Lt4 = 0;
                        WorkData.I.Lt5 = 0;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = bk.Ll2;
                        WorkData.I.Ll3 = bk.Ll1;
                        WorkData.I.Ll4 = 0;
                        WorkData.I.Ll5 = 0;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        ;
                        WorkData.I.Wl2 = bk.Wl2;
                        WorkData.I.Wl3 = bk.Wl1;
                        WorkData.I.Wl4 = byte.Parse(types[wl + 3].Def);
                        WorkData.I.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    default:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = byte.Parse(types[lk + 1].Def);
                        WorkData.I.Lk3 = byte.Parse(types[lk + 2].Def);
                        WorkData.I.Lk4 = bk.Lk2;
                        WorkData.I.Lk5 = bk.Lk1;

                        if (wsp1 < types[sp + 1].Hani_Min2
                            || wsp1 < types[sp + 2].Hani_Min2
                            || (wsp * 3) < types[sp + 3].Hani_Min2) {
                            WorkData.I.Sp1 = decimal.Parse(types[sp].Def);
                            WorkData.I.Sp2 = decimal.Parse(types[sp + 1].Def);
                            WorkData.I.Sp3 = decimal.Parse(types[sp + 2].Def);
                            WorkData.I.Sp4 = wsp1 - (WorkData.I.Sp1 + WorkData.I.Sp2 + WorkData.I.Sp3);
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp;
                            WorkData.I.Sp3 = wsp;
                            WorkData.I.Sp4 = wsp1 - wsp * 3;
                        }

                        WorkData.I.Sp5 = bk.Sp2 - bk.Lt1 + bk.Lt2;

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = 0;
                        WorkData.I.Lh3 = 0;
                        WorkData.I.Lh4 = bk.Lh2;
                        WorkData.I.Lh5 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = 0;
                        WorkData.I.Lt3 = 0;
                        WorkData.I.Lt4 = bk.Lt2;
                        WorkData.I.Lt5 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = 0;
                        WorkData.I.Ll3 = 0;
                        WorkData.I.Ll4 = bk.Ll2;
                        WorkData.I.Ll5 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        WorkData.I.Wl2 = byte.Parse(types[wl + 1].Def);
                        WorkData.I.Wl3 = byte.Parse(types[wl + 2].Def);
                        WorkData.I.Wl4 = bk.Wl2;
                        WorkData.I.Wl5 = bk.Wl1;
                        break;
                }

                break;
            case 1:
                //--- LH1入力有り(LH2=0,LH3=0,LH4=0,LH5=0) -----
                wsp1 = (decimal)(bk.B - (bk.Sp1 + bk.Lt1));
                wsu = (int)(Math.Truncate(wsp1 / decimal.Parse(defMax) + 1));
                wsp = wsp1 / wsu;

                switch (wsu) {
                    case 1:
                        WorkData.I.Lk1 = bk.Lk1;
                        WorkData.I.Sp1 = (byte)wsp;
                        WorkData.I.Lh1 = bk.Lh1;
                        WorkData.I.Lt1 = bk.Lt1;
                        WorkData.I.Wl1 = bk.Wl1;
                        break;
                    case 2:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = bk.Lk1;
                        if (wsp1 - (wsp * (wsu - 1)) < types[sp + 1].Hani_Min2) {
                            WorkData.I.Sp1 = byte.Parse(types[sp].Def);
                            WorkData.I.Sp2 = wsp1 - WorkData.I.Sp1;
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp1 - (wsp * (wsu - 1));
                        }

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        WorkData.I.Wl2 = bk.Wl1;

                        break;
                    case 3:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = byte.Parse(types[lk + 1].Def);
                        WorkData.I.Lk2 = bk.Lk1;
                        if (wsp1 < types[sp + 1].Hani_Min2
                            || wsp1 - (wsp * (wsu - 1)) < types[sp + 2].Hani_Min2) {
                            WorkData.I.Sp1 = byte.Parse(types[sp].Def);
                            WorkData.I.Sp2 = byte.Parse(types[sp + 1].Def);
                            WorkData.I.Sp3 = wsp1 - (WorkData.I.Sp1 + WorkData.I.Sp2);
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp;
                            WorkData.I.Sp3 = (wsp1 - (wsp * (wsu - 1)));
                        }

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = 0;
                        WorkData.I.Lh3 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = 0;
                        WorkData.I.Lt3 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = 0;
                        WorkData.I.Ll3 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        WorkData.I.Wl2 = byte.Parse(types[wl + 1].Def);
                        WorkData.I.Wl3 = bk.Wl1;

                        break;
                    case 4:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = byte.Parse(types[lk + 1].Def);
                        WorkData.I.Lk3 = byte.Parse(types[lk + 2].Def);
                        WorkData.I.Lk4 = bk.Lk1;
                        if (wsp1 < types[sp + 1].Hani_Min2
                            || wsp1 < types[sp + 2].Hani_Min2
                            || (wsp1 - (wsp * (wsu - 1))) < types[sp + 3].Hani_Min2) {
                            WorkData.I.Sp1 = byte.Parse(types[sp].Def);
                            WorkData.I.Sp2 = byte.Parse(types[sp + 1].Def);
                            WorkData.I.Sp3 = byte.Parse(types[sp + 2].Def);
                            WorkData.I.Sp4 = wsp1 - (WorkData.I.Sp1 + WorkData.I.Sp2 + WorkData.I.Sp3);
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp;
                            WorkData.I.Sp3 = wsp;
                            WorkData.I.Sp4 = (wsp1 - (wsp * (wsu - 1)));
                        }

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = 0;
                        WorkData.I.Lh3 = 0;
                        WorkData.I.Lh4 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = 0;
                        WorkData.I.Lt3 = 0;
                        WorkData.I.Lt4 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = 0;
                        WorkData.I.Ll3 = 0;
                        WorkData.I.Ll4 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        WorkData.I.Wl2 = byte.Parse(types[wl + 1].Def);
                        WorkData.I.Wl3 = byte.Parse(types[wl + 2].Def);
                        WorkData.I.Wl3 = bk.Wl1;
                        break;
                    default:
                        WorkData.I.Lk1 = byte.Parse(types[lk].Def);
                        WorkData.I.Lk2 = byte.Parse(types[lk + 1].Def);
                        WorkData.I.Lk3 = byte.Parse(types[lk + 2].Def);
                        WorkData.I.Lk3 = byte.Parse(types[lk + 3].Def);
                        WorkData.I.Lk5 = bk.Lk1;
                        if (wsp1 < types[sp + 1].Hani_Min2
                            || wsp1 < types[sp + 2].Hani_Min2
                            || wsp1 < types[sp + 3].Hani_Min2
                            || (wsp1 - (wsp * (wsu - 1))) < types[sp + 4].Hani_Min2) {
                            WorkData.I.Sp1 = byte.Parse(types[sp].Def);
                            WorkData.I.Sp2 = byte.Parse(types[sp + 1].Def);
                            WorkData.I.Sp3 = byte.Parse(types[sp + 2].Def);
                            WorkData.I.Sp4 = byte.Parse(types[sp + 2].Def);
                            WorkData.I.Sp5 = wsp1 -
                                             (WorkData.I.Sp1 + WorkData.I.Sp2 + WorkData.I.Sp3 + WorkData.I.Sp4);
                        }
                        else {
                            WorkData.I.Sp1 = wsp;
                            WorkData.I.Sp2 = wsp;
                            WorkData.I.Sp3 = wsp;
                            WorkData.I.Sp3 = wsp;
                            WorkData.I.Sp4 = (wsp1 - (wsp * (wsu - 1)));
                        }

                        WorkData.I.Lh1 = 0;
                        WorkData.I.Lh2 = 0;
                        WorkData.I.Lh3 = 0;
                        WorkData.I.Lh4 = 0;
                        WorkData.I.Lh5 = bk.Lh1;

                        WorkData.I.Lt1 = 0;
                        WorkData.I.Lt2 = 0;
                        WorkData.I.Lt3 = 0;
                        WorkData.I.Lt4 = 0;
                        WorkData.I.Lt5 = bk.Lt1;

                        WorkData.I.Ll1 = 0;
                        WorkData.I.Ll2 = 0;
                        WorkData.I.Ll3 = 0;
                        WorkData.I.Ll4 = 0;
                        WorkData.I.Ll5 = bk.Ll1;

                        WorkData.I.Wl1 = byte.Parse(types[wl].Def);
                        WorkData.I.Wl2 = byte.Parse(types[wl + 1].Def);
                        WorkData.I.Wl3 = byte.Parse(types[wl + 2].Def);
                        WorkData.I.Wl4 = byte.Parse(types[wl + 3].Def);
                        WorkData.I.Wl5 = bk.Wl1;
                        break;
                }

                break;
        }

        //--- SP1により基準を変更 -----
        WorkData.I.Org = WorkData.I.Sp1 < types[sp].Hani_Min2 ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// データタイプの配列番号を取得
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private int DataTypesIndex(string type) {
        var types = WorkDataTypes.List;
        for (var i = 0; i < WorkDataTypes.Count; i++) {
            if (types[i].DM == type) {
                return i;
            }
        }

        return 0;
    }

    /// <summary>
    /// データタイプのSP1〜SP5より最大のデフォルト値を取得
    /// </summary>
    /// <param name="sp"></param>
    /// <returns></returns>
    private string DataTypesDefMax(int sp) {
        var types = WorkDataTypes.List;
        var defMax = types[sp].Def;

        if (string.Compare(defMax, types[sp + 1].Def, StringComparison.Ordinal) == -1) {
            defMax = types[sp + 1].Def;
        }

        if (string.Compare(defMax, types[sp + 2].Def, StringComparison.Ordinal) == -1) {
            defMax = types[sp + 2].Def;
        }

        if (string.Compare(defMax, types[sp + 3].Def, StringComparison.Ordinal) == -1) {
            defMax = types[sp + 3].Def;
        }

        if (string.Compare(defMax, types[sp + 4].Def, StringComparison.Ordinal) == -1) {
            defMax = types[sp + 4].Def;
        }

        return defMax;
    }

    /// <summary>
    /// 最終ロンジを取得
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private static int DataTypesLastLongi(WorkData data) {
        var longi = 0;

        if (data.Lh1 > 0) {
            longi = 1;
        }

        if (data.Lh2 > 0) {
            longi = 2;
        }

        if (data.Lh3 > 0) {
            longi = 3;
        }

        if (data.Lh4 > 0) {
            longi = 4;
        }

        if (data.Lh5 > 0) {
            longi = 5;
        }

        return longi;
    }
}