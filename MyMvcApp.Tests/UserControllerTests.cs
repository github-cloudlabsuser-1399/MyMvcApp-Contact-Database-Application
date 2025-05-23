using Xunit;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Controllers;
using MyMvcApp.Models;
using System.Collections.Generic;

namespace MyMvcApp.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WithUserList()
        {
            // Arrange
            UserController.userlist = new List<User> { new User { Id = 1, Name = "Test", Email = "test@email.com" } };
            var controller = new UserController();

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<User>>(viewResult.Model);
            Assert.Single(model);
        }

        [Fact]
        public void Details_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            UserController.userlist = new List<User>();
            var controller = new UserController();

            // Act
            var result = controller.Details(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Details_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test", Email = "test@email.com" };
            UserController.userlist = new List<User> { user };
            var controller = new UserController();

            // Act
            var result = controller.Details(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(user, viewResult.Model);
        }

        [Fact]
        public void Create_Post_ValidUser_RedirectsToIndex()
        {
            // Arrange
            UserController.userlist = new List<User>();
            var controller = new UserController();
            var user = new User { Id = 1, Name = "Test", Email = "test@email.com" };
            controller.ModelState.Clear();

            // Act
            var result = controller.Create(user);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Single(UserController.userlist);
        }

        [Fact]
        public void Create_Post_InvalidUser_ReturnsViewWithModel()
        {
            // Arrange
            var controller = new UserController();
            controller.ModelState.AddModelError("Name", "Required");
            var user = new User();

            // Act
            var result = controller.Create(user);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(user, viewResult.Model);
        }

        [Fact]
        public void Edit_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            UserController.userlist = new List<User>();
            var controller = new UserController();

            // Act
            var result = controller.Edit(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_Get_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test", Email = "test@email.com" };
            UserController.userlist = new List<User> { user };
            var controller = new UserController();

            // Act
            var result = controller.Edit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(user, viewResult.Model);
        }

        [Fact]
        public void Edit_Post_ValidUser_UpdatesUserAndRedirects()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Old", Email = "old@email.com" };
            UserController.userlist = new List<User> { user };
            var controller = new UserController();
            var updatedUser = new User { Id = 1, Name = "New", Email = "new@email.com" };
            controller.ModelState.Clear();

            // Act
            var result = controller.Edit(1, updatedUser);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Equal("New", user.Name);
            Assert.Equal("new@email.com", user.Email);
        }

        [Fact]
        public void Edit_Post_InvalidUser_ReturnsViewWithModel()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Old", Email = "old@email.com" };
            UserController.userlist = new List<User> { user };
            var controller = new UserController();
            controller.ModelState.AddModelError("Name", "Required");
            var updatedUser = new User { Id = 1, Name = "New", Email = "new@email.com" };

            // Act
            var result = controller.Edit(1, updatedUser);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(updatedUser, viewResult.Model);
        }

        [Fact]
        public void Delete_Get_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            UserController.userlist = new List<User>();
            var controller = new UserController();

            // Act
            var result = controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Delete_Get_ReturnsViewResult_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test", Email = "test@email.com" };
            UserController.userlist = new List<User> { user };
            var controller = new UserController();

            // Act
            var result = controller.Delete(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(user, viewResult.Model);
        }

        [Fact]
        public void Delete_Post_RemovesUserAndRedirects()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Test", Email = "test@email.com" };
            UserController.userlist = new List<User> { user };
            var controller = new UserController();

            // Act
            var result = controller.Delete(1, null);

            // Assert
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
            Assert.Empty(UserController.userlist);
        }
    }
}
