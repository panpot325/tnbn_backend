-- 加工ワークデータ用排他処理のテーブル
DROP TABLE IF EXISTS tnbn_kakowk_data_haita;
CREATE TABLE tnbn_kakowk_data_haita
(
    syain INTEGER,     -- 社員番号を設定
    pcnim VARCHAR(50), -- コンピュータ名を設定
    sno   CHAR(6),     -- 船番6桁を設定
    blk   CHAR(8),     -- ブロック名8桁を設定
    bzi   CHAR(16),    -- 部材名16桁を設定　板継有りの場合は基準点側の部材名を設定
    pcs   CHAR(1)      --
);

INSERT INTO tnbn_kakowk_data_haita (syain, pcnim, sno, blk, bzi, pcs)
VALUES (2010019, 'OAM09', '768   ', '', '', '');