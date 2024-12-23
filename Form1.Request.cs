using System;
using BackendMonitor.type;
using BackendMonitor.type.index;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

// ReSharper disable InvertIf


namespace BackendMonitor;

/// <summary>
/// Request
/// </summary>
public partial class Form1 {
    /// <summary>
    /// @船番一覧書込
    /// </summary>
    public void SnoIndexWrite() {
        G.LogWrite(@"【船番一覧から抽出】");

        Command1.Enabled = false;
        _finish = false;

        _itrnCnt = SnoIndex.Exist ? 0 : -1;
        SendData(
            _itrnCnt == -1
                ? SnoIndexEmptyCmd(_unit) //船番一覧無し書込
                : SnoIndexWriteCmd(_unit) //船番一覧書込
        );
    }

    /// <summary>
    /// @ブロック名一覧書込
    /// </summary>
    public void BlkIndexWrite() {
        G.LogWrite(@"【ブロック一覧から抽出】");

        _finish = false;
        BlkIndex.Fetch(RequestKey.Sno, true);
        _itrnCnt = BlkIndex.Exist ? 0 : -1;

        SendData(
            _itrnCnt == -1
                ? BlkIndexEmptyCmd(_unit) //ブロック名一覧無し書込コマンド
                : BlkIndexWriteCmd(_unit) //ブロック名一覧書込コマンド
        );
    }

    /// <summary>
    /// @部材名一覧書込
    /// </summary>
    public void BziIndexWrite() {
        G.LogWrite(@"【部材一覧データ作成】");

        _finish = false;
        BziIndex.Fetch(RequestKey.Sno, RequestKey.Blk, true);
        _itrnCnt = BziIndex.Exist ? 0 : -1;
        SendData(
            _itrnCnt == -1
                ? BziIndexEmptyCmd(_unit) //部材名一覧無し書込コマンド
                : BziIndexWriteCmd(_unit) //部材名一覧書込コマンド
        );
    }

    /// <summary>
    /// @稼動データキー取得
    /// </summary>
    public void WorkKeyRequest() {
        Command1.Enabled = false;
        var workState = WorkStates.List[_unit];
        workState.Clear();

        //稼動データキーの取得コマンド
        var cmd = WorkDataKeyCmd(_unit);
        if (cmd != "") {
            _gCmd = "";
            if (G.PingCheck()) {
                workState.Start_Count++;
                workState.YMD = DateTime.Now.ToString("yyyy/MM/dd");
                workState.StrTime = DateTime.Now.ToString("HH:mm:ss");
                workState.StrTime2 = workState.StrTime;
                SendData(cmd, false, false);
            }
            else {
                SetText(@"単板ライン 接続処理実施中...", "単板ライン\n続処理実施中...");
                G.LogWrite(@"接続処理Msg設定 単板ライン 接続処理実施中...");
                Timer2.Enabled = true;
            }
        }
    }

    /// <summary>
    /// @要求データキー取得
    /// </summary>
    public void DataKeyRequest() {
        _sendCnt = 0;
        Command1.Enabled = false;
        //要求データキーの取得コマンド
        SendData(RequestDataKeyCmd(_unit), false);
    }

    /// <summary>
    /// @加工ワークデータ書込
    /// </summary>
    public void WorkDataWrite() {
        if (_sendCnt == 0
            || RequestKey.Sno != CheckKey.Sno
            || RequestKey.Blk != CheckKey.Blk
            || RequestKey.Bzi != CheckKey.Bzi
            || RequestKey.Pcs != CheckKey.Pcs) {
            _finish = false;

            //加工ワークデータ作成
            WorkData.I.Fetch(RequestKey.Sno, RequestKey.Blk, RequestKey.Bzi, RequestKey.Pcs);
            _itrnCnt = WorkData.Exist ? 0 : -1;
            if (_itrnCnt == -1) {
                SendData(WorkDataEmptyCmd(_unit)); //加工ワークデータ無し書込コマンド
            }
            else {
                WorkData.UpdateStatus(1, _unit, RequestKey.Sno, RequestKey.Blk, RequestKey.Bzi, RequestKey.Pcs);
                switch (MonitorMessage.RequestBit) {
                    case C.REQ_MIR:
                        Invert(); //ミラー反転処理
                        break;
                    case C.REQ_SPI:
                        Rotate(); //回転処理
                        break;
                }

                //加工ワークデータ書込コマンド
                SendData(WorkDataWriteCmd(_unit)); //加工ワークデータ書込コマンド
            }
        }
        else {
            _sendCnt = 0;
            WorkData.UpdateStatus(2, _unit, RequestKey.Sno, RequestKey.Blk, RequestKey.Bzi, RequestKey.Pcs);
            SendData(WorkDataDoneCmd(_unit)); //加工ワークデータ書込完了コマンド
        }
    }
}