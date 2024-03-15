using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Infrastructure
{
    public class GenerateID : IGenerateID
    {
        public string GenerateId(string prefix)
        {
            Random random = new Random();
            return $"{prefix}{random.NextInt64(10000000, 99999999)}";
        }
    }
}
