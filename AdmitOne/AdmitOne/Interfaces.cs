using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne
{
    public interface ILogPeopleIn
    {
        bool TryLogin(string username, string password);
    }
}
