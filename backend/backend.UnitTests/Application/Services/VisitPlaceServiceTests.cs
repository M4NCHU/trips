using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class VisitPlaceServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<VisitPlaceService>> _loggerMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly Mock<IBaseUrlService> _baseUrlServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;
    private readonly VisitPlaceService _service;

    public VisitPlaceServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<VisitPlaceService>>();
        _imageServiceMock = new Mock<IImageService>();
        _baseUrlServiceMock = new Mock<IBaseUrlService>();
        _mapperMock = new Mock<IMapper>();

        _baseUrlServiceMock.Setup(b => b.GetBaseUrl()).Returns("http://example.com");

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<backend.Domain.Mappings.VisitPlaceMapper>();
        });
        _mapper = config.CreateMapper();

        _service = new VisitPlaceService(
            _unitOfWorkMock.Object,
            _imageServiceMock.Object,
            _baseUrlServiceMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetVisitPlaces_ShouldReturnAllVisitPlaces_WhenTheyExist()
    {
        // Arrange
        var visitPlaces = new List<VisitPlaceModel>
        {
            new VisitPlaceModel { Id = Guid.NewGuid(), Name = "Place 1", PhotoUrl = "photo1.jpg" },
            new VisitPlaceModel { Id = Guid.NewGuid(), Name = "Place 2", PhotoUrl = "photo2.jpg" }
        };

        var visitPlaceDTOs = new List<VisitPlaceDTO>
        {
            new VisitPlaceDTO { Id = visitPlaces[0].Id, Name = visitPlaces[0].Name, PhotoUrl = $"http://example.com/Images/VisitPlace/{visitPlaces[0].PhotoUrl}" },
            new VisitPlaceDTO { Id = visitPlaces[1].Id, Name = visitPlaces[1].Name, PhotoUrl = $"http://example.com/Images/VisitPlace/{visitPlaces[1].PhotoUrl}" }
        };

        _unitOfWorkMock.Setup(u => u.VisitPlaces.GetAllAsync()).ReturnsAsync(visitPlaces);
        _mapperMock.Setup(m => m.Map<IEnumerable<VisitPlaceDTO>>(visitPlaces)).Returns(visitPlaceDTOs);

        // Act
        var result = await _service.GetVisitPlaces();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVisitPlaces = Assert.IsAssignableFrom<IEnumerable<VisitPlaceDTO>>(okResult.Value);
        Assert.Equal(2, returnedVisitPlaces.Count());
    }

    [Fact]
    public async Task GetVisitPlace_ShouldReturnVisitPlace_WhenItExists()
    {
        // Arrange
        var visitPlaceId = Guid.NewGuid();
        var visitPlace = new VisitPlaceModel { Id = visitPlaceId, Name = "Test Place", PhotoUrl = "photo.jpg" };

        var visitPlaceDTO = new VisitPlaceDTO
        {
            Id = visitPlaceId,
            Name = "Test Place",
            PhotoUrl = $"http://example.com/Images/VisitPlace/{visitPlace.PhotoUrl}"
        };

        _unitOfWorkMock.Setup(u => u.VisitPlaces.GetByIdAsync(visitPlaceId)).ReturnsAsync(visitPlace);
        _mapperMock.Setup(m => m.Map<VisitPlaceDTO>(visitPlace)).Returns(visitPlaceDTO);

        // Act
        var result = await _service.GetVisitPlace(visitPlaceId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedVisitPlace = Assert.IsType<VisitPlaceDTO>(okResult.Value);
        Assert.Equal(visitPlaceId, returnedVisitPlace.Id);
    }

    [Fact]
    public async Task GetVisitPlace_ShouldReturnNotFound_WhenVisitPlaceDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.VisitPlaces.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((VisitPlaceModel)null);

        // Act
        var result = await _service.GetVisitPlace(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostVisitPlace_ShouldReturnCreatedVisitPlace_WhenSuccessful()
    {
        // Arrange
        var createVisitPlaceDTO = new CreateVisitPlaceDTO { Name = "New Place", ImageFile = new Mock<IFormFile>().Object };
        var visitPlaceModel = new VisitPlaceModel { Id = Guid.NewGuid(), Name = createVisitPlaceDTO.Name, PhotoUrl = "newPhoto.jpg" };
        var visitPlaceDTO = new VisitPlaceDTO { Id = visitPlaceModel.Id, Name = visitPlaceModel.Name, PhotoUrl = $"http://example.com/Images/VisitPlace/{visitPlaceModel.PhotoUrl}" };

        _mapperMock.Setup(m => m.Map<VisitPlaceModel>(createVisitPlaceDTO)).Returns(visitPlaceModel);
        _mapperMock.Setup(m => m.Map<VisitPlaceDTO>(visitPlaceModel)).Returns(visitPlaceDTO);
        _unitOfWorkMock.Setup(u => u.VisitPlaces.AddAsync(It.IsAny<VisitPlaceModel>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _service.PostVisitPlace(createVisitPlaceDTO);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result); 
        var createdVisitPlace = Assert.IsType<VisitPlaceDTO>(createdResult.Value); 
        Assert.Equal(createVisitPlaceDTO.Name, createdVisitPlace.Name);
        _unitOfWorkMock.Verify(u => u.VisitPlaces.AddAsync(It.IsAny<VisitPlaceModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }


    [Fact]
    public async Task DeleteVisitPlace_ShouldReturnNoContent_WhenVisitPlaceIsDeleted()
    {
        // Arrange
        var visitPlaceId = Guid.NewGuid();
        var visitPlace = new VisitPlaceModel { Id = visitPlaceId };

        _unitOfWorkMock.Setup(u => u.VisitPlaces.GetByIdAsync(visitPlaceId)).ReturnsAsync(visitPlace);
        _unitOfWorkMock.Setup(u => u.VisitPlaces.DeleteAsync(visitPlaceId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteVisitPlace(visitPlaceId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.VisitPlaces.DeleteAsync(visitPlaceId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteVisitPlace_ShouldReturnNotFound_WhenVisitPlaceDoesNotExist()
    {
        // Arrange
        var visitPlaceId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.VisitPlaces.GetByIdAsync(visitPlaceId))
            .ReturnsAsync((VisitPlaceModel)null); 

        // Act
        var result = await _service.DeleteVisitPlace(visitPlaceId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }


}
