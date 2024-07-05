using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using RentABike.Application.Services;
using RentABike.Domain.Dtos;
using RentABike.Domain.Entities;
using RentABike.Domain.Enums;
using RentABike.Domain.Exceptions;
using RentABike.Domain.Interfaces;

namespace RentABike.Tests;

public class RentServiceTest
{
    private readonly RentService _rentService;
    private readonly Mock<IRentRepository> _rentRepository = new();
    private readonly Mock<IDeliveryPersonRepository> _deliveryPersonRepository = new();
    private readonly Mock<IBikeRepository> _bikeRepository = new();

    public RentServiceTest()
    {
        _rentService =
            new RentService(_rentRepository.Object, _deliveryPersonRepository.Object, _bikeRepository.Object);
    }

    [SetUp]
    public void Setup()
    {
        var mockDeliveryPerson = new Mock<DeliveryPerson>();
        mockDeliveryPerson.Object.DriverLicenseType = LicenseType.A;
        
        _rentRepository
            .Setup(s => s.Add(It.IsAny<Rent>()))
            .ReturnsAsync(It.IsAny<Rent>());

        _deliveryPersonRepository
            .Setup(s => s.CheckId(It.IsAny<string>()))
            .ReturnsAsync(true);

        _deliveryPersonRepository
            .Setup(s => s.GetById(It.IsAny<string>()))
            .ReturnsAsync(mockDeliveryPerson.Object);

        _deliveryPersonRepository
            .Setup(s => s.GetDriverLicenseTypeById(It.IsAny<string>()))
            .ReturnsAsync(LicenseType.A);

        _bikeRepository
            .Setup(s => s.CheckId(It.IsAny<int>()))
            .ReturnsAsync(true);
    }

    [Test]
    public async Task Rent_SevenDaysRental_ReturnsRentalValue()
    {
        var rentDto = new Mock<RentDto>().Object;
        rentDto.EndDate = DateTime.Today.AddDays(8);
        rentDto.InformedEndDate = DateTime.Today.AddDays(8);

        var response = await _rentService.Rent(rentDto);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(210));
            Assert.That(response?.PenaltyValue, Is.EqualTo(0));
            Assert.That(response?.TotalAmount, Is.EqualTo(210));
        });
    }

    [Test]
    public async Task Rent_FifteenDaysRental_ReturnsRentalValue()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(16);
        rent.InformedEndDate = DateTime.Today.AddDays(16);

        var response = await _rentService.Rent(rent);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(420));
            Assert.That(response?.PenaltyValue, Is.EqualTo(0));
            Assert.That(response?.TotalAmount, Is.EqualTo(420));
        });
    }

    [Test]
    public async Task Rent_ThirtyDaysRental_ReturnsRentalValue()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(31);
        rent.InformedEndDate = DateTime.Today.AddDays(31);

        var response = await _rentService.Rent(rent);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(660));
            Assert.That(response?.PenaltyValue, Is.EqualTo(0));
            Assert.That(response?.TotalAmount, Is.EqualTo(660));
        });
    }

    [Test]
    public async Task Rent_FortyFiveDaysRental_ReturnsRentalValue()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(46);
        rent.InformedEndDate = DateTime.Today.AddDays(46);

        var response = await _rentService.Rent(rent);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(900));
            Assert.That(response?.PenaltyValue, Is.EqualTo(0));
            Assert.That(response?.TotalAmount, Is.EqualTo(900));
        });
    }

    [Test]
    public async Task Rent_FiftyDaysRental_ReturnsRentalValue()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(51);
        rent.InformedEndDate = DateTime.Today.AddDays(51);

        var response = await _rentService.Rent(rent);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(900));
            Assert.That(response?.PenaltyValue, Is.EqualTo(0));
            Assert.That(response?.TotalAmount, Is.EqualTo(900));
        });
    }

    [Test]
    public async Task Rent_UnderSevenDaysRental_ThrowsException()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(5);
        rent.InformedEndDate = DateTime.Today.AddDays(5);

        var exception = Assert.ThrowsAsync<RentalPeriodTooLowException>(async () => await _rentService.Rent(rent));
        Assert.That(string.Equals(exception.Message, $"Rental period '4' cannot be under 7 days"), Is.True);
    }

    [Test]
    public async Task Return_InformedEndDateGreaterThanEndDate_ReturnsLatePenaltyValue()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(8);
        rent.InformedEndDate = DateTime.Today.AddDays(20);

        var response = await _rentService.Rent(rent);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(210));
            Assert.That(response?.PenaltyValue, Is.EqualTo(600));
            Assert.That(response?.TotalAmount, Is.EqualTo(810));
        });
    }

    [Test]
    public async Task Return_InformedEndDateLesserThanEndDate_ReturnsEarlyPenaltyFee()
    {
        var rent = new Mock<RentDto>().Object;
        rent.EndDate = DateTime.Today.AddDays(20);
        rent.InformedEndDate = DateTime.Today.AddDays(8);

        var response = await _rentService.Rent(rent);

        Assert.Multiple(() =>
        {
            Assert.That(response?.RentalValue, Is.EqualTo(532));
            Assert.That(response?.PenaltyValue, Is.EqualTo(470.4));
            Assert.That(response?.TotalAmount, Is.EqualTo(1002.4));
        });
    }
}