using Mapster;
using Microsoft.AspNetCore.Http;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Exceptions;
using RentABike.Domain.Interfaces;

namespace RentABike.Application.Services;

public class DeliveryPersonService(IDeliveryPersonRepository deliveryPersonRepository) : IDeliveryPersonService
{
    public async Task Create(DeliveryPersonDto deliveryPersonDto)
    {
        var isCnpjAlreadyRegistered = await deliveryPersonRepository.CheckCnpj(deliveryPersonDto.Cnpj);
        if (isCnpjAlreadyRegistered)
            throw new DeliveryPersonCnpjAlreadyExistsException(deliveryPersonDto.Cnpj);
        
        var isDriverLicenseAlreadyRegistered = await deliveryPersonRepository.CheckDriverLicense(deliveryPersonDto.DriverLicenseNumber);
        if (isDriverLicenseAlreadyRegistered)
            throw new DeliveryPersonDriverLicenseAlreadyExistsException(deliveryPersonDto.DriverLicenseNumber);
        
        var deliveryPerson = deliveryPersonDto.Adapt<DeliveryPerson>();
        await deliveryPersonRepository.Add(deliveryPerson);
    }
    
    public async Task<IEnumerable<DeliveryPersonDto>> GetAll()
    {
        var deliveryPeople = await deliveryPersonRepository.GetAll();
        return deliveryPeople.Adapt<IEnumerable<DeliveryPersonDto>>();
    }

    public async Task<DeliveryPersonDto> GetByCnpj(string cnpj)
    {
        var deliveryPerson = await deliveryPersonRepository.GetByCnpj(cnpj);
        if (deliveryPerson == null)
            throw new DeliveryPersonNotFoundCnpjException(cnpj);

        return deliveryPerson.Adapt<DeliveryPersonDto>();
    }

    public async Task<DeliveryPersonDto> Update(DeliveryPersonDto newDeliveryPersonDto)
    {
        var deliveryPerson = await deliveryPersonRepository.GetById(newDeliveryPersonDto.DeliveryPersonId);
        if (deliveryPerson == null)
            throw new DeliveryPersonNotFoundCnpjException(newDeliveryPersonDto.Cnpj);

        newDeliveryPersonDto.Adapt(deliveryPerson);
        
        await deliveryPersonRepository.Update(deliveryPerson);
        return deliveryPerson.Adapt<DeliveryPersonDto>();
    }

    public async Task DeleteById(string id)
    {
        await deliveryPersonRepository.Delete(id);
    }
}