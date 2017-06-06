using Acr.UserDialogs;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Controllers;
using Almg.MobileSigner.Helpers;
using Almg.MobileSigner.Model;
using Almg.MobileSigner.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Pages
{
    public class SignedDocumentsPage: ContentPage
    {
        private const int ITEM_PAGE_COUNT = 20;
        private MessageCollection<SignedDocument> requests = new MessageCollection<SignedDocument>(20);
        private SignatureRequestController signatureRequestController = new SignatureRequestController();
        private CustomListView listView;
        private ActionController actionController = new ActionController();

        public SignedDocumentsPage()
        {
            NavigationPage.SetHasBackButton(this, false);

            listView = new CustomListView
            {
                ItemsSource = requests,
                RowHeight = 100,
                IsPullToRefreshEnabled = true,
                Margin = new Thickness(Device.OnPlatform(0, 5, 5), Device.OnPlatform(5, 0, 0), 5, 0),
                HasUnevenRows = true,
                ItemTemplate = new DataTemplate(() =>
                {
                    Grid gridTitle = new Grid
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
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
                                new ColumnDefinition { Width = 150 }
                            }
                    };

                    gridTitle.HorizontalOptions = LayoutOptions.FillAndExpand;
                    Label labelTitle = new Label();
                    labelTitle.SetBinding(Label.TextProperty, "Title");
                    labelTitle.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelTitle.VerticalOptions = LayoutOptions.FillAndExpand;
                    labelTitle.FontSize = 18;
                    labelTitle.LineBreakMode = LineBreakMode.WordWrap;

                    Label labelData = new Label();
                    labelData.SetBinding(Label.TextProperty, "DateStr");
                    labelData.HorizontalOptions = LayoutOptions.EndAndExpand;
                    labelData.FontSize = 12;
                    labelData.HorizontalTextAlignment = TextAlignment.End;
                    labelData.TextColor = Color.FromHex("#0A80D0");
                    labelData.Margin = new Thickness(0, 2, 0, 0);

                    Label labelDescription = new Label();
                    labelDescription.SetBinding(Label.TextProperty, "Description");
                    labelDescription.FontSize = 14;
                    labelDescription.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelDescription.VerticalOptions = LayoutOptions.FillAndExpand;
                    labelDescription.LineBreakMode = LineBreakMode.TailTruncation;

                    Label labelAuthors = new Label();
                    labelAuthors.SetBinding(Label.TextProperty, "Author");
                    labelAuthors.TextColor = Color.FromHex("#0A80D0");
                    labelAuthors.FontSize = 12;
                    labelAuthors.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelAuthors.VerticalOptions = LayoutOptions.FillAndExpand;
                    labelAuthors.VerticalTextAlignment = TextAlignment.End;
                    labelAuthors.Margin = new Thickness(0, 5, 0, 0);

                    Label labelStatus = new Label();
                    labelStatus.SetBinding(Label.TextProperty, "Status");
                    //labelStatus.TextColor = Color.FromHex("#0A80D0");
                    labelStatus.FontAttributes = FontAttributes.Italic;
                    labelStatus.FontSize = 12;
                    labelStatus.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelStatus.VerticalOptions = LayoutOptions.FillAndExpand;
                    labelStatus.HorizontalTextAlignment = TextAlignment.End;
                    labelStatus.VerticalTextAlignment = TextAlignment.End;
                    labelStatus.Margin = new Thickness(0, 5, 0, 0);

                    gridTitle.Children.Add(labelTitle, 0, 0);
                    gridTitle.Children.Add(labelData, 1, 0);
                    gridTitle.HorizontalOptions = LayoutOptions.FillAndExpand;
                    gridTitle.VerticalOptions = LayoutOptions.FillAndExpand;

                    gridStatus.Children.Add(labelAuthors, 0, 0);
                    gridStatus.Children.Add(labelStatus, 1, 0);
                    gridStatus.VerticalOptions = LayoutOptions.FillAndExpand;
                    gridStatus.HorizontalOptions = LayoutOptions.FillAndExpand;

                    var vc = new ViewCell
                    {
                        View = new StackLayout
                        {
                            VerticalOptions = LayoutOptions.CenterAndExpand,
                            Padding = new Thickness(Device.OnPlatform(10, 0, 0), 5),
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children =
                                {
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
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

            this.Title = AppResources.FINISHED_REQUESTS;

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

            requests.CollectionChanged += (sender, e) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    labelEmpty.IsVisible = requests.Count == 0;
                    view.ForceLayout();
                });
            };

            Action refreshSignatures = async () =>
            {
                try
                {
                    listView.IsRefreshing = true;
                    await LoadSignatureRequests(0);
                    listView.IsRefreshing = false;
                }
                catch (Exception e)
                {
                    listView.IsRefreshing = false;
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        DialogHelper.ShowAlertOK(AppResources.APP_TITLE, AppResources.ERROR_UNABLE_TO_REACH_REQUEST_LIST_API);
                    });
                }
            };

            listView.RefreshCommand = new Command(refreshSignatures);

            listView.ItemSelected += async delegate (object sender, SelectedItemChangedEventArgs e)
            {
                SignedDocument signedDocument = (SignedDocument)e.SelectedItem;
                ((ListView)sender).SelectedItem = null;

                if (signedDocument == null)
                {
                    return;
                }

                if (signedDocument.Documents.Count > 1)
                {
                    var childPage = new ChildDocumentPage(signedDocument, false);
                    await Navigation.PushAsync(childPage);
                    await childPage.PageClosedTask;
                }
                else
                {
                    try
                    {
                        Document doc = signedDocument.Documents.First();
                        using (var progress = DialogHelper.ShowProgress(AppResources.LOADING_DOCUMENT))
                        {
                            await doc.LoadXmlDocument();
                        }
                        SignaturePage assinarPage = new SignaturePage(doc.Title, signedDocument, doc, false);
                        await Navigation.PushAsync(assinarPage);
                        await assinarPage.PageClosedTask;
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.Alert(AppResources.DOCUMENT_LOADING_FAILED, AppResources.APP_TITLE, AppResources.OK);
                    }
                }
            };

            listView.ItemAppearing += async (sender, e) =>
            {
                var selected = e.Item as SignedDocument;

                if (requests.MoreAvailable && requests.ToList().Last() == selected)
                {
                    await LoadSignatureRequests(requests.CurrentPage + 1);
                }
            };

			bool showingActionSheet = false;
            listView.LongClicked += async (sender, e) =>
            {
				if (showingActionSheet)
				{
					return;
				}

                SignedDocument selected = (SignedDocument)e.Item;

                List<string> buttons = new List<string>
                {
                    AppResources.DOCUMENT_METADATA
                };

                if (selected.AttachmentCount > 0)
                {
                    buttons.Add(AppResources.ATTACHMENTS);
                }

                Dictionary<string, InboxMessageAction> dictActions = selected.Actions.
                                                               Where(a => AppResources.ResourceManager.GetString(a.Name)!=null).
                                                               ToDictionary(x => AppResources.ResourceManager.GetString(x.Name), 
                                                                            x => x);
                
                if(dictActions.Keys.Count>0)
                {
                    buttons.AddRange(dictActions.Keys);
                }

				showingActionSheet = true;
                var selectedActionName = await this.DisplayActionSheet(AppResources.ACTIONS, AppResources.CANCEL, null, buttons.ToArray());
				showingActionSheet = false;

                if(string.IsNullOrEmpty(selectedActionName))
                {
                    return;
                }

                if (selectedActionName == AppResources.ATTACHMENTS)
                {
                    await Navigation.PushAsync(new AttachmentsPage(selected));
                }
                else if (selectedActionName == AppResources.DOCUMENT_METADATA)
                {
                    await Navigation.PushAsync(new DocumentInfo(selected));
                } else if(dictActions.ContainsKey(selectedActionName))
                {
                    var action = dictActions[selectedActionName];
                    if (action!=null)
                    {
                        await actionController.ExecuteAction(action, async () => await LoadSignatureRequests(0));
                    }
                }
            };

            LoadSignatureRequests(0);
        }

        private async Task LoadSignatureRequests(int page)
        {
            listView.IsRefreshing = true;
            var signatureRequests = await signatureRequestController.GetSignedDocuments(page, ITEM_PAGE_COUNT);
            requests.Update(signatureRequests, 0, page, page==0);
            listView.IsRefreshing = false;
        }
                
    }
}
