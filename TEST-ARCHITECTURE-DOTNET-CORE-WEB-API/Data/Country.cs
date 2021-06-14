using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TEST_ARCHITECTURE_DOTNET_CORE_WEB_API.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public virtual ICollection<Hotel> Hotels { get; set; }
    }
}
