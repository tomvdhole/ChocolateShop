using System;

namespace ChocolateShopApi.Common.Exceptions
{
    public class EntityException: Exception
    {
        public EntityException(string error)
            :base(error)
        { }
    }
}

