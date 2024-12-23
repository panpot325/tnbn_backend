namespace BackendMonitor.type.singleton;

/// <summary>
/// 要求データキーチェッククラス
/// </summary>
public class CheckKey : PKey {
    /// Static Instance
    private static readonly CheckKey _instance = new();

    /// Instance Getter
    // ReSharper disable once ConvertToAutoPropertyWhenPossible
    public static CheckKey Instance => _instance;

    public static string Sno {
        get => _instance.SNO.Trim();
        set => _instance.SNO = value;
    }

    public static string Blk {
        get => _instance.BLK.Trim();
        set => _instance.BLK = value;
    }

    public static string Bzi {
        get => _instance.BZI.Trim();
        set => _instance.BZI = value;
    }

    public static string Pcs {
        get => _instance.PCS.Trim();
        set => _instance.PCS = value;
    }

    /// <summary>
    /// Set
    /// </summary>
    public static CheckKey Init() {
        _instance.Set(RequestKey.Sno, RequestKey.Blk, RequestKey.Bzi, RequestKey.Pcs);

        return _instance;
    }

    /// <summary>
    /// Private Constructor
    /// </summary>
    private CheckKey() {
    }
}