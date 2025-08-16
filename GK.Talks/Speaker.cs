namespace GK.Talks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a single speaker
    /// </summary>
    /// </summary>
    public class Speaker
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? Experience { get; set; }
        public bool HasBlog { get; set; }
        public string BlogURL { get; set; }
        public WebBrowser Browser { get; set; }
        public List<string> Certifications { get; set; }
        public string Employer { get; set; }
        public int RegistrationFee { get; private set; }
        public List<Session> Sessions { get; set; }

        private readonly List<string> Technologies = ["Cobol", "Punch Cards", "Commodore", "VBScript"];
        private readonly List<string> Domains = ["aol.com", "prodigy.com", "compuserve.com"];
        private readonly List<string> Employers = ["Pluralsight", "Microsoft", "Google"];

        /// <summary>
		/// Register a speaker
		/// Prams 
		/// strFirstName speakers first name
		///	strLastName ^^^ last name
		/// Email the email
		/// blogs etc.....
		/// </summary>
		/// <returns>speakerID</returns>
		public RegisterResponse Register(RegisterInput input)
        {
            if(!SpeakerDetailsChecker.ValidateSpeakerDetails(this, out var error))
            {
                return new RegisterResponse(error);
            }

            if(!SpeakerStandardsAreMet())
            {
                return new RegisterResponse(RegisterError.SpeakerDoesNotMeetStandards);
            }

            if (!Sessions.Any())
            {
                return new RegisterResponse(RegisterError.NoSessionsProvided);
            }

            var approved = false;

            foreach (var session in Sessions)
            {
                if (Technologies.Any(tech => session.Title.Contains(tech) || session.Description.Contains(tech)))
                {
                    session.Approved = false;
                }
                else
                {
                    session.Approved = true;
                    approved = true;
                }
            }

            if(!approved)
            {
                return new RegisterResponse(RegisterError.NoSessionsApproved);
            }

            RegistrationFee = GetRegistrationFee();

            try
            {
                return new RegisterResponse(input.Repository.SaveSpeaker(this));
            }
            catch (Exception e)
            {
                return new RegisterResponse(RegisterError.DatabaseError, e.Message);
            }
        }

        private bool SpeakerStandardsAreMet()
        {
            return (Experience > 10 || HasBlog || Certifications.Count() > 3 || Employers.Contains(Employer)) ||
                   EmailDomainIsValid();
        }

        private int GetRegistrationFee() => Experience switch
        {
            <= 1 => 500,
            <= 3 => 250,
            <= 5 => 100,
            <= 9 => 50,
            _ => 0,
        };

        private bool EmailDomainIsValid()
        {
            var emailDomain = Email.Split('@').Last();

            return !Domains.Contains(emailDomain) && (!(Browser.Name == WebBrowserName.InternetExplorer && Browser.MajorVersion < 9));
        }
    }
}
