using System;
using BackendMonitor.Properties;
using BackendMonitor.type;
using BackendMonitor.type.index;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

// ReSharper disable InvertIf
// ReSharper disable MemberCanBeMadeStatic.Local

namespace BackendMonitor;

/// <summary>
/// Send Cmd
/// </summary>
public partial class Form1 {
    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾋﾞｯﾄ単位)
     * 00 02 000A 4220 00000060 B6 00
     * 00 03 000A 4220 00000180 76 00
     * 00 05 000A 4220 00000330 66 00
     */
    /// <summary>
    /// @読出ビットコマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string RecvBitCmd(int unit) {
        Log.Sub_LogWrite($@"【読出ビットコマンド】装置No:{unit}");

        return SCmd.GetCmd(
            C.REQ_RD_B,
            G.UnitCode(unit),
            'B',
            unit switch {
                C.UNIT_2 => "60",
                C.UNIT_3 => "180",
                C.UNIT_5 => "330",
                _ => ""
            },
            unit switch {
                C.UNIT_2 => "B6",
                C.UNIT_3 => "76",
                C.UNIT_5 => "66",
                _ => ""
            }
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾜｰﾄﾞ単位)
     * 01 02 000A 5720 000000F0 10 00
     * 01 03 000A 5720 000001C0 10 00
     * 01 05 000A 5720 000001F0 10 00
     */
    /// <summary>
    /// /@要求データキーの取得コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string RequestDataKeyCmd(int unit) {
        Log.Sub_LogWrite(@"【要求データキーの取得コマンド】");

        return SCmd.GetCmd(
            C.REQ_RD_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            unit switch {
                C.UNIT_2 => Settings.Default.Prg_Mode == C.MODE_SAI ? "00000130" : "000000F0",
                C.UNIT_3 => Settings.Default.Prg_Mode == C.MODE_SAI ? "00000200" : "000001C0",
                C.UNIT_5 => Settings.Default.Prg_Mode == C.MODE_SAI ? "00000230" : "000001F0",
                _ => ""
            },
            C.REQ_WORD_DEVICE_COUNT
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾜｰﾄﾞ単位)
     * 01 02 000A 4420 000007D0 10 00
     * 01 03 000A 4420 000007D0 10 00
     * 01 05 000A 4420 00000064 10 00
     */
    /// <summary>
    /// @稼動データキーの取得コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkDataKeyCmd(int unit) {
        Log.Sub_LogWrite(@"【稼動データキーの取得コマンド】");

        return SCmd.GetCmd(
            C.REQ_RD_W,
            G.UnitCode(unit),
            C.DEVICE_D,
            unit switch {
                C.UNIT_2 => "000007D0",
                C.UNIT_3 => "000007D0",
                C.UNIT_5 => "00000064",
                _ => ""
            },
            C.DAT_WORD_DEVICE_COUNT
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000100 01 00 10
     * 02 03 000A 4220 000001E0 01 00 10
     * 02 05 000A 4220 00000380 01 00 10
     */
    /// <summary>
    /// @船番一覧無し書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string SnoIndexEmptyCmd(int unit) {
        Log.Sub_LogWrite(@"【船番一覧無し書込コマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} 船番一覧なし 送信中...",
            $@"{G.UnitName(unit)}\船番一覧なし{Environment.NewLine}送信中..."
        );

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000100",
                C.UNIT_3 => "000001E0",
                C.UNIT_5 => "00000380",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000101 01 00 10
     * 02 03 000A 4220 000001E1 01 00 10
     * 02 05 000A 4220 00000381 01 00 10
     */
    /// <summary>
    /// @ブロック名一覧無し書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BlkIndexEmptyCmd(int unit) {
        Log.Sub_LogWrite(@"【ブロック名一覧無し書込コマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} ブロック名一覧なし 送信中...",
            $@"{G.UnitName(unit)}\ブロック名一覧なし{Environment.NewLine}送信中..."
        );

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000101",
                C.UNIT_3 => "000001E1",
                C.UNIT_5 => "00000381",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000102 01 00 10
     * 02 03 000A 4220 000001E2 01 00 10
     * 02 05 000A 4220 00000382 01 00 10
     */
    /// <summary>
    /// @部材名一覧無し書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BziIndexEmptyCmd(int unit) {
        Log.Sub_LogWrite(@"【部材名一覧無し書込コマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} 部材＋舷一覧なし 送信中...",
            $@"{G.UnitName(unit)}\部材＋舷一覧なし{Environment.NewLine}送信中..."
        );

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000102",
                C.UNIT_3 => "000001E2",
                C.UNIT_5 => "00000382",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000103 01 00 10
     * 02 03 000A 4220 000001E3 01 00 10
     * 02 05 000A 4220 00000383 01 00 10
     */
    /// <summary>
    /// @加工ワークデータ無し書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkDataEmptyCmd(int unit) {
        Log.Sub_LogWrite(@"【加工ワークデータ無し書込コマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} 加工ワークデータなし 送信中...",
            $@"{G.UnitName(unit)}\加工ワークデータなし{Environment.NewLine}送信中..."
        );

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000103",
                C.UNIT_3 => "000001E3",
                C.UNIT_5 => "00000383",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000110 01 00 00
     * 02 03 000A 4220 000001F0 01 00 00
     * 02 05 000A 4220 00000390 01 00 00
     */
    /// <summary>
    /// @船番一覧書込完了コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string SnoIndexDoneCmd(int unit) {
        Log.Sub_LogWrite(@"【船番一覧書込完了コマンド】");

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000110",
                C.UNIT_3 => "000001F0",
                C.UNIT_5 => "00000390",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_OFF
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000111 01 00 00
     * 02 03 000A 4220 000001F1 01 00 00
     * 02 05 000A 4220 00000391 01 00 00
     */
    /// <summary>
    /// @ブロック名一覧書込完了コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BlkIndexDoneCmd(int unit) {
        Log.Sub_LogWrite(@"【ブロック名一覧書込完了コマンド】");

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000111",
                C.UNIT_3 => "000001F1",
                C.UNIT_5 => "00000391",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_OFF
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000112 01 00 00
     * 02 03 000A 4220 000001F2 01 00 00
     * 02 05 000A 4220 00000392 01 00 00
     */
    /// <summary>
    /// @部材名一覧書込完了コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BziIndexDoneCmd(int unit) {
        Log.Sub_LogWrite(@"【部材名一覧書込完了コマンド】");

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000112",
                C.UNIT_3 => "000001F2",
                C.UNIT_5 => "00000392",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_OFF
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
     * 02 02 000A 4220 00000113 03 00 0000
     * 02 03 000A 4220 000001F3 03 00 0000
     * 02 05 000A 4220 00000393 03 00 0000
     */
    /// <summary>
    /// @加工ワークデータ書込完了コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkDataDoneCmd(int unit) {
        Log.Sub_LogWrite(@"【加工ワークデータ書込完了コマンド】");

        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000113",
                C.UNIT_3 => "000001F3",
                C.UNIT_5 => "00000393",
                _ => ""
            },
            C.TRI_BIT_DEVICE_COUNT,
            C.TRI_BIT_WRITE_OFF
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)
     * 03 02 000A 5720 ******** 03 00 202020202020
     * 03 03 000A 5720 ******** 03 00 202020202020
     * 03 05 000A 5720 ******** 03 00 202020202020
     */
    /// <summary>
    /// @船番一覧クリアコマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string SnoIndexClearCmd(int unit) {
        Log.Sub_LogWrite(@"【船番一覧クリアコマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} 船番一覧 クリア中...",
            $@"{G.UnitName(unit)}\船番一覧{Environment.NewLine}クリア中..."
        );

        var deviceNumber = 768 + _clrIter * 3;

        _clrIter++;
        if (_clrIter >= C.SNO_MAX_PLC) {
            switch (_unit) {
                case C.UNIT_2:
                    _unit = C.UNIT_3;
                    _clrIter = C.SNO_MAX;
                    break;
                case C.UNIT_3:
                    _unit = C.UNIT_5;
                    _clrIter = C.SNO_MAX;
                    break;
                default:
                    _unit = C.UNIT_2;
                    _clrIter = C.BLK_MAX;
                    _clrState = 2; //船番一覧ｸﾘｱ完了、ﾌﾞﾛｯｸ一覧ｸﾘｱへ進む
                    break;
            }
        }

        return SCmd.GetCmd(
            C.REQ_WR_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            deviceNumber,
            C.SNO_WORD_DEVICE_COUNT,
            C.SNO_WORD_WRITE_CLEAR
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)
     * 03 02 000A 5720 ******** 04 00 2020202020202020
     * 03 03 000A 5720 ******** 04 00 2020202020202020
     * 03 05 000A 5720 ******** 04 00 2020202020202020
     */
    /// <summary>
    /// @ブロック名一覧クリアコマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BlkIndexClearCmd(int unit) {
        Log.Sub_LogWrite(@"【ブロック名一覧クリアコマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} ブロック名一覧 クリア中...",
            $@"{G.UnitName(unit)}\ブロック名一覧{Environment.NewLine}クリア中..."
        );

        var deviceNumber = 928 + _clrIter * 4;

        _clrIter += 1;
        if (_clrIter >= C.BLK_MAX_PLC) {
            switch (_unit) {
                case C.UNIT_2:
                    _unit = C.UNIT_3;
                    _clrIter = C.BLK_MAX;
                    break;
                case C.UNIT_3:
                    _unit = C.UNIT_5;
                    _clrIter = C.BLK_MAX;
                    break;
                default:
                    _unit = C.UNIT_1;
                    //ItrnClrCnt = C.C_BLK_MAX;
                    _clrState = 0; //船番&ﾌﾞﾛｯｸ一覧ｸﾘｱ完了
                    break;
            }
        }

        return SCmd.GetCmd(
            C.REQ_WR_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            deviceNumber,
            C.BLK_WORD_DEVICE_COUNT,
            C.BLK_WORD_WRITE_CLEAR
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)
     * 03 02 000A 5720 ******** 03 00 ************
     * 03 03 000A 5720 ******** 03 00 ************
     * 03 05 000A 5720 ******** 03 00 ************
     */
    /// <summary>
    /// @船番一覧書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string SnoIndexWriteCmd(int unit) {
        Log.Sub_LogWrite(@"【船番一覧書込コマンド】");
        SetText(
            $@"{G.UnitShortName(_unit)} 船番一覧 送信中...",
            $@"{G.UnitName(_unit)}\船番一覧{Environment.NewLine}送信中..."
        );

        var writeData = SnoIndex.List[_iter].ToUpper();
        var deviceNumber = 768 + _iter * 3;

        _iter++;
        _finish = _iter >= C.SNO_MAX;

        return SCmd.GetCmd(
            C.REQ_WR_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            deviceNumber,
            C.SNO_WORD_DEVICE_COUNT,
            SCmd.HexWriteData(writeData)
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)
     * 03 02 000A 5720 ******** 04 00 ************
     * 03 03 000A 5720 ******** 04 00 ************
     * 03 05 000A 5720 ******** 04 00 ************
    */
    /// <summary>
    /// @ブロック名一覧書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BlkIndexWriteCmd(int unit) {
        Log.Sub_LogWrite(@"【ブロック名一覧書込コマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} ﾌﾞﾛｯｸ名一覧 送信中...",
            $@"{G.UnitName(unit)}\ﾌﾞﾛｯｸ名一覧{Environment.NewLine}送信中..."
        );

        var writeData = BlkIndex.List[_iter].ToUpper();
        var deviceNumber = 928 + _iter * 4;

        _iter++;
        _finish = _iter >= C.BLK_MAX;

        return SCmd.GetCmd(
            C.REQ_WR_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            deviceNumber,
            C.BLK_WORD_DEVICE_COUNT,
            SCmd.HexWriteData(writeData)
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)
     * 03 02 000A 5720 ******** 09 00 ************
     * 03 03 000A 5720 ******** 09 00 ************
     * 03 05 000A 5720 ******** 09 00 ************
    */
    /// <summary>
    /// @部材名一覧書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BziIndexWriteCmd(int unit) {
        Log.Sub_LogWrite(@"【部材名一覧書込コマンド】");
        SetText($@"{G.UnitShortName(unit)} 部材＋舷一覧 送信中...",
            $@"{G.UnitName(unit)}\部材＋舷一覧{Environment.NewLine}送信中..."
        );

        var writeData = BziIndex.BziList[_iter].ToUpper() + BziIndex.PcsList[_iter].ToUpper();
        var deviceNumber = 1328 + _iter * 9;

        _iter++;
        _finish = _iter >= C.BZI_MAX;

        return SCmd.GetCmd(C.REQ_WR_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            deviceNumber,
            C.BZI_WORD_DEVICE_COUNT,
            SCmd.HexWriteData(writeData)
        );
    }

    /**
     * ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)
     * 03 02 000A 5720 ******** ** 00 ************
     * 03 03 000A 5720 ******** ** 00 ************
     * 03 05 000A 5720 ******** ** 00 ************
     */
    /// <summary>
    /// @加工ワークデータ書込コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkDataWriteCmd(int unit) {
        Log.Sub_LogWrite(@"【加工ワークデータ書込コマンド】");
        SetText(
            $@"{G.UnitShortName(unit)} 加工ﾜｰｸﾃﾞｰﾀ 送信中...",
            $@"{G.UnitName(unit)}\加工ワークデータ{Environment.NewLine}送信中..."
        );

        var workDataType = WorkDataTypes.List[_iter];

        _iter += 1;
        _finish = _iter >= WorkDataTypes.Count;

        return SCmd.GetCmd(
            C.REQ_WR_W,
            G.UnitCode(unit),
            C.DEVICE_W,
            workDataType.DeviceNumber,
            workDataType.DeviceCount,
            workDataType.WriteData
        );
    }

    /// <summary>
    /// 船番一覧書込要求コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string SnoIndexRequestCmd(int unit) {
        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000110",
                C.UNIT_3 => "000001F0",
                C.UNIT_5 => "00000390",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /// <summary>
    /// ブロック名一覧書込要求コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BlkIndexRequestCmd(int unit) {
        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000111",
                C.UNIT_3 => "000001F1",
                C.UNIT_5 => "00000391",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /// <summary>
    /// 部材名一覧書込要求コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string BziIndexRequestCmd(int unit) {
        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000112",
                C.UNIT_3 => "000001F2",
                C.UNIT_5 => "00000392",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /// <summary>
    /// ワークデータ書込要求コマンド
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkDataRequestCmd(int unit) {
        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000113",
                C.UNIT_3 => "000001F3",
                C.UNIT_5 => "00000393",
                _ => ""
            },
            C.ONE_BIT_DEVICE_COUNT,
            C.ONE_BIT_WRITE_ON
        );
    }

    /// <summary>
    /// 稼働開始要求要求コマンド
    /// デバッグ用
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkStartRequestCmd(int unit) {
        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000063",
                _ => ""
            },
            "04",
            "1001"
        );
    }

    /// <summary>
    /// 稼働終了要求要求コマンド
    /// デバッグ用
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    public string WorkStopRequestCmd(int unit) {
        return SCmd.GetCmd(
            C.REQ_WR_B,
            G.UnitCode(unit),
            C.DEVICE_B,
            unit switch {
                C.UNIT_2 => "00000063",
                _ => ""
            },
            "04",
            "0000"
        );
    }
}