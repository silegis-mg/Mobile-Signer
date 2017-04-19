using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Services
{
    /// <summary>
    /// The class XslCompiledTransform is not available on PCL projects. 
    /// We will use this interface to access the implementation on each native projects.
    /// </summary>
    public interface IXslTransformer
    {
        string Transform(string xslt, string xml);
    }
}
