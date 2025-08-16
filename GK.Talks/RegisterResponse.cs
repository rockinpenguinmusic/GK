namespace GK.Talks
{
    public class RegisterResponse
    {
        public RegisterResponse(int speakerId) => SpeakerId = speakerId;
        public RegisterResponse(RegisterError registerError) => RegisterError = registerError;

        public RegisterResponse(RegisterError registerError, string errorMessage)
        {
            RegisterError = registerError;
            ErrorMessage = errorMessage;
        }

        public RegisterError RegisterError { get; }
        public int SpeakerId { get; }
        public string ErrorMessage { get; }
    }
}