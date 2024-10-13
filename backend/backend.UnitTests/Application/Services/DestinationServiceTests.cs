using AutoMapper;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

public class DestinationServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly Mock<IWebHostEnvironment> _hostEnvironmentMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IBaseUrlService> _baseUrlServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ILogger<DestinationService>> _loggerMock;
    private readonly IMapper _mapper;
    private readonly DestinationService _destinationService;

    public DestinationServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _imageServiceMock = new Mock<IImageService>();
        _hostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<DestinationService>>();

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);

        _baseUrlServiceMock = new Mock<IBaseUrlService>();
        _baseUrlServiceMock.Setup(b => b.GetBaseUrl()).Returns("http://localhost");

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<backend.Domain.Mappings.DestinationMapper>();
        });
        _mapper = config.CreateMapper();

        _destinationService = new DestinationService(
            _unitOfWorkMock.Object,
            _imageServiceMock.Object,
            _hostEnvironmentMock.Object,
            _baseUrlServiceMock.Object,
            _loggerMock.Object,
            _mapper
        );
    }

    [Fact]
    public async Task GetDestination_ShouldReturnOkObjectResult_WhenDestinationExists()
    {
        // Arrange
        var destinationId = Guid.NewGuid();
        var CreatedAt = DateTime.UtcNow;
        var ModifiedAt = DateTime.UtcNow;

        var destinationModel = new DestinationModel
        {
            Id = destinationId,
            Name = "Test Destination",
            PhotoUrl = "test.jpg",
            CategoryId = Guid.NewGuid(),
            CreatedAt = CreatedAt,
            ModifiedAt = ModifiedAt,
            Location = "Test Location",
            Description = "Description of the test destination",
            Price = 10.0,
        };

        _unitOfWorkMock.Setup(u => u.Destinations.GetByIdAsync(destinationId))
            .ReturnsAsync(destinationModel);

        // Act
        var result = await _destinationService.GetDestination(destinationId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);

        var returnedDestination = Assert.IsType<DestinationDTO>(okResult.Value);
        Assert.NotNull(returnedDestination);
        Assert.Equal(destinationModel.Id, returnedDestination.Id);
        Assert.Equal(destinationModel.Name, returnedDestination.Name);
        Assert.Equal("http://localhost/Images/Destinations/test.jpg", returnedDestination.PhotoUrl);
        Assert.Equal(destinationModel.Price, returnedDestination.Price);
        Assert.Equal(destinationModel.CategoryId, returnedDestination.CategoryId);
        Assert.Equal(destinationModel.Description, returnedDestination.Description);
        Assert.Equal(destinationModel.Location, returnedDestination.Location);
    }

    [Fact]
    public async Task GetDestination_ShouldReturnNotFound_WhenDestinationDoesNotExist()
    {
        // Arrange
        var destinationId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Destinations.GetByIdAsync(destinationId))
            .ReturnsAsync((DestinationModel)null);

        // Act
        var result = await _destinationService.GetDestination(destinationId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostDestination_ShouldReturnOkObjectResult_WhenSuccessfullyCreated()
    {
        // Arrange
        var createDestinationDTO = new CreateDestinationDTO
        {
            Name = "New Destination",
            ImageFile = new Mock<IFormFile>().Object
        };

        var destinationModel = new DestinationModel { Id = Guid.NewGuid(), Name = "New Destination" };

        _unitOfWorkMock.Setup(u => u.Destinations.AddAsync(It.IsAny<DestinationModel>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _destinationService.PostDestination(createDestinationDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _unitOfWorkMock.Verify(u => u.Destinations.AddAsync(It.IsAny<DestinationModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PutDestination_ShouldUpdateAndSaveDestination_WhenValidRequest()
    {
        // Arrange
        var destinationId = Guid.NewGuid();
        var createDestinationDTO = new CreateDestinationDTO
        {
            Name = "Updated Destination",
            ImageFile = new Mock<IFormFile>().Object
        };

        var existingDestination = new DestinationModel
        {
            Id = destinationId,
            Name = "Old Destination",
            PhotoUrl = "oldPhoto.jpg",
            CategoryId = Guid.NewGuid(),
            Location = "Old Location",
            Description = "Old Description",
            Price = 20.0
        };

        _unitOfWorkMock.Setup(u => u.Destinations.GetByIdAsync(destinationId)).ReturnsAsync(existingDestination);

        _unitOfWorkMock.Setup(u => u.Destinations.DestinationExistsAsync(destinationId)).ReturnsAsync(true);


        _imageServiceMock.Setup(i => i.SaveImage(It.IsAny<IFormFile>(), "Destinations", It.IsAny<string>()))
            .ReturnsAsync("newPhoto.jpg");

        _unitOfWorkMock.Setup(u => u.Destinations.UpdateAsync(It.IsAny<DestinationModel>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _destinationService.PutDestination(destinationId, createDestinationDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedDestination = Assert.IsType<DestinationModel>(okResult.Value);

        Assert.Equal("Updated Destination", updatedDestination.Name);
        Assert.Equal("newPhoto.jpg", updatedDestination.PhotoUrl);
        Assert.NotEqual(DateTime.MinValue, updatedDestination.ModifiedAt);

        _unitOfWorkMock.Verify(u => u.Destinations.UpdateAsync(It.IsAny<DestinationModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteDestination_ShouldReturnNoContent_WhenDestinationIsDeleted()
    {
        // Arrange
        var destinationId = Guid.NewGuid();
        var destinationModel = new DestinationModel { Id = destinationId, Name = "Test Destination" };

        _unitOfWorkMock.Setup(u => u.Destinations.GetByIdAsync(destinationId)).ReturnsAsync(destinationModel);

        _unitOfWorkMock.Setup(u => u.Destinations.DeleteAsync(destinationId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _destinationService.DeleteDestination(destinationId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.Destinations.DeleteAsync(destinationId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteDestination_ShouldReturnNotFound_WhenDestinationDoesNotExist()
    {
        // Arrange
        _unitOfWorkMock.Setup(u => u.Destinations.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((DestinationModel)null);

        // Act
        var result = await _destinationService.DeleteDestination(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
