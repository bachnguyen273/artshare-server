using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.CustomExceptions
{
    public class RegistrationException : Exception
    {
        public RegistrationException(string message) : base(message) { }
    }
}
