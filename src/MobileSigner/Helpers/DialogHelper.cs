using Acr.UserDialogs;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Helpers
{
    public class DialogHelper
    {
        public static IDisposable ShowProgress(string title)
        {
            var config = new ProgressDialogConfig()
                    .SetTitle(title)
                    .SetIsDeterministic(false)
                    .SetMaskType(MaskType.Black);
                    
            var progressDlg = UserDialogs.Instance.Progress(config);
            return progressDlg;
        }

        public static void ShowAlertOK(string title, string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                UserDialogs.Instance.Alert(message, title, AppResources.OK);
            });
        }

        public static Task<bool> ShowConfirm(string title, string message)
        {
            var task = new TaskCompletionSource<bool>();

            Action<bool> action = par =>
            {
                task.SetResult(par);
            };

			Device.BeginInvokeOnMainThread(() =>
			{
					UserDialogs.Instance.Confirm(new ConfirmConfig()
													  .SetMessage(message)
													  .SetTitle(title)
													  .SetOkText(AppResources.YES)
													  .SetCancelText(AppResources.NO)
													  .SetAction(action));
			});
            return task.Task;
        }

        public static Task<string> AskForPassword(string message, string title)
        {
            var task = new TaskCompletionSource<string>();
            var promptConfig = new PromptConfig()
                                    .SetCancellable(true)
                                    .SetOkText(AppResources.OK)
                                    .SetCancelText(AppResources.CANCEL)
                                    .SetMessage(message)
                                    .SetTitle(title)
                                    .SetPlaceholder(AppResources.PASSWORD)
                                    .SetInputMode(InputType.Password);

            promptConfig.OnAction = entrada =>
            {
                if (entrada.Ok)
                {
                    task.SetResult(entrada.Value);
                }
                else
                {
                    task.SetCanceled();
                }
            };

            UserDialogs.Instance.Prompt(promptConfig);
            return task.Task;
        }

        public static void ShowMessage(string message)
        {
            ShowAlertOK(AppResources.APP_TITLE, message);
        }
    }
}
