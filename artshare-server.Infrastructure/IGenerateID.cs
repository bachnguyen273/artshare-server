using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Infrastructure
{
    public interface IGenerateID
    {
        string GenerateId(string prefix);
    }
}
