namespace BackendMonitor.type.singleton;

/// <summary>
/// Pキークラス
/// </summary>
public abstract class PKey() {
    protected string SNO; //船番
    protected string BLK; //ブロック名
    protected string BZI; //部材名
    protected string PCS; //
    
    /// <summary>
    /// Set
    /// </summary>
    protected PKey Set(string sno, string blk, string bzi, string pcs) {
        SNO = sno;
        BLK = blk;
        BZI = bzi;
        PCS = pcs;
        
        return this;
    }
}