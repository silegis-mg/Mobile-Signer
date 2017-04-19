using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Xml.Xsl;
using System.Xml;
using Java.IO;
using System.IO;
using Almg.MobileSigner.Services;
using Almg.MobileSigner.Droid.XML;

[assembly: Xamarin.Forms.Dependency(typeof(XSLTTransformerImpl))]
namespace Almg.MobileSigner.Droid.XML
{
    public class XSLTTransformerImpl: IXslTransformer
    {
        public string Transform(string xslt, string xml)
        {
            XmlReader xsltReader = XmlReader.Create(new System.IO.StringReader(xslt));

            var transform = new XslCompiledTransform();
            transform.Load(xsltReader);
            String transformedResult = null;
            XmlReader inputXmlReader;
            using (var inputReader = new System.IO.StringReader(xml))
            {
                inputXmlReader = XmlReader.Create(inputReader);

                using (var writer = new System.IO.StringWriter())
                {
                    transform.Transform(inputXmlReader, null, writer);
                    transformedResult = writer.GetStringBuilder().ToString();
                }
            }
            return transformedResult;
        }

    }
}