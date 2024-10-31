using Moq;
using ITNOte.me.ModelView;
using ITNOte.me.Model.Notes;
using ITNOte.me.Model.Storage;
using ITNOte.me.Model.User;

namespace UnitTests
{
    [TestFixture]
    public class RedactorModelViewTests
    {
        private Mock<IStorage> _repoStorageMock;
        private User _testUser;
        private RedactorModelView _redactorModelView;

        [SetUp]
        public void SetUp()
        {
            _repoStorageMock = new Mock<IStorage>();
            _testUser = new User("TestUser", null) { GeneralFolder = new Folder("RootFolder") };
            
            _redactorModelView = new RedactorModelView(_testUser)
            {
                _storage = _repoStorageMock.Object
            };
        }

        [Test]
        public void LogOutUsername_ShouldReturnCorrectMessage()
        {
            Assert.That(_redactorModelView.LogOutUsername, Is.EqualTo("logout from TestUser"));
        }

        [Test]
        public void NameOfNewSource_ShouldTriggerPropertyChanged()
        {
            var eventTriggered = false;
            _redactorModelView.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(_redactorModelView.NameOfNewSource))
                    eventTriggered = true;
            };

            _redactorModelView.NameOfNewSource = "NewSourceName";

            Assert.Multiple(() =>
            {
                Assert.That(eventTriggered, Is.True);
                Assert.That(_redactorModelView.NameOfNewSource, Is.EqualTo("NewSourceName"));
            });
        }

        [Test]
        public void IsValidNameOfSource_ShouldReturnFalse_WhenNameAlreadyExists()
        {
            var existingFolder = new Folder("ExistingFolder", _testUser.GeneralFolder);
            _testUser.GeneralFolder.Children?.Add(existingFolder);
            
            _redactorModelView.NameOfNewSource = "ExistingFolder";
            
            var isValidMethod = typeof(RedactorModelView).GetMethod("IsValidNameOfSource", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var result = (bool)isValidMethod!.Invoke(_redactorModelView, null)!;

            Assert.That(result, Is.False);
        }

        [Test]
        public async Task NewFolder_ShouldCreateNewFolder_WhenNameIsValid()
        {
            _redactorModelView.NameOfNewSource = "NewFolder";

            _redactorModelView.NewFolder.Execute(null);

            Assert.IsTrue(_testUser.GeneralFolder.Children!.Any(f => f.Name == "NewFolder"));
            _repoStorageMock.Verify(repo => repo.SaveUser(_testUser), Times.Once);
        }

        [Test]
        public async Task NewFolder_ShouldShowErrorMessage_WhenNameIsInvalid()
        {
            _redactorModelView.NameOfNewSource = "Invalid/Name";
            
            _redactorModelView.NewFolder.Execute(null);
        
            Assert.That(_redactorModelView.NameOfNewSource, Is.EqualTo("Invalid/Name"));
        }
    }
}
