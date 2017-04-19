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
using Xamarin.Forms;
using Almg.MobileSigner.Components;
using Almg.MobileSigner.Droid;

[assembly: ExportRenderer(typeof(CustomImageCell), typeof(CustomImageCellRenderer))]
namespace Almg.MobileSigner.Droid
{
    public class CustomImageCellRenderer : ImageCellRenderer
    {
        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, Android.Views.ViewGroup parent, Android.Content.Context context)
        {
            LinearLayout cell = (LinearLayout)base.GetCellCore(item, convertView, parent, context);
            ImageView image = (ImageView)cell.GetChildAt(0);
            image.SetScaleType(ImageView.ScaleType.Center);
            return cell;
        }
    }
}