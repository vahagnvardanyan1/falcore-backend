namespace VTS.BLL.Configuration;

public class JobSchedulingSettings
{
    public const string Section = nameof(JobSchedulingSettings);

    public double GeofenceMonitoringIntervalMinutes { get; set; }
    public double FuelAlertMonitoringIntervalMinutes { get; set; }
    public double VehicleInsuranceCheckIntervalHours { get; set; }
}
