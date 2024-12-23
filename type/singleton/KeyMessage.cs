namespace BackendMonitor.type.singleton;

/// <summary>
/// キー用受信メッセージクラス
/// </summary>
public class KeyMessage : RCmd {
    /// Static Instance
    private static readonly KeyMessage _instance = new();

    /// <summary>
    /// Set
    /// </summary>
    public static KeyMessage Set() {
        _instance.Set(
            ResponseMessage.Sh,
            ResponseMessage.FinishCode,
            ResponseMessage.IllegalCode,
            ResponseMessage.ReadData
        );

        return _instance;
    }

    /// <summary>
    /// Private Constructor
    /// </summary>
    private KeyMessage() {
    }
}