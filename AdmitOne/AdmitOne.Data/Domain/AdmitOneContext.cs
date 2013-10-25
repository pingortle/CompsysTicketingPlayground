using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data.Domain
{
    public class AdmitOneContext<T> : DataContext
    {
        public AdmitOneContext(IDbConnection configString) : base(configString) { }

        public AdmitOneContext()
            : base(ConfigurationManager.ConnectionStrings["Test"].ConnectionString, AdmitOneMapper.MappingInstance) { }
    }
}
