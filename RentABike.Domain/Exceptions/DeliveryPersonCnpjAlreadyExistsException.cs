namespace RentABike.Domain.Exceptions;

public class DeliveryPersonCnpjAlreadyExistsException(string cnpj) : Exception
{
    private string Cnpj { get; } = cnpj;
    public override string Message => $"A user with the CNPJ '{Cnpj}' already exists";
}