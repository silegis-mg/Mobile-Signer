using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Exceptions
{
    public class DigitalSignatureError : Exception
    {
        public DigitalSignatureError(string message, Exception e) : base(message, e)
        {
        }

        public DigitalSignatureError(string message) : base(message)
        {
        }
    }
}
