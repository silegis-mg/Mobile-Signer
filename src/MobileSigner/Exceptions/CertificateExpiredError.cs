using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Exceptions
{
    public class CertificateExpiredError : DigitalSignatureError
    {
        public CertificateExpiredError() : base(AppResources.CERTIFICATE_EXPIRED)
        {
        }
    }
}
