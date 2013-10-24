using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AdmitOne.Data
{
    public static class Mapping
    {
        public static XmlMappingSource GetMapping()
        {
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DbMap.map"))
            {
                return XmlMappingSource.FromStream(stream);
            }
        }

        private static XmlMappingSource _mapping;
        public static XmlMappingSource MappingInstance { get { return _mapping = _mapping ?? GetMapping(); } }
    }
}
