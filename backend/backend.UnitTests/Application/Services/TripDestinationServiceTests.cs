using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class TripDestinationServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<TripDestinationService>> _loggerMock;
    private readonly TripDestinationService _tripDestinationService;

    public TripDestinationServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<TripDestinationService>>();

        _tripDestinationService = new TripDestinationService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetTripDestinations_ShouldReturnTripDestinations_WhenTripDestinationsExist()
    {
        // Arrange
        var tripDestinations = new List<TripDestinationModel>
        {
            new TripDestinationModel { Id = Guid.NewGuid(), TripId = Guid.NewGuid() },
            new TripDestinationModel { Id = Guid.NewGuid(), TripId = Guid.NewGuid() }
        };

        var tripDestinationDTOs = new List<TripDestinationDTO>
        {
            new TripDestinationDTO { Id = tripDestinations[0].Id, TripId = tripDestinations[0].TripId },
            new TripDestinationDTO { Id = tripDestinations[1].Id, TripId = tripDestinations[1].TripId }
        };

        _unitOfWorkMock.Setup(u => u.TripDestinations.GetAllAsync()).ReturnsAsync(tripDestinations);
        _mapperMock.Setup(m => m.Map<IEnumerable<TripDestinationDTO>>(tripDestinations)).Returns(tripDestinationDTOs);

        // Act
        var result = await _tripDestinationService.GetTripDestinations();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTripDestinations = Assert.IsAssignableFrom<IEnumerable<TripDestinationDTO>>(okResult.Value);
        Assert.Equal(2, returnedTripDestinations.Count());
    }

    [Fact]
    public async Task GetTripDestination_ShouldReturnTripDestination_WhenTripDestinationExists()
    {
        // Arrange
        var tripDestinationId = Guid.NewGuid();
        var tripDestination = new TripDestinationModel { Id = tripDestinationId };
        var tripDestinationDTO = new TripDestinationDTO { Id = tripDestinationId };

        _unitOfWorkMock.Setup(u => u.TripDestinations.GetByIdAsync(tripDestinationId)).ReturnsAsync(tripDestination);
        _mapperMock.Setup(m => m.Map<TripDestinationDTO>(tripDestination)).Returns(tripDestinationDTO);

        // Act
        var result = await _tripDestinationService.GetTripDestination(tripDestinationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTripDestination = Assert.IsType<TripDestinationDTO>(okResult.Value);
        Assert.Equal(tripDestinationId, returnedTripDestination.Id);
    }

    [Fact]
    public async Task GetTripDestination_ShouldReturnNotFound_WhenTripDestinationDoesNotExist()
    {
        // Arrange
        var tripDestinationId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.TripDestinations.GetByIdAsync(tripDestinationId)).ReturnsAsync((TripDestinationModel)null);

        // Act
        var result = await _tripDestinationService.GetTripDestination(tripDestinationId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostTripDestinationAsync_ShouldAddTripDestination_WhenValidRequest()
    {
        // Arrange
        var tripDestinationDTO = new TripDestinationDTO { TripId = Guid.NewGuid() };
        var tripDestinationModel = new TripDestinationModel { TripId = tripDestinationDTO.TripId };

        _mapperMock.Setup(m => m.Map<TripDestinationModel>(tripDestinationDTO)).Returns(tripDestinationModel);
        _unitOfWorkMock.Setup(u => u.TripDestinations.AddAsync(It.IsAny<TripDestinationModel>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _tripDestinationService.PostTripDestinationAsync(tripDestinationDTO);

        // Assert
        Assert.Equal(tripDestinationDTO.TripId, result.TripId);
        _unitOfWorkMock.Verify(u => u.TripDestinations.AddAsync(It.IsAny<TripDestinationModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PutTripDestination_ShouldUpdateTripDestination_WhenValidRequest()
    {
        // Arrange
        var tripDestinationId = Guid.NewGuid();
        var tripDestinationDTO = new TripDestinationDTO { Id = tripDestinationId, TripId = Guid.NewGuid() };
        var tripDestinationModel = new TripDestinationModel { Id = tripDestinationId, TripId = tripDestinationDTO.TripId };

        _unitOfWorkMock.Setup(u => u.TripDestinations.GetByIdAsync(tripDestinationId)).ReturnsAsync(tripDestinationModel);

        _mapperMock.Setup(m => m.Map(tripDestinationDTO, tripDestinationModel));

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _tripDestinationService.PutTripDestination(tripDestinationId, tripDestinationDTO);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.TripDestinations.UpdateAsync(tripDestinationModel), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }


    [Fact]
    public async Task DeleteTripDestination_ShouldReturnNoContent_WhenTripDestinationIsDeleted()
    {
        // Arrange
        var tripDestinationId = Guid.NewGuid();
        var tripDestinationModel = new TripDestinationModel { Id = tripDestinationId };

        _unitOfWorkMock.Setup(u => u.TripDestinations.GetByIdAsync(tripDestinationId)).ReturnsAsync(tripDestinationModel);
        _unitOfWorkMock.Setup(u => u.TripDestinations.DeleteAsync(tripDestinationId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _tripDestinationService.DeleteTripDestination(tripDestinationId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.TripDestinations.DeleteAsync(tripDestinationId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task CountTripDestinations_ShouldReturnCorrectCount_WhenTripDestinationsExist()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.TripDestinations.CountTripDestinationsByTripIdAsync(tripId)).ReturnsAsync(5);

        // Act
        var count = await _tripDestinationService.CountTripDestinations(tripId);

        // Assert
        Assert.Equal(5, count);
    }
}
