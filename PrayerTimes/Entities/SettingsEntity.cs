namespace PrayerTimes.Entities;
public class SettingsEntity
{
    public int Id { get; set; }
    public required string Country { get; set; }
    public required string City { get; set; }
    public string? Method { set; get; }

}
