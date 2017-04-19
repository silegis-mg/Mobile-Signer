using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Droid.Renderers;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace Almg.MobileSigner.Droid.Renderers
{
    public class CustomListViewRenderer : ListViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var view = (CustomListView)e.NewElement;

                Control.ItemLongClick += (s, args) =>
                {
                    //this.Control.SetItemChecked(args.Position, true);
                    Java.Lang.Object item = this.Control.GetItemAtPosition(args.Position);
                    var selectedItem = item.GetType().GetProperty("Instance").GetValue(item);
                    view.OnLongClicked(selectedItem);
                };
            }
        }
    }
}