-- 加工ワークデータを保管するテーブル
DROP TABLE IF EXISTS tnbn_kakowk_data;
CREATE TABLE tnbn_kakowk_data
(
    sno             CHAR(6)  NOT NULL, -- 船番6桁を設定
    blk             CHAR(8)  NOT NULL, -- ブロック名8桁を設定
    bzi             CHAR(16) NOT NULL, -- 部材名16桁を設定　板継有りの場合は基準点側の部材名を設定
    pcs             CHAR(1)  NOT NULL, --
    gr1             SMALLINT,          -- 皮板材質　0:なし，1:32K，2:36K，3:MILD，4:その他
    gr2             SMALLINT,          -- 皮板材質　0:なし，1:32K，2:36K，3:MILD，4:その他
    gr3             SMALLINT,          -- 皮板材質　0:なし，1:32K，2:36K，3:MILD，4:その他
    gr4             SMALLINT,          -- 皮板材質　0:なし，1:32K，2:36K，3:MILD，4:その他
    gr5             SMALLINT,          -- 皮板材質　0:なし，1:32K，2:36K，3:MILD，4:その他
    lk1             SMALLINT,          -- 0:FB，1:AG，2:LB，3:L2，4:BP，5:その他
    lk2             SMALLINT,          -- 0:FB，1:AG，2:LB，3:L2，4:BP，5:その他
    lk3             SMALLINT,          -- 0:FB，1:AG，2:LB，3:L2，4:BP，5:その他
    lk4             SMALLINT,          -- 0:FB，1:AG，2:LB，3:L2，4:BP，5:その他
    lk5             SMALLINT,          -- 0:FB，1:AG，2:LB，3:L2，4:BP，5:その他
    l               DECIMAL(8, 1),     -- 板継ぎがある場合は合計長さを設定
    b               DECIMAL(8, 1),     -- 板継が有り幅が違う場合は最大値の板幅を設定
    tmax            DECIMAL(8, 1),     -- 板継有りの場合の最大板厚を設定する。1枚の場合も設定
    t1              DECIMAL(8, 1),     -- 板継有りの場合の基準点側から1番目の板の板厚
    t2              DECIMAL(8, 1),     -- 板継有りの場合の基準点側から2番目の板の板厚
    t3              DECIMAL(8, 1),     -- 板継有りの場合の基準点側から3番目の板の板厚
    t4              DECIMAL(8, 1),     -- 板継有りの場合の基準点側から4番目の板の板厚
    t5              DECIMAL(8, 1),     -- 板継有りの場合の基準点側から5番目の板の板厚
    it1             DECIMAL(8, 1),     -- 最初の板継位置の基準点側の皮板耳からの距離
    it2             DECIMAL(8, 1),     -- 2番目の板継位置の基準点側の皮板耳からの距離
    it3             DECIMAL(8, 1),     -- 3番目の板継位置の基準点側の皮板耳からの距離
    it4             DECIMAL(8, 1),     -- 4番目の板継位置の基準点側の皮板耳からの距離
    sp1             DECIMAL(8, 1),     -- ロンジ板厚内側までの距離を設定(南min:200,max:最大移動距離)
    sp2             DECIMAL(8, 1),     -- ヘッド1からヘッド2までの距離(min:650,max:最大移動距離)
    sp3             DECIMAL(8, 1),     -- ヘッド2からヘッド3までの距離(min:650,max:最大移動距離)
    sp4             DECIMAL(8, 1),     -- ヘッド3からヘッド4までの距離(min:650,max:最大移動距離)
    sp5             DECIMAL(8, 1),     -- ヘッド4からヘッド5までの距離(min:650,max:最大移動距離)
    lh1             DECIMAL(8, 1),     -- 複数ロンジに切れる場合は基準点側の最初のロンジ高さ設定，ダミー=0												
    lh2             DECIMAL(8, 1),     -- --
    lh3             DECIMAL(8, 1),     -- --
    lh4             DECIMAL(8, 1),     -- --
    lh5             DECIMAL(8, 1),     -- --
    lt1             DECIMAL(8, 1),     -- 複数ロンジに切れる場合は基準点側の最初のロンジウェブ板厚，ダミー=0												
    lt2             DECIMAL(8, 1),     -- --
    lt3             DECIMAL(8, 1),     -- --
    lt4             DECIMAL(8, 1),     -- --
    lt5             DECIMAL(8, 1),     -- --
    ll1             DECIMAL(8, 1),     -- 複数ロンジに切れる場合は基準点側の最初のロンジ長さ設定 シフト量は含まずロンジの実長を設定する。ダミー=0"												
    ll2             DECIMAL(8, 1),     -- --
    ll3             DECIMAL(8, 1),     -- --
    ll4             DECIMAL(8, 1),     -- --
    ll5             DECIMAL(8, 1),     -- --
    wl1             SMALLINT,          -- 溶接脚長を指示，ダミー=0
    wl2             SMALLINT,          -- --
    wl3             SMALLINT,          -- --
    wl4             SMALLINT,          -- --
    wl5             SMALLINT,          -- --
    is1             DECIMAL(8, 1),     -- 基準点側のみ指示(皮板内＋値、皮板外－値)
    stp1            DECIMAL(8, 1),     -- 最初に一時停止する基準点側の皮板耳からの距離
    stp2            DECIMAL(8, 1),     -- 2番目に一時停止する基準点側の皮板耳からの距離
    stp3            DECIMAL(8, 1),     -- 3番目に一時停止する基準点側の皮板耳からの距離
    stp4            DECIMAL(8, 1),     -- 4番目に一時停止する基準点側の皮板耳からの距離
    stp5            DECIMAL(8, 1),     -- 5番目に一時停止する基準点側の皮板耳からの距離
    org             SMALLINT,          -- 正基準(西):0，反基準:1(データ仕様による)
    yoteibi_kari    INTEGER,           -- 仮付け予定日
    yoteibi_hon     INTEGER,           -- 本付け予定日
    yoteibi_kyosei  INTEGER,           -- 歪み矯正予定日
    jissibi_kari    INTEGER,           -- 仮付装置へデータ送信完了した日
    jissibi_hon     INTEGER,           -- 本付装置へデータ送信完了した日
    jissibi_kyosei  INTEGER,           -- 歪み矯正機へデータ送信完了した日
    status_kari     SMALLINT,          -- 0:未送信／1:送信済み(レスポンス待ち)／2:完了(レスポンス有)
    status_hon      SMALLINT,          -- --
    status_kyosei   SMALLINT,          -- --
    create_date     INTEGER,           -- データ作成日
    create_syain    INTEGER,           -- データ作成社員番号
    update_date     INTEGER,           -- データ更新日
    update_syain    INTEGER,           -- データ更新社員
    jissijkn_kari   INTEGER,           -- 仮付装置へデータ送信完了した時間
    jissijkn_hon    INTEGER,           -- 本付装置へデータ送信完了した時間
    jissijkn_kyosei INTEGER,           -- 歪み矯正機へデータ送信完了した時間
    cres_flg        SMALLINT,          -- S舷データ自動作成 0(ﾃﾞﾌｫﾙﾄ):作成(西基準)／1:作成しない／2:作成(東基準)
    crep_flg        SMALLINT,          -- P舷データ自動作成 0:作成(西基準)／1(ﾃﾞﾌｫﾙﾄ):作成しない／2:作成(東基準)
    CONSTRAINT pk_tnbn_kakowk_data PRIMARY KEY (sno, blk, bzi, pcs)
);

-- $ psql onozo_202409
-- onozo_202409=# \copy tnbn_kakowk_data from TNBN_KAKOWK_DATA.csv with csv header
-- copy 8383

-- 船番一覧取得
SELECT sno FROM tnbn_kakowk_data 
           WHERE sno NOT IN (SELECT sno FROM tnbn_fin_ship_mst) GROUP BY sno  ORDER BY sno;

-- ブロック名一覧取得
SELECT blk FROM tnbn_kakowk_data
WHERE sno NOT IN (SELECT sno FROM tnbn_fin_ship_mst) AND sno = :sno GROUP BY blk ORDER BY blk;

-- 部材名一覧取得
SELECT bzi, pcs FROM tnbn_kakowk_data
                WHERE sno NOT IN (SELECT sno FROM tnbn_kakowk_data_haita) AND sno = :sno AND blk = :blk  
                GROUP BY bzi, pcs ORDER BY bzi, pcs;

-- ワークデータ取得
SElECT sno, blk, bzi, pcs FROM tnbn_kakowk_data WHERE sno = :sno AND blk = :blk AND bzi = :bzi AND pcs = :pcs
