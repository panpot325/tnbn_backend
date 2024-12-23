namespace BackendMonitor.type.singleton;

/// <summary>
/// 要求データキークラス
/// </summary>
public class RequestKey : PKey {
    /// Static Instance
    private static readonly RequestKey _instance = new();

    /// Instance Getter
    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public static RequestKey Instance => _instance;

    public static string Sno => _instance.SNO.Trim();
    public static string Blk => _instance.BLK.Trim();
    public static string Bzi => _instance.BZI.Trim();
    public static string Pcs => _instance.PCS.Trim();

    /// <summary>
    /// Set
    /// </summary>
    public static RequestKey Init(string sno, string blk, string bzi, string pcs) {
        _instance.Set(sno, blk, bzi, pcs);

        return _instance;
    }

    /// <summary>
    /// Private Constructor
    /// </summary>
    private RequestKey() {
    }
}