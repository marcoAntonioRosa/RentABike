using RentABike.Domain.Dtos;

namespace RentABike.Domain.Interfaces;

public interface IDeliveryPersonService
{
    Task Create(DeliveryPersonDto deliveryPersonDto);
    Task<IEnumerable<DeliveryPersonDto>> GetAll();
    Task<DeliveryPersonDto> GetByCnpj(string cnpj);
    Task<DeliveryPersonDto> Update(DeliveryPersonDto newDeliveryPersonDto);
    Task DeleteById(string id);
}