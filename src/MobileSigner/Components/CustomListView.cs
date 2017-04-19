using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Components
{
    public class CustomListView: ListView
    {
        public event EventHandler<ItemTappedEventArgs> LongClicked;
        
        public CustomListView()
        {
            //hides empty cell's separators on iOS
            this.Footer = new Label(); 
        }

        public void OnLongClicked(object item)
        {
            LongClicked?.Invoke(this, new ItemTappedEventArgs(this, item));
        }
    }
}
