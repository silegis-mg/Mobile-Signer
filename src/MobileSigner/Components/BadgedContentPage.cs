using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Almg.MobileSigner.Components
{
    /// <summary>
    /// Content page com configurações da badge usada dentro da BadgedTabbedPage
    /// </summary>
    public class BadgedContentPage: ContentPage
    {
        public static BindableProperty BadgeTextProperty = BindableProperty.CreateAttached("BadgeText", typeof(string), typeof(BadgedTabbedPage), default(string), BindingMode.OneWay);
        public static BindableProperty BadgeColorProperty = BindableProperty.CreateAttached("BadgeColor", typeof(Color), typeof(BadgedTabbedPage), Color.Default, BindingMode.OneWay);

        public string BadgeText
        {
            get
            {
                return (string)GetValue(BadgeTextProperty);
            }
            set
            {
                SetValue(BadgeTextProperty, value);
            }
        }
    }
}
