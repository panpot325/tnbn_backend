-- 監視PGの設定を保持する
DROP TABLE IF EXISTS tnbn_kanshi_settei;
CREATE TABLE tnbn_kanshi_settei
(
    t_interval   INTEGER,     -- Timer1(要求ﾋﾞｯﾄ監視用)のintervalを設定(単位:m sec)
    w_protocol   SMALLINT,    -- Winsock1.Protocolを設定　　0:sckTCPProtocol/1:sckUDPProtocol
    w_remotehost VARCHAR(15), -- Winsock1.RemoteHostを設定　　'172.16.0.37'
    w_remoteport INTEGER,     -- Winsock1.RemotePortを設定　　8192  (2000H)
    w_localport  INTEGER,     -- Winsock1.LocalPortを設定　　　　0　(Passiveｵｰﾌﾟﾝ)
    c_interval   INTEGER,     -- 監視PGｽﾀｰﾄ時、単板ラインとの接続状況を監視する時間を設定
    t_interval2  INTEGER,     -- Timer2(再接続処理用)のintervalを設定(単位:msec)
    CONSTRAINT   pk_tnbn_kanshi_settei PRIMARY KEY (w_remotehost)
);

INSERT INTO tnbn_kanshi_settei (t_interval, w_protocol, w_remotehost, w_remoteport, w_localport, c_interval, t_interval2)
VALUES (1000, 0, '172.16.6.37', 8192, 0, 3, 60000);
