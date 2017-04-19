using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Components
{

    public interface IPromptDialog
    {
        Task<string> Show(PromptDialog config);
    }

    public class PromptDialog
    {
        public string Text { get; set; }
        public string Title { get; set; }
        public string OkText { get; set; }
        public string CancelText { get; set; }
        public bool Password { get; set; }

        public PromptDialog(string title, string text, bool password, string okText, string cancelText)
        {
            Title = title;
            Text = text;
            OkText = okText;
            CancelText = cancelText;
            this.Password = password;
        }

        public PromptDialog(string title, string text): this(title, text, false, AppResources.OK, AppResources.CANCEL)
        {
        }

        public PromptDialog(string title, string text, bool password) : this(title, text, password, AppResources.OK, AppResources.CANCEL)
        {
        }

        public Task<string> Show()
        {
            return DependencyService.Get<IPromptDialog>().Show(this);
        }
    }
}
