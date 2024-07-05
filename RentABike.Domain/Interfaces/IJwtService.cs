using RentABike.Domain.Entities;

namespace RentABike.Domain.Interfaces;

public interface IJwtService
{
    string CreateToken(User user, IList<string> roles);
}