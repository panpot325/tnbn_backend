using System.Collections.Generic;

namespace BackendMonitor.type;

/// <summary>
/// 稼動状況監視コレクションククラス
///  @Singleton Design Pattern
/// </summary>
public class WorkStates {
    /// Static Instance
    private static readonly WorkStates _instance = new();

    /// Static Members
    private readonly List<WorkState> _list;

    // Static Property
    public static List<WorkState> List => _instance._list;
    public static int Count => List.Count;

    /// <summary>
    /// Private Constructor
    /// </summary>
    private WorkStates() {
        _list = [
            new WorkState(0),
            new WorkState(1),
            new WorkState(2),
            new WorkState(3),
            new WorkState(4),
            new WorkState(5)
        ];
    }
}