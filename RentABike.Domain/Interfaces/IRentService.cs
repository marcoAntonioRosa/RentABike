using RentABike.Domain.Dtos;

namespace RentABike.Domain.Interfaces;

public interface IRentService
{
    Task<RentalDto> Rent(RentDto rentDto);
}