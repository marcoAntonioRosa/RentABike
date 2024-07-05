using RentABike.Domain.Entities;
using RentABike.Domain.Enums;

namespace RentABike.Domain.Interfaces;

public interface IDeliveryPersonRepository
{
    Task<DeliveryPerson> Add(DeliveryPerson deliveryPerson);
    Task<IEnumerable<DeliveryPerson>> GetAll();
    Task<DeliveryPerson?> GetById(string id);
    Task<DeliveryPerson?> GetByCnpj(string cnpj);
    Task<LicenseType> GetDriverLicenseTypeById(string id);
    Task<bool> CheckId(string id);
    Task<bool> CheckCnpj(string cnpj);
    Task<bool> CheckDriverLicense(string driverLicense);
    Task<DeliveryPerson> Update(DeliveryPerson deliveryPerson);
    Task Delete(string id);
}