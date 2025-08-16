namespace GK.Talks.Tests
{
    using AutoFixture;
    using Moq;
    using Moq.AutoMock;

    public class SpeakerTests
    {
        private readonly Fixture _fixture = new ();
        private readonly AutoMocker _mocker = new();
        private readonly Speaker _sut = new();

        private readonly RegisterInput _registerInput;
        private readonly Mock<IRepository> _repositoryMock = new();

        public SpeakerTests()
        {
            _registerInput = new RegisterInput(
                _repositoryMock.Object,
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<int>(),
                _fixture.Create<bool>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<int>(),
                _fixture.Create<string>());
        }


        [Fact]
        public void GivenNullFirstName_WhenCallRegister_ReturnsFirstNameRequired()
        {
            _sut.FirstName = null;

            var result = _sut.Register(_registerInput);

            Assert.NotNull(result);
            Assert.Equal(RegisterError.FirstNameRequired, result.RegisterError);
        }

        [Fact]
        public void GivenBlankFirstName_WhenCallRegister_ReturnsFirstNameRequired()
        {
            _sut.FirstName = string.Empty;

            var result = _sut.Register(_registerInput);

            Assert.NotNull(result);
            Assert.Equal(RegisterError.FirstNameRequired, result.RegisterError);
        }

        [Fact]
        public void GivenNullLastName_WhenCallRegister_ReturnsLastNameRequired()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = null;

            var result = _sut.Register(_registerInput);

            Assert.NotNull(result);
            Assert.Equal(RegisterError.LastNameRequired, result.RegisterError);
        }

        [Fact]
        public void GivenBlankLastName_WhenCallRegister_ReturnsLastNameRequired()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = string.Empty;

            var result = _sut.Register(_registerInput);

            Assert.NotNull(result);
            Assert.Equal(RegisterError.LastNameRequired, result.RegisterError);
        }

        [Fact]
        public void GivenNullEmail_WhenCallRegister_ReturnsEmailRequired()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = null;

            var result = _sut.Register(_registerInput);

            Assert.NotNull(result);
            Assert.Equal(RegisterError.EmailRequired, result.RegisterError);
        }

        [Fact]
        public void GivenBlankEmail_WhenCallRegister_ReturnsEmailRequired()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = string.Empty;

            var result = _sut.Register(_registerInput);
         
            Assert.NotNull(result);
            Assert.Equal(RegisterError.EmailRequired, result.RegisterError);
        }

        [Fact]
        public void GivenExpLessThan10_WhenCallRegister_ReturnsSpeakerDoesNotMeetStandards()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = $"{_fixture.Create<string>()}@aol.com";
            _sut.Experience = 5;
            _sut.Certifications = _fixture.Create<List<string>>();
            _sut.Browser = new WebBrowser()
            {
                MajorVersion = 8,
                Name = WebBrowserName.InternetExplorer
            };
            var result = _sut.Register(_registerInput);
            Assert.NotNull(result);
            Assert.Equal(RegisterError.SpeakerDoesNotMeetStandards, result.RegisterError);
        }

        [Fact]
        public void GivenNoSessions_WhenCallRegister_ReturnsNoSessionsProvided()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = _fixture.Create<string>();
            _sut.Experience = 15;
            _sut.Sessions = [];
            _sut.Browser = _fixture.Create<WebBrowser>();
            var result = _sut.Register(_registerInput);
            Assert.NotNull(result);
            Assert.Equal(RegisterError.NoSessionsProvided, result.RegisterError);
        }

        [Fact]
        public void GivenSessionsWithTechInTitle_WhenCallRegister_ReturnsNoSessionsApproved()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = $"{_fixture.Create<string>()}@aol.com";
            _sut.Experience = 15;
            _sut.Sessions =
            [
                _fixture
                    .Build<Session>()
                    .With(session => session.Approved, false)
                    .With(session => session.Title, $"{_fixture.Create<string>()}Cobol{_fixture.Create<string>()}")
                    .Create()
            ];

            _sut.Browser = _fixture.Create<WebBrowser>();
            var result = _sut.Register(_registerInput);
            Assert.NotNull(result);
            Assert.Equal(RegisterError.NoSessionsApproved, result.RegisterError);
        }

        [Fact]
        public void GivenSessionsWithTechInDescription_WhenCallRegister_ReturnsNoSessionsApproved()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = $"{_fixture.Create<string>()}@aol.com";
            _sut.Experience = 15;
            _sut.Sessions =
            [
                _fixture
                    .Build<Session>()
                    .With(session => session.Approved, false)
                    .With(session => session.Description, $"{_fixture.Create<string>()}Cobol{_fixture.Create<string>()}")
                    .Create()
            ];
            _sut.Browser = _fixture.Create<WebBrowser>();
            var result = _sut.Register(_registerInput);
            Assert.NotNull(result);
            Assert.Equal(RegisterError.NoSessionsApproved, result.RegisterError);
        }

        [Theory]
        [InlineData(1, 500)]
        [InlineData(2, 250)]
        [InlineData(3, 250)]
        [InlineData(4, 100)]
        [InlineData(5, 100)]
        [InlineData(6, 50)]
        [InlineData(7, 50)]
        [InlineData(8, 50)]
        [InlineData(9, 50)]
        [InlineData(10, 0)]
        public void GivenApproved_WhenCallRegister_CorrectRegistrationFeeAppliedAndSuccessfulSave(int experienceLevel, int expectedRegistrationFee)
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = _fixture.Create<string>();
            _sut.Experience = experienceLevel;
            _sut.Certifications = _fixture.Create<List<string>>();
            _sut.Sessions =
            [
                _fixture.Create<Session>()
            ];
            _sut.Browser = _fixture.Create<WebBrowser>();
            var result = _sut.Register(_registerInput);
            Assert.NotNull(result);
            Assert.Equal(expectedRegistrationFee, _sut.RegistrationFee);
            _repositoryMock.Verify(repo => repo.SaveSpeaker(It.IsAny<Speaker>()), Times.Once);
        }

        [Fact]
        public void GivenDatabaseError_WhenCallRegister_ReturnsDatabaseError()
        {
            _sut.FirstName = _fixture.Create<string>();
            _sut.LastName = _fixture.Create<string>();
            _sut.Email = _fixture.Create<string>();
            _sut.Experience = 1;
            _sut.Certifications = _fixture.Create<List<string>>();
            _sut.Sessions =
            [
                _fixture.Create<Session>()
            ];
            _sut.Browser = _fixture.Create<WebBrowser>();

            var databaseErrorMessage = _fixture.Create<string>();

            _repositoryMock
                .Setup(repo => repo.SaveSpeaker(It.IsAny<Speaker>()))
                .Throws(new Exception(databaseErrorMessage));

            var result = _sut.Register(_registerInput);
            Assert.NotNull(result);
            _repositoryMock.Verify(repo => repo.SaveSpeaker(It.IsAny<Speaker>()), Times.Once);
            Assert.Equal(RegisterError.DatabaseError, result.RegisterError);
            Assert.Equal(databaseErrorMessage, result.ErrorMessage);
        }
    }
}
