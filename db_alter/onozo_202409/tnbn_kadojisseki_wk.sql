-- 単板ラインの稼動状況を一時的に保持する
-- 板単位でログを出力できるように使うテーブル
DROP TABLE IF EXISTS tnbn_kadojisseki_wk;
CREATE TABLE tnbn_kadojisseki_wk
(
    taisyo    VARCHAR(2) NOT NULL ,    -- '2'(仮付)／'3'(本付)／'5'（矯正）
    sno       CHAR(6),       -- 船番6桁を設定
    blk       CHAR(8),       -- ブロック名8桁を設定
    bzi       char(16),      -- 部材名16桁を設定　板継有りの場合は基準点側の部材名を設定
    pcs       char(1),       --
    l         DECIMAL(8, 1), -- 板継ぎがある場合は合計長さを設定
    b         DECIMAL(8, 1), -- 板継が有り幅が違う場合は最大値の板幅を設定
    tmax      DECIMAL(8, 1), -- 板継有りの場合の最大板厚を設定する。1枚の場合も設定
    honsu     SMALLINT,      -- ロンジの総本数(LH1～5で0以外のモノをカウント)
    ymd       varchar(10),   -- yyyy/mm/dd(監視PCのシステム日付)
    str_time  VARCHAR(8),    -- hh:nn:ss 運転開始時のシステム時間(※１)
    str_time2 VARCHAR(8),    -- hh:nn:ss 運転開始時のシステム時間(再開時) 
    end_time  VARCHAR(8),    -- hh:nn:ss 運転開始ビット=0の時の時間
    ttl_time  INTEGER,       -- STR_TIME～END_TIMEの経過時間(秒)
    kad_time  INTEGER,       -- STR_TIME2～END_TIMEの経過時間(秒)(※２)
    stp_time  INTEGER,       -- TTL_TIME－KAD_TIME
    cnt       SMALLINT,      -- 運転開始ビット=0の時(運転終了の時)、カウントアップ
    CONSTRAINT pk_tnbn_kadojisseki_wk PRIMARY KEY (taisyo)
);

INSERT INTO tnbn_kadojisseki_wk (taisyo, sno, blk, bzi, pcs, l, b, tmax, honsu, ymd, str_time, str_time2, end_time, ttl_time, kad_time, stp_time, cnt)
VALUES ('2', '787   ', 'B-13    ', 'T-K             ', 'P', 15200.0, 2690.0, 22.0, 5, '2024/03/02', '10:08:45', '15:53:37', '15:56:55', 20890, 1256, 19634, 34),
       ('3', '787   ', 'B-13    ', 'T-K             ', 'P', 15200.0, 2690.0, 22.0, 5, '2024/03/02', '08:39:55', '16:27:34', '16:25:23', 27928, 2151, 25777, 12),
       ('5', '787   ', 'B-13    ', 'T-K             ', 'P', 15200.0, 2690.0, 22.0, 5, '2023/09/16', '13:01:04', '15:45:13', '15:47:26', 9982, 570, 9412, 5)
;

