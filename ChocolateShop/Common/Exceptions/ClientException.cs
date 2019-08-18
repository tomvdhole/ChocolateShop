using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChocolateShopApp.Common.Exceptions
{
    public class ClientException: Exception
    {
        public ClientException(string error)
           : base(error)
        { }
    }
}
