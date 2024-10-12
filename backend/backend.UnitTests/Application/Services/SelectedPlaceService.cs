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

public class SelectedPlaceServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IBaseUrlService> _baseUrlServiceMock;
    private readonly Mock<ILogger<SelectedPlaceService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;
    private readonly SelectedPlaceService _selectedPlaceService;

    public SelectedPlaceServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _baseUrlServiceMock = new Mock<IBaseUrlService>();
        _loggerMock = new Mock<ILogger<SelectedPlaceService>>();
        _mapperMock = new Mock<IMapper>();

        _baseUrlServiceMock.Setup(b => b.GetBaseUrl()).Returns("http://example.com");

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<backend.Domain.Mappings.SelectedPlacesMapper>();
        });
        _mapper = config.CreateMapper();

        _selectedPlaceService = new SelectedPlaceService(
            _unitOfWorkMock.Object,
            _baseUrlServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetSelectedPlaces_ShouldReturnSelectedPlaces_WhenSelectedPlacesExist()
    {
        // Arrange
        var selectedPlaces = new List<SelectedPlaceModel>
        {
            new SelectedPlaceModel { Id = Guid.NewGuid(), TripDestinationId = Guid.NewGuid(), VisitPlaceId = Guid.NewGuid() },
            new SelectedPlaceModel { Id = Guid.NewGuid(), TripDestinationId = Guid.NewGuid(), VisitPlaceId = Guid.NewGuid() }
        };

        var selectedPlaceDTOs = new List<SelectedPlaceDTO>
        {
            new SelectedPlaceDTO { Id = selectedPlaces[0].Id, TripDestinationId = selectedPlaces[0].TripDestinationId, VisitPlaceId = selectedPlaces[0].VisitPlaceId },
            new SelectedPlaceDTO { Id = selectedPlaces[1].Id, TripDestinationId = selectedPlaces[1].TripDestinationId, VisitPlaceId = selectedPlaces[1].VisitPlaceId }
        };

        _unitOfWorkMock.Setup(u => u.SelectedPlaces.GetAllAsync()).ReturnsAsync(selectedPlaces);
        _mapperMock.Setup(m => m.Map<IEnumerable<SelectedPlaceDTO>>(selectedPlaces)).Returns(selectedPlaceDTOs);

        // Act
        var result = await _selectedPlaceService.GetSelectedPlaces();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedSelectedPlaces = Assert.IsAssignableFrom<IEnumerable<SelectedPlaceDTO>>(okResult.Value);
        Assert.Equal(2, returnedSelectedPlaces.Count());
    }

    [Fact]
    public async Task GetSelectedPlace_ShouldReturnSelectedPlace_WhenSelectedPlaceExists()
    {
        // Arrange
        var selectedPlaceId = Guid.NewGuid();
        var selectedPlace = new SelectedPlaceModel
        {
            Id = selectedPlaceId,
            TripDestinationId = Guid.NewGuid(),
            VisitPlaceId = Guid.NewGuid()
        };

        var selectedPlaceDTO = new SelectedPlaceDTO
        {
            Id = selectedPlaceId,
            TripDestinationId = selectedPlace.TripDestinationId,
            VisitPlaceId = selectedPlace.VisitPlaceId
        };

        _unitOfWorkMock.Setup(u => u.SelectedPlaces.GetByIdAsync(selectedPlaceId)).ReturnsAsync(selectedPlace);
        _mapperMock.Setup(m => m.Map<SelectedPlaceDTO>(selectedPlace)).Returns(selectedPlaceDTO);

        // Act
        var result = await _selectedPlaceService.GetSelectedPlace(selectedPlaceId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedSelectedPlace = Assert.IsType<SelectedPlaceDTO>(okResult.Value);
        Assert.Equal(selectedPlaceId, returnedSelectedPlace.Id);
    }

    [Fact]
    public async Task GetSelectedPlace_ShouldReturnNotFound_WhenSelectedPlaceDoesNotExist()
    {
        // Arrange
        var selectedPlaceId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.SelectedPlaces.GetByIdAsync(selectedPlaceId)).ReturnsAsync((SelectedPlaceModel)null);

        // Act
        var result = await _selectedPlaceService.GetSelectedPlace(selectedPlaceId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostSelectedPlace_ShouldAddSelectedPlace_WhenValidRequest()
    {
        // Arrange
        var createSelectedPlaceDTO = new CreateSelectedPlaceDTO
        {
            TripDestinationId = Guid.NewGuid(),
            VisitPlaceId = Guid.NewGuid()
        };

        var selectedPlaceModel = new SelectedPlaceModel
        {
            Id = Guid.NewGuid(),
            TripDestinationId = createSelectedPlaceDTO.TripDestinationId,
            VisitPlaceId = createSelectedPlaceDTO.VisitPlaceId
        };

        _mapperMock.Setup(m => m.Map<SelectedPlaceModel>(createSelectedPlaceDTO)).Returns(selectedPlaceModel);
        _unitOfWorkMock.Setup(u => u.SelectedPlaces.AddAsync(It.IsAny<SelectedPlaceModel>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _selectedPlaceService.PostSelectedPlace(createSelectedPlaceDTO);

        // Assert
        Assert.Equal(createSelectedPlaceDTO.TripDestinationId, result.TripDestinationId);
        Assert.Equal(createSelectedPlaceDTO.VisitPlaceId, result.VisitPlaceId);
        _unitOfWorkMock.Verify(u => u.SelectedPlaces.AddAsync(It.IsAny<SelectedPlaceModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PutSelectedPlace_ShouldUpdateSelectedPlace_WhenValidRequest()
    {
        // Arrange
        var selectedPlaceId = Guid.NewGuid();
        var updateSelectedPlaceDTO = new SelectedPlaceDTO
        {
            Id = selectedPlaceId,
            TripDestinationId = Guid.NewGuid(),
            VisitPlaceId = Guid.NewGuid()
        };

        var selectedPlaceModel = new SelectedPlaceModel { Id = selectedPlaceId };

        _unitOfWorkMock.Setup(u => u.SelectedPlaces.GetByIdAsync(selectedPlaceId)).ReturnsAsync(selectedPlaceModel);
        _mapperMock.Setup(m => m.Map<SelectedPlaceModel>(updateSelectedPlaceDTO)).Returns(selectedPlaceModel);

        // Act
        var result = await _selectedPlaceService.PutSelectedPlace(selectedPlaceId, updateSelectedPlaceDTO);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.SelectedPlaces.UpdateAsync(It.IsAny<SelectedPlaceModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteSelectedPlace_ShouldReturnNoContent_WhenSelectedPlaceIsDeleted()
    {
        // Arrange
        var selectedPlaceId = Guid.NewGuid();
        var selectedPlace = new SelectedPlaceModel { Id = selectedPlaceId };

        _unitOfWorkMock.Setup(u => u.SelectedPlaces.GetByIdAsync(selectedPlaceId)).ReturnsAsync(selectedPlace);
        _unitOfWorkMock.Setup(u => u.SelectedPlaces.DeleteAsync(selectedPlaceId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _selectedPlaceService.DeleteSelectedPlace(selectedPlaceId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.SelectedPlaces.DeleteAsync(selectedPlaceId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
