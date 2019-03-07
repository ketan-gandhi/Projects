using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CylanceGUID.Exceptions
{
    public class RecordNotFound : Exception
    {
        public RecordNotFound()
        {
        }
        public RecordNotFound(string message) : base(message)
        {
        }
        

    }

    public class InvalidRequestParameter : Exception
    {
        public InvalidRequestParameter()
        {
        }
        public InvalidRequestParameter(string message) : base(message)
        {
        }


    }

    public class RecordAlreadyExists : Exception
    {
        public RecordAlreadyExists()
        {
        }
        public RecordAlreadyExists(string message) : base(message)
        {
        }


    }
}
