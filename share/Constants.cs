namespace BackendMonitor.share {
    public static class Constants {
        //-- 要求ビット判定
        public const string REQ_SNO = "10000000"; //@c船番要求
        public const string REQ_BLK = "01000000"; //@cブロック要求
        public const string REQ_BZI = "00100000"; //@c部材要求
        public const string REQ_DAT = "00010000"; //@cデータ要求
        public const string REQ_MIR = "00001000"; //@cミラー要求
        public const string REQ_SPI = "00000100"; //@c回転要求
        public const string REQ_STA = "00000010"; //@c稼動開始
        public const string REQ_STP = "00000001"; //@c稼動終了
        public const string REQ_NOP = "00000000"; //@SKIP

        //-- リクエストコマンド種別
        public const string REQ_RD_B = "00"; //@cSH読出ビット // ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾋﾞｯﾄ単位)
        public const string REQ_RD_W = "01"; //@cSH読出ワード // ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾜｰﾄﾞ単位)
        public const string REQ_WR_B = "02"; //@cSH書込ビット // ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
        public const string REQ_WR_W = "03"; //@cSH書込ワード//ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)

        //-- レスポンストコマンド種別
        public const string RES_RD_B = "80"; //応答SH ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾋﾞｯﾄ単位)
        public const string RES_RD_W = "81"; //応答SH ﾃﾞﾊﾞｲｽﾒﾓﾘの一括読出し(ﾜｰﾄﾞ単位)
        public const string RES_WR_B = "82"; //応答SH ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾋﾞｯﾄ単位)
        public const string RES_WR_W = "83"; //応答SH ﾃﾞﾊﾞｲｽﾒﾓﾘの一括書込み(ﾜｰﾄﾞ単位)

        //-- 装置番号
        public const int UNIT_1 = 1;
        public const int UNIT_2 = 2;
        public const int UNIT_3 = 3;
        public const int UNIT_4 = 4;

        public const int UNIT_5 = 5;

        //public const string UNIT_CODE_2 = "02"; //@cPc名仮付 // PC番号(パネル組立仮付け装置)
        //public const string UNIT_CODE_3 = "03"; //@cPc名本付 // PC番号(２０電極溶接装置)
        //public const string UNIT_CODE_5 = "05"; //@cPc名矯正 // PC番号(単板歪み矯正機)
        public const string UNIT_CODE_2 = "01"; //@cPc名仮付 // PC番号(パネル組立仮付け装置)
        public const string UNIT_CODE_3 = "01"; //@cPc名本付 // PC番号(２０電極溶接装置)
        public const string UNIT_CODE_5 = "01"; //@cPc名矯正 // PC番号(単板歪み矯正機)

        //-- デバイス名
        public const string DEVICE_B = "4220"; //@cデバイス名B // ﾃﾞﾊﾞｲｽｺｰﾄﾞ B0(42h,20h)
        public const string DEVICE_W = "5720"; //@cデバイス名W // ﾃﾞﾊﾞｲｽｺｰﾄﾞ W0(57h,20h)
        public const string DEVICE_D = "4420"; //@cデバイス名D // ﾃﾞﾊﾞｲｽｺｰﾄﾞ D0(44h,20h)
        public const string DEVICE_R = "5220"; //@cデバイス名R // ﾃﾞﾊﾞｲｽｺｰﾄﾞ R0(52h,20h)

        //-- デバイス数
        public const string ONE_BIT_DEVICE_COUNT = "01";
        public const string TRI_BIT_DEVICE_COUNT = "03";

        //-- ビット送信データ
        public const string ONE_BIT_WRITE_ON = "10";
        public const string ONE_BIT_WRITE_OFF = "00";
        public const string TRI_BIT_WRITE_OFF = "0000";

        //-- ワード送信データ
        public const string SNO_WORD_DEVICE_COUNT = "03";
        public const string BLK_WORD_DEVICE_COUNT = "04";
        public const string BZI_WORD_DEVICE_COUNT = "09";
        public const string REQ_WORD_DEVICE_COUNT = "10";
        public const string DAT_WORD_DEVICE_COUNT = "10";
        public const string SNO_WORD_WRITE_CLEAR = "202020202020";
        public const string BLK_WORD_WRITE_CLEAR = "2020202020202020";

        public const string AC_TIMER = "000A"; //@cACPU監視タイマ // 2500ms (000Ah)

        //-- 加工ワークデータタイプに関するConst
        public const string NYU_MODE_NUM = "NUM"; //@NYU_MODE_NUM
        public const string KEISHIKI_16 = "16bitB"; //@KEISHIKI_16
        public const string KEISHIKI_32 = "32bitB"; //@KEISHIKI_32
        public const string KEISHIKI_M16 = "m16bitB"; //@KEISHIKI_m16
        public const string KEISHIKI_ASC = "ASCII"; //@KEISHIKI_ASC

        //-- 各一覧の上限
        public const int SNO_MAX_PLC = 50; //@cSnoMaxPLC
        public const int BLK_MAX_PLC = 100; //@cBlkMaxPLC
        public const int SNO_MAX = 50; //@cSnoMax
        public const int BLK_MAX = 40; //@cBlkMax
        public const int BZI_MAX = 40; //@cBziMax

        public const string MODE_ONO = "ONOMICHI";
        public const string MODE_MUK = "MUKAISHIMA";
        public const string MODE_SAI = "SAIKI";

        public const long IP_SUCCESS = 0;
        public const long IP_STATUS_BASE = 11000;
        public const long IP_BUF_TOO_SMALL = (11000 + 1);
        public const long IP_DEST_NET_UNREACHABLE = (11000 + 2);
        public const long IP_DEST_HOST_UNREACHABLE = (11000 + 3);
        public const long IP_DEST_PROT_UNREACHABLE = (11000 + 4);
        public const long IP_DEST_PORT_UNREACHABLE = (11000 + 5);
        public const long IP_NO_RESOURCES = (11000 + 6);
        public const long IP_BAD_OPTION = (11000 + 7);
        public const long IP_HW_ERROR = (11000 + 8);
        public const long IP_PACKET_TOO_BIG = (11000 + 9);
        public const long IP_REQ_TIMED_OUT = (11000 + 10);
        public const long IP_BAD_REQ = (11000 + 11);
        public const long IP_BAD_ROUTE = (11000 + 12);
        public const long IP_TTL_EXPIRED_TRANSIT = (11000 + 13);
        public const long IP_TTL_EXPIRED_REASSEM = (11000 + 14);
        public const long IP_PARAM_PROBLEM = (11000 + 15);
        public const long IP_SOURCE_QUENCH = (11000 + 16);
        public const long IP_OPTION_TOO_BIG = (11000 + 17);
        public const long IP_BAD_DESTINATION = (11000 + 18);
        public const long IP_ADDR_DELETED = (11000 + 19);
        public const long IP_SPEC_MTU_CHANGE = (11000 + 20);
        public const long IP_MTU_CHANGE = (11000 + 21);
        public const long IP_UNLOAD = (11000 + 22);
        public const long IP_ADDR_ADDED = (11000 + 23);
        public const long IP_GENERAL_FAILURE = (11000 + 50);
        public const long MAX_IP_STATUS = (11000 + 50);
        public const long IP_PENDING = (11000 + 255);
        public const long PING_TIMEOUT = 500;
        public const long WS_VERSION_REQD = 0x101;
        public const long MIN_SOCKETS_REQD = 1;
        public const long SOCKET_ERROR = -1;
        public const long INADDR_NONE = 0xFFFFFFFF;
        public const long MAX_WSA_DESCRIPTION = 256; //@MAX_WSADescription
        public const long MAX_WSA_SYS_STATUS = 128; //@MAX_WSASYSStatus
    }
}