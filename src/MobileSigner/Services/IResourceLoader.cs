using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Services
{
    public interface IResourceLoader
    {
        Stream OpenFile(string name);
    }
}
