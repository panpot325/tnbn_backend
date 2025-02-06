using System.Drawing;
using BackendMonitor.share;
using BackendMonitor.type.singleton;
using G = BackendMonitor.share.Globals;
using C = BackendMonitor.share.Constants;

namespace BackendMonitor;

/// <summary>
/// Response
/// </summary>
public partial class Form1 {
    private void ReadBitResponseTest() {
        if (!AppConfig.DebugMode) {
            return;
        }

        switch (_unit) {
            case C.UNIT_2:
                button2.BackColor = MonitorMessage.RequestBit == C.REQ_SNO ? Color.Red : Color.White;
                button3.BackColor = MonitorMessage.RequestBit == C.REQ_BLK ? Color.Red : Color.White;
                button4.BackColor = MonitorMessage.RequestBit == C.REQ_BZI ? Color.Red : Color.White;
                button5.BackColor = MonitorMessage.RequestBit == C.REQ_DAT ? Color.Red : Color.White;
                if (G.Mid(ResponseMessage.ReadData, 4, 1) == "1"
                    && G.Mid(ResponseMessage.ReadData, 7, 1) == "1") {
                    button6.BackColor = Color.Red;
                    button7.BackColor = Color.White;
                }
                else {
                    button6.BackColor = Color.White;
                    button7.BackColor = Color.Red;
                }

                break;
            case C.UNIT_3:
                button2.BackColor = MonitorMessage.RequestBit == C.REQ_SNO ? Color.Blue : Color.White;
                button3.BackColor = MonitorMessage.RequestBit == C.REQ_BLK ? Color.Blue : Color.White;
                button4.BackColor = MonitorMessage.RequestBit == C.REQ_BZI ? Color.Blue : Color.White;
                button5.BackColor = MonitorMessage.RequestBit == C.REQ_DAT ? Color.Blue : Color.White;
                break;
            default:
                button2.BackColor = MonitorMessage.RequestBit == C.REQ_SNO ? Color.Yellow : Color.White;
                button3.BackColor = MonitorMessage.RequestBit == C.REQ_BLK ? Color.Yellow : Color.White;
                button4.BackColor = MonitorMessage.RequestBit == C.REQ_BZI ? Color.Yellow : Color.White;
                button5.BackColor = MonitorMessage.RequestBit == C.REQ_DAT ? Color.Yellow : Color.White;
                break;
        }
    }
}