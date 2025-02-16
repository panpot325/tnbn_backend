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

        //同一材料でログ出力が2回目以降は書き換える
        if (workState.CNT > 1) {
            Log.RemoveLast();
        }

        var min0 = workState.Ttl_TIME / 60;
        var sec0 = workState.Ttl_TIME - min0 * 60;

        var min1 = workState.KAD_TIME / 60;
        var sec1 = workState.KAD_TIME - min1 * 60;

        var min2 = workState.Stp_Time / 60;
        var sec2 = workState.Stp_Time - min2 * 60;

        //新しいログを追加行に書き込む
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
        Log.CreateDir(Settings.Default.Csv_Path);
        Log.Write(fileName);
    }

    /// <summary>
    /// GetFilePath
    /// </summary>
    /// <returns></returns>
    private string GetFilePath() {
        var workState = WorkStates.List[_unit];
        return _unit switch {
            C.UNIT_2 => $"{Settings.Default.Csv_Path}\\{workState.SNO.Trim()}-{DateTime.Now:yyyy-MM}-仮付.csv",
            C.UNIT_3 => $"{Settings.Default.Csv_Path}\\{workState.SNO.Trim()}-{DateTime.Now:yyyy-MM}-本付.csv",
            C.UNIT_5 => $"{Settings.Default.Csv_Path}\\{workState.SNO.Trim()}-{DateTime.Now:yyyy-MM}-矯正.csv",
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
        var lk = WorkDataTypes.DmIndex("W715");

        //--- データタイプのWL1の配列番号を取得 -----
        var wl = WorkDataTypes.DmIndex("W745");

        //--- データタイプのSP1の配列番号を取得 -----
        var sp = WorkDataTypes.DmIndex("W72C");

        //データタイプのSP1〜SP5より最大のデフォルト値を取得
        var defMax = WorkDataTypes.DefMax(sp);

        //加工ワークデータを一時退避
        var workData = WorkData.I;
        var types = WorkDataTypes.List;

        //最終ロンジを取得
        var longi = DataTypesLastLongi(workData);

        //各項目の初期値を設定
        var lk1 = workData.Lk1;
        var lk2 = workData.Lk2;
        var lk3 = workData.Lk3;
        var lk4 = workData.Lk4;
        var lk5 = workData.Lk5;

        var sp1 = workData.Sp1;
        var sp2 = workData.Sp2;
        var sp3 = workData.Sp3;
        var sp4 = workData.Sp4;
        var sp5 = workData.Sp5;

        var lh1 = workData.Lh1;
        var lh2 = workData.Lh2;
        var lh3 = workData.Lh3;
        var lh4 = workData.Lh4;
        var lh5 = workData.Lh5;

        var lt1 = workData.Lt1;
        var lt2 = workData.Lt2;
        var lt3 = workData.Lt3;
        var lt4 = workData.Lt4;
        var lt5 = workData.Lt5;

        var ll1 = workData.Ll1;
        var ll2 = workData.Ll2;
        var ll3 = workData.Ll3;
        var ll4 = workData.Ll4;
        var ll5 = workData.Ll5;

        var wl1 = workData.Wl1;
        var wl2 = workData.Wl2;
        var wl3 = workData.Wl3;
        var wl4 = workData.Wl4;
        var wl5 = workData.Wl5;

        var b = workData.B;

        switch (longi) {
            case 5:
                //--- LH5入力有り -----
                //--- ロンジ形状を入替 -----
                workData.Lk1 = lk5;
                workData.Lk2 = lk4;
                workData.Lk3 = lk3;
                workData.Lk4 = lk2;
                workData.Lk5 = lk1;

                //--- ロンジスペースを算出 -----
                workData.Sp1 = b - (sp1 + sp2 + sp3 + sp4 + sp5 + lt5);
                workData.Sp2 = sp5 - lt4 + lt5;
                workData.Sp3 = sp4 - lt3 + lt4;
                workData.Sp4 = sp3 - lt2 + lt3;
                workData.Sp5 = sp2 - lt1 + lt2;

                //--- ロンジ高さを入替 -----
                workData.Lh1 = lh5;
                workData.Lh2 = lh4;
                workData.Lh3 = lh3;
                workData.Lh4 = lh2;
                workData.Lh5 = lh1;

                //--- ロンジウェブ板厚を入替 -----
                workData.Lt1 = lt5;
                workData.Lt2 = lt4;
                workData.Lt3 = lt3;
                workData.Lt4 = lt2;
                workData.Lt5 = lt1;


                //--- ロンジ全長を入替 -----
                workData.Ll1 = ll5;
                workData.Ll2 = ll4;
                workData.Ll3 = ll3;
                workData.Ll4 = ll2;
                workData.Ll5 = ll1;

                //-- 溶接脚長を入替 -----
                workData.Wl1 = wl5;
                workData.Wl2 = wl4;
                workData.Wl3 = wl3;
                workData.Wl4 = wl2;
                workData.Wl5 = wl1;
                break;
            case 4:
                var wsp1 = b - (sp1 + sp2 + sp3 + sp4 + lt4);
                var wsu = (int)(Math.Truncate(wsp1 / decimal.Parse(defMax) + 1));
                var wsp = Math.Truncate(wsp1 / wsu);

                switch (wsu) {
                    case 1:
                        workData.Lk1 = lk4;
                        workData.Lk2 = lk3;
                        workData.Lk3 = lk2;
                        workData.Lk4 = lk1;
                        workData.Lk5 = byte.Parse(types[lk + 4].Def);

                        workData.Sp1 = wsp;
                        workData.Sp2 = sp4 - lt3 + lt4;
                        workData.Sp3 = sp3 - lt2 + lt3;
                        workData.Sp4 = sp2 - lt1 + lt2;

                        workData.Lh1 = lh4;
                        workData.Lh2 = lh3;
                        workData.Lh3 = lh2;
                        workData.Lh4 = lh1;
                        workData.Lh5 = 0;

                        workData.Lt1 = lt4;
                        workData.Lt2 = lt3;
                        workData.Lt3 = lt2;
                        workData.Lt4 = lt1;
                        workData.Lt5 = 0;

                        workData.Ll1 = ll4;
                        workData.Ll2 = ll3;
                        workData.Ll3 = ll2;
                        workData.Ll4 = ll1;
                        workData.Ll5 = 0;

                        workData.Wl1 = wl4;
                        workData.Wl2 = wl3;
                        workData.Wl3 = wl2;
                        workData.Wl4 = wl1;
                        workData.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    default:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = lk4;
                        workData.Lk3 = lk3;
                        workData.Lk4 = lk2;
                        workData.Lk5 = lk1;
                        if (wsp1 - wsp * 1 < types[sp + 1].Hani_Min2) {
                            workData.Sp1 = decimal.Parse(types[sp].Def);
                            workData.Sp2 = wsp1 - workData.Sp1;
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp1 - wsp * 1;
                        }

                        workData.Sp3 = sp4 - lt3 + lt4;
                        workData.Sp4 = sp3 - lt2 + lt3;
                        workData.Sp5 = sp2 - lt1 + lt2;

                        workData.Lh1 = 0;
                        workData.Lh2 = lh4;
                        workData.Lh3 = lh3;
                        workData.Lh4 = lh2;
                        workData.Lh5 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = lt4;
                        workData.Lt3 = lt3;
                        workData.Lt4 = lt2;
                        workData.Lt5 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = ll4;
                        workData.Ll3 = ll3;
                        workData.Ll4 = ll2;
                        workData.Ll5 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = wl4;
                        workData.Wl3 = wl3;
                        workData.Wl4 = wl2;
                        workData.Wl5 = wl1;
                        break;
                }


                break;
            case 3:
                //--- LH3入力有り(LH4=0,LH5=0) -----
                wsp1 = b - (sp1 + sp2 + sp3 + lt3);
                wsu = (int)(Math.Truncate(wsp1 / decimal.Parse(defMax) + 1));
                wsp = Math.Truncate(wsp1 / wsu);
                switch (wsu) {
                    case 1:
                        workData.Lk1 = lk3;
                        workData.Lk2 = lk2;
                        workData.Lk3 = lk1;
                        workData.Lk4 = byte.Parse(types[lk + 3].Def);
                        workData.Lk5 = byte.Parse(types[lk + 4].Def);

                        workData.Sp1 = wsp;
                        workData.Sp2 = sp3 - lt2 + lt3;
                        workData.Sp3 = sp2 - lt1 + lt2;

                        workData.Lh1 = lh3;
                        workData.Lh2 = lh2;
                        workData.Lh3 = lh1;
                        workData.Lh4 = 0;
                        workData.Lh5 = 0;

                        workData.Lt1 = lt3;
                        workData.Lt2 = lt2;
                        workData.Lt3 = lt1;
                        workData.Lt4 = 0;
                        workData.Lt5 = 0;

                        workData.Ll1 = ll3;
                        workData.Ll2 = ll2;
                        workData.Ll3 = ll1;
                        workData.Ll4 = 0;
                        workData.Ll5 = 0;

                        workData.Wl1 = wl3;
                        workData.Wl2 = wl2;
                        workData.Wl3 = wl1;
                        workData.Wl4 = byte.Parse(types[wl + 3].Def);
                        workData.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    case 2:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = lk3;
                        workData.Lk3 = lk2;
                        workData.Lk4 = lk1;
                        workData.Lk5 = byte.Parse(types[lk + 4].Def);

                        if (wsp1 - wsp * 1 < types[sp + 1].Hani_Min2) {
                            workData.Sp1 = decimal.Parse(types[sp].Def);
                            workData.Sp2 = wsp1 - workData.Sp1;
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp1 - wsp * 1;
                        }

                        workData.Sp3 = sp3 - lt2 + lt3;
                        workData.Sp4 = sp2 - lt1 + lt2;

                        workData.Lh1 = 0;
                        workData.Lh2 = lh3;
                        workData.Lh3 = lh2;
                        workData.Lh4 = lh1;
                        workData.Lh5 = 0;

                        workData.Lt1 = 0;
                        workData.Lt2 = lt3;
                        workData.Lt3 = lt2;
                        workData.Lt4 = lt1;
                        workData.Lt5 = 0;

                        workData.Ll1 = 0;
                        workData.Ll2 = ll3;
                        workData.Ll3 = ll2;
                        workData.Ll4 = ll1;
                        workData.Ll5 = 0;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = wl3;
                        workData.Wl3 = wl2;
                        workData.Wl4 = wl1;
                        workData.Wl5 = byte.Parse(types[wl + 4].Def);
                        break;
                    default:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = byte.Parse(types[lk + 1].Def);
                        workData.Lk3 = lk3;
                        workData.Lk4 = lk2;
                        workData.Lk5 = lk1;

                        if (wsp < types[sp + 1].Hani_Min2
                            || wsp1 - wsp * 2 < types[sp + 2].Hani_Min2) {
                            workData.Sp1 = decimal.Parse(types[sp].Def);
                            workData.Sp2 = decimal.Parse(types[sp + 1].Def);
                            workData.Sp3 = wsp1 - (workData.Sp1 + workData.Sp2);
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp;
                            workData.Sp3 = wsp1 - wsp * 2;
                        }

                        workData.Sp4 = sp3 - lt2 + lt3;
                        workData.Sp5 = sp2 - lt1 + lt2;

                        workData.Lh1 = 0;
                        workData.Lh2 = 0;
                        workData.Lh3 = lh3;
                        workData.Lh4 = lh2;
                        workData.Lh5 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = 0;
                        workData.Lt3 = lt3;
                        workData.Lt4 = lt2;
                        workData.Lt5 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = 0;
                        workData.Ll3 = ll3;
                        workData.Ll4 = ll2;
                        workData.Ll5 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = byte.Parse(types[wl + 1].Def);
                        workData.Wl3 = wl3;
                        workData.Wl4 = wl2;
                        workData.Wl5 = wl1;

                        break;
                }

                break;
            case 2:
                //--- LH2入力有り(LH3=0,LH4=0,LH5=0) -----
                wsp1 = b - (sp1 + sp2 + lt2);
                wsu = (int)Math.Truncate(wsp1 / decimal.Parse(defMax) + 1);
                wsp = Math.Truncate(wsp1 / wsu);

                switch (wsu) {
                    case 1:
                        workData.Lk1 = lk2;
                        workData.Lk2 = lk1;
                        workData.Lk3 = byte.Parse(types[lk + 2].Def);
                        workData.Lk4 = byte.Parse(types[lk + 3].Def);
                        workData.Lk5 = byte.Parse(types[lk + 4].Def);

                        workData.Sp1 = wsp;
                        workData.Sp2 = sp2 - lt1 + lt2;

                        workData.Lh1 = lh2;
                        workData.Lh2 = lh1;
                        workData.Lh3 = 0;
                        workData.Lh4 = 0;
                        workData.Lh5 = 0;

                        workData.Lt1 = lt2;
                        workData.Lt2 = lt1;
                        workData.Lt3 = 0;
                        workData.Lt4 = 0;
                        workData.Lt5 = 0;

                        workData.Ll1 = ll2;
                        workData.Ll2 = ll1;
                        workData.Ll3 = 0;
                        workData.Ll4 = 0;
                        workData.Ll5 = 0;

                        workData.Wl1 = wl2;
                        workData.Wl2 = wl1;
                        workData.Wl3 = byte.Parse(types[wl + 2].Def);
                        workData.Wl4 = byte.Parse(types[wl + 3].Def);
                        workData.Wl5 = byte.Parse(types[wl + 4].Def);
                        break;
                    case 2:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = lk2;
                        workData.Lk3 = lk1;
                        workData.Lk4 = byte.Parse(types[lk + 3].Def);
                        workData.Lk5 = byte.Parse(types[lk + 4].Def);

                        if (wsp1 - wsp * (wsu - 1) < types[sp + 1].Hani_Min2) {
                            workData.Sp1 = decimal.Parse(types[sp].Def);
                            workData.Sp2 = wsp1 - workData.Sp1;
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp1 - wsp * (wsu - 1);
                        }

                        workData.Sp3 = sp2 - lt1 + lt2;

                        workData.Lh1 = 0;
                        workData.Lh2 = lh2;
                        workData.Lh3 = lh1;
                        workData.Lh4 = 0;
                        workData.Lh5 = 0;

                        workData.Lt1 = 0;
                        workData.Lt2 = lt2;
                        workData.Lt3 = lt1;
                        workData.Lt4 = 0;
                        workData.Lt5 = 0;

                        workData.Ll1 = 0;
                        workData.Ll2 = ll2;
                        workData.Ll3 = ll1;
                        workData.Ll4 = 0;
                        workData.Ll5 = 0;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = wl2;
                        workData.Wl3 = wl1;
                        workData.Wl4 = byte.Parse(types[wl + 3].Def);
                        workData.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    case 3:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = byte.Parse(types[lk + 1].Def);
                        workData.Lk3 = lk2;
                        workData.Lk4 = lk1;
                        workData.Lk5 = byte.Parse(types[lk + 4].Def);

                        if (wsp < types[sp + 1].Hani_Min2
                            || wsp1 - wsp * (wsu - 1) < types[sp + 2].Hani_Min2) {
                            workData.Sp1 = decimal.Parse(types[sp].Def);
                            workData.Sp2 = decimal.Parse(types[sp + 1].Def);
                            workData.Sp3 = wsp1 - (workData.Sp1 + workData.Sp2);
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp;
                            workData.Sp3 = wsp1 - wsp * (wsu - 1);
                        }

                        workData.Sp4 = sp2 - lt1 + lt2;

                        workData.Lh1 = 0;
                        workData.Lh2 = 0;
                        workData.Lh3 = lh2;
                        workData.Lh4 = lh1;
                        workData.Lh5 = 0;

                        workData.Lt1 = 0;
                        workData.Lt2 = 0;
                        workData.Lt3 = lt2;
                        workData.Lt4 = lt1;
                        workData.Lt5 = 0;

                        workData.Ll1 = 0;
                        workData.Ll2 = 0;
                        workData.Ll3 = ll2;
                        workData.Ll4 = ll1;
                        workData.Ll5 = 0;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = byte.Parse(types[wl + 1].Def);
                        workData.Wl3 = wl2;
                        workData.Wl4 = wl1;
                        workData.Wl5 = byte.Parse(types[wl + 4].Def);

                        break;
                    default:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = byte.Parse(types[lk + 1].Def);
                        workData.Lk3 = byte.Parse(types[lk + 2].Def);
                        workData.Lk4 = lk2;
                        workData.Lk5 = lk1;

                        if (wsp < types[sp + 1].Hani_Min2
                            || wsp < types[sp + 2].Hani_Min2
                            || wsp1 - wsp * 3 < types[sp + 3].Hani_Min2) {
                            workData.Sp1 = decimal.Parse(types[sp].Def);
                            workData.Sp2 = decimal.Parse(types[sp + 1].Def);
                            workData.Sp3 = decimal.Parse(types[sp + 2].Def);
                            workData.Sp4 = wsp1 - (workData.Sp1 + workData.Sp2 + workData.Sp3);
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp;
                            workData.Sp3 = wsp;
                            workData.Sp4 = wsp1 - wsp * 3;
                        }

                        workData.Sp5 = sp2 - lt1 + lt2;

                        workData.Lh1 = 0;
                        workData.Lh2 = 0;
                        workData.Lh3 = 0;
                        workData.Lh4 = lh2;
                        workData.Lh5 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = 0;
                        workData.Lt3 = 0;
                        workData.Lt4 = lt2;
                        workData.Lt5 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = 0;
                        workData.Ll3 = 0;
                        workData.Ll4 = ll2;
                        workData.Ll5 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = byte.Parse(types[wl + 1].Def);
                        workData.Wl3 = byte.Parse(types[wl + 2].Def);
                        workData.Wl4 = wl2;
                        workData.Wl5 = wl1;
                        break;
                }

                break;
            case 1:
                //--- LH1入力有り(LH2=0,LH3=0,LH4=0,LH5=0) -----
                wsp1 = b - (sp1 + lt1);
                wsu = (int)(Math.Truncate(wsp1 / decimal.Parse(defMax) + 1));
                wsp = Math.Truncate(wsp1 / wsu);

                switch (wsu) {
                    case 1:
                        workData.Lk1 = lk1;
                        workData.Sp1 = (byte)wsp;
                        workData.Lh1 = lh1;
                        workData.Lt1 = lt1;
                        workData.Ll1 = ll1;
                        workData.Wl1 = wl1;
                        break;
                    case 2:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = lk1;
                        if (wsp1 - wsp * (wsu - 1) < types[sp + 1].Hani_Min2) {
                            workData.Sp1 = byte.Parse(types[sp].Def);
                            workData.Sp2 = wsp1 - workData.Sp1;
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp1 - wsp * (wsu - 1);
                        }

                        workData.Lh1 = 0;
                        workData.Lh2 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = wl1;

                        break;
                    case 3:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = byte.Parse(types[lk + 1].Def);
                        workData.Lk3 = lk1;
                        if (wsp < types[sp + 1].Hani_Min2
                            || wsp1 - wsp * (wsu - 1) < types[sp + 2].Hani_Min2) {
                            workData.Sp1 = byte.Parse(types[sp].Def);
                            workData.Sp2 = byte.Parse(types[sp + 1].Def);
                            workData.Sp3 = wsp1 - (workData.Sp1 + workData.Sp2);
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp;
                            workData.Sp3 = wsp1 - wsp * (wsu - 1);
                        }

                        workData.Lh1 = 0;
                        workData.Lh2 = 0;
                        workData.Lh3 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = 0;
                        workData.Lt3 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = 0;
                        workData.Ll3 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = byte.Parse(types[wl + 1].Def);
                        workData.Wl3 = wl1;

                        break;
                    case 4:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = byte.Parse(types[lk + 1].Def);
                        workData.Lk3 = byte.Parse(types[lk + 2].Def);
                        workData.Lk4 = lk1;
                        if (wsp < types[sp + 1].Hani_Min2
                            || wsp < types[sp + 2].Hani_Min2
                            || wsp1 - wsp * (wsu - 1) < types[sp + 3].Hani_Min2) {
                            workData.Sp1 = byte.Parse(types[sp].Def);
                            workData.Sp2 = byte.Parse(types[sp + 1].Def);
                            workData.Sp3 = byte.Parse(types[sp + 2].Def);
                            workData.Sp4 = wsp1 - (workData.Sp1 + workData.Sp2 + workData.Sp3);
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp;
                            workData.Sp3 = wsp;
                            workData.Sp4 = wsp1 - wsp * (wsu - 1);
                        }

                        workData.Lh1 = 0;
                        workData.Lh2 = 0;
                        workData.Lh3 = 0;
                        workData.Lh4 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = 0;
                        workData.Lt3 = 0;
                        workData.Lt4 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = 0;
                        workData.Ll3 = 0;
                        workData.Ll4 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = byte.Parse(types[wl + 1].Def);
                        workData.Wl3 = byte.Parse(types[wl + 2].Def);
                        workData.Wl4 = wl1;
                        break;
                    default:
                        workData.Lk1 = byte.Parse(types[lk].Def);
                        workData.Lk2 = byte.Parse(types[lk + 1].Def);
                        workData.Lk3 = byte.Parse(types[lk + 2].Def);
                        workData.Lk4 = byte.Parse(types[lk + 3].Def);
                        workData.Lk5 = lk1;
                        if (wsp < types[sp + 1].Hani_Min2
                            || wsp < types[sp + 2].Hani_Min2
                            || wsp < types[sp + 3].Hani_Min2
                            || wsp1 - wsp * 4 < types[sp + 4].Hani_Min2) {
                            workData.Sp1 = byte.Parse(types[sp].Def);
                            workData.Sp2 = byte.Parse(types[sp + 1].Def);
                            workData.Sp3 = byte.Parse(types[sp + 2].Def);
                            workData.Sp4 = byte.Parse(types[sp + 3].Def);
                            workData.Sp5 = wsp1 -
                                           (workData.Sp1 + workData.Sp2 + workData.Sp3 + workData.Sp4);
                        }
                        else {
                            workData.Sp1 = wsp;
                            workData.Sp2 = wsp;
                            workData.Sp3 = wsp;
                            workData.Sp3 = wsp;
                            workData.Sp4 = wsp1 - wsp * 4;
                        }

                        workData.Lh1 = 0;
                        workData.Lh2 = 0;
                        workData.Lh3 = 0;
                        workData.Lh4 = 0;
                        workData.Lh5 = lh1;

                        workData.Lt1 = 0;
                        workData.Lt2 = 0;
                        workData.Lt3 = 0;
                        workData.Lt4 = 0;
                        workData.Lt5 = lt1;

                        workData.Ll1 = 0;
                        workData.Ll2 = 0;
                        workData.Ll3 = 0;
                        workData.Ll4 = 0;
                        workData.Ll5 = ll1;

                        workData.Wl1 = byte.Parse(types[wl].Def);
                        workData.Wl2 = byte.Parse(types[wl + 1].Def);
                        workData.Wl3 = byte.Parse(types[wl + 2].Def);
                        workData.Wl4 = byte.Parse(types[wl + 3].Def);
                        workData.Wl5 = wl1;
                        break;
                }

                break;
        }

        //--- SP1により基準を変更 -----
        workData.Org = workData.Sp1 < types[sp].Hani_Min2 ? (byte)1 : (byte)0;
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