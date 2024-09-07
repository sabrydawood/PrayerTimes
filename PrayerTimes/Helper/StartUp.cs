using Microsoft.Win32;
namespace PrayerTimes.Helper;

public class StartUp
{
    public static void SetStartup(bool enable)
    {
        string appName = "PrayerTimes";
        string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true))
        {
            if (enable)
            {
                key.SetValue(appName, exePath);
            }
            else
            {
                key.DeleteValue(appName, false);
            }
        }
    }
 }




