using Acr.UserDialogs;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Exceptions;
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
    public class ChildDocumentPage : ContentPage
    {
        private BaseInboxMessage request;

        public Task<bool> PageClosedTask
        {
            get
            {
                return tcs.Task;
            }
        }

        private TaskCompletionSource<bool> tcs { get; set; }

        public ChildDocumentPage(BaseInboxMessage request, bool toSign)
        {
            this.tcs = new TaskCompletionSource<bool>();
            this.request = request;
            this.Title = request.Title;
            //this.Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5);

            var listView = new CustomListView
            {
                ItemsSource = request.Documents,
                RowHeight = 60,
                IsPullToRefreshEnabled = false,
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
                                new ColumnDefinition { Width = 70 }
                            }
                    };

                    gridTitle.HorizontalOptions = LayoutOptions.FillAndExpand;

                    Label labelTitle = new Label();
                    labelTitle.SetBinding(Label.TextProperty, "Title");
                    labelTitle.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelTitle.FontSize = 18;

                    Label labelDescription = new Label();
                    labelDescription.SetBinding(Label.TextProperty, "Description");
                    labelDescription.FontSize = 12;
					labelDescription.Margin = new Thickness(0, 5, 0, 0);
                    labelDescription.HorizontalOptions = LayoutOptions.FillAndExpand;
                    labelDescription.LineBreakMode = LineBreakMode.TailTruncation;

                    gridTitle.Children.Add(labelTitle, 0, 0);

                    return new ViewCell
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
                                        labelDescription
                                    }
                                }
                            }
                        }
                    };
                })
            };

			listView.Margin = new Thickness(Device.OnPlatform(0, 10, 10), Device.OnPlatform(20, 0, 0), 10, 5);
            listView.ItemSelected += async delegate (object sender, SelectedItemChangedEventArgs e)
            {
                Document doc = (Document)e.SelectedItem;
                ((ListView)sender).SelectedItem = null;

                if (doc == null)
                {
                    return;
                }

                await LoadXMLs();
                await Navigation.PushAsync(new SignaturePage((toSign?AppResources.SIGN_DOCUMENT:doc.Title), request, doc, false));
            };

            Button btnAssinar = new Button()
            {
                Text = AppResources.SIGN_DOCUMENTS,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            if(toSign)
            {
                var relativeLayout = new RelativeLayout();

                relativeLayout.Children.Add(listView, Constraint.RelativeToParent((parent) =>
                {
                    return parent.X;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Y;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Height - 50;
                }));

                relativeLayout.Children.Add(btnAssinar, Constraint.RelativeToView(listView, (Parent, sibling) =>
                {
                    return 0;
                }), Constraint.RelativeToView(listView, (parent, sibling) =>
                {
                    return sibling.Height;
                }), Constraint.RelativeToParent((parent) =>
                {
                    return parent.Width;
                }), Constraint.Constant(50));

                this.Content = relativeLayout;
            } else {
                this.Content = listView;
            }

            btnAssinar.Clicked += async delegate (object sender, EventArgs e)
            {
                await LoadXMLs();
                await Sign();
            };
        }

        private async Task LoadXMLs()
        {
            bool xmlLoaded = request.Documents.All(doc => doc.XmlLoaded);
            if(!xmlLoaded)
            {
                using (var progress = DialogHelper.ShowProgress(AppResources.LOADING_DOCUMENT))
                {
                    try
                    {
                        foreach (Document doc in request.Documents)
                        {
                            await doc.LoadXmlDocument();
                        }
                    }
                    catch (Exception)
                    {
                        UserDialogs.Instance.Alert(AppResources.DOCUMENT_LOADING_FAILED, AppResources.APP_TITLE, AppResources.OK);
                    }
                }
            }
        }

        private async Task Sign()
        {
            try
            {
                await SignerHelper.Sign(this.request);
                await Navigation.PopAsync();
                tcs.SetResult(true);
            }
            catch (DigitalSignatureError e)
            {
                ShowMessage(e.Message);
            }
            catch (TaskCanceledException)
            {
                ShowMessage(AppResources.SIGNATURE_CANCELLED);
            }
            catch (Exception e)
            {
                //TODO: log this unexpected error
                ShowMessage(e.Message);
            }
        }

        private void ShowMessage(String message)
        {
            UserDialogs.Instance.Alert(message, AppResources.APP_TITLE, AppResources.OK);
        }
    }
}
