using Microsoft.EntityFrameworkCore;
using System.Media;
using System.Net.Http;
using System.Text.Json;
using System.Timers;


namespace PrayerTimes.Helper
{
    internal class Shduler
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Dictionary<string, string> _timings;
        private readonly System.Timers.Timer _timer;
        private readonly System.Timers.Timer _refetchTimer;

        private readonly HttpClient _httpClient;
        private readonly AppDbContext _dbContext;
        private readonly string _country;
        private readonly string _city;
        private readonly string _method;
        private DateTime _lastFetchDate;

        private readonly TextBox _fajrLabel;
        private readonly TextBox _sunriseLabel;
        private readonly TextBox _dhuhrLabel;
        private readonly TextBox _asrLabel;
        private readonly TextBox _maghribLabel;
        private readonly TextBox _ishaLabel;
        private TextBox _nextPrayerLabel;
        private TextBox _timeLeftLabel;
        private Dictionary<string, string> SlawatNames = new();
        public Shduler(AppDbContext dbContext, TextBox fajrLabel, TextBox sunriseLabel, TextBox dhuhrLabel, TextBox asrLabel, TextBox maghribLabel, TextBox ishaLabel, TextBox nextPrayerLabel, TextBox timeLeftLabel, string country = "egypt", string city = "cairo", string method = "8")
        {
            // Store references to the labels
            SlawatNames.Add("Fajr", "الفجر");
            SlawatNames.Add("Sunrise", "الشروق");
            SlawatNames.Add("Dhuhr", "الظهر");
            SlawatNames.Add("Asr", "العصر");
            SlawatNames.Add("Maghrib", "المغرب");
            SlawatNames.Add("Isha", "العشاء");
            _fajrLabel = fajrLabel;
            _sunriseLabel = sunriseLabel;
            _dhuhrLabel = dhuhrLabel;
            _asrLabel = asrLabel;
            _maghribLabel = maghribLabel;
            _ishaLabel = ishaLabel;

            _cancellationTokenSource = new CancellationTokenSource();
            _timings = new Dictionary<string, string>();
            _httpClient = new HttpClient();
            _dbContext = dbContext;
            _country = country;
            _city = city;
            _method = method;
            _lastFetchDate = DateTime.MinValue;
            Task.Run(async () =>
            {
                await PerformScheduledTask();
            });
            ScheduleTaskAtSpecificTime(TimeOfDay.Hour01Am);
            _nextPrayerLabel = nextPrayerLabel;
            _timeLeftLabel = timeLeftLabel;

            // Create a timer that updates every minute (60000 ms)
            _timer = new System.Timers.Timer(60000);
            _timer.Elapsed += UpdateTimeLeft;
            _timer.Start();
            // Create the refetch timer
            _refetchTimer = new System.Timers.Timer(2000); // Refetch after 2 seconds
            _refetchTimer.Elapsed += RefetchPrayerTimes;
            _refetchTimer.AutoReset = false; // Only run once when triggered

            // Immediately show the next prayer and time left
            UpdateTimeLeft(null, null);
        }

        private async void ScheduleTaskAtSpecificTime(TimeOfDay timeOfDay)
        {
            var cancellationToken = _cancellationTokenSource.Token;

            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var nextRunTime = GetNextRunTime(timeOfDay);

                var delay = nextRunTime - now;
                if (delay.TotalMilliseconds > 0)
                {
                    await Task.Delay(delay, cancellationToken);
                }

                // Perform the scheduled task
                await PerformScheduledTask();

                // Schedule the next run
                nextRunTime = nextRunTime.AddDays(1); // Schedule daily
                delay = nextRunTime - DateTime.Now;
                await Task.Delay(delay, cancellationToken);
            }
        }

        private void UpdateTimeLeft(object sender, ElapsedEventArgs e)
        {
            try
            {
                // Get the current time
                var now = DateTime.Now;

                // Find the next prayer time
                var nextPrayer = GetNextPrayerTime(now);

                if (nextPrayer != null)
                {
                    var nextPrayerTime = DateTime.ParseExact(nextPrayer.Value.Value, "HH:mm", null);
                    var timeLeft = nextPrayerTime - now;

                    // Update the UI (ensure you are invoking on the UI thread)
                    if (_nextPrayerLabel.InvokeRequired)
                    {
                        _nextPrayerLabel.Invoke(new Action(() =>
                        {
                            _nextPrayerLabel.Text = SlawatNames[nextPrayer.Value.Key];
                        }));
                    }
                    else
                    {
                        _nextPrayerLabel.Text = SlawatNames[nextPrayer.Value.Key];
                    }

                    if (_timeLeftLabel.InvokeRequired)
                    {
                        _timeLeftLabel.Invoke(new Action(() =>
                        {
                            _timeLeftLabel.Text = $"{timeLeft.Hours}h {timeLeft.Minutes}m";
                        }));
                    }
                    else
                    {
                        _timeLeftLabel.Text = $"{timeLeft.Hours}h {timeLeft.Minutes}m";
                    }

                    // If the next prayer is tomorrow, handle that logic
                    if (nextPrayer.Value.Key == "Fajr (Next Day)")
                    {
                        // Stop the 1-minute timer and only use the 2-second refetch timer
                        _timer.Stop();
                        _refetchTimer.Start();
                    }
                    else
                    {
                        // Continue with the 1-minute timer
                        _refetchTimer.Stop();
                    }
                }
                else
                {
                    // Handle the case where no prayer times are found (unlikely in a normal setup)
                    _nextPrayerLabel.Text = "No More prayers today Geting Next Firts Pray";
                    _timeLeftLabel.Text = "";

                    // Start the 2-second refetch timer
                    _refetchTimer.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating time left: {ex.Message}");
            }
        }

        private void RefetchPrayerTimes(object sender, ElapsedEventArgs e)
        {
            // Fetch new prayer times
            // Replace _timings with new values from the API
            _timings = FetchPrayerTimesAsync().Result;

            // Update the UI with new prayer times
            if (_nextPrayerLabel.InvokeRequired)
            {
                _nextPrayerLabel.Invoke(new Action(() =>
                {
                    UpdateTimeLeft(null, null);
                }));
            }
            else
            {
                UpdateTimeLeft(null, null);
            }
        }
        private KeyValuePair<string, string>? GetNextPrayerTime(DateTime now)
        {
            // Check for any upcoming prayer times today
            foreach (var prayer in _timings)
            {
                var prayerTime = DateTime.ParseExact(prayer.Value, "HH:mm", null);

                if (now < prayerTime)
                {
                    // Return the next prayer if it hasn't occurred yet
                    return prayer;
                }
            }

            // If all prayers for today have passed, return Fajr for the next day
            if (_timings.TryGetValue("Fajr", out var fajrTime))
            {
                // Parse the Fajr time for the next day
                var nextDayFajrTime = DateTime.ParseExact(fajrTime, "HH:mm", null).AddDays(1);

                return new KeyValuePair<string, string>("Fajr (Next Day)", nextDayFajrTime.ToString("HH:mm"));
            }

            return null; // No prayers found (though this case should never occur in a normal setup)

        }
        private DateTime GetNextRunTime(TimeOfDay timeOfDay)
        {
            var now = DateTime.Now;
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day, timeOfDay.Hour, timeOfDay.Minute, 0);

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1); // Schedule for the next day if the time has already passed
            }
            return nextRunTime;
        }

        private async Task PerformScheduledTask()
        {
            // Fetch data only if it's not already fetched today
            if (_timings.Count == 0 || IsNewDay())
            {
                _timings = await FetchPrayerTimesAsync();
                _lastFetchDate = DateTime.Now.Date;
            }
            CheckPrayerTimes();
            UpdatePrayerTimesUI();
        }
        private void UpdatePrayerTimesUI()
        {
            if (_fajrLabel.InvokeRequired)
            {
                _fajrLabel.Invoke(new Action(UpdatePrayerTimesUI));
                return;
            }

            if (_timings != null)
            {
                _fajrLabel.Text = _timings.TryGetValue("Fajr", out var fajr) ? fajr : "";
                _sunriseLabel.Text = _timings.TryGetValue("Sunrise", out var sunrise) ? sunrise : "";
                _dhuhrLabel.Text = _timings.TryGetValue("Dhuhr", out var dhuhr) ? dhuhr : "";
                _asrLabel.Text = _timings.TryGetValue("Asr", out var asr) ? asr : "";
                _maghribLabel.Text = _timings.TryGetValue("Maghrib", out var maghrib) ? maghrib : "";
                _ishaLabel.Text = _timings.TryGetValue("Isha", out var isha) ? isha : "";
            }
        }
        private bool IsNewDay()
        {
            return DateTime.Now.Date > _lastFetchDate;
        }

        private async Task<Dictionary<string, string>> FetchPrayerTimesAsync()
        {
            try
            {
                var date = DateTime.Now.ToString("dd-MM-yyyy");
                var url = $"https://api.aladhan.com/v1/timingsByCity/{date}?city={_city}&country={_country}&method={_method}";
                var response = await _httpClient.GetStringAsync(url);
                var jsonDocument = JsonDocument.Parse(response);
                var rootElement = jsonDocument.RootElement;
                if (rootElement.TryGetProperty("data", out var dataElement) &&
                    dataElement.TryGetProperty("timings", out var timingsElement))
                {
                    var timings = new Dictionary<string, string>();
                    foreach (var property in timingsElement.EnumerateObject())
                    {
                        timings[property.Name] = property.Value.GetString();
                    }
                    return timings;
                }
                return [];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return [];
            }
        }

        private void CheckPrayerTimes()
        {
            var now = DateTime.Now;
            var currentTime = now.ToString("HH:mm");

            if (_timings.ContainsValue(currentTime))
            {
                var prayerName = _timings.FirstOrDefault(x => x.Value == currentTime).Key;
                MessageBox.Show($"{prayerName} Time: {currentTime}");
                PlayRingtone();
            }
        }

        private void PlayRingtone()
        {
            try
            {
                // Assuming the sound file is in the root of your project and will be copied to the output directory
                string soundFilePath = Path.Combine(Application.StartupPath, "Azan.mp3");

                // Play the sound
                SoundPlayer player = new(soundFilePath);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}");
            }
        }

        private class TimeOfDay
        {
            public int Hour { get; }
            public int Minute { get; }

            public TimeOfDay(int hour, int minute)
            {
                Hour = hour;
                Minute = minute;
            }

            public static TimeOfDay Hour6AM = new TimeOfDay(6, 0);
            public static TimeOfDay Hour01Am = new TimeOfDay(0, 1);
        }
    }
}
