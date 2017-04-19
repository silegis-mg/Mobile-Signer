using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using Almg.MobileSigner.Components;
using MobileSigner.iOS;
using UIKit;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomListView), typeof(CustomListViewRenderer))]
namespace MobileSigner.iOS
{
    public class CustomListViewRenderer : ListViewRenderer
    {
		private UILongPressGestureRecognizer longPressGestureRecognizer;
		private CustomListView list;

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
			this.Control.SeparatorInset = UIEdgeInsets.Zero;
			this.Control.LayoutMargins = UIEdgeInsets.Zero;
			this.Control.PreservesSuperviewLayoutMargins = false;

			list = e.NewElement as CustomListView;

			if (list == null || Control == null) 
				return;

			longPressGestureRecognizer = new UILongPressGestureRecognizer(LongPressMethod)
			{
				MinimumPressDuration = 1.0f
			};

			if (e.NewElement == null)
			{
				if (longPressGestureRecognizer != null)
				{
					this.RemoveGestureRecognizer(longPressGestureRecognizer);
				}
			}

			if (e.OldElement == null)
			{
				this.AddGestureRecognizer(longPressGestureRecognizer);

			}
        }

		private void LongPressMethod(UILongPressGestureRecognizer gestureRecognizer)
		{
			var p = gestureRecognizer.LocationInView(Control);
			var longPressOnTheList = Control.IndexPathForRowAtPoint(p);

			if (longPressOnTheList == null) 
				return;

			// if a the long press is on a row on the listView
			var counter = 0;
			foreach (var item in list.ItemsSource)
			{
				if (counter == longPressOnTheList.Row)
				{
					list.OnLongClicked(item);
					break;
				}
				counter += 1;
			}
		}
    }
}