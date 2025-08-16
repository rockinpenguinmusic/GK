namespace GK.Talks
{
    using System;

    public record RegisterInput(
        IRepository Repository, 
        string FirstName, 
        string LastName, 
        string Email, 
        int Exp, 
        bool HasBlog, 
        string URL, 
        string Browser, 
        string CsvCertifications, 
        string Emp, 
        int Fee, 
        string CsvSess);
}
