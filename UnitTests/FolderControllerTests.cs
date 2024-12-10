using System.Collections.ObjectModel;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOteAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests
{
    [TestFixture]
    public class FolderControllerTests
    {
        private Mock<IStorage> _mockStorage;
        private FolderController _controller;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<IStorage>();
            _controller = new FolderController(_mockStorage.Object);
        }

        [Test]
        public async Task GetAllChildren_ReturnsOkResult_WithExpectedChildren()
        {
            // Arrange
            int folderId = 1;
            var expectedChildren = new ObservableCollection<AbstractSource>
            {
                new Note { Id = 1, Name = "Child1" },
                new Note { Id = 2, Name = "Child2" },
                new Note { Id = 3, Name = "Child3" }
            };

            _mockStorage
                .Setup(s => s.GetAllChildren(folderId))
                .ReturnsAsync(expectedChildren);

            // Act
            var result = await _controller.GetAllChildren(folderId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var response = result.Value as ObservableCollection<AbstractSource>;
            Assert.IsNotNull(response);
            Assert.That(response.Count, Is.EqualTo(expectedChildren.Count));
            CollectionAssert.AreEqual(expectedChildren, response);
        }

        [Test]
        public async Task GetAllChildren_EmptyChildren_ReturnsOkResult_WithEmptyCollection()
        {
            // Arrange
            int folderId = 2;
            var expectedChildren = new ObservableCollection<AbstractSource>();

            _mockStorage
                .Setup(s => s.GetAllChildren(folderId))
                .ReturnsAsync(expectedChildren);

            // Act
            var result = await _controller.GetAllChildren(folderId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var response = result.Value as ObservableCollection<AbstractSource>;
            Assert.IsNotNull(response);
            Assert.That(response.Count, Is.EqualTo(0));
        }

        [Test]
        public async Task GetAllChildren_NonExistentFolder_ReturnsOkResult_WithNull()
        {
            // Arrange
            int folderId = 999;
            ObservableCollection<AbstractSource>? expectedChildren = null;

            _mockStorage
                .Setup(s => s.GetAllChildren(folderId))!
                .ReturnsAsync(expectedChildren);

            // Act
            var result = await _controller.GetAllChildren(folderId) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.IsNull(result.Value);
        }
    }
    
}
