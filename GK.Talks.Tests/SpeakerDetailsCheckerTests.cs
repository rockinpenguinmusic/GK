using AutoFixture;

namespace GK.Talks.Tests
{
    public class SpeakerDetailsCheckerTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public void GivenValidSpeaker_WhenCheckValidity_ReturnsTrue()
        {
            var speaker = new Speaker
            {
                FirstName = _fixture.Create<string>(),
                LastName = _fixture.Create<string>(),
                Email = _fixture.Create<string>()
            };

            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.True(result);
            Assert.Equal(RegisterError.NoErrors, error);
        }

        [Fact]
        public void GivenNullFirstName_WhenCheckValidity_ReturnsFalseAndError()
        {
            var speaker = new Speaker
            {
                FirstName = null,
                LastName = _fixture.Create<string>(),
                Email = _fixture.Create<string>()
            };
            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.False(result);
            Assert.Equal(RegisterError.FirstNameRequired, error);
        }

        [Fact]
        public void GivenBlankFirstName_WhenCheckValidity_ReturnsFalseAndError()
        {
            var speaker = new Speaker
            {
                FirstName = string.Empty,
                LastName = _fixture.Create<string>(),
                Email = _fixture.Create<string>()
            };
            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.False(result);
            Assert.Equal(RegisterError.FirstNameRequired, error);
        }

        [Fact]
        public void GivenNullLastName_WhenCheckValidity_ReturnsFalseAndError()
        {
            var speaker = new Speaker
            {
                FirstName = _fixture.Create<string>(),
                LastName = null,
                Email = _fixture.Create<string>()
            };
            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.False(result);
            Assert.Equal(RegisterError.LastNameRequired, error);
        }

        [Fact]
        public void GivenBlankLastName_WhenCheckValidity_ReturnsFalseAndError()
        {
            var speaker = new Speaker
            {
                FirstName = _fixture.Create<string>(),
                LastName = string.Empty,
                Email = _fixture.Create<string>()
            };
            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.False(result);
            Assert.Equal(RegisterError.LastNameRequired, error);
        }

        [Fact]
        public void GivenNullEmail_WhenCheckValidity_ReturnsFalseAndError()
        {
            var speaker = new Speaker
            {
                FirstName = _fixture.Create<string>(),
                LastName = _fixture.Create<string>(),
                Email = null
            };
            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.False(result);
            Assert.Equal(RegisterError.EmailRequired, error);
        }

        [Fact]
        public void GivenBlankEmail_WhenCheckValidity_ReturnsFalseAndError()
        {
            var speaker = new Speaker
            {
                FirstName = _fixture.Create<string>(),
                LastName = _fixture.Create<string>(),
                Email = string.Empty
            };
            var result = SpeakerDetailsChecker.ValidateSpeakerDetails(speaker, out var error);

            Assert.False(result);
            Assert.Equal(RegisterError.EmailRequired, error);
        }
    }
}
