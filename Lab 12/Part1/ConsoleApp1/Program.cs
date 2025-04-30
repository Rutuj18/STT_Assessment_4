using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace WindowsFormsAlarmApp
{
    public partial class Form1 : Form
    {
        private DateTime targetTime;
        private Timer timer;
        private Random rand;
        private bool alarmTriggered = false;

        public Form1()
        {
            InitializeComponent();
            InitializeAlarmApp();
        }

        private void InitializeAlarmApp()
        {
            this.Text = "Alarm Clock App ⏰";
            this.Size = new Size(400, 200);
            this.BackColor = Color.White;

            // Label
            Label label = new Label
            {
                Text = "Enter Alarm Time (HH:MM:SS):",
                Location = new Point(20, 30),
                AutoSize = true
            };
            this.Controls.Add(label);

            // TextBox
            TextBox timeInput = new TextBox
            {
                Name = "TimeInput",
                Location = new Point(220, 25),
                Width = 120
            };
            this.Controls.Add(timeInput);

            // Button
            Button startButton = new Button
            {
                Text = "Start Alarm",
                Location = new Point(140, 70),
                Width = 100
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            // Timer setup
            timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;

            rand = new Random();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            string input = this.Controls["TimeInput"].Text;

            if (!DateTime.TryParseExact(input, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out targetTime))
            {
                MessageBox.Show("❌ Invalid format. Please use HH:MM:SS (24-hour).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            alarmTriggered = false;
            timer.Start();
            MessageBox.Show($"✅ Alarm set for {targetTime:HH:mm:ss}.", "Alarm Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (alarmTriggered)
                return;

            // Change background color randomly
            this.BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));

            if (DateTime.Now.ToString("HH:mm:ss") == targetTime.ToString("HH:mm:ss"))
            {
                timer.Stop();
                alarmTriggered = true;
                this.BackColor = Color.White;
                MessageBox.Show("⏰ DING DING DING! Alarm time reached!", "Alarm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}

