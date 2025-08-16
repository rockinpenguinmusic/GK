namespace GK.Talks
{
    public class SpeakerDetailsChecker
    {
        public static bool ValidateSpeakerDetails(Speaker speaker, out RegisterError error)
        {
            error = speaker switch
            {
                var s when string.IsNullOrWhiteSpace(s.FirstName) => RegisterError.FirstNameRequired,
                var s when string.IsNullOrWhiteSpace(s.LastName) => RegisterError.LastNameRequired,
                var s when string.IsNullOrWhiteSpace(s.Email) => RegisterError.EmailRequired,
                _ => RegisterError.NoErrors
            };

            return error == RegisterError.NoErrors;
        }
    }
}
