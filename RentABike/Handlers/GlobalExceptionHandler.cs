using System.Diagnostics;
using System.Reflection.Metadata;
using Amazon.S3;
using Microsoft.AspNetCore.Diagnostics;
using RentABike.Domain.Exceptions;

namespace RentABike.Handlers;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId
        );

        var (statusCode, title, message) = MapException(exception);

        await Results.Problem(
            title: title,
            statusCode: statusCode,
            extensions: new Dictionary<string, object?>
            {
                { "message", message },
                { "traceId", traceId }
            }
        ).ExecuteAsync(httpContext);

        return true;
    }

    private static (int StatusCode, string Title, string Message) MapException(Exception exception)
    {
        return exception switch
        {
            BikeAlreadyExistsException => (StatusCodes.Status409Conflict, "Bike already exists", exception.Message),
            BikeCannotBeDeletedException => (StatusCodes.Status409Conflict, "Cannot delete bike", exception.Message),
            BikeLicensePlateNotFoundException => (StatusCodes.Status404NotFound, "Bike not found", exception.Message),
            
            RentalEarlyReturnFeeException => (StatusCodes.Status500InternalServerError, "Invalid plan category price", exception.Message),
            RentalPeriodTooLowException => (StatusCodes.Status406NotAcceptable, "Rental period is too low", exception.Message),
            
            DeliveryPersonCnpjAlreadyExistsException => (StatusCodes.Status409Conflict, "User CNPJ already being used", exception.Message),
            DeliveryPersonDriverLicenseAlreadyExistsException => (StatusCodes.Status409Conflict, "User driver's license already being used", exception.Message),
            DeliveryPersonNotAllowedToRentException => (StatusCodes.Status403Forbidden, "Invalid driver's license type", exception.Message),
            DeliveryPersonNotFoundCnpjException => (StatusCodes.Status404NotFound, "User not found", exception.Message),
            
            ImageFormatLimitationException => (StatusCodes.Status406NotAcceptable, "Invalid image type", exception.Message),
            AmazonS3Exception => (StatusCodes.Status404NotFound, "Image not found", exception.Message),
            
            _ => (StatusCodes.Status500InternalServerError, "An error occurred while processing your request", "")
        };
    }
}