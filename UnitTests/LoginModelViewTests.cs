using Moq;
using ITNOte.me.ModelView;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;

namespace UnitTests
{
    [TestFixture]
    public class LoginModelViewTests
    {
        private Mock<IStorage> _repoStorageMock;
        private LoginModelView _loginModelView;
        private Mock<PasswordHasher> _hasherMock;

        [SetUp]
        public void Setup()
        {
            _repoStorageMock = new Mock<IStorage>();
            _hasherMock = new Mock<PasswordHasher>();

            _loginModelView = new LoginModelView
            {
                _storage = new Storage(_repoStorageMock.Object)
            };
        }


        [Test]
        public async Task LoginUser_ShouldShowError_WhenInvalidNickname()
        {
            _repoStorageMock.Setup(repo => repo.HasNicknameInStorage("InvalidUser")).ReturnsAsync(false);

            _loginModelView.Nickname = "InvalidUser";

            _loginModelView.LoginUser.Execute(null);

            Assert.That(_loginModelView.Nickname, Is.EqualTo(string.Empty));
        }

        [Test]
        public async Task LoginUser_ShouldShowError_WhenInvalidPassword()
        {
            var testUser = new User("TestUser", _hasherMock.Object.HashPassword("hashed_password"));
            _repoStorageMock.Setup(repo => repo.HasNicknameInStorage("TestUser")).ReturnsAsync(true);
            _repoStorageMock.Setup(repo => repo.GetUserFromStorage<User>("TestUser")).ReturnsAsync(testUser);

            _loginModelView.Nickname = "TestUser";
            _loginModelView.Password = "WrongPassword";

            _loginModelView.LoginUser.Execute(null);

            Assert.That(_loginModelView.Password, Is.EqualTo(string.Empty));
        }
    }
}
