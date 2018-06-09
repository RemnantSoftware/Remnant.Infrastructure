using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remnant.Core.Exceptions
{
  public class BlueMercsException : Exception
  {
    public BlueMercsException(string message) 
      : base(message)
    {      
    }

    public BlueMercsException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

  }
}
