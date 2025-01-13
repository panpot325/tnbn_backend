// ReSharper disable InconsistentNaming

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using BackendMonitor.type.singleton;
using Timer = System.Timers.Timer;

namespace BackendMonitor;

public partial class Form1 {
    public Timer Timer1;
    public Timer Timer2;
    public Label Text1;
    //public TextBox Text1;
    public Button Command1;
    private Button button1;
    private Button button2;
    private Button button3;
    private Button button4;
    private Button button5;
    private Button button6;
    private Button button7;

    private void InitComponent() {
        Timer1 = new Timer();
        Timer2 = new Timer();
        Text1 = new Label();
        Command1 = new Button();
        button1 = new Button();
        button2 = new Button();
        button3 = new Button();
        button4 = new Button();
        button5 = new Button();
        button6 = new Button();
        button7 = new Button();
        ((ISupportInitialize)(Timer1)).BeginInit();
        ((ISupportInitialize)(Timer2)).BeginInit();
        SuspendLayout();
        // 
        // Timer1
        // 
        Timer1.Interval = KanshiSettei.Interval;
        Timer1.Enabled = false;
        Timer1.SynchronizingObject = this;
        Timer1.Elapsed += Timer1_Elapsed;
        // 
        // Timer2
        // 
        Timer2.Interval = KanshiSettei.Interval2 - KanshiSettei.IntervalC * 1000;
        Timer2.Enabled = false;
        Timer2.SynchronizingObject = this;
        Timer2.Elapsed += Timer2_Elapsed;
        // 
        // Text1
        // 
        Text1.Name = @"Text1";
        Text1.Text = "";
        Text1.Location = new Point(48, 214);
        Text1.Size = new Size(680, 100);
        Text1.TabIndex = 1;
        Text1.Font = new Font("ＭＳ Ｐゴシック", 10.875F, FontStyle.Bold,
            GraphicsUnit.Point, 128);
        // 
        // Command1
        // 
        Command1.Name = @"Command1";
        Command1.Text = @"終了";
        Command1.Enabled = false;
        Command1.Location = new Point(48, 28);
        Command1.Size = new Size(288, 99);
        Command1.TabIndex = 0;
        Command1.UseVisualStyleBackColor = true;
        Command1.Click += Command1_Click;
        // 
        // button1
        // 
        button1.Name = @"button1";
        button1.Text = @"テスト";
        button1.Location = new Point(409, 28);
        button1.Size = new Size(288, 99);
        button1.TabIndex = 2;
        button1.UseVisualStyleBackColor = true;
        button1.Click += button1_Click_1;
        // 
        // button2
        // 
        button2.Name = @"button2";
        button2.Text = @"船番一覧要求";
        button2.Location = new Point(48, 353);
        button2.Size = new Size(288, 109);
        button2.TabIndex = 3;
        button2.UseVisualStyleBackColor = true;
        button2.Click += button2_Click;
        // 
        // button3
        // 
        button3.Name = @"button3";
        button3.Text = @"ブロック名一覧要求";
        button3.Location = new Point(409, 362);
        button3.Size = new Size(288, 100);
        button3.TabIndex = 4;
        button3.UseVisualStyleBackColor = true;
        button3.Click += button3_Click;
        // 
        // button4
        // 
        button4.Name = @"button4";
        button4.Text = @"部材名一覧要求";
        button4.Location = new Point(48, 497);
        button4.Size = new Size(288, 100);
        button4.TabIndex = 5;
        button4.UseVisualStyleBackColor = true;
        button4.Click += button4_Click;
        // 
        // button5
        // 
        button5.Name = @"button5";
        button5.Text = @"ワークデータ要求";
        button5.Location = new Point(409, 497);
        button5.Size = new Size(287, 100);
        button5.TabIndex = 6;
        button5.UseVisualStyleBackColor = true;
        button5.Click += button5_Click;
        // 
        // button6
        // 
        button6.Name = @"button6";
        button6.Text = @"稼働開始";
        button6.Location = new Point(48, 632);
        button6.Size = new Size(287, 100);
        button6.TabIndex = 6;
        button6.UseVisualStyleBackColor = true;
        button6.Click += button6_Click;
        // 
        // button6
        // 
        button7.Name = @"button6";
        button7.Text = @"稼働終了";
        button7.Location = new Point(409, 632);
        button7.Size = new Size(287, 100);
        button7.TabIndex = 6;
        button7.UseVisualStyleBackColor = true;
        button7.Click += button7_Click;
        // 
        // Form1
        // 
        Name = @"Form1";
        Text = @"Form1";
        AutoScaleDimensions = new SizeF(13F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        Size = new Size(500, 600);
        ClientSize = new Size(1161, 966);
        Controls.Add(Text1);
        Controls.Add(Command1);
        Controls.Add(button1);
        Controls.Add(button2);
        Controls.Add(button3);
        Controls.Add(button4);
        Controls.Add(button5);
        Controls.Add(button6);
        Controls.Add(button7);
        ControlBox = false;
        
        Load += Form1_Load;
        FormClosed += Form1_FormClosed;
        FormClosing += Form1_FormClosing;
        ((ISupportInitialize)(Timer1)).EndInit();
        ((ISupportInitialize)(Timer2)).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }
}