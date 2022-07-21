using System;
using System.Runtime.Serialization;

namespace FINN.CAD.Exceptions
{
    [Serializable]
    internal class InstanceNotCreatedException : System.Exception
    {
        public InstanceNotCreatedException()
        {
        }

        public InstanceNotCreatedException(string message) : base(message)
        {
        }

        public InstanceNotCreatedException(string message, System.Exception innerException) : base(message,
            innerException)
        {
        }

        protected InstanceNotCreatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}