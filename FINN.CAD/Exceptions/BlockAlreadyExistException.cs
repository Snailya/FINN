using System;
using System.Runtime.Serialization;

namespace FINN.CAD.Exceptions
{
    [Serializable]
    internal class BlockAlreadyExistException : Exception
    {
        public BlockAlreadyExistException()
        {
        }

        public BlockAlreadyExistException(string message) : base(message)
        {
        }

        public BlockAlreadyExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BlockAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}