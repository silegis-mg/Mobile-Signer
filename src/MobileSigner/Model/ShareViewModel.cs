using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Model
{
    public class ShareViewModel
    {
        public Command ShareCommand { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }

        public ShareViewModel()
        {
            ShareCommand = new Command(Share);
        }

        public void Share()
        {
            MessagingCenter.Send<ShareViewModel>(this, "Share");
        }
    }
}
