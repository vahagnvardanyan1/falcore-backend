using NetTopologySuite.Geometries;

namespace VTS.Common.Utilities;

public static class GeospatialHelper
{
    public static bool IsPointInCircle(Point point, Point center, double radiusMeters)
    {
        if (point == null || center == null)
            return false;

        // Use Haversine formula for accurate geodetic distance (WGS84)
        const double EarthRadiusMeters = 6371000;

        var lat1 = ToRadians(center.Y);
        var lat2 = ToRadians(point.Y);
        var deltaLat = ToRadians(point.Y - center.Y);
        var deltaLon = ToRadians(point.X - center.X);

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var distance = EarthRadiusMeters * c;

        return distance <= radiusMeters;
    }

    private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}
