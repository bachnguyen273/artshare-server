using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artshare_server.Services.FilterModels.Helpers
{
    public static class Utils
    {
        public static object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }
    }
}
