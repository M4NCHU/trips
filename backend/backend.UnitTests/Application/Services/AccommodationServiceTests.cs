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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

public class AccommodationServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IImageService> _imageServiceMock; 
    private readonly AccommodationService _accommodationService;

    public AccommodationServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _imageServiceMock = new Mock<IImageService>();  

        // Use the mock in the service constructor
        _accommodationService = new AccommodationService(
            _unitOfWorkMock.Object,
            _imageServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task GetAccommodations_ShouldReturnAccommodations_WhenTheyExist()
    {
        // Arrange
        var accommodations = new List<AccommodationModel>
        {
            new AccommodationModel { Id = Guid.NewGuid(), Name = "Hotel 1" },
            new AccommodationModel { Id = Guid.NewGuid(), Name = "Hotel 2" }
        };

        var accommodationDTOs = new List<AccommodationDTO>
        {
            new AccommodationDTO { Id = accommodations[0].Id, Name = "Hotel 1" },
            new AccommodationDTO { Id = accommodations[1].Id, Name = "Hotel 2" }
        };

        _unitOfWorkMock.Setup(u => u.Accommodations.GetAccommodations(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(accommodations);

        _mapperMock.Setup(m => m.Map<IEnumerable<AccommodationDTO>>(accommodations))
            .Returns(accommodationDTOs);

        // Act
        var result = await _accommodationService.GetAccommodations();

        // Assert
        var okResult = Assert.IsType<ActionResult<IEnumerable<AccommodationDTO>>>(result);
        var returnedAccommodations = Assert.IsAssignableFrom<IEnumerable<AccommodationDTO>>(okResult.Value);
        Assert.Equal(2, returnedAccommodations.Count());
    }

    [Fact]
    public async Task GetAccommodation_ShouldReturnNotFound_WhenAccommodationDoesNotExist()
    {
        // Arrange
        var accommodationId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Accommodations.GetByIdAsync(accommodationId))
            .ReturnsAsync((AccommodationModel)null);

        // Act
        var result = await _accommodationService.GetAccommodation(accommodationId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostAccommodation_ShouldReturnCreatedAtAction_WhenSuccessfullyCreated()
    {
        // Arrange
        var createAccommodationDTO = new AccommodationDTO
        {
            Name = "New Hotel",
            ImageFile = new Mock<IFormFile>().Object // Mock the ImageFile
        };

        var accommodationModel = new AccommodationModel { Id = Guid.NewGuid(), Name = "New Hotel" };

        _mapperMock.Setup(m => m.Map<AccommodationModel>(createAccommodationDTO))
            .Returns(accommodationModel);

        _unitOfWorkMock.Setup(u => u.Accommodations.AddAsync(It.IsAny<AccommodationModel>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _accommodationService.PostAccommodation(createAccommodationDTO);

        // Assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetAccommodation", createdAtResult.ActionName);
        _unitOfWorkMock.Verify(u => u.Accommodations.AddAsync(It.IsAny<AccommodationModel>()), Times.Once);
    }

    [Fact]
    public async Task PutAccommodation_ShouldReturnNoContent_WhenSuccessfullyUpdated()
    {
        // Arrange
        var accommodationDTO = new AccommodationDTO { Id = Guid.NewGuid(), Name = "Updated Hotel" };

        var accommodationModel = new AccommodationModel { Id = accommodationDTO.Id, Name = "Updated Hotel" };

        _unitOfWorkMock.Setup(u => u.Accommodations.GetByIdAsync(accommodationDTO.Id))
            .ReturnsAsync(accommodationModel);

        _mapperMock.Setup(m => m.Map<AccommodationModel>(accommodationDTO))
            .Returns(accommodationModel);

        _unitOfWorkMock.Setup(u => u.Accommodations.UpdateAsync(accommodationModel))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _accommodationService.PutAccommodation(accommodationDTO.Id, accommodationDTO);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAccommodation_ShouldReturnNoContent_WhenSuccessfullyDeleted()
    {
        // Arrange
        var accommodationId = Guid.NewGuid();
        var accommodationModel = new AccommodationModel { Id = accommodationId, Name = "Test Hotel" };

        _unitOfWorkMock.Setup(u => u.Accommodations.GetByIdAsync(accommodationId))
            .ReturnsAsync(accommodationModel);

        _unitOfWorkMock.Setup(u => u.Accommodations.DeleteAsync(accommodationId))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _accommodationService.DeleteAccommodation(accommodationId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }
}
