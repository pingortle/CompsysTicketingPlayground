using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdmitOne
{
    public sealed class LoginManager : ILogPeopleIn
    {
        private static Random _random = new Random(0);

        public LoginManager()
        {
            _random.Next();
        }

        public bool TryLogin(string username, string password)
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(_random.Next(0, 4)));
            return _random.Next(0, 2) == 0;
        }
    }
}
