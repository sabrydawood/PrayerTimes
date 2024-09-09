using PrayerTimes.Database;
using PrayerTimes.Entities;
using PrayerTimes.Helper;
using System.Timers;

namespace PrayerTimes
{
    public partial class MainForm : Form
    {
        private readonly AppDbContext _dbContext;
        private readonly System.Timers.Timer clockTimer;
        private readonly NotifyIcon trayIcon;
        private readonly ContextMenuStrip trayMenu;
        private Shduler _scheduler;

        public MainForm(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            clockTimer = new System.Timers.Timer(1000);
            clockTimer.Elapsed += SetClockTime;
            clockTimer.Start();
            InitializeComponent();
            List<ListItem> ReginsList = new();
            ReginsList.Add(new ListItem()
            {
                Text = "Egypt",
                Value = "egypt"

            });
            List<ListItem> CityList = new();
            CityList.Add(new ListItem()
            {
                Text = "Cairo",
                Value = "cairo"

            });
            CityList.Add(new ListItem()
            {
                Text = "Manura",
                Value = "Mansura"

            });
            IEnumerable<ListItem> Regins = ReginsList;
            IEnumerable<ListItem> Cities = CityList;

            ContryInput.Items.Clear();
            CityInput.Items.Clear();
            // Contry
            ContryInput.DataSource = Regins.ToList();
            ContryInput.DisplayMember = "Text";
            ContryInput.ValueMember = "Value";

            // City
            CityInput.DataSource = Cities.ToList();
            CityInput.DisplayMember = "Text";
            CityInput.ValueMember = "Value";
            var Settings = _dbContext.Settings.FirstOrDefault(g => g.Id == 1); ;
            if (Settings != null)
            {
                ContryPlaceholder.Text = Settings.Country;
                CityPlaceholder.Text = Settings.City;
                MethodPlaceholder.Text = Settings.Method;
            InitializeScheduler(Settings.Country, Settings.City, Settings.Method);
            }else
            {
                SettingsEntity newItem = new()
                {
                    Country = "egypt",
                    City = "cairo",
                    Method = "8"
                };

                _dbContext.Settings.Add(newItem);
                ContryPlaceholder.Text = "egypt";
                CityPlaceholder.Text = "cairo";
                MethodPlaceholder.Text = "8";
                _dbContext.SaveChanges();
                InitializeScheduler("egypt", "cairo", "8");
            }
            trayMenu = new ContextMenuStrip();
            var restoreMenuItem = new ToolStripMenuItem("Restore");
            var exitMenuItem = new ToolStripMenuItem("Exit");
            trayMenu.Items.AddRange([restoreMenuItem, exitMenuItem]);
            restoreMenuItem.Click += OnRestore;
            exitMenuItem.Click += OnExit;
            string ApplicationIcon = Application.StartupPath + "\\Icon.ico";
            Icon AppIcon = new(ApplicationIcon);
            this.Icon = AppIcon;
            this.ShowInTaskbar = true;
            trayIcon = new NotifyIcon
            {
                Icon = AppIcon, 
                ContextMenuStrip = trayMenu,
                Visible = true
            };
            trayIcon.DoubleClick += (sender, e) => OnRestore(sender, e);
            this.Resize += MainForm_Resize;
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
        
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                trayIcon.BalloonTipTitle = "Application Minimized";
                trayIcon.BalloonTipText = "Right-click the tray icon to restore.";
                trayIcon.ShowBalloonTip(3000);
            }
        }

        private void OnRestore(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void OnExit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            clockTimer.Stop();
            clockTimer.Dispose();
            Application.Exit();
        }
        private void SetClockTime(object sender, ElapsedEventArgs e)
        {
            if (this.IsHandleCreated && !this.IsDisposed)
            {
                this.Invoke((Action)(() => Clock.Text = DateTime.Now.ToString("hh:mm:ss tt")));
            }
        }

        private void InitializeScheduler(string contry = "egypt", string city = "cairo", string? method = "8")
        {
            var Settings = _dbContext.Settings.FirstOrDefault(g => g.Id == 1);
            if (Settings != null)
            {

                ContryPlaceholder.Text = Settings.Country;
                CityPlaceholder.Text = Settings.City;
                MethodPlaceholder.Text = Settings.Method;
                var dbContext = new AppDbContext();
                _scheduler = new Shduler(dbContext, Fajr, Sunrise, Dhuhr, Asr, Maghrib, Isha, NextPray, TimeLeft, contry, city, method);

            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            _scheduler?.Dispose();
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
       private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void ContryInput_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    
        private void SaveButton_Click(object sender, EventArgs e) {
            ListItem? selectedCountry = ContryInput.SelectedItem as ListItem;
            ListItem? selectedCity = CityInput.SelectedItem as ListItem;
            if(selectedCountry?.Value != null && selectedCity?.Value != null)
            {
                var Settings = _dbContext.Settings.FirstOrDefault(g => g.Id == 1); ;
                if (Settings != null)
                {
                    Settings.Country = selectedCountry?.Value;
                    Settings.City = selectedCity?.Value;

                    MessageBox.Show("Updated Data");
                }
                else
                {
                    SettingsEntity newItem = new()
                    {
                        Country = selectedCountry?.Value,
                        City = selectedCity?.Value,
                        Method = "8"
                    };

                    _dbContext.Settings.Add(newItem);
                    MessageBox.Show("New Data");

                }
                ContryPlaceholder.Text = Settings.Country;
                CityPlaceholder.Text = Settings.City;
                MethodPlaceholder.Text = Settings.Method;
                _dbContext.SaveChanges();
            }

        }
    }
}
