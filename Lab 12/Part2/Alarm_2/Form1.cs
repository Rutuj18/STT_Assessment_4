using System;
using System.Drawing;
using System.Windows.Forms;

namespace AlarmClockApp
{
    public partial class Form1 : Form
    {
        private DateTime alarmTime;
        private Timer timer;
        private Random random;
        private bool isAlarmSet = false;

        public Form1()
        {
            InitializeComponent();
            InitializeAlarmClock();
        }

        private void InitializeAlarmClock()
        {
            this.Text = "Alarm Clock";
            this.Size = new Size(400, 200);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            Label instructionLabel = new Label
            {
                Text = "Enter Alarm Time (HH:MM:SS):",
                Location = new Point(20, 30),
                AutoSize = true
            };
            this.Controls.Add(instructionLabel);

            TextBox timeInput = new TextBox
            {
                Name = "TimeInput",
                Location = new Point(220, 25),
                Width = 120
            };
            this.Controls.Add(timeInput);

            Button startButton = new Button
            {
                Text = "Start Alarm",
                Location = new Point(140, 70),
                Width = 100
            };
            startButton.Click += StartButton_Click;
            this.Controls.Add(startButton);

            timer = new Timer();
            timer.Interval = 1000; // 1 second
            timer.Tick += Timer_Tick;

            random = new Random();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            TextBox timeInput = this.Controls["TimeInput"] as TextBox;
            if (DateTime.TryParseExact(timeInput.Text, "HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out alarmTime))
            {
                isAlarmSet = true;
                timer.Start();
                MessageBox.Show($"Alarm set for {alarmTime:HH:mm:ss}", "Alarm Set", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid time format. Please enter time as HH:MM:SS.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isAlarmSet)
                return;

            this.BackColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));

            if (DateTime.Now.ToString("HH:mm:ss") == alarmTime.ToString("HH:mm:ss"))
            {
                timer.Stop();
                isAlarmSet = false;
                this.BackColor = Color.White;
                MessageBox.Show("Time's up!", "Alarm", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
