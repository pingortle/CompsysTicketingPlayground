namespace AdmitOne.Persistence.Internal
{
    // http://stackoverflow.com/a/19130718
    internal static class SuperUglyHack
    {
        private static void FixEfProviderBadBadBad()
        {
            // The compiler mysteriously decides to simply exclude EntityFramework.SqlServer.dll
            // from the bin folder unless I do this:
            var instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance;
        }
    }
}
