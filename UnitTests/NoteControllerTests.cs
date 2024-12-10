using ITNOteAPI.Controllers;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace UnitTests;
    [TestFixture]
    public class NotesControllerTests
    {
        private Mock<IStorage> _mockStorage;
        private NotesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockStorage = new Mock<IStorage>();
            _controller = new NotesController(_mockStorage.Object);
        }

        [Test]
        public async Task GetNoteContent_ReturnsOkResult_WithExpectedContent()
        {
            // Arrange
            int noteId = 1;
            string noteName = "TestNote";
            string expectedContent = "This is a test note.";
            _mockStorage
                .Setup(s => s.ReadNote(noteId, noteName))
                .ReturnsAsync(expectedContent);

            // Act
            var result = await _controller.GetNoteContent(noteId, noteName) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            var response = result.Value;
            Assert.IsNotNull(response);

            // Use reflection to check properties of an anonymous object
            var idProperty = response.GetType().GetProperty("Id");
            var nameProperty = response.GetType().GetProperty("Name");
            var contentProperty = response.GetType().GetProperty("Content");

            Assert.IsNotNull(idProperty);
            Assert.IsNotNull(nameProperty);
            Assert.IsNotNull(contentProperty);

            Assert.That(idProperty.GetValue(response), Is.EqualTo(noteId));
            Assert.That(nameProperty.GetValue(response), Is.EqualTo(noteName));
            Assert.That(contentProperty.GetValue(response), Is.EqualTo(expectedContent));
        }


        [Test]
        public async Task CreateNote_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var noteDto = new NoteDto
            {
                Name = "NewNote",
                Content = "Note content",
                ParentFolderName = "ParentFolder",
                ParentFolderId = 10
            };

            var createdNoteId = 1;

            _mockStorage
                .Setup(s => s.CreateNewSource(It.IsAny<Note>()))
                .Callback<AbstractSource>(note =>
                {
                    var concreteNote = note as Note;
                    Assert.IsNotNull(concreteNote, "Callback received a null or incorrect type.");
                    concreteNote.Id = createdNoteId; // Simulate setting the ID
                });

            // Act
            var result = await _controller.CreateNote(noteDto) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(201));

            var response = result.Value as Note;
            Assert.That(response?.Id, Is.EqualTo(createdNoteId));
            Assert.That(response.Name, Is.EqualTo(noteDto.Name));
            Assert.That(response.Content, Is.EqualTo(noteDto.Content));
        }

        [Test]
        public async Task UpdateNoteContent_ReturnsNoContent()
        {
            // Arrange
            int noteId = 1;
            string noteName = "TestNote";
            string newContent = "Updated content";

            _mockStorage
                .Setup(s => s.WriteInNote(noteId, noteName, newContent))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateNoteContent(noteId, noteName, newContent);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteNoteContent_ReturnsNoContent()
        {
            // Arrange
            int noteId = 1;

            _mockStorage
                .Setup(s => s.DeleteNote(noteId))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteNoteContent(noteId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
