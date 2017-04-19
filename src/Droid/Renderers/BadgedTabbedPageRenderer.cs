using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Android.Database;
using Xamarin.Forms.Platform.Android.AppCompat;
using Xamarin.Forms;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Almg.MobileSigner.Droid.Util;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Droid.Renderers;

[assembly: ExportRenderer(typeof(BadgedTabbedPage), typeof(BadgedTabbedPageRenderer))]
namespace Almg.MobileSigner.Droid.Renderers
{
    //Baseado no https://github.com/xabre/xamarin-forms-tab-badge/blob/master/Source/Plugin.Badge.Droid/BadgedTabbedPageRenderer.cs
    public class BadgedTabbedPageRenderer : TabbedPageRenderer
    {
        protected readonly Dictionary<Element, Badge> BadgeViews = new Dictionary<Element, Badge>();
        private TabLayout tabLayout;
        private TabLayout.SlidingTabStrip tabStrip;

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            tabLayout = ViewGroup.FindChildOfType<TabLayout>();
            if (tabLayout == null)
            {
                return;
            }

            tabStrip = tabLayout.FindChildOfType<TabLayout.SlidingTabStrip>();

            for (var i = 0; i < tabLayout.TabCount; i++)
            {
                AddTabBadge(i);
            }

            Element.ChildAdded += OnTabAdded;
            Element.ChildRemoved += OnTabRemoved;
        }

        private void AddTabBadge(int tabIndex)
        {
            var element = Element.Children[tabIndex];
            var tabbedContentPage = (BadgedContentPage)element;
            var view = tabLayout?.GetTabAt(tabIndex).CustomView ?? tabStrip?.GetChildAt(tabIndex);

            var badgeView = (view as ViewGroup)?.FindChildOfType<Badge>();

            if (badgeView == null)
            {
                var imageView = (view as ViewGroup)?.FindChildOfType<ImageView>();

                var badgeTarget = imageView?.Drawable != null
                    ? (Android.Views.View)imageView
                    : (view as ViewGroup)?.FindChildOfType<TextView>();

                badgeView = new Badge(Context, badgeTarget);
            }

            BadgeViews[element] = badgeView;

            var badgeText = tabbedContentPage.BadgeText;
            badgeView.Text = badgeText;
            element.PropertyChanged += OnTabbedPagePropertyChanged;
        }

        protected virtual void OnTabbedPagePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var element = sender as Element;
            var contentPage = (BadgedContentPage)element;

            if (element == null)
                return;

            Badge badgeView;
            if (!BadgeViews.TryGetValue(element, out badgeView))
            {
                return;
            }

            if (e.PropertyName == BadgedContentPage.BadgeTextProperty.PropertyName)
            {
                badgeView.Text = contentPage.BadgeText;
                return;
            }
        }

        private void OnTabRemoved(object sender, ElementEventArgs e)
        {
            e.Element.PropertyChanged -= OnTabbedPagePropertyChanged;
            BadgeViews.Remove(e.Element);
        }

        private async void OnTabAdded(object sender, ElementEventArgs e)
        {
            var page = e.Element as Page;
            if (page == null)
                return;

            var tabIndex = Element.Children.IndexOf(page);
            AddTabBadge(tabIndex);
        }

        protected override void Dispose(bool disposing)
        {
            if (Element != null)
            {
                foreach (var tab in Element.Children)
                {
                    tab.PropertyChanged -= OnTabbedPagePropertyChanged;
                }
                Element.ChildRemoved -= OnTabRemoved;
                Element.ChildAdded -= OnTabAdded;
                BadgeViews.Clear();
            }
            base.Dispose(disposing);
        }
    }
}