using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Almg.MobileSigner.Controllers
{
    public class ActionController
    {
        public async Task ExecuteAction(InboxMessageAction action,
                                        RefreshMessagesCallback refreshCallback)
        {

            string questionMessage = AppResources.ResourceManager.GetString(action.Name + "_QUESTION");
            string progressMessage = AppResources.ResourceManager.GetString(action.Name + "_PROGRESS");
            string failedMessage = AppResources.ResourceManager.GetString(action.Name + "_FAILED");

            if (string.IsNullOrEmpty(questionMessage))
            {
                questionMessage = AppResources.ResourceManager.GetString("DEFAULT_ACTION_QUESTION");
            }

            if (string.IsNullOrEmpty(progressMessage))
            {
                progressMessage = AppResources.ResourceManager.GetString("DEFAULT_ACTION_PROGRESS");
            }

            if (string.IsNullOrEmpty(failedMessage))
            {
                failedMessage = AppResources.ResourceManager.GetString("DEFAULT_ACTION_FAILED");
            }

            if(!string.IsNullOrEmpty(action.ConfirmMessage))
            {
                questionMessage = action.ConfirmMessage;
            }

            var ok = await DialogHelper.ShowConfirm(AppResources.APP_TITLE, questionMessage);
            if (ok)
            {
                using (var progress = DialogHelper.ShowProgress(progressMessage))
                {
                    ActionController actionCtrl = new ActionController();
                    ok = await actionCtrl.ExecuteAction(action);
                    if (ok)
                    {
                        await refreshCallback();
                    }
                    else
                    {
                        DialogHelper.ShowAlertOK(AppResources.APP_TITLE, failedMessage);
                    }
                }
            }
        }

        private async Task<bool> ExecuteAction(InboxMessageAction inboxAction)
        {
            try
            {
                HttpResponseMessage response = null;
                switch (inboxAction.Method)
                {
                    case "GET":
                        response = await HttpRequest.GetHttpResponse(EndPointHelper.GetHost() + inboxAction.Url);
                        break;
                    case "POST":
                        response = await HttpRequest.Post(EndPointHelper.GetHost() + inboxAction.Url, (HttpContent)null);
                        break;
                    case "DELETE":
                        response = await HttpRequest.Delete(EndPointHelper.GetHost() + inboxAction.Url);
                        break;
                    case "PUT":
                        response = await HttpRequest.Put(EndPointHelper.GetHost() + inboxAction.Url);
                        break;
                    default:
                        throw new Exception("Action has an unsupported HTTP method");
                }
                return response.IsSuccessStatusCode;
            } catch(Exception e)
            {
                return false;
            }
        }
    }
}
