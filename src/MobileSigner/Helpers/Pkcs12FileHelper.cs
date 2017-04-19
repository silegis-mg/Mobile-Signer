using System;
using System.IO;
using System.Threading.Tasks;
using PCLStorage;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;
using System.Linq;

namespace Almg.MobileSigner.Helpers
{
	public class Pkcs12FileHelper
	{
		const string CERT_FILE = "cert.pfx";

		public static async Task<bool> Save(string pfxBase64)
		{
			byte[] data = Convert.FromBase64String(pfxBase64);
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			var file = await rootFolder.CreateFileAsync(CERT_FILE, CreationCollisionOption.ReplaceExisting);
            using (var stream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
                stream.Dispose();
            }
			return true;
		}

		public static async Task<bool> Save(Stream stream)
		{
			IFolder rootFolder = FileSystem.Current.LocalStorage;
			var file = await rootFolder.CreateFileAsync(CERT_FILE, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccess.ReadAndWrite))
            {
                StreamHelper.CopyStream(stream, fileStream);
                fileStream.Flush();
            }
			return true;
		}

        public static async void DeleteFile()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var result = await rootFolder.CheckExistsAsync(CERT_FILE);
            if(result == ExistenceCheckResult.FileExists)
            {
                var file = await rootFolder.GetFileAsync(CERT_FILE);
                await file.DeleteAsync();
            }
        }

        public static async Task<bool> Exists()
        {
            IFolder rootFolder = FileSystem.Current.LocalStorage;
            var exists = await rootFolder.CheckExistsAsync(CERT_FILE);
            return exists == ExistenceCheckResult.FileExists;
        }

		public static async Task<bool> LoadFileTo(MemoryStream memoryStream)
		{
            try
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                var file = await rootFolder.GetFileAsync(CERT_FILE);
                using (var stream = await file.OpenAsync(FileAccess.Read))
                {
                    StreamHelper.CopyStream(stream, memoryStream);
                }
                return true;
            } catch(Exception e)
            {
                return false;
            }
		}

        public static PfxEntry Load(Stream pfx, string password)
        {
            var pkcs = new Pkcs12Store(pfx, password.ToCharArray());
            var i = pkcs.Count;
            
            while (i-- > 0)
            {
                string alias = pkcs.Aliases.Cast<string>().ElementAt(i);
                var cert = pkcs.GetCertificate(alias);
                var key = pkcs.GetKey(alias);

                if (key == null)
                {
                    continue;
                }

                return new PfxEntry
                {
                    Certificate = cert.Certificate,
                    Key = key.Key
                };
            }
            return null;
        }
    }

    public class PfxEntry
    {
        public X509Certificate Certificate { get; set; }
        public AsymmetricKeyParameter Key { get; set; }

        public string DadosCertificado
        {
            get
            {
                return Certificate.IssuerDN.ToString() + Certificate.SubjectDN.ToString();
            }
        }
    }
}

