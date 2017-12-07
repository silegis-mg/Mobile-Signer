using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Helpers
{
    public class HashUtil
    {
        public static string SHA1(MemoryStream stream)
        {
            IDigest hash = new Sha1Digest();

            byte[] result = new byte[hash.GetDigestSize()];

            byte[] buffer = new byte[4092];
            int bytesRead;
            stream.Position = 0;

            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                hash.BlockUpdate(buffer, 0, bytesRead);
            }

            hash.DoFinal(result, 0);
            
            return string.Join("", result.Select(b => b.ToString("x2")).ToArray());
        }

    }
}
