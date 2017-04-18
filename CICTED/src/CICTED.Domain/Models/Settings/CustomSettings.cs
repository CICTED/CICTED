using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CICTED.Domain.Models.Settings
{
    public class CustomSettings
    {
        public string ConnectionString { get; set; }
        public string TwillioAccountSID { get; set; }
        public string TwillioToken { get; set; }
        public string TwillioURL { get; set; }
        public string TwillioNumber { get; set; }
    }
}
