using Acr.UserDialogs;
using Almg.MobileSigner.Exceptions;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using Almg.MobileSigner.Services;
using Almg.Signer.XAdES;
using Almg.Signer.XAdES.Interfaces;
using Org.BouncyCastle.X509;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Almg.MobileSigner.Helpers.HttpRequest;

namespace Almg.MobileSigner.Helpers
{
    public class SignerHelper
    {
        private static PolicyIdentifier policy;

        static SignerHelper()
        {
            var localFileLoader = DependencyService.Get<IResourceLoader>();
            using (Stream streamXslt = localFileLoader.OpenFile("PA_AD_RA_v2_3.xml"))
            {
                byte[] policyFileData = StreamHelper.StreamToByteArray(streamXslt);
                policy = new PolicyIdentifier("http://politicas.icpbrasil.gov.br/PA_AD_RA_v2_3.xml", policyFileData, "qipT2sA6/iY9Z14EeVBdoiDWxdqsu5WEuY23mxbADmE=");
            }
        }

        public static async Task Sign(BaseInboxMessage request)
        {
            try
            {
                var certificate = new MemoryStream();
                if (await Pkcs12FileHelper.LoadFileTo(certificate))
                {
                    var password = await DialogHelper.AskForPassword(AppResources.INPUT_PKCS12_PASSWORD, AppResources.SIGN_DOCUMENT);
                    PfxEntry pfxEntry = null;

                    try
                    {
                        pfxEntry = Pkcs12FileHelper.Load(certificate, password);
                    }
                    catch (IOException e)
                    {
                        throw new DigitalSignatureError(AppResources.UNABLE_TO_OPEN_PRIVATE_KEY, e);
                    }

                    if (pfxEntry != null)
                    {

                        if (!pfxEntry.Certificate.IsValidNow)
                        {
                            throw new CertificateExpiredError();
                        }

                        var xadesSigner = XAdESCrossPlatformSigner.Current;
                        using (DialogHelper.ShowProgress(AppResources.SIGNING))
                        {
                            List<Signature> signatures = new List<Signature>();
                            foreach(Document doc in request.Documents) {

                                var signed = new MemoryStream();
                                var toSign = new MemoryStream(doc.Content);
                                xadesSigner.Sign(toSign, signed, "#documento", pfxEntry.Certificate, pfxEntry.Key, policy);

                                Signature signature = new Signature();
                                signature.Id = doc.Id;
                                byte[] baSigned = StreamHelper.StreamToByteArray(signed);
                                signature.SignatureData = Encoding.UTF8.GetString(baSigned, 0, baSigned.Length);
                                signatures.Add(signature);
                            }

                            var response = await HttpRequest.Post(EndPointHelper.GetSendSignature(), 
                                                                  JsonHelper.ToJson<List<Signature>>(signatures),
                                                                  Encoding.UTF8,
                                                                  "application/json");
                            if (!response.IsSuccessStatusCode)
                            {
                                throw new DigitalSignatureError(AppResources.UNABLE_TO_SEND_DIGITAL_SIGNATURE);
                            }
                        }
                    }
                    else
                    {
                        throw new DigitalSignatureError(AppResources.UNABLE_TO_OPEN_PRIVATE_KEY);
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                throw new DigitalSignatureError(AppResources.SIGNATURE_CANCELLED, ex);
            }
            catch(HttpException ex)
            {
                throw new DigitalSignatureError(AppResources.DIGITAL_SIGNATURE_FAILED, ex);
            }
            catch(DigitalSignatureError ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                //TODO: logar esse erro no servidor
                //UserDialogs.Instance.Alert(e.Message, AppResources.APP_TITLE, AppResources.OK);
                throw new DigitalSignatureError(AppResources.DIGITAL_SIGNATURE_FAILED_UNKNOW_REASON, ex); ;
            }
        }
    }
}
