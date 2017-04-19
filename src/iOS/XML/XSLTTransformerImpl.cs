using System;
using System.Xml;
using System.Xml.Xsl;
using Almg.MobileSigner.Services;
using MobileSigner.iOS;

[assembly: Xamarin.Forms.Dependency(typeof(XSLTTransformerImpl))]
namespace MobileSigner.iOS
{
	public class XSLTTransformerImpl : IXslTransformer
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
