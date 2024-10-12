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

public class CategoryServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CategoryService _categoryService;

    public CategoryServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var loggerMock = new Mock<ILogger<CategoryService>>();

        _categoryService = new CategoryService(_unitOfWorkMock.Object, loggerMock.Object);
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
        var result = await _categoryService.GetCategories();

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
    public async Task PostCategory_ShouldReturnCreatedAtAction_WhenSuccessfullyCreated()
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
        Assert.Equal(categoryDTO.Name, result.Name);
        _unitOfWorkMock.Verify(u => u.Categories.AddAsync(It.IsAny<CategoryModel>()), Times.Once);
    }

    [Fact]
    public async Task DeleteCategory_ShouldReturnNoContent_WhenSuccessfullyDeleted()
    {
        // Arrange
        var categoryId = Guid.NewGuid();

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
