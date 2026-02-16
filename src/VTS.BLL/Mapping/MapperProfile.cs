using AutoMapper;
using VTS.BLL.DTOs;
using VTS.DAL.Entities;
using NetTopologySuite.Geometries;

namespace VTS.BLL.Mapping;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Vehicle, VehicleDto>()
            .ForMember(dest => dest.VIN, opt => opt.MapFrom(src => src.VIN))
            .ReverseMap();

        CreateMap<Tenant, TenantDto>().ReverseMap();
        CreateMap<GpsPosition, GpsPositionDto>().ReverseMap();
        CreateMap<VehicleInsurance, VehicleInsuranceDto>().ReverseMap();
        CreateMap<VehicleTechnicalInspection, VehicleTechnicalInspectionDto>().ReverseMap();
        CreateMap<Notification, NotificationDto>().ReverseMap();

        CreateMap<GeoFence, GeofenceDto>()
            .ForMember(dest => dest.Center, opt => opt.MapFrom(src => new PointDto
            {
                Latitude = src.Center.Y,
                Longitude = src.Center.X
            }))
            .ReverseMap()
            .ForMember(dest => dest.Center, opt => opt.MapFrom(src => CreatePoint(src.Center)));

        CreateMap<FuelAlert, FuelAlertDto>()
            .ForMember(dest => dest.AlertType, opt => opt.MapFrom(src => (FuelAlertTypeDto)src.AlertType))
            .ReverseMap()
            .ForMember(dest => dest.AlertType, opt => opt.MapFrom(src => (FuelAlertType)src.AlertType));

        CreateMap<VehiclePart, VehiclePartDto>().ReverseMap();
    }

    private static Point CreatePoint(PointDto dto)
    {
        var factory = new GeometryFactory(new PrecisionModel(), 4326);
        return factory.CreatePoint(new Coordinate(dto.Longitude, dto.Latitude));
    }
}
