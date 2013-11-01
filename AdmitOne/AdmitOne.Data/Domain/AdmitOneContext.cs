using System.Configuration;
using System.Data;
using System.Data.Linq;

namespace AdmitOne.Data.Domain
{
    public class AdmitOneContext : DataContext
    {
        public AdmitOneContext(IDbConnection configString) : base(configString) { }

        public AdmitOneContext()
            : base(ConfigurationManager.ConnectionStrings["Test"].ConnectionString, AdmitOneMapper.MappingInstance) { }
    }
}
