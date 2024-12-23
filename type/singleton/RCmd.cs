namespace BackendMonitor.type.singleton;

/// <summary>
/// 受信メッセージ基底クラス
/// </summary>
public abstract class RCmd {
    protected string SH; //サブヘッダ
    protected string Finish_Code; //終了CD
    protected string Illegal_Code; //異常CD
    protected string Read_Data; //読込データ
    protected string Request_Bit; //要求ビット

    /// <summary>
    /// Set
    /// </summary>
    /// <param name="sh"></param>
    /// <param name="finishCode"></param>
    /// <param name="illegalCode"></param>
    /// <param name="readData"></param>
    /// <param name="requestBit"></param>
    /// <returns></returns>
    protected RCmd Set(
        string sh,
        string finishCode,
        string illegalCode,
        string readData,
        string requestBit = ""
    ) {
        SH = sh;
        Finish_Code = finishCode;
        Illegal_Code = illegalCode;
        Read_Data = readData;
        Request_Bit = requestBit;

        return this;
    }

    /// <summary>
    /// Clear
    /// </summary>
    /// <returns></returns>
    protected RCmd Clear() {
        SH = "";
        Finish_Code = "";
        Illegal_Code = "";
        Read_Data = "";
        Request_Bit = "";

        return this;
    }
}