using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Almg.MobileSigner.Controllers;
using Almg.MobileSigner.Components;

namespace Almg.MobileSigner.Pages
{
    public delegate Task RefreshItems(DocumentListType documentListType, int page);

	public class PendingRequestsPage : BadgedTabbedPage
	{
		private const int ITEM_PAGE_COUNT = 20;
		private const string MINE = "MEU_SETOR";
		private const string OTHER = "OUTRO_SETOR";

		private bool ShowArchived { get; set; }

		private SignatureRequestController signatureRequestsCtrl = new SignatureRequestController();
		private Dictionary<string, MessageCollection<SignatureRequest>> signatureRequestGroups = new Dictionary<string, MessageCollection<SignatureRequest>>();

		private AppUpdateController updateCtrl = new AppUpdateController();

		private async Task LoadSignatureRequests(DocumentListType documentListType, int page)
		{
			try
			{
				await updateCtrl.CheckUpdate();

				List<Task<SignatureRequestInboxPage>> tasks = new List<Task<SignatureRequestInboxPage>>();
				Task<SignatureRequestInboxPage> requestsMine = null;
				Task<SignatureRequestInboxPage> requestsOthers = null;

				if (documentListType == DocumentListType.PENDING_MINE || documentListType == DocumentListType.ALL)
				{
					requestsMine = signatureRequestsCtrl.GetSignatureRequests(MINE, ShowArchived, page, ITEM_PAGE_COUNT);
					tasks.Add(requestsMine);
				}

				if (documentListType == DocumentListType.PENDING_OTHER || documentListType == DocumentListType.ALL)
				{
					requestsOthers = signatureRequestsCtrl.GetSignatureRequests(OTHER, ShowArchived, page, ITEM_PAGE_COUNT);
					tasks.Add(requestsOthers);
				}

				await Task.WhenAll(tasks.ToArray());

				if (requestsMine != null)
				{
					signatureRequestGroups[MINE].Update(requestsMine.Result.Requests,
														ShowArchived ? requestsMine.Result.NotReadCountArchived + requestsMine.Result.NotReadCount : requestsMine.Result.NotReadCount,
														page,
														page == 0);
				}

				if (requestsOthers != null)
				{
					signatureRequestGroups[OTHER].Update(requestsOthers.Result.Requests,
														 ShowArchived ? requestsOthers.Result.NotReadCountArchived + requestsOthers.Result.NotReadCount : requestsOthers.Result.NotReadCount,
														 page,
														 page == 0);
				}
			}
			catch (Exception e)
			{
				if (e is HttpRequest.HttpException &&
					((HttpRequest.HttpException)e).HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
				{
                    DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.UNAUTHORIZED_HTTP);
                    //the jwt token is invalid or expired. Send the user to the login screen.
                    Device.BeginInvokeOnMainThread(() =>
					{
						App.Current.MainPage = new LoginPage();
					});
				}
				else
				{
					DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.FAILED_LOADING_SIGNATURE_REQUESTS);
				}
			}
		}

		public PendingRequestsPage(String title)
		{
			this.Title = title;

			ShowArchived = false;

			signatureRequestGroups.Add(MINE, new MessageCollection<SignatureRequest>(ITEM_PAGE_COUNT));
			signatureRequestGroups.Add(OTHER, new MessageCollection<SignatureRequest>(ITEM_PAGE_COUNT));

			this.Children.Add(new DocumentListPage(DocumentListType.PENDING_MINE, "user.png", LoadSignatureRequests, signatureRequestGroups[MINE]));
			this.Children.Add(new DocumentListPage(DocumentListType.PENDING_OTHER, "users.png", LoadSignatureRequests, signatureRequestGroups[OTHER]));

			CreateFilterToolbar(this);

			Refresh();
		}

		private void Refresh()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Refreshing(true);
				Task.Run(() => LoadSignatureRequests(DocumentListType.ALL, 0));
			});
		}

		public void Refreshing(bool value)
		{
			foreach (var c in this.Children)
			{
				((DocumentListPage)c).Refreshing = value;
			}
		}

		private void CreateFilterToolbar(Page page)
		{
			ToolbarItem ti = new ToolbarItem();
			ti.Priority = 0;

			if (Device.OS == TargetPlatform.iOS)
			{
				ti.Order = ToolbarItemOrder.Primary;
				ti.Icon = (FileImageSource)FileImageSource.FromFile("menu.png");
			}
			else {
				ti.Order = ToolbarItemOrder.Secondary;
			}

			ti.Text = AppResources.SHOW_ARCHIVED;
			ti.Command = new Command(async () =>
			{
				if (Device.OS == TargetPlatform.iOS)
				{
					if (await ShowActionSheet())
					{
						ShowArchived = !ShowArchived;
						Refresh();
					}
				}
				else {
					ShowArchived = !ShowArchived;
					Refresh();
				}
				ti.Text = ShowArchived ? AppResources.HIDE_ARCHIVED : AppResources.SHOW_ARCHIVED;
			});

			page.ToolbarItems.Clear();
			page.ToolbarItems.Add(ti);
		}

		private async Task<bool> ShowActionSheet()
		{
			var buttons = new String[] { ShowArchived ? AppResources.HIDE_ARCHIVED : AppResources.SHOW_ARCHIVED };
			var selectedActionName = await this.DisplayActionSheet(AppResources.ACTIONS, AppResources.CLOSE, null, buttons);
			if (selectedActionName == buttons[0])
			{
				return true;
			}
			return false;
		}
    }
}
