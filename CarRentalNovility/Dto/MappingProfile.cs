using AutoMapper;
using CarRentalNovility.Entities;

namespace CarRentalNovility.Web.Dto
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClientAccount, ClientAccountDto>()
                .ReverseMap();
            CreateMap<Client, ClientDto>()
                .ReverseMap();
            CreateMap<Car, CarDto>()
                .ReverseMap();
            CreateMap<CarType, CarTypeDto>()
                .ReverseMap();
            CreateMap<Reservation, ReservationDto>()
                .ReverseMap();
            CreateMap<BrowseReservationsParameters, BrowseReservationsParametersDto>()
                .ReverseMap();
            CreateMap<BookCarReservationParameters, BookCarReservationParametersDto>()
                .ReverseMap();
        }
    }
}
