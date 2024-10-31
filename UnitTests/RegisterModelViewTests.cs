using Moq;
using ITNOte.me.ModelView;
using ITNOte.me.Model.Storage;

namespace UnitTests
{
    [TestFixture]
    public class RegisterModelViewTests
    {
        private Mock<IStorage> _repoStorageMock;
        private RegisterModelView _viewModel;

        [SetUp]
        public void Setup()
        {
            _repoStorageMock = new Mock<IStorage>();
            
            _viewModel = new RegisterModelView
            {
                _storage = new Storage(_repoStorageMock.Object) 
            };
        }

        [Test]
        public void Nickname_Validation_ShouldFail_IfTooShort()
        {
            _viewModel.Nickname = "a";
            
            var result = _viewModel.IsCorrectName();
            
            Assert.That(result, Is.False, "Nickname should be at least 2 characters long.");
        }

        [Test]
        public void Nickname_Validation_ShouldFail_IfAlreadyExists()
        {
            _viewModel.Nickname = "existingUser";
            _repoStorageMock.Setup(x => x.HasNicknameInStorage("existingUser")).Returns(true);
            
            var result = _viewModel.IsCorrectName();
            
            Assert.That(result, Is.False, "Nickname should not already exist in storage.");
        }

        [Test]
        public void Password_Validation_ShouldFail_IfTooShort()
        {
            _viewModel.Password = "123";
            
            var result = _viewModel.IsCorrectPassword();
            
            Assert.That(result, Is.False, "Password should be at least 4 characters long.");
        }

        [Test]
        public void Password_Validation_ShouldFail_IfContainsInvalidCharacters()
        {
            _viewModel.Password = "pass@word";
            
            var result = _viewModel.IsCorrectPassword();
            
            Assert.That(result, Is.False, "Password should only contain Latin letters, numbers, and underscores.");
        }

        [Test]
        public async Task RegisterUser_Command_ShouldShowErrorMessage_IfPasswordsDoNotMatch()
        {
            _viewModel.Nickname = "newUser";
            _viewModel.Password = "password123";
            _viewModel.PasswordRepeat = "password321";
            _repoStorageMock.Setup(x => x.HasNicknameInStorage(It.IsAny<string>())).Returns(false);
            
            _viewModel.RegisterUser.Execute(null);
            
            Assert.That(_viewModel.PasswordRepeat, Is.EqualTo(""), "PasswordRepeat should be reset if passwords do not match.");
        }
    }
}
