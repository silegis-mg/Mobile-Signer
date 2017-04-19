using System;
using Almg.MobileSigner.Components;
using MobileSigner.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MenuTableView), typeof(MenuTableViewRenderer))]
namespace MobileSigner.iOS
{

	public class MenuTableViewRenderer : TableViewRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<TableView> e)
		{
			base.OnElementChanged(e);

			if (Control == null)
				return;

			var tableView = Control as UITableView;
			//The footer will hide empty cells
			tableView.TableFooterView = new UIView();
		}
	}

}
