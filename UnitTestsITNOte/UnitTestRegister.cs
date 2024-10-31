using Moq;
using NUnit.Framework;
using ITNOte.me.ModelView;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;

namespace UnitTestsITNOte
{
    [TestFixture]
    public class RegisterModelViewTests
    {
        private Mock<IRepoStorage> _repoStorageMock;
        private Mock<IHasher> _hasherMock;
        private RegisterModelView _viewModel;

        [SetUp]
        public void Setup()
        {
            // Создаем моки для хранилища и хэшера
            _repoStorageMock = new Mock<IRepoStorage>();
            _hasherMock = new Mock<IHasher>();

            // Создаем объект RegisterModelView и передаем моки
            _viewModel = new RegisterModelView
            {
                Storage = new Storage { RepoStorage = _repoStorageMock.Object, Hasher = _hasherMock.Object }
            };
        }

        [Test]
        public void Nickname_Validation_ShouldFail_IfTooShort()
        {
            // Arrange
            _viewModel.Nickname = "a";

            // Act
            var result = _viewModel.IsCorrectName();

            // Assert
            Assert.IsFalse(result, "Nickname should be at least 2 characters long.");
        }

        [Test]
        public void Nickname_Validation_ShouldFail_IfAlreadyExists()
        {
            // Arrange
            _viewModel.Nickname = "existingUser";
            _repoStorageMock.Setup(x => x.HasNicknameInStorage("existingUser")).Returns(true);

            // Act
            var result = _viewModel.IsCorrectName();

            // Assert
            Assert.IsFalse(result, "Nickname should not already exist in storage.");
        }

        [Test]
        public void Password_Validation_ShouldFail_IfTooShort()
        {
            // Arrange
            _viewModel.Password = "123";

            // Act
            var result = _viewModel.IsCorrectPassword();

            // Assert
            Assert.IsFalse(result, "Password should be at least 4 characters long.");
        }

        [Test]
        public void Password_Validation_ShouldFail_IfContainsInvalidCharacters()
        {
            // Arrange
            _viewModel.Password = "pass@word";

            // Act
            var result = _viewModel.IsCorrectPassword();

            // Assert
            Assert.IsFalse(result, "Password should only contain Latin letters, numbers, and underscores.");
        }

        [Test]
        public async Task RegisterUser_Command_ShouldShowErrorMessage_IfPasswordsDoNotMatch()
        {
            // Arrange
            _viewModel.Nickname = "newUser";
            _viewModel.Password = "password123";
            _viewModel.PasswordRepeat = "password321";
            _repoStorageMock.Setup(x => x.HasNicknameInStorage(It.IsAny<string>())).Returns(false);

            // Act
            await _viewModel.RegisterUser.Execute(null);

            // Assert
            // Здесь можно проверить, вызвался ли MessageBox с сообщением об ошибке
            Assert.AreEqual("", _viewModel.PasswordRepeat, "PasswordRepeat should be reset if passwords do not match.");
        }

        // [Test]
        // public async Task RegisterUser_Command_ShouldRegisterUser_IfValid()
        // {
        //     _viewModel.Nickname = "newUser";
        //     _viewModel.Password = "password123";
        //     _viewModel.PasswordRepeat = "password123";
        //     _repoStorageMock.Setup(x => x.HasNicknameInStorage(It.IsAny<string>())).Returns(false);
        //     
        //     await _viewModel.RegisterUser.Execute(null);
        //     
        //     _repoStorageMock.Verify(x => x.SaveUser(It.IsAny<User>()), Times.Once, "User should be saved if registration is valid.");
        // }
    }
}
