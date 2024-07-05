namespace RentABike.Domain.Exceptions;

public class DeliveryPersonNotFoundCnpjException(string cnpj) : Exception
{
    private string Cnpj { get; } = cnpj;
    public override string Message => $"Couldn't find a user with the cnpj '{Cnpj}'";
}