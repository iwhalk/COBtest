using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Shared.Models
{
    [Serializable()]
    public class ValidationException : Exception
    {
        private string ObjectName;

        protected ValidationException()
           : base()
        { }

        public ValidationException(string message) :
           base(message)
        {
            ObjectName = string.Empty;
        }

        public ValidationException(string objectName, string message)
           : base(message)
        {
            ObjectName = objectName;
        }

        public ValidationException(string objectName, string message, Exception innerException) :
           base(message, innerException)
        {
            ObjectName = objectName;
        }

        protected ValidationException(SerializationInfo info,
                                    StreamingContext context)
           : base(info, context)
        { }

        public string InvalidObject
        { get { return ObjectName; } }
    }
}
