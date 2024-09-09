using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Media;
using System.Text.Json;
using System.Timers;


namespace PrayerTimes.Helper
{
    internal class Shduler : ShdulerBase
    {
        private readonly CancellationTokenSource _cancellationTokenSource;
        private Dictionary<string, string> _timings;
        private readonly System.Timers.Timer _timer;

        private readonly HttpClient _httpClient;
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
        private readonly TextBox _nextPrayerLabel;
        private readonly TextBox _timeLeftLabel;
        private readonly Dictionary<string, string> SlawatNames = [];
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
            _timings = [];
            _httpClient = new HttpClient();
            dbContext = dbContext;
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
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += UpdateTimeLeft;
            _timer.Start();
            UpdateTimeLeft(null, null);
        }

        //private async void ScheduleTaskAtSpecificTime(TimeOfDay timeOfDay)
        //{
        //    var cancellationToken = _cancellationTokenSource.Token;

        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        var now = DateTime.Now;
        //        var nextRunTime = GetNextRunTime(timeOfDay);

        //        var delay = nextRunTime - now;
        //        if (delay.TotalMilliseconds > 0)
        //        {
        //            await Task.Delay(delay, cancellationToken);

        //        }else
        //        {
        //        await PerformScheduledTask();
        //        nextRunTime = nextRunTime.AddDays(1);
        //        delay = nextRunTime - DateTime.Now;
        //        await Task.Delay(delay, cancellationToken);
        //        }
        //    }
        //}
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
                    try
                    {
                        await Task.Delay(delay, cancellationToken);
                        await PerformScheduledTask();
                    }
                    catch (TaskCanceledException)
                    {  
                    }
                }
            }
        }

        private void UpdateTimeLeft(object? sender, ElapsedEventArgs? e)
        {
                var now = DateTime.Now;
                var nextPrayer = GetNextPrayerTime(now);
            try
            {
                if (nextPrayer != null)
                {
                    var nextPrayerTime = DateTime.ParseExact(nextPrayer.Value.Value, "HH:mm:ss", null);
                    var timeLeft = nextPrayerTime - now;
                    if (_nextPrayerLabel.InvokeRequired)
                    {
                        _nextPrayerLabel.Invoke(new Action(() =>
                        {
                            if (SlawatNames.TryGetValue(nextPrayer.Value.Key, out var prayerName))
                            {
                                _nextPrayerLabel.Text = SlawatNames[nextPrayer.Value.Key];
                            }
                            else
                            {
                                _nextPrayerLabel.Text = nextPrayer.Value.Key;
                            }
                        }));
                    }
                    else
                    {
                        if (SlawatNames.TryGetValue(nextPrayer.Value.Key, out var prayerName))
                        {
                            _nextPrayerLabel.Text = SlawatNames[nextPrayer.Value.Key];
                        }else
                        {
                        _nextPrayerLabel.Text = nextPrayer.Value.Key;
                        }
                    }
                    if (_timeLeftLabel.InvokeRequired)
                    {
                        _timeLeftLabel.Invoke(new Action(() =>
                        {
                            _timeLeftLabel.Text = $"{timeLeft.Hours}h {timeLeft.Minutes}m {timeLeft.Seconds}s";
                        }));
                    }
                    else
                    {
                        _timeLeftLabel.Text = $"{timeLeft.Hours}h {timeLeft.Minutes}m {timeLeft.Seconds}s";
                    }
                    if (nextPrayer.Value.Key == "Fajr (Next Day)")
                    {
                        _timer.Stop();
                    }
                }
                else
                {
                    _nextPrayerLabel.Text = "No More prayers today Geting Next Firts Pray";
                    _timeLeftLabel.Text = "";
                }
            }
            catch (Exception ex)
            {
                if (nextPrayer != null && ex.Message.Contains($"the given key '{nextPrayer.Value.Key}' was not present in the dictionary"))
                {
                    MessageBox.Show($"Error updating time left: {nextPrayer.Value.Key}");
                }
                else
                {
                MessageBox.Show($"Error updating time left: {ex.Message}");
                }
            }
        }

        private void RefetchPrayerTimes(object sender, ElapsedEventArgs e)
        {
            _timings = FetchPrayerTimesAsync().Result;
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
            foreach (var prayer in _timings)
            {
                var prayerTime = DateTime.ParseExact(prayer.Value, "HH:mm:ss", null);

                if (now < prayerTime)
                {
                    return prayer;
                }
            }
            if (_timings.TryGetValue("Fajr", out var fajrTime))
            {
                var nextDayFajrTime = DateTime.ParseExact(fajrTime, "HH:mm:ss", null).AddDays(1);

                return new KeyValuePair<string, string>("Fajr (Next Day)", nextDayFajrTime.ToString("HH:mm:ss"));
            }

            return null;

        }
        private static DateTime GetNextRunTime(TimeOfDay timeOfDay)
        {
            var now = DateTime.Now;
            var nextRunTime = new DateTime(now.Year, now.Month, now.Day, timeOfDay.Hour, timeOfDay.Minute, 0);

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }
            return nextRunTime;
        }

        private async Task PerformScheduledTask()
        {
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
                        timings[property.Name] = $"{property.Value.GetString()}:00";
                    }
                    return timings;
                }

                //var timings = new Dictionary<string, string>();
                //timings["Fajr"] =       "06:40:00";
                //timings["Sunrise"] =    "06:43:00";
                //timings["Dhuhr"] =      "06:46:00";
                //timings["Asr"] =        "06:49:00";
                //timings["Maghrib"] =    "06:42:00";
                //timings["Isha"] =       "06:45:00";
                //return timings;
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
           try
            {
                var now = DateTime.Now;
                var currentTime = now.ToString("HH:mm:ss");
                if (_timings.ContainsValue(currentTime))
                {
                    var prayerName = _timings.FirstOrDefault(x => x.Value == currentTime).Key;
                    string PrayerName = SlawatNames[prayerName];
                    string Message = $"حان الان أذان {PrayerName} من الساعة {currentTime}";
                    MessageBox.Show(Message);
                    PlayRingtone();
                }
            } 
            catch (Exception ex)
            {
                //MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private static void PlayRingtone()
        {
            try
            {
               string soundFilePath = Path.Combine(Application.StartupPath, "Azan.wav");
                using SoundPlayer player = new(soundFilePath);
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error playing sound: {ex.Message}");
            }
        }

        internal void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _httpClient.Dispose();
        }

        private class TimeOfDay(int hour, int minute)
        {
            public int Hour { get; } = hour;
            public int Minute { get; } = minute;

            public static TimeOfDay Hour6AM = new(6, 0);
            public static TimeOfDay Hour01Am = new(0, 1);
        }
    }
}
