namespace AdmitOne
{
    public interface ILogPeopleIn
    {
        bool TryLogin(string username, string password);
    }
}
