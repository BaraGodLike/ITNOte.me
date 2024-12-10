using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;
using ITNOteAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests
{
    [TestFixture]
    public class UsersControllerTests
    {
        private Mock<IStorage> _mockStorage;
        private UsersController _controller;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<IStorage>();
            _controller = new UsersController(_mockStorage.Object);
        }

        [Test]
        public async Task RegisterUser_Success_ReturnsOkResult()
        {
            // Arrange
            var userName = "TestUser";
            var password = "TestPassword";

            _mockStorage
                .Setup(s => s.SaveUser(It.IsAny<User>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.RegisterUser(userName, password) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("User registered successfully."));
        }
        

        [Test]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "TestUser",
                Password = "WrongPassword"
            };

            var storedUser = new User
            {
                Name = "TestUser",
                Password = Storage.Hasher.HashPassword("TestPassword")
            };

            _mockStorage
                .Setup(s => s.GetUserFromStorage<User>(userDto.Name))
                .ReturnsAsync(storedUser);

            // Act
            var result = await _controller.Login(userDto) as UnauthorizedObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(401));
            Assert.That(result.Value, Is.EqualTo("Invalid credentials."));
        }

        [Test]
        public async Task GetUser_Exists_ReturnsUser()
        {
            // Arrange
            var userName = "TestUser";
            var expectedUser = new User
            {
                Name = userName,
                Password = [],
                GeneralFolder = new Folder(userName)
            };

            _mockStorage
                .Setup(s => s.HasNicknameInStorage(userName))
                .ReturnsAsync(true);

            _mockStorage
                .Setup(s => s.GetUserFromStorage<User>(userName))
                .ReturnsAsync(expectedUser);

            // Act
            var result = await _controller.GetUser(userName) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var response = result.Value as User;
            Assert.IsNotNull(response);
            Assert.That(response.Name, Is.EqualTo(expectedUser.Name));
        }

        [Test]
        public async Task GetUser_DoesNotExist_ReturnsUnauthorized()
        {
            // Arrange
            var userName = "NonExistentUser";

            _mockStorage
                .Setup(s => s.HasNicknameInStorage(userName))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.GetUser(userName) as UnauthorizedResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(401));
        }
    }
}
