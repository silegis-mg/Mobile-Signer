using Acr.UserDialogs;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Controllers;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Model.Converters;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    public class DocumentListPage: BadgedContentPage
    {
        private MessageCollection<SignatureRequest> mySignatureRequests;
        private CustomListView listView;
        private DocumentListType documentType;
        private SignatureRequestController signatureRequestController = new SignatureRequestController();

        private RefreshMessagesCallback refreshSignatures;
        private ActionController actionController = new ActionController();

        public DocumentListPage(DocumentListType documentType, String icon, RefreshItems refreshCallback, MessageCollection<SignatureRequest> requestsGroup)
        {
            mySignatureRequests = requestsGroup;

			NavigationPage.SetHasBackButton(this, false);
            
            listView = new CustomListView
            {
                ItemsSource = mySignatureRequests,
                RowHeight = 100,
                IsPullToRefreshEnabled = true,
				Margin = new Thickness(Device.OnPlatform(0, 5, 5), Device.OnPlatform(5, 0, 0), 5, 0),
                ItemTemplate = new DataTemplate(() =>
                {

                    Grid gridTitle = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        RowDefinitions =
                            {
                                new RowDefinition { Height = GridLength.Auto }
                            },
                        ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                new ColumnDefinition { Width = 80 }
                            }
                    };

                    Grid gridStatus = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        RowDefinitions =
                            {
                                new RowDefinition { Height = GridLength.Star }
                            },
                        ColumnDefinitions =
                            {
                                new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                                new ColumnDefinition { Width = 120 }
                            }
                    };

                    Label labelTitle = new Label();
                    labelTitle.SetBinding(Label.TextProperty, "Title");
                    labelTitle.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelTitle.FontSize = 18;
                    labelTitle.LineBreakMode = LineBreakMode.TailTruncation;
                    labelTitle.SetBinding(Label.FontAttributesProperty, "Read", BindingMode.Default, new ReadFontAttributeConverter());
                    
                    Label labelData = new Label();
                    labelData.SetBinding(Label.TextProperty, "DateStr");
                    labelData.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelData.FontSize = 12;
					labelData.HorizontalTextAlignment = TextAlignment.End;
                    labelData.TextColor = Color.FromHex("#0A80D0");
					labelData.Margin = new Thickness(0, 2, 0, 0);
                    labelData.SetBinding(Label.FontAttributesProperty, "Read", BindingMode.Default, new ReadFontAttributeConverter());

                    Label labelDescription = new Label();
                    labelDescription.SetBinding(Label.TextProperty, "Description");
                    labelDescription.FontSize = 14;
                    labelDescription.HorizontalOptions = LayoutOptions.FillAndExpand;
					labelDescription.VerticalOptions = LayoutOptions.FillAndExpand;
                    labelDescription.LineBreakMode = LineBreakMode.TailTruncation;
                    labelDescription.SetBinding(Label.FontAttributesProperty, "Read", BindingMode.Default, new ReadFontAttributeConverter());

                    Label labelAuthors = new Label();
                    labelAuthors.SetBinding(Label.TextProperty, "Author");
                    labelAuthors.TextColor = Color.FromHex("#0A80D0");
                    labelAuthors.FontSize = 12;
                    labelAuthors.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelAuthors.VerticalOptions = LayoutOptions.EndAndExpand;
					labelAuthors.Margin = new Thickness(0, 5, 0, 0);
                    labelAuthors.LineBreakMode = LineBreakMode.TailTruncation;
                    labelAuthors.VerticalTextAlignment = TextAlignment.End;
                    labelAuthors.HorizontalTextAlignment = TextAlignment.Start;
                    labelAuthors.SetBinding(Label.FontAttributesProperty, "Read", BindingMode.Default, new ReadFontAttributeConverter());

                    Label labelStatus = new Label();
                    labelStatus.SetBinding(Label.TextProperty, "Status");
                    labelStatus.FontAttributes = FontAttributes.Italic;
                    labelStatus.FontSize = 12;
                    labelStatus.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelStatus.VerticalOptions = LayoutOptions.EndAndExpand;
                    labelStatus.HorizontalTextAlignment = TextAlignment.End;
                    labelStatus.VerticalTextAlignment = TextAlignment.End;
                    labelStatus.Margin = new Thickness(0, 5, 0, 0);

                    gridTitle.Children.Add(labelTitle, 0, 0);
                    gridTitle.Children.Add(labelData, 1, 0);

                    gridStatus.Children.Add(labelAuthors, 0, 0);
                    gridStatus.Children.Add(labelStatus, 1, 0);
                    gridStatus.HeightRequest = 50;

                    var vc = new ViewCell
                    {                        
                        View = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.CenterAndExpand,
							Padding = new Thickness(Device.OnPlatform(10,0,0), 5),
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children =
                                {
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.FillAndExpand,
                                        HorizontalOptions = LayoutOptions.FillAndExpand,
                                        Spacing = 0,
                                        Children =
                                        {
                                            gridTitle,
                                            labelDescription,
                                            gridStatus
                                        }
                                    }
                                }
                        }
                    };

                    /*var mine = documentType == DocumentListType.PENDING_MINE;
                    var ignoreAction = new MenuItem { Text = mine ? AppResources.CANCEL : AppResources.IGNORE, IsDestructive = true };
                    ignoreAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
                    ignoreAction.Clicked += async (sender, e) => {

                        var mi = ((MenuItem)sender);
                        var sig = (SignatureRequest)mi.CommandParameter;

                        if (mine)
                        {
                            await CancelRequest(sig);
                        }
                        else
                        {
                            await IgnoreRequest(sig);
                        }
                    };

                    //TODO: Check https://bugzilla.xamarin.com/show_bug.cgi?id=45027
                    vc.ContextActions.Add(ignoreAction);*/
                    return vc;
                })
            };

            Label labelEmpty = new Label();
            labelEmpty.Text = AppResources.NO_REQUESTS;
            labelEmpty.HorizontalOptions = LayoutOptions.FillAndExpand;
            labelEmpty.VerticalOptions = LayoutOptions.Start;
            labelEmpty.Margin = new Thickness(20);
            labelEmpty.HorizontalTextAlignment = TextAlignment.Center;
            labelEmpty.FontSize = 16;

            this.documentType = documentType;
            this.Title = (documentType==DocumentListType.PENDING_MINE ? AppResources.MY_DOCUMENTS:AppResources.OTHER_DOCUMENTS);

            if (Device.OS == TargetPlatform.iOS)
                this.Icon = (FileImageSource)FileImageSource.FromFile(icon);

            var view = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    labelEmpty,
                    listView
                }
            };
            
            this.Content = view;
            
            mySignatureRequests.CollectionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    reloadBadgeCount();
                    labelEmpty.IsVisible = mySignatureRequests.Count == 0;
                    view.ForceLayout();
                    Refreshing = false;
                });
            };
            
            refreshSignatures = async () =>
            {
                try
                {
                    await refreshCallback(DocumentListType.ALL, 0);
                } catch(Exception e)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        UserDialogs.Instance.Alert(AppResources.ERROR_UNABLE_TO_REACH_REQUEST_LIST_API, AppResources.APP_TITLE, AppResources.OK);
                    });
                }
            };

            listView.RefreshCommand = new Command(() => refreshSignatures());

            listView.ItemSelected += async delegate (object sender, SelectedItemChangedEventArgs e)
            {
                SignatureRequest signatureRequest = (SignatureRequest)e.SelectedItem;

                ((ListView)sender).SelectedItem = null;

                if (signatureRequest == null)
                {
                    return;
                }

                bool reload = false;

                if(!signatureRequest.Read)
                {
                    signatureRequest.Read = true;
                    signatureRequestController.MarkAsRead(signatureRequest);
                    mySignatureRequests.NotReadCount--;
                    reloadBadgeCount();
                }
                    
                if (signatureRequest.Documents.Count>1)
                {
                    var childPage = new ChildDocumentPage(signatureRequest, signatureRequest.CanBeSigned);
                    await Navigation.PushAsync(childPage);
                    reload = await childPage.PageClosedTask;
                }
                else
                {
                    try
                    {
                        Document doc = signatureRequest.Documents.First();
                        using (var progress = DialogHelper.ShowProgress(AppResources.LOADING_DOCUMENT))
                        {
                            await doc.LoadXmlDocument();
                        }

                        SignaturePage assinarPage = new SignaturePage(AppResources.SIGN_DOCUMENT, signatureRequest, doc, signatureRequest.CanBeSigned);
                        await Navigation.PushAsync(assinarPage);
                        reload = await assinarPage.PageClosedTask;
                    } catch(Exception ex)
                    {
                        UserDialogs.Instance.Alert(AppResources.DOCUMENT_LOADING_FAILED, AppResources.APP_TITLE, AppResources.OK);
                    }
                }

                if (reload)
                {
                    await refreshSignatures();
                }
            };

            listView.ItemAppearing += (sender, e) =>
            {
                var selected = e.Item as SignatureRequest;
                
                if (mySignatureRequests.MoreAvailable && mySignatureRequests.ToList().Last() == selected)
                {
                    refreshCallback(documentType, mySignatureRequests.CurrentPage+1);
                }
            };


			bool showingActionSheet = false;

            listView.LongClicked += async (sender, e) =>
            {
				if (showingActionSheet)
				{
					return;
				}

                SignatureRequest selected = (SignatureRequest)e.Item;

                List<string> buttons = new List<string>
                {
                    AppResources.DOCUMENT_METADATA
                };
                                
                if(selected.AttachmentCount>0)
                {
                    buttons.Add(AppResources.ATTACHMENTS);
                }

                if(!selected.Archived)
                {
                    buttons.Add(AppResources.ARCHIVE);
                }

                Dictionary<string, InboxMessageAction> dictActions = selected.Actions.
                                               Where(a => AppResources.ResourceManager.GetString(a.Name) != null).
                                               ToDictionary(x => AppResources.ResourceManager.GetString(x.Name),
                                                            x => x);

                if (dictActions.Keys.Count > 0)
                {
                    buttons.AddRange(dictActions.Keys);
                }

				showingActionSheet = true;
                var selectedActionName = await this.DisplayActionSheet(AppResources.ACTIONS, AppResources.CLOSE, null, buttons.ToArray());
				showingActionSheet = false;

				if (string.IsNullOrEmpty(selectedActionName))
                {
                    return;
                }

                if (selectedActionName == AppResources.ATTACHMENTS)
                {
                    await Navigation.PushAsync(new AttachmentsPage(selected));
                }
                else if(selectedActionName == AppResources.DOCUMENT_METADATA)
                {
                    await Navigation.PushAsync(new DocumentInfo(selected));
                }
                else if(selectedActionName == AppResources.ARCHIVE)
                {
                    await ArchiveRequest(selected);
                }
                else if (dictActions.ContainsKey(selectedActionName))
                {
                    var action = dictActions[selectedActionName];
                    if (action != null)
                    {
                        await actionController.ExecuteAction(action, refreshSignatures);
                    }
                }
            };

            //CreateFilterToolbar(this);

            MessagingCenter.Subscribe<string>(this, "Notification", s => refreshSignatures(), null);
        }

        public bool Refreshing
        {
            get
            {
                return listView.IsRefreshing;
            }
            set
            {
                listView.IsRefreshing = value;
            }
        }

        private async Task ArchiveRequest(SignatureRequest req)
        {
            using (var progress = DialogHelper.ShowProgress(AppResources.ARCHIVING))
            {
                var ok = await signatureRequestController.MarkAsArchived(req);
                if (ok)
                {
                    await refreshSignatures();
                }
            }
        }

        private void reloadBadgeCount()
        {
            int count = mySignatureRequests.NotReadCount;
            if (count > 0)
            {
                BadgeText = count.ToString();
            }
            else
            {
                BadgeText = "";
            }
        }
    }
}