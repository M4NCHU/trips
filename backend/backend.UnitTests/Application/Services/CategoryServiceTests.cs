using Xunit;
using Moq;
using backend.Application.Services;
using backend.Domain.DTOs;
using backend.Infrastructure.Respository;
using backend.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using AutoMapper;
using Microsoft.AspNetCore.Http;

public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IImageService> _imageServiceMock;
    private readonly Mock<IWebHostEnvironment> _hostEnvironmentMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly Mock<IBaseUrlService> _baseUrlServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IMapper _mapper;
    private readonly Mock<ILogger<CategoryService>> _loggerMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _imageServiceMock = new Mock<IImageService>();
        _hostEnvironmentMock = new Mock<IWebHostEnvironment>();
        _mapperMock = new Mock<IMapper>();
        _loggerMock = new Mock<ILogger<CategoryService>>();

        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        var httpContext = new DefaultHttpContext();
        _httpContextAccessorMock.Setup(h => h.HttpContext).Returns(httpContext);

        _baseUrlServiceMock = new Mock<IBaseUrlService>();
        _baseUrlServiceMock.Setup(b => b.GetBaseUrl()).Returns("http://localhost");

        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<backend.Domain.Mappings.CategoryMapper>();
        });
        _mapper = config.CreateMapper();

        _categoryService = new CategoryService(
            _unitOfWorkMock.Object,
            _mapper,
            _imageServiceMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task GetCategories_ShouldReturnCategories_WhenCategoriesExist()
    {
        // Arrange
        var categories = new List<CategoryModel>
    {
        new CategoryModel { Id = Guid.NewGuid(), Name = "Category1" },
        new CategoryModel { Id = Guid.NewGuid(), Name = "Category2" }
    };

        _unitOfWorkMock.Setup(u => u.Categories.GetAllAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetCategories(2, 10);

        // Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<CategoryDTO>>>(result);  
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);              
        var returnedCategories = Assert.IsAssignableFrom<IEnumerable<CategoryDTO>>(okResult.Value);
        Assert.Equal(2, returnedCategories.Count());
    }

    [Fact]
    public async Task GetCategory_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId))
            .ReturnsAsync((CategoryModel)null);

        // Act
        var result = await _categoryService.GetCategory(categoryId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostCategory_ShouldReturnOkObjectResult_WhenSuccessfullyCreated()
    {
        // Arrange
        var categoryDTO = new CreateCategoryRequestDTO
        {
            Name = "NewCategory",
            Description = "NewDescription",
            PhotoUrl = "newPhotoUrl.jpg"
        };

        var categoryModel = new CategoryModel { Id = Guid.NewGuid(), Name = categoryDTO.Name };

        _unitOfWorkMock.Setup(u => u.Categories.AddAsync(It.IsAny<CategoryModel>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _categoryService.PostCategory(categoryDTO);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _unitOfWorkMock.Verify(u => u.Categories.AddAsync(It.IsAny<CategoryModel>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnNoContent_WhenSuccessfullyDeleted()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var category = new CategoryModel { Id = categoryId, Name = "Test Category" };

        _unitOfWorkMock.Setup(u => u.Categories.GetByIdAsync(categoryId))
            .ReturnsAsync(category);

        _unitOfWorkMock.Setup(u => u.Categories.DeleteAsync(categoryId))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(true);

        // Act
        var result = await _categoryService.DeleteCategory(categoryId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _unitOfWorkMock.Verify(u => u.Categories.DeleteAsync(categoryId), Times.Once);
    }

}
