namespace PrayerTimes
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            ContryLabel = new Label();
            CityLabel = new Label();
            SaveButton = new Button();
            ContryInput = new ComboBox();
            CityInput = new ComboBox();
            ContryPlaceholder = new TextBox();
            SettingsLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            CityPlaceholder = new TextBox();
            label3 = new Label();
            MethodPlaceholder = new TextBox();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            Fajr = new TextBox();
            Sunrise = new TextBox();
            Dhuhr = new TextBox();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            Asr = new TextBox();
            Maghrib = new TextBox();
            Isha = new TextBox();
            label11 = new Label();
            NextPray = new TextBox();
            label12 = new Label();
            TimeLeft = new TextBox();
            Clock = new TextBox();
            SuspendLayout();
            // 
            // ContryLabel
            // 
            resources.ApplyResources(ContryLabel, "ContryLabel");
            ContryLabel.ForeColor = SystemColors.ButtonHighlight;
            ContryLabel.Name = "ContryLabel";
            ContryLabel.Click += label1_Click;
            // 
            // CityLabel
            // 
            resources.ApplyResources(CityLabel, "CityLabel");
            CityLabel.ForeColor = SystemColors.ButtonHighlight;
            CityLabel.Name = "CityLabel";
            CityLabel.Click += label1_Click;
            // 
            // SaveButton
            // 
            resources.ApplyResources(SaveButton, "SaveButton");
            SaveButton.Name = "SaveButton";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // ContryInput
            // 
            ContryInput.FormattingEnabled = true;
            resources.ApplyResources(ContryInput, "ContryInput");
            ContryInput.Name = "ContryInput";
            ContryInput.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // CityInput
            // 
            CityInput.FormattingEnabled = true;
            resources.ApplyResources(CityInput, "CityInput");
            CityInput.Name = "CityInput";
            CityInput.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // ContryPlaceholder
            // 
            ContryPlaceholder.AccessibleRole = AccessibleRole.Outline;
            ContryPlaceholder.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(ContryPlaceholder, "ContryPlaceholder");
            ContryPlaceholder.Name = "ContryPlaceholder";
            ContryPlaceholder.ReadOnly = true;
            ContryPlaceholder.UseWaitCursor = true;
            // 
            // SettingsLabel
            // 
            resources.ApplyResources(SettingsLabel, "SettingsLabel");
            SettingsLabel.ForeColor = SystemColors.ButtonHighlight;
            SettingsLabel.Name = "SettingsLabel";
            SettingsLabel.Click += label1_Click;
            // 
            // label1
            // 
            resources.ApplyResources(label1, "label1");
            label1.ForeColor = SystemColors.ButtonHighlight;
            label1.Name = "label1";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            resources.ApplyResources(label2, "label2");
            label2.ForeColor = SystemColors.ButtonHighlight;
            label2.Name = "label2";
            label2.Click += label1_Click;
            // 
            // CityPlaceholder
            // 
            CityPlaceholder.AccessibleRole = AccessibleRole.Outline;
            CityPlaceholder.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(CityPlaceholder, "CityPlaceholder");
            CityPlaceholder.Name = "CityPlaceholder";
            CityPlaceholder.ReadOnly = true;
            CityPlaceholder.UseWaitCursor = true;
            CityPlaceholder.TextChanged += textBox2_TextChanged;
            // 
            // label3
            // 
            resources.ApplyResources(label3, "label3");
            label3.ForeColor = SystemColors.ButtonHighlight;
            label3.Name = "label3";
            label3.Click += label1_Click;
            // 
            // MethodPlaceholder
            // 
            MethodPlaceholder.AccessibleRole = AccessibleRole.Outline;
            MethodPlaceholder.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(MethodPlaceholder, "MethodPlaceholder");
            MethodPlaceholder.Name = "MethodPlaceholder";
            MethodPlaceholder.ReadOnly = true;
            MethodPlaceholder.UseWaitCursor = true;
            MethodPlaceholder.TextChanged += textBox2_TextChanged;
            // 
            // label4
            // 
            resources.ApplyResources(label4, "label4");
            label4.ForeColor = SystemColors.ButtonHighlight;
            label4.Name = "label4";
            label4.Click += label1_Click;
            // 
            // label5
            // 
            resources.ApplyResources(label5, "label5");
            label5.ForeColor = SystemColors.ButtonHighlight;
            label5.Name = "label5";
            label5.Click += label1_Click;
            // 
            // label6
            // 
            resources.ApplyResources(label6, "label6");
            label6.ForeColor = SystemColors.ButtonHighlight;
            label6.Name = "label6";
            label6.Click += label1_Click;
            // 
            // label7
            // 
            resources.ApplyResources(label7, "label7");
            label7.ForeColor = SystemColors.ButtonHighlight;
            label7.Name = "label7";
            label7.Click += label1_Click;
            // 
            // Fajr
            // 
            Fajr.AccessibleRole = AccessibleRole.Outline;
            Fajr.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Fajr, "Fajr");
            Fajr.Name = "Fajr";
            Fajr.ReadOnly = true;
            Fajr.UseWaitCursor = true;
            // 
            // Sunrise
            // 
            Sunrise.AccessibleRole = AccessibleRole.Outline;
            Sunrise.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Sunrise, "Sunrise");
            Sunrise.Name = "Sunrise";
            Sunrise.ReadOnly = true;
            Sunrise.UseWaitCursor = true;
            Sunrise.TextChanged += textBox2_TextChanged;
            // 
            // Dhuhr
            // 
            Dhuhr.AccessibleRole = AccessibleRole.Outline;
            Dhuhr.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Dhuhr, "Dhuhr");
            Dhuhr.Name = "Dhuhr";
            Dhuhr.ReadOnly = true;
            Dhuhr.UseWaitCursor = true;
            Dhuhr.TextChanged += textBox2_TextChanged;
            // 
            // label8
            // 
            resources.ApplyResources(label8, "label8");
            label8.ForeColor = SystemColors.ButtonHighlight;
            label8.Name = "label8";
            label8.Click += label1_Click;
            // 
            // label9
            // 
            resources.ApplyResources(label9, "label9");
            label9.ForeColor = SystemColors.ButtonHighlight;
            label9.Name = "label9";
            label9.Click += label1_Click;
            // 
            // label10
            // 
            resources.ApplyResources(label10, "label10");
            label10.ForeColor = SystemColors.ButtonHighlight;
            label10.Name = "label10";
            label10.Click += label1_Click;
            // 
            // Asr
            // 
            Asr.AccessibleRole = AccessibleRole.Outline;
            Asr.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Asr, "Asr");
            Asr.Name = "Asr";
            Asr.ReadOnly = true;
            Asr.UseWaitCursor = true;
            // 
            // Maghrib
            // 
            Maghrib.AccessibleRole = AccessibleRole.Outline;
            Maghrib.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Maghrib, "Maghrib");
            Maghrib.Name = "Maghrib";
            Maghrib.ReadOnly = true;
            Maghrib.UseWaitCursor = true;
            Maghrib.TextChanged += textBox2_TextChanged;
            // 
            // Isha
            // 
            Isha.AccessibleRole = AccessibleRole.Outline;
            Isha.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Isha, "Isha");
            Isha.Name = "Isha";
            Isha.ReadOnly = true;
            Isha.UseWaitCursor = true;
            Isha.TextChanged += textBox2_TextChanged;
            // 
            // label11
            // 
            resources.ApplyResources(label11, "label11");
            label11.ForeColor = SystemColors.ButtonHighlight;
            label11.Name = "label11";
            label11.Click += label1_Click;
            // 
            // NextPray
            // 
            NextPray.AccessibleRole = AccessibleRole.Outline;
            NextPray.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(NextPray, "NextPray");
            NextPray.Name = "NextPray";
            NextPray.ReadOnly = true;
            NextPray.UseWaitCursor = true;
            NextPray.TextChanged += textBox2_TextChanged;
            // 
            // label12
            // 
            resources.ApplyResources(label12, "label12");
            label12.ForeColor = SystemColors.ButtonHighlight;
            label12.Name = "label12";
            label12.Click += label1_Click;
            // 
            // TimeLeft
            // 
            TimeLeft.AccessibleRole = AccessibleRole.Outline;
            TimeLeft.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(TimeLeft, "TimeLeft");
            TimeLeft.Name = "TimeLeft";
            TimeLeft.ReadOnly = true;
            TimeLeft.UseWaitCursor = true;
            TimeLeft.TextChanged += textBox2_TextChanged;
            // 
            // Clock
            // 
            Clock.AccessibleRole = AccessibleRole.Outline;
            Clock.BorderStyle = BorderStyle.FixedSingle;
            resources.ApplyResources(Clock, "Clock");
            Clock.Name = "Clock";
            Clock.ReadOnly = true;
            Clock.UseWaitCursor = true;
            Clock.TextChanged += textBox2_TextChanged;
            // 
            // MainForm
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.WindowFrame;
            Controls.Add(TimeLeft);
            Controls.Add(Clock);
            Controls.Add(NextPray);
            Controls.Add(Isha);
            Controls.Add(Dhuhr);
            Controls.Add(MethodPlaceholder);
            Controls.Add(Maghrib);
            Controls.Add(Sunrise);
            Controls.Add(CityPlaceholder);
            Controls.Add(Asr);
            Controls.Add(Fajr);
            Controls.Add(label12);
            Controls.Add(ContryPlaceholder);
            Controls.Add(label11);
            Controls.Add(CityInput);
            Controls.Add(label10);
            Controls.Add(ContryInput);
            Controls.Add(label7);
            Controls.Add(label3);
            Controls.Add(label9);
            Controls.Add(SaveButton);
            Controls.Add(label6);
            Controls.Add(label2);
            Controls.Add(label8);
            Controls.Add(CityLabel);
            Controls.Add(label5);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(SettingsLabel);
            Controls.Add(ContryLabel);
            MaximizeBox = false;
            Name = "MainForm";
            Load += MainForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label ContryLabel;
        private Label CityLabel;
        private Button SaveButton;
        private ComboBox ContryInput;
        private ComboBox CityInput;
        private TextBox ContryPlaceholder;
        private Label SettingsLabel;
        private Label label1;
        private Label label2;
        private TextBox CityPlaceholder;
        private Label label3;
        private TextBox MethodPlaceholder;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox Fajr;
        private TextBox Sunrise;
        private TextBox Dhuhr;
        private Label label8;
        private Label label9;
        private Label label10;
        private TextBox Asr;
        private TextBox Maghrib;
        private TextBox Isha;
        private Label label11;
        private TextBox NextPray;
        private Label label12;
        private TextBox TimeLeft;
        private TextBox Clock;
    }
}
