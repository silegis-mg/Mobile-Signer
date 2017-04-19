using Almg.MobileSigner.Components;
using Almg.MobileSigner.Droid;
using Android.App;
using Android.Widget;
using System.Threading.Tasks;
using Xamarin.Forms;
using AppCompatAlertDialog = Android.Support.V7.App;

[assembly: Dependency(typeof(EntryPopupLoader))]
namespace Almg.MobileSigner.Droid
{
    public class EntryPopupLoader : IPromptDialog
    {
        public Task<string> Show(PromptDialog config)
        {
            var edit = new EditText(Forms.Context);
            if (config.Password)
            {
                edit.InputType = Android.Text.InputTypes.TextVariationPassword | Android.Text.InputTypes.ClassText;
            }

            var task = new TaskCompletionSource<string>();
            var builder = new AlertDialog.Builder(Forms.Context)
                .SetCancelable(true)
                .SetMessage(config.Text)
                .SetTitle(config.Title)
                .SetView(edit)
                .SetPositiveButton(config.OkText, (s, a) =>
                {
                    task.SetResult(edit.Text);
                })
                .SetNegativeButton(config.CancelText, (senderAlert, args) =>
                {
                    task.SetCanceled();
                })
                .Show();
            
            return task.Task;
        }
    }
}

