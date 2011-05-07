using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LSHS.Messages
{
    public class ErrorMessage
    {
        public Exception Error { get; private set; }

        public ErrorMessage(Exception exception)
        {
            // TODO: Complete member initialization
            this.Error = exception;
        }
    }
}