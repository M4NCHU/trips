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

public class TripParticipantServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseUrlService> _baseUrlServiceMock;
    private readonly Mock<ILogger<TripParticipantService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;
    private readonly TripParticipantService _service;

    public TripParticipantServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _baseUrlServiceMock = new Mock<IBaseUrlService>();
        _loggerMock = new Mock<ILogger<TripParticipantService>>();
        _mapperMock = new Mock<IMapper>();

        _baseUrlServiceMock.Setup(b => b.GetBaseUrl()).Returns("http://example.com");

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<backend.Domain.Mappings.TripParticipantMapper>();
        });
        _mapper = config.CreateMapper();

        _service = new TripParticipantService(
            _unitOfWorkMock.Object,
            _baseUrlServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetTripParticipants_ShouldReturnAllTripParticipants_WhenTheyExist()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var participants = new List<TripParticipantModel>
        {
            new TripParticipantModel { Id = Guid.NewGuid(), TripId = tripId, ParticipantId = Guid.NewGuid() },
            new TripParticipantModel { Id = Guid.NewGuid(), TripId = tripId, ParticipantId = Guid.NewGuid() }
        };

        var participantDTOs = new List<TripParticipantDTO>
        {
            new TripParticipantDTO { Id = participants[0].Id, TripId = participants[0].TripId, ParticipantId = participants[0].ParticipantId },
            new TripParticipantDTO { Id = participants[1].Id, TripId = participants[1].TripId, ParticipantId = participants[1].ParticipantId }
        };

        _unitOfWorkMock.Setup(u => u.TripParticipants.GetTripParticipantsByTripIdAsync(tripId)).ReturnsAsync(participants);
        _mapperMock.Setup(m => m.Map<IEnumerable<TripParticipantDTO>>(participants)).Returns(participantDTOs);

        // Act
        var result = await _service.GetTripParticipants(tripId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedParticipants = Assert.IsAssignableFrom<IEnumerable<TripParticipantDTO>>(okResult.Value);
        Assert.Equal(2, returnedParticipants.Count());
    }

    [Fact]
    public async Task GetTripParticipants_ShouldReturnNotFound_WhenNoParticipantsExist()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.TripParticipants.GetTripParticipantsByTripIdAsync(tripId))
            .ReturnsAsync(new List<TripParticipantModel>());

        // Act
        var result = await _service.GetTripParticipants(tripId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateTripParticipant_ShouldReturnCreatedResult_WhenSuccessful()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        var participantId = Guid.NewGuid();
        var trip = new TripModel { Id = tripId, Title = "Test Trip" };
        var participant = new ParticipantModel { Id = participantId, FirstName = "John" };

        var tripParticipantModel = new TripParticipantModel
        {
            Id = Guid.NewGuid(),
            TripId = tripId,
            ParticipantId = participantId
        };

        var tripParticipantDTO = new TripParticipantDTO
        {
            Id = tripParticipantModel.Id,
            TripId = tripId,
            ParticipantId = participantId
        };

        _unitOfWorkMock.Setup(u => u.Trips.GetByIdAsync(tripId)).ReturnsAsync(trip);
        _unitOfWorkMock.Setup(u => u.Participants.GetByIdAsync(participantId)).ReturnsAsync(participant);
        _unitOfWorkMock.Setup(u => u.TripParticipants.AddAsync(It.IsAny<TripParticipantModel>())).Returns(Task.CompletedTask);
        _mapperMock.Setup(m => m.Map<TripParticipantDTO>(It.IsAny<TripParticipantModel>())).Returns(tripParticipantDTO);

        // Act
        var result = await _service.CreateTripParticipant(tripId, participantId);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdParticipant = Assert.IsType<TripParticipantDTO>(createdAtActionResult.Value);
        Assert.Equal(tripId, createdParticipant.TripId);
        Assert.Equal(participantId, createdParticipant.ParticipantId);
    }

    [Fact]
    public async Task CreateTripParticipant_ShouldReturnNotFound_WhenTripDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Trips.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TripModel)null);

        // Act
        var result = await _service.CreateTripParticipant(Guid.NewGuid(), Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task CreateTripParticipant_ShouldReturnNotFound_WhenParticipantDoesNotExist()
    {
        // Arrange
        var tripId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Trips.GetByIdAsync(tripId)).ReturnsAsync(new TripModel { Id = tripId });
        _unitOfWorkMock.Setup(u => u.Participants.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((ParticipantModel)null);

        // Act
        var result = await _service.CreateTripParticipant(tripId, Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundObjectResult>(result.Result);
    }

    [Fact]
    public async Task DeleteTripParticipant_ShouldReturnNoContent_WhenSuccessfullyDeleted()
    {
        // Arrange
        var tripParticipantId = Guid.NewGuid();
        var tripParticipant = new TripParticipantModel { Id = tripParticipantId, TripId = Guid.NewGuid(), ParticipantId = Guid.NewGuid() };

        _unitOfWorkMock.Setup(u => u.TripParticipants.GetByIdAsync(tripParticipantId)).ReturnsAsync(tripParticipant);
        _unitOfWorkMock.Setup(u => u.TripParticipants.DeleteAsync(tripParticipantId)).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteTripParticipant(tripParticipantId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.TripParticipants.DeleteAsync(tripParticipantId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteTripParticipant_ShouldReturnNotFound_WhenTripParticipantDoesNotExist()
    {
        // Arrange
        var tripParticipantId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.TripParticipants.GetByIdAsync(tripParticipantId))
            .ReturnsAsync((TripParticipantModel)null); 

        // Act
        var result = await _service.DeleteTripParticipant(tripParticipantId);

        // Assert
        Assert.IsType<NotFoundResult>(result);  
    }


}
