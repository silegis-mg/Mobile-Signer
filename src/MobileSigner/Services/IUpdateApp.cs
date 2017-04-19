using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Services
{
    /// <summary>
    /// Platform specific application version check and update
    /// </summary>
    public interface IUpdateApp
    {
        String GetVersion();
        void Update();
    }
}
