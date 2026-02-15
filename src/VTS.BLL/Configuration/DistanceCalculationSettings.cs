using System;

namespace VTS.BLL.Configuration;

public class DistanceCalculationSettings
{
    public const string Section = nameof(DistanceCalculationSettings);

    public int MinDistance { get; set; }
    public int MaxDistance { get; set; }
    public int MaxSpeedKmh { get; set; }
}
