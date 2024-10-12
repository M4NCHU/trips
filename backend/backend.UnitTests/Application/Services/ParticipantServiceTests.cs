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

public class ParticipantServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly Mock<IBaseUrlService> _baseUrlServiceMock;
    private readonly Mock<ILogger<ParticipantService>> _loggerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;
    private readonly ParticipantService _participantService;

    public ParticipantServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _imageServiceMock = new Mock<IImageService>();
        _baseUrlServiceMock = new Mock<IBaseUrlService>();
        _loggerMock = new Mock<ILogger<ParticipantService>>();
        _mapperMock = new Mock<IMapper>();

        _baseUrlServiceMock.Setup(b => b.GetBaseUrl()).Returns("http://example.com");

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<backend.Domain.Mappings.ParticipantMapper>();
        });
        _mapper = config.CreateMapper();

        _participantService = new ParticipantService(
            _unitOfWorkMock.Object,
            _imageServiceMock.Object,
            _baseUrlServiceMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetParticipants_ShouldReturnParticipants_WhenParticipantsExist()
    {
        // Arrange
        var participants = new List<ParticipantModel>
        {
            new ParticipantModel { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" },
            new ParticipantModel { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith" }
        };

        var participantDTOs = new List<ParticipantDTO>
        {
            new ParticipantDTO { Id = participants[0].Id, FirstName = "John", LastName = "Doe" },
            new ParticipantDTO { Id = participants[1].Id, FirstName = "Jane", LastName = "Smith" }
        };

        _unitOfWorkMock.Setup(u => u.Participants.GetAllAsync()).ReturnsAsync(participants);
        _mapperMock.Setup(m => m.Map<IEnumerable<ParticipantDTO>>(participants)).Returns(participantDTOs);

        // Act
        var result = await _participantService.GetParticipants();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedParticipants = Assert.IsAssignableFrom<IEnumerable<ParticipantDTO>>(okResult.Value);
        Assert.Equal(2, returnedParticipants.Count());
    }

    [Fact]
    public async Task GetParticipant_ShouldReturnParticipant_WhenParticipantExists()
    {
        // Arrange
        var participantId = Guid.NewGuid();
        var participant = new ParticipantModel
        {
            Id = participantId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        var participantDTO = new ParticipantDTO
        {
            Id = participantId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com"
        };

        _unitOfWorkMock.Setup(u => u.Participants.GetByIdAsync(participantId)).ReturnsAsync(participant);
        _mapperMock.Setup(m => m.Map<ParticipantDTO>(participant)).Returns(participantDTO);

        // Act
        var result = await _participantService.GetParticipant(participantId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedParticipant = Assert.IsType<ParticipantDTO>(okResult.Value);
        Assert.Equal(participantId, returnedParticipant.Id);
        Assert.Equal("John", returnedParticipant.FirstName);
        Assert.Equal("Doe", returnedParticipant.LastName);
    }

    [Fact]
    public async Task GetParticipant_ShouldReturnNotFound_WhenParticipantDoesNotExist()
    {
        // Arrange
        var participantId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Participants.GetByIdAsync(participantId)).ReturnsAsync((ParticipantModel)null);

        // Act
        var result = await _participantService.GetParticipant(participantId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostParticipant_ShouldCreateParticipant_WhenValidRequest()
    {
        // Arrange
        var createParticipantDTO = new CreateParticipantDTO
        {
            FirstName = "John",
            LastName = "Doe",
            ImageFile = new Mock<IFormFile>().Object // Simulating an image file
        };

        var participantModel = new ParticipantModel { Id = Guid.NewGuid(), FirstName = createParticipantDTO.FirstName, LastName = createParticipantDTO.LastName };

        _mapperMock.Setup(m => m.Map<ParticipantModel>(createParticipantDTO)).Returns(participantModel);
        _unitOfWorkMock.Setup(u => u.Participants.AddAsync(It.IsAny<ParticipantModel>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _participantService.PostParticipant(createParticipantDTO);

        // Assert
        Assert.Equal(createParticipantDTO.FirstName, result.FirstName);
        Assert.Equal(createParticipantDTO.LastName, result.LastName);
        _unitOfWorkMock.Verify(u => u.Participants.AddAsync(It.IsAny<ParticipantModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task PutParticipant_ShouldReturnNoContent_WhenParticipantIsUpdated()
    {
        // Arrange
        var participantId = Guid.NewGuid();
        var updateParticipantDTO = new ParticipantDTO
        {
            Id = participantId,
            FirstName = "John",
            LastName = "Doe"
        };

        _unitOfWorkMock.Setup(u => u.Participants.GetByIdAsync(participantId)).ReturnsAsync(new ParticipantModel { Id = participantId });

        // Act
        var result = await _participantService.PutParticipant(participantId, updateParticipantDTO);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.Participants.UpdateAsync(It.IsAny<ParticipantModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteParticipant_ShouldReturnNoContent_WhenParticipantIsDeleted()
    {
        // Arrange
        var participantId = Guid.NewGuid();
        var participant = new ParticipantModel { Id = participantId, FirstName = "John", LastName = "Doe" };

        _unitOfWorkMock.Setup(u => u.Participants.GetByIdAsync(participantId)).ReturnsAsync(participant);
        _unitOfWorkMock.Setup(u => u.Participants.DeleteAsync(participantId)).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync()).ReturnsAsync(true);

        // Act
        var result = await _participantService.DeleteParticipant(participantId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.Participants.DeleteAsync(participantId), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
