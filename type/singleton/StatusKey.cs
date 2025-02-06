namespace BackendMonitor.type.singleton;

/// <summary>
/// 稼動状況キークラス
/// </summary>
public class StatusKey : PKey {
    /// Static Instance
    private static readonly StatusKey _instance = new();

    /// Instance Getter
    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public static StatusKey Instance => _instance;

    public static string Sno => _instance.SNO.Trim();
    public static string Blk => _instance.BLK.Trim();
    public static string Bzi => _instance.BZI.Trim();
    public static string Pcs => _instance.PCS.Trim();

    /// <summary>
    /// Set
    /// </summary>
    public static StatusKey Init(string sno, string blk, string bzi, string pcs) {
        _instance.Set(sno, blk, bzi, pcs);

        return _instance;
    }

    /// <summary>
    /// Private Constructor
    /// </summary>
    private StatusKey() {
    }
}