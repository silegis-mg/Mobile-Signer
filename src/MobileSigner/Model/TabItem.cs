using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Model
{
    public class TabItem
    {
        public TabItem(string name)
        {
            this.Name = name;
        }

        public string Name { private set; get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
